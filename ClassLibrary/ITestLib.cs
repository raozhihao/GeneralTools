using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface ITestLib
    {
        void T1(string str);

        string T2();
    }

    public class TestLib : ITestLib
    {
        public void T1(string str)
        {
            Console.WriteLine(str);
        }

        public string T2()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
