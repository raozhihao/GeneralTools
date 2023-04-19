﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

using GeneralTool.General.WPFHelper;

namespace LanguageSwitch
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.ViewModel = new MainViewModel();

            this.DataContext = ViewModel;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
            this.ViewModel.Loaded();

            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var b = 0;
                var a = 1 / b;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
