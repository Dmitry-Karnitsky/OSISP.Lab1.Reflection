using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Viewer
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Variables
            List<string> types = new List<string>();
            List<string> methods = new List<string>();
            List<string> constructors = new List<string>();
            #endregion

            #region Assembly Load
            try
            {
                types = Viewer.LoadAssemblyAndGetTypes();
                Console.WriteLine("Assembly succesfully loaded.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found. Make sure that you've entered a correct relevant path.");
            }
            catch (IOException)
            {
                Console.WriteLine("File load error.");
                Console.ReadLine();
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Unknown error. Sad :(");
                Console.ReadLine();
                return;
            }
            #endregion

            #region Assembly Types Showing
            Console.WriteLine("Available types are:");

            int i = 0;
            foreach (string str in types)
            {
                Console.WriteLine(String.Format("    {0}. {1}", i, str));
                i++;
            }
            #endregion

            #region Type Choosing
            Console.WriteLine("Enter an index number of assembly:");

            int menuNumber;
            string inputNumber = Console.ReadLine();

            try
            {
                menuNumber = Convert.ToInt32(inputNumber);
                Viewer.LoadType(types.ElementAt(menuNumber));

            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Wrong menu number.");
                Console.ReadLine();
                return;
            }
            catch (FormatException)
            {
                Console.WriteLine("Not a number.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine();
            #endregion

            #region Constructors Showing
            constructors = Viewer.GetTypeConstructors();
            methods = Viewer.GetTypeMethods();

            Console.WriteLine("Type succesfully loaded. Available constructors are:");
            i = 0;
            foreach (string str in constructors)
            {
                Console.WriteLine(String.Format("    {0}. {1}", i, str));
                i++;
            }
            Console.WriteLine();
            #endregion

            #region Constructor Invoking
            Console.WriteLine("Enter an index number of a constructor:");
            inputNumber = Console.ReadLine();
            string inputParameters;
            object[] parameters = null;

            try
            {
                menuNumber = Convert.ToInt32(inputNumber);
                Console.WriteLine("Enter the constructor parameters(if it exists). Use space as a delimiter.");
                inputParameters = Console.ReadLine();
                if (inputParameters == "")
                {
                    inputParameters = null;
                }
                else
                {
                    parameters = inputParameters.Split(" ".ToCharArray());
                }

                Viewer.InvokeConstructor(constructors[menuNumber], parameters);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Wrong menu number.");
                Console.ReadLine();
                return;
            }
            catch (FormatException)
            {
                Console.WriteLine("Not a number.");
                Console.ReadLine();
                return;
            }
            catch (System.Reflection.TargetParameterCountException)
            {
                Console.WriteLine("Wrong number of parameters.");
                Console.ReadLine();
                return;
            }
            catch (System.ArgumentException)
            {
                Console.WriteLine("Wrong parameters type.");
                Console.ReadLine();
                return;
            }
            #endregion

            #region Methods Showing
            Console.WriteLine();
            Console.WriteLine("Constructor successfully invoked. Available methods are:");
            i = 0;
            foreach (string str in methods)
            {
                Console.WriteLine(String.Format("    {0}. {1}", i, str));
                i++;
            }
            #endregion

            #region Method Invoking
            Console.WriteLine("Enter an index number of a method:");
            inputNumber = Console.ReadLine();
            try
            {
                menuNumber = Convert.ToInt32(inputNumber);
                Console.WriteLine("Enter the method parameters(if it exists). Use space as a delimiter.");

                inputParameters = Console.ReadLine();

                parameters = null;
                if (inputParameters == "")
                {
                    inputParameters = null;
                }
                else
                {
                    parameters = inputParameters.Split(" ".ToCharArray());
                }

                Console.WriteLine(Viewer.InvokeMethod(methods[menuNumber], parameters));

            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Wrong menu number.");
                Console.ReadLine();
                return;
            }
            catch (FormatException)
            {
                Console.WriteLine("Not a number.");
                Console.ReadLine();
                return;
            }
            catch (System.Reflection.TargetParameterCountException)
            {
                Console.WriteLine("Wrong number of parameters.");
                Console.ReadLine();
                return;
            }
            catch (System.ArgumentException)
            {
                Console.WriteLine("Wrong parameters type.");
                Console.ReadLine();
                return;
            }
            #endregion
            
            Console.ReadLine();
        }
    }
}
