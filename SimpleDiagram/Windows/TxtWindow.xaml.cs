using System.Windows;

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
            Txt.Text = txt;
            _ = Txt.Focus();
        }

        public string ResultTxt { get; internal set; }

        private void SubmitMethod(object sender, RoutedEventArgs e)
        {
            ResultTxt = Txt.Text;
            DialogResult = true;
        }
    }
}
