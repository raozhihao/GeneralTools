using System;
using System.Collections.Concurrent;
using System.Windows;

namespace GeneralTool.CoreLibrary.WPFHelper
{
    /// <summary>
    /// 窗体注册管理
    /// </summary>
    public class WpfWindowManager
    {
        private static readonly Lazy<WpfWindowManager> instance;
        /// <summary>
        /// 窗体注册管理实例
        /// </summary>
        public static WpfWindowManager Instance { get; private set; }

        static WpfWindowManager()
        {
            instance = new Lazy<WpfWindowManager>(() => new WpfWindowManager());
            Instance = instance.Value;
        }

        private readonly ConcurrentDictionary<string, Window> windows = new ConcurrentDictionary<string, Window>();

        /// <summary>
        /// 注册窗体信息
        /// </summary>
        /// <param name="key">给予窗体的唯一key</param>
        /// <param name="windowType">窗体类型</param>
        public void RegisterWindow(string key, Type windowType)
        {
            var windowInstance = Activator.CreateInstance(windowType) as Window;
            this.windows.TryAdd(key, windowInstance);
        }

        /// <summary>
        /// 注册窗体
        /// </summary>
        /// <param name="key"></param>
        /// <param name="instanceWindow"></param>
        public void RegisterWindow(string key, Window instanceWindow)
        {
            this.windows.TryAdd(key, instanceWindow);
        }

        private void NoResultWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sender is Window w)
            {
                if (w.DataContext != null)
                {
                    var obj = w.DataContext;
                    var disposeMethod = obj.GetType().GetMethod("Dispose");
                    disposeMethod?.Invoke(obj, null);
                }
            }
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <typeparam name="T">窗体的DataContext类型</typeparam>
        /// <param name="key">要打开窗体的唯一key</param>
        /// <param name="context">窗体的DataContext</param>
        public void ShowWindow<T>(string key, T context) where T : class
        {
            if (this.windows.TryGetValue(key, out var window))
            {
                this.RegisterClosing(window, context);
                window.Show();
            }
        }

        /// <summary>
        /// 以模态显示窗体
        /// </summary>
        /// <typeparam name="T">窗体的DataContext类型</typeparam>
        /// <param name="key">要打开窗体的唯一key</param>
        /// <param name="context">窗体的DataContext</param>
        /// <returns></returns>
        public bool? ShowWindowDialog<T>(string key, T context) where T : class
        {
            if (this.windows.TryGetValue(key, out var window))
            {
                this.RegisterClosing(window, context);
                return window.ShowDialog();
            }
            return null;
        }

        private void RegisterClosing(Window window, object context)
        {
            window.DataContext = context;
            var @event = window.GetType().GetEvent("Closing");
            if (@event != null)
            {
                var method = this.GetType().GetMethod(nameof(NoResultWindow_Closing), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var @delegate = Delegate.CreateDelegate(@event.EventHandlerType, this, method);
                @event.AddEventHandler(window, @delegate);
            }
        }
    }
}
