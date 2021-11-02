using GeneralTool.General.Adb;
using GeneralTool.General.Enums;
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
            testc();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void testc()
        {
            var reslut = 1.ToEnum<AdbDeviceState>();
            var result = "Host".ToEnum<AdbDeviceState>();



        }
    }
}
