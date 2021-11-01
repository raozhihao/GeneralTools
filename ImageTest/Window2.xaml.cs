using System.Windows;
using System.Windows.Input;

namespace ImageTest
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            this.Loaded += Window2_Loaded;
            var dh = new DragMoveHelper();
            this.bor.Tag = dh;
            dh.DragInit(this.bor, this.g2);
        }

        private void Window2_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            (this.bor.Tag as DragMoveHelper).RealseDrag();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            (this.bor.Tag as DragMoveHelper).DragAgain();
        }
    }
}
