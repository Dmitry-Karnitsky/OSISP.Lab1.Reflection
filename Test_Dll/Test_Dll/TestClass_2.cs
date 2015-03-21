using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Dll
{
    public class TestClass_2
    {
        public TestClass_2(int i, string str, double d)
        {

        }

        public TestClass_2(string str)
        {

        }

        public override string ToString()
        {
            return "TestClass_2 called. Class don't have the constructor without parameters.";
        }

        public override bool Equals(object obj)
        {
            return false;
        }
        public override int GetHashCode()
        {
            return 12345678;
        }
    }
}
