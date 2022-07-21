using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppTest
{
    /// <summary>
    /// UserControl2.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        public event Action<string> ClickEv;

        public event Func<string, string> Cli;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.ClickEv?.Invoke(this.tt.Text);
            if (this.Cli != null)
            {
                var sss = this.Cli(this.tt.Text);
            }

        }
    }
}
