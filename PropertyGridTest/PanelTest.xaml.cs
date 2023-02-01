using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// PanelTest.xaml 的交互逻辑
    /// </summary>
    public partial class PanelTest : Window
    {
        private PanelViewModel model = new PanelViewModel();
        public PanelTest()
        {
            InitializeComponent();
            this.DataContext = model;
            this.Loaded += PanelTest_Loaded;
        }

        private void PanelTest_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private int index = 0;
        private void Add(object sender, RoutedEventArgs e)
        {
            this.model.Infos.Add(new KeyValuePair<int, string>(index++, Guid.NewGuid().ToString()));
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (this.model.Infos.Count != 0)
                this.model.Infos.RemoveAt(0);
        }
    }
}
