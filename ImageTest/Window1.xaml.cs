using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GeneralTool.General.WPFHelper;

namespace ImageTest
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.DataContext = new Window1ViewModel();
            this.Loaded += Window1_Loaded;
        }

        private void Pv_ImageDrawEndEvent(object sender, GeneralTool.General.Models.ImageCutRectEventArgs e)
        {
            this.rectTxt.Text = e.CutPixelRect + "";
        }

        private void Pv_MouseMoveEvent(object sender, GeneralTool.General.Models.ImageMouseEventArgs e)
        {
            this.Title = e.CurrentPixelPoint + "";
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var open = new Microsoft.Win32.OpenFileDialog();
            if (open.ShowDialog().Value)
            {
                this.pv.ImageSource = new BitmapImage(new Uri(open.FileName, UriKind.Absolute));
            }
        }


        private void AddShape(object sender, RoutedEventArgs e)
        {
            var point = this.pv.CurrentPoint;
            var shape = new Ellipse() { Fill = Brushes.Red, Width = 100, Height = 100 };
            shape.MouseDown += Shape_MouseDown;

            this.pv.AddShape(shape, point.X, point.Y);
            new DragMoveHelper().DragInit(shape, this.pv, shape.RenderTransform as TranslateTransform);
        }

        private void Shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                this.pv.ClearShape(sender as Shape);

        }
    }

    public class Window1ViewModel : BaseNotifyModel
    {
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.RegisterProperty(ref this.imageSource, value);
        }
        public Window1ViewModel()
        {
            this.ImageSource = new BitmapImage(new Uri(@"C:\Users\raozh\Pictures\Camera\Image_20210317142344246.bmp", UriKind.Absolute));
        }

        private bool startDrawRect;
        public bool StartDrawRect { get => this.startDrawRect; set => this.RegisterProperty(ref this.startDrawRect, value); }

        public ICommand DrawRectCommand
        {
            get => new SimpleCommand(DrawRectMethod);
        }

        private void DrawRectMethod()
        {
            this.StartDrawRect = true;
        }

        private int scaleValue;
        public int ScaleValue { get => this.scaleValue; set => this.RegisterProperty(ref this.scaleValue, value); }
    }
}
