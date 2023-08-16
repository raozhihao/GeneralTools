using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using GeneralTool.CoreLibrary.DbHelper;
using GeneralTool.CoreLibrary.Extensions;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var size = GeneralTool.CoreLibrary.Win32.DesktopHelper.DesktopSize;
            var primary = Screen.PrimaryScreen;


        }
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TypeCode aypeCode { get; set; }
        public double Value { get; set; }


        // public Model Mc { get; set; }

        // public List<MC> Strings { get; set; }
    }

    public class MC
    {
        public int Id { get; set; }
        public string Abc { get; set; }
    }
}
