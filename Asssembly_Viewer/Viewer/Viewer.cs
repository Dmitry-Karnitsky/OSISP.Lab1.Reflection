using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Viewer
{
    static class Viewer
    {
        #region Private Fields
        private static Assembly assembly;
        private static string name = "Test_Dll.dll";
        private static List<Type> assemblyTypes = new List<Type>();
        private static List<MethodInfo> typeMethods = new List<MethodInfo>();
        private static List<ConstructorInfo> typeConstructors = new List<ConstructorInfo>();
        private static Type assemblyType;
        private static Object typeObject;
        #endregion

        public static string AssemblyName
        {
            get { return name; }
            set
            {
                if (value != null || value != "")
                    name = value;
            }
        }

        public static List<string> LoadAssemblyAndGetTypes()
        {
            LoadAssembly();
            ResetFieldsToDefault();

            List<string> result = new List<string>();
            assemblyTypes.AddRange(assembly.ExportedTypes);
            foreach (Type t in assemblyTypes)
            {
                result.Add(t.ToString());
            }
            return result;
        }

        public static void LoadType(string typeName)
        {
            FindTypeConstructorsAndTypeMethods(typeName);
        }

        public static List<string> GetTypeConstructors()
        {
            List<string> result = new List<string>();
            foreach (ConstructorInfo ci in typeConstructors)
            {
                result.Add(ci.ToString());
            }
            return result;
        }

        public static List<string> GetTypeMethods()
        {
            List<string> result = new List<string>();
            foreach (MethodInfo mi in typeMethods)
            {
                result.Add(mi.ToString());
            }
            return result;
        }

        public static object InvokeMethod(string methodSignature, params object[] parameters)
        {
            try
            {

                MethodInfo mi = FindMethodInType(methodSignature);
                ParameterInfo[] paramInfo = mi.GetParameters();
                if (parameters == null || paramInfo.Length == 0)
                {
                    return mi.Invoke(typeObject, null);
                }
                else
                {
                    int i = 0;

                    if (paramInfo[0].ParameterType.IsArray)
                    {
                        Array arrayParams = (Array)System.Activator.CreateInstance(paramInfo[0].ParameterType, new object[] { parameters.Length });
                        Type elementType = paramInfo[0].ParameterType.GetElementType();

                        i = 0;
                        foreach (object obj in parameters)
                        {
                            arrayParams.SetValue(Convert.ChangeType(parameters[i], elementType), i);
                            i++;
                        }
                        return mi.Invoke(typeObject, new object[] { arrayParams });
                    }
                    else
                    {
                        dynamic[] methodParams = new dynamic[parameters.Length];

                        i = 0;
                        foreach (ParameterInfo info in paramInfo)
                        {
                            methodParams[i] = Convert.ChangeType(parameters[i], info.ParameterType);
                            i++;
                        }

                        return mi.Invoke(typeObject, methodParams);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new TargetParameterCountException();
            }
        }

        public static object InvokeConstructor(string ctorSignature, params object[] parameters)
        {
            try
            {
                ConstructorInfo ci = FindTypeConstructor(ctorSignature);
                ParameterInfo[] paramInfo = ci.GetParameters();
                if (parameters == null || paramInfo.Length == 0)
                {
                    typeObject = ci.Invoke(null);
                    return typeObject;
                }
                else
                {
                    int i = 0;

                    if (paramInfo[0].ParameterType.IsArray)
                    {
                        Array arrayParams = (Array)System.Activator.CreateInstance(paramInfo[0].ParameterType, new object[] { parameters.Length });
                        Type elementType = paramInfo[0].ParameterType.GetElementType();

                        i = 0;
                        foreach (object obj in parameters)
                        {
                            arrayParams.SetValue(Convert.ChangeType(parameters[i], elementType), i);
                            i++;
                        }
                        typeObject = ci.Invoke(new object[] { arrayParams });
                        return typeObject;
                    }
                    else
                    {
                        dynamic[] ctorParams = new dynamic[parameters.Length];

                        i = 0;
                        foreach (ParameterInfo info in paramInfo)
                        {
                            ctorParams[i] = Convert.ChangeType(parameters[i], info.ParameterType);
                            i++;
                        }
                        typeObject = ci.Invoke(ctorParams);
                        return typeObject;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new TargetParameterCountException();
            }

        }


        #region Private Methods

        private static Type FindTypeInAssembly(string typeName)
        {
            foreach (Type t in assemblyTypes)
            {
                if (String.Equals(typeName, t.ToString()))
                {
                    return t;
                }
            }
            return null;
        }
        private static ConstructorInfo FindTypeConstructor(string ctorSignature)
        {
            foreach (ConstructorInfo ci in typeConstructors)
            {
                if (String.Equals(ci.ToString(), ctorSignature))
                {
                    return ci;
                }
            }
            return null;
        }
        private static MethodInfo FindMethodInType(string methodSignature)
        {
            foreach (MethodInfo mi in typeMethods)
            {
                if (String.Equals(methodSignature, mi.ToString()))
                {
                    return mi;
                }
            }
            return null;
        }
        private static void LoadAssembly()
        {
            try
            {
                assembly = Assembly.LoadFile(Viewer.GetAbsolutePath());
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }
        private static void ResetFieldsToDefault()
        {
            assemblyType = null;
            typeObject = null;
            assemblyTypes.Clear();
            typeMethods.Clear();
            typeConstructors.Clear();
        }
        private static void FindTypeConstructorsAndTypeMethods(string typeName)
        {
            assemblyType = FindTypeInAssembly(typeName);
            typeConstructors.AddRange(assemblyType.GetTypeInfo().GetConstructors());
            typeMethods.AddRange(assemblyType.GetTypeInfo().GetMethods());
        }
        private static string GetAbsolutePath()
        {
            return Path.GetFullPath(name);
        }

        #endregion

    }
}
