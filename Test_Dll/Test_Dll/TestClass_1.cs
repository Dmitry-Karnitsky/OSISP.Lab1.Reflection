using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Dll
{
    public class TestClass_1
    {
        public TestClass_1()
        {

        }

        public TestClass_1(int i)
        {

        }
        
        public DateTime ShowDateAndTime()
        {
            Console.WriteLine("ShowDateAndTime");
            return DateTime.Now;
        }

        public void VoidMethodWithParameters(int i, double d)
        {
            Console.WriteLine("VoidMethodWithParameters");
            Console.WriteLine(String.Format("{0}, {1}", i , d));

        }

        public void VoidMethodWithoutParameters()
        {
            Console.WriteLine("VoidMethodWithoutParameters");
            Console.WriteLine("Should return... nothing.");
        }

        public int MethodWithIntegerReturnValue(string str)
        {
            Console.WriteLine("MethodWithIntegerReturnValue");
            Console.WriteLine("Should return 10. I don't know why. Just for fun.");
            return 10;
        }

        public int MethodWithIntegerReturnValue(int i)
        {
            Console.WriteLine("MethodWithIntegerReturnValue");
            Console.WriteLine("Should return i*2.");
            return i*2;
        }

        public int MethodWithIntegerReturnValue(params int[] values)
        {
            Console.WriteLine("MethodWithIntegerReturnValue");
            Console.WriteLine("Should return sum of incoming parameters.");
            return values.Sum();
        }
    }
}
