using System;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 单例模式,也适用于WinFrm与WPF
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SingleInstance<T>
    {
        private static Lazy<T> instance;

        private static readonly object locker = new object();

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="createFunc">创建对象实例委托,如果为null,需要先使用 <see cref="SetInstanceFunc(Func{T})"/>进行实例先注入,如果忆设置且此值不为null则可能会创建新实例</param>
        /// <param name="createdOperationAction">创建完成后所需要做的事情</param>
        /// <returns></returns>
        public static T GetInstance(Func<T> createFunc = null, Action<T> createdOperationAction = null)
        {
            lock (locker)
            {
                if (instance == null)
                {
                    if (createFunc != null)
                        instance = new Lazy<T>(createFunc);
                    else
                        instance = new Lazy<T>(() => (T)Activator.CreateInstance(typeof(T)));
                    var type = typeof(T);
                    if (type == typeof(System.Windows.Forms.Form))
                    {
                        var closedEvent = type.GetEvent(nameof(System.Windows.Forms.Form.FormClosed));
                        closedEvent.AddEventHandler(instance.Value, new System.Windows.Forms.FormClosedEventHandler((o, e) => instance = null));
                    }
                    else if (type == typeof(System.Windows.Window))
                    {
                        var closedEvent = type.GetEvent(nameof(System.Windows.Window.Closed));
                        closedEvent.AddEventHandler(instance.Value, new EventHandler((o, e) => instance = null));
                    }
                    createdOperationAction?.Invoke(instance.Value);
                }
            }

            return instance.Value;
        }

        /// <summary>
        /// 设置实例
        /// </summary>
        /// <param name="createFunc"></param>
        public static void SetInstanceFunc(Func<T> createFunc) => instance = new Lazy<T>(createFunc);

        /// <summary>
        /// 获取实例 需要先使用 <see cref="SetInstanceFunc(Func{T})"/> 进入注入,如无注入,则调用其无参构造函数
        /// </summary>
        public static T Instance => GetInstance();
    }
}
