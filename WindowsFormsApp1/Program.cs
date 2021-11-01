using GeneralTool.General.Adb;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //testc();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void testc()
        {
            var adb = new AdbHelper(@"C:\Code\Ruizi\AndroidAgingTest\App_Debug\adb\adb.exe");

            var elementResult = adb.FindElemeteForText("设置");
            var elementsResult = adb.FindElementsCords("//node");

            var element = elementResult.ResultItem;
            var clickResult= adb.Click(element.X, element.Y);

          
        }
    }
}
