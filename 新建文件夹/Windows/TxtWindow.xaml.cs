using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleDiagram.Windows
{
    /// <summary>
    /// TxtWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TxtWindow : Window
    {
        public TxtWindow(string txt)
        {
            InitializeComponent();
            this.Txt.Text = txt;
            this.Txt.Focus();
        }

        public string ResultTxt { get; internal set; }

        private void SubmitMethod(object sender, RoutedEventArgs e)
        {
            this.ResultTxt = this.Txt.Text;
            this.DialogResult = true;
        }
    }
}
