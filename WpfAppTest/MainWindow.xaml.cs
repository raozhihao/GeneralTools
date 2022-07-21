using GeneralTool.General.Attributes;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.WPFHelper.UIEditorConverts;
using System.Windows;
using System.Windows.Media;

namespace WpfAppTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            this.DataContext = new VIewModels.MainViewModel();
            this.pro.SelectedObject = new Person();
            this.Loaded += this.MainWindow_Loaded;
             //this.pro.SelectedObject = this.btn;
            //this.bb.AddHandler(Button.MouseDownEvent,new MouseButtonEventHandler(this.Button_MouseDown),true);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
          //  this.pro.SelectedObject = this.btn;

        }

        private void Button_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void ImageViewControl_CutPanelVisibleChanged(object sender, GeneralTool.General.Models.ImageCutRectEventArgs e)
        {

        }
    }

    public class Person
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string MyProperty { get;  }
        //public double Double { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public ILog Log { get; set; } = new ConsoleLogInfo();
        public Brush Brush { get; set; } = Brushes.Black;

        public FontStyle FontStyle { get; set; } = FontStyles.Normal;
        //public Gender Gender { get; set; }

        //[System.ComponentModel.ReadOnly(true)]
        //public bool Boolean { get; set; }

        //[UIEditor(typeof(ObjectExpandeUIEditor))]
        //public Point Point { get; set; }
    }

    public enum Gender
    {
        Body, Gril
    }
}
