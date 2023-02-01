using System;
using System.Collections;
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
using System.Windows.Shell;

using GeneralTool.General.Attributes;
using GeneralTool.General.Interfaces;
using GeneralTool.General.WPFHelper.UIEditorConverts;

using static System.Net.Mime.MediaTypeNames;

namespace PropertyGridTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var dic = new Dictionary<string, Attribute>();
            //dic.Add(typeof(TaskbarItemInfo).FullName, new System.ComponentModel.DescriptionAttribute("Task 测试"));
            //dic.Add(typeof(Test).FullName, new System.ComponentModel.DescriptionAttribute("Task 测试"));

            //var converts = new Dictionary<string, IUIEditorConvert>();
            //converts.Add(typeof(ImageSource).FullName, new IconUIConvert());
            //converts.Add(typeof(ICollection).FullName, new ICollectionUIConvert());
            //converts.Add(typeof(Test).FullName, new ObjectExpandeUIEditor());

            //this.PropertyGrid.SetConverts(converts);
            //this.PropertyGrid.SetAttributes(dic);
            this.PropertyGrid.SelectedObject = new Person();// new TestModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Gender Gender { get; set; }

        [UIEditorAttribute(typeof(ExpandeUIEditor))]
        public Boy Boy { get; set; }
    }

    public enum Gender
    {
        Boy, Girl
    }

   
    public class Boy
    {
        public int Als { get; set; }
    }

    public class TestModel
    {
        public int Id { get; set; }
        //public List<int> List { get; set; }
        public Test Test { get; set; }
    }

    public class Test
    {
        public int Id { get; set; }
    }
}
