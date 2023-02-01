using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

using Microsoft.Win32;

namespace PropertyGridTest
{
    /// <summary>
    /// ImageSourceUIControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageSourceUIControl : UserControl
    {
        private PropertyInfo propertyInfo;
        private object instance;

        public ImageSourceUIControl(PropertyInfo propertyInfo, object instance)
        {
            InitializeComponent();
            this.propertyInfo = propertyInfo;
            this.instance = instance;
        }


        public string Text
        {
            get => this.txt.Text;
            set
            {
                this.txt.Text = value;
            }
        }

        public static readonly DependencyProperty IconImageProperty = DependencyProperty.Register(nameof(IconImage), typeof(ImageSource), typeof(ImageSourceUIControl));
        public ImageSource IconImage
        {
            get => this.GetValue(IconImageProperty) as ImageSource;
            set => this.SetValue(IconImageProperty, value);
        }

        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "图标|*.ico";
            if (open.ShowDialog().Value)
            {
                this.Text = open.FileName;
                var image = new BitmapImage(new Uri(open.FileName));
                this.IconImage = image;
            }
        }
    }
}
