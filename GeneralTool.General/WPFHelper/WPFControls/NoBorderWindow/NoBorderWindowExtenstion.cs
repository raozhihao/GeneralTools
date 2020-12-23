using System.Windows;
using System.Windows.Input;

namespace GeneralTool.General.WPFHelper.WPFControls.NoBorderWindow
{
    /// <summary>
    /// 无边框窗体行为注入,配合 WindowResources.xaml 样式使用
    /// </summary>
    public static class NoBorderWindowExtenstion
    {
        /// <summary>
        /// 设置窗体的行为,配合 WindowResources.xaml 样式使用
        /// 在App.xaml中加入
        /// <code>
        /// ResourceDictionary Source="pack://application:,,,/GeneralTool.Mvvm;component/WPFControls/XamlResources/WindowResources.xaml" 
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="window"></param>
        /// <returns></returns>
        public static T SetWindowBehaviour<T>(this T window) where T : Window
        {
            window.Loaded += Window_Loaded;
            return window;
        }

        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = sender as Window;
            window.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            window.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            window.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            window.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
        }

        private static void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            Window window = sender as Window;
            e.CanExecute = window.ResizeMode != ResizeMode.NoResize;
        }

        private static void OnMinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Window window = sender as Window;
            SystemCommands.MinimizeWindow(window);
        }

        private static void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            Window window = sender as Window;
            e.CanExecute = window.ResizeMode == ResizeMode.CanResize || window.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private static void OnMaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Window window = sender as Window;
            SystemCommands.MaximizeWindow(window);
        }

        private static void OnRestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Window window = sender as Window;
            SystemCommands.RestoreWindow(window);
        }

        private static void OnCloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Window window = sender as Window;
            SystemCommands.CloseWindow(window);
        }
    }
}
