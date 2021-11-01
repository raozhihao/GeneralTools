using GeneralTool.General.Adb;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        AdbHelper adb = new AdbHelper(@"C:\Code\Self\SamsungMobileFileTest\bin\Debug\adb\adb.exe");
        private async void button1_Click(object sender, EventArgs e)
        {
            var path = @"C:\Code\Self\SamsungMobileFileTest\bin\Debug\adb\adb.exe";
            GeneralTool.General.ProcessHelpers.ProcessHelper.Run(path, " shell input keyevent 3 ");

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var adb = new AdbHelper(@"C:\Code\Ruizi\AndroidAgingTest\App_Debug\adb\adb.exe");

            //var elementResult = adb.FindElemeteForText("设置");
            //var elementsResult = adb.FindElementsCords("//node");

            //var element = elementResult.ResultItem;
            //var clickResult = adb.Click(element.X, element.Y);

            var result = adb.GetScreen();
            this.pictureBox1.Image = result.ResultItem;

        }
    }
}
