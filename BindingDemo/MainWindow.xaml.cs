using GeneralTool.General.LinqExtensions;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BindingDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var s = "hello ".Fomart();

            s = "h e b\fd".TrimAll(' ','\f');
            InitializeComponent();
            //this.DataContext = new MainViewModel();
            IEnumerable<string> ie = new List<string>() { "1", "2" };
            IEnumerable<int> e = ie.ConvertAll<string, int>();
        }
    }
   

}
