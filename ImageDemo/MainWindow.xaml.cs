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
using System.Windows.Navigation;
using System.Windows.Shapes;

using GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes;

using Microsoft.Win32;

namespace ImageDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private BaseShape shape;
        private void LoadImageMethod(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog();
            if (open.ShowDialog().Value)
            {
                this.ImageControl.Bitmap = new System.Drawing.Bitmap(open.FileName);
            }
        }

        private void AddRectMethod(object sender, RoutedEventArgs e)
        {
            this.shape = new RectShape( new Rect(100, 100, 100, 100));
            
            this.shape.Path.Stroke = Brushes.Red;
            this.shape.Path.StrokeThickness = 1;
            this.ImageControl.AddCustomeShape(this.shape);
        }

        private void GetRectMethod(object sender, RoutedEventArgs e)
        {
            var points = this.shape.PixelPoints;
        }
    }
}
