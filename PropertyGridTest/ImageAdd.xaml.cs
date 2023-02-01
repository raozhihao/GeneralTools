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

namespace PropertyGridTest
{
    /// <summary>
    /// ImageAdd.xaml 的交互逻辑
    /// </summary>
    public partial class ImageAdd : Window
    {
        public ImageAdd()
        {
            InitializeComponent();

            this.Loaded += ImageAdd_Loaded;
           
        }

        private void ImageAdd_Loaded(object sender, RoutedEventArgs e)
        {
            this.Test();
        }

        private void Test()
        {
            var path = new Path()
            {
                Stroke=Brushes.Red,
                StrokeThickness=3,
                Data = PathGeometry.Parse("M100 100L100 300")
            };
            var p1= this.ImageControl.TranslateToPixelPoint(new Point(100,100));
            this.ImageControl.AddElement(path);
        }

        private void ImageControl_ImageMouseMoveEvent(GeneralTool.General.Models.ImageMouseEventArgs obj)
        {
            this.PosTxt.Text = $"{(int)obj.CurrentPixelPoint.X} - {(int)obj.CurrentPixelPoint.Y}";
        }
    }
}
