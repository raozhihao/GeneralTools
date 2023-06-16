using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using MahApps.Metro.Controls;

using SocketApplication.Windows;

namespace SocketApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
        }

        private void CreateServerMethod(object sender, RoutedEventArgs e)
        {
            //_ = MetroTable.Items.Add(new MetroTabItem()
            //{
            //    Header = "Server" + MetroTable.Items.Count,
            //    Content = new TextBlock(new Run(Guid.NewGuid().ToString()))
            //});

            var createWindow = new CreateWindow("创建服务端")
            {
                DataContext = viewModel
            };
            var result= createWindow.ShowDialog();
            if (result.Value)
            {

            }
            
        }
    }
}
