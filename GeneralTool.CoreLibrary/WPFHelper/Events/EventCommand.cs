using System;
using System.Windows.Input;

namespace GeneralTool.CoreLibrary.WPFHelper.Events
{
    /// <summary>
    /// 事件命令
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class EventCommand<T> : IEventCommand where T : EventArgs
    {
        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="action">
        /// 事件委托
        /// </param>
        public EventCommand(EventHandler<T> action)
        {
            InvokeAction = action;
        }

        #endregion Public 构造函数

        #region Public 事件

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion Public 事件

        #region Public 属性

        /// <summary>
        /// 事件的委托包装
        /// </summary>
        public EventHandler ActionEventHandler
        {
            get
            {
                return new EventHandler((o, s) => { InvokeAction?.Invoke(o, (T)s); });
            }
        }

        /// <summary>
        /// 指示是否处理事件
        /// </summary>
        public Func<T, bool> CanExecuteDelegate { get; set; }

        /// <summary>
        /// 自定义事件参数
        /// </summary>
        public object CommandParameter { get; private set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 事件处理委托
        /// </summary>
        public EventHandler<T> InvokeAction { get; set; }

        /// <summary>
        /// 事件源
        /// </summary>
        public object Source { get; private set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 是否能处理
        /// </summary>
        /// <param name="parameter">
        /// </param>
        /// <returns>
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate?.Invoke((T)parameter) ?? true;
        }

        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="parameter">
        /// </param>
        public void Execute(object parameter)
        {
            InvokeAction?.Invoke(Source, (T)parameter);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public void RemoveEvent()
        {
            if (Source == null)
                throw new ArgumentException($"事件的源 {Source} 没有找到");

            if (string.IsNullOrWhiteSpace(EventName))
                throw new ArgumentException($"事件源 {Source} 的事件名称 {EventName} 没有找到");

            System.Reflection.EventInfo @event = Source.GetType().GetEvent(EventName) ?? throw new ArgumentException($"在源 {Source} 中没有找到事件 {EventName}");
            EventHandler action = ActionEventHandler;
            Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, action.Target, action.Method);
            @event.RemoveEventHandler(Source, handler);
        }

        /// <summary>
        /// 重新添加事件
        /// </summary>
        public void ResertAddEvent()
        {
            if (Source == null)
                throw new ArgumentException($"事件的源 {Source} 没有找到");

            if (string.IsNullOrWhiteSpace(EventName))
                throw new ArgumentException($"事件源 {Source} 的事件名称 {EventName} 没有找到");

            System.Reflection.EventInfo @event = Source.GetType().GetEvent(EventName) ?? throw new ArgumentException($"在源 {Source} 中没有找到事件 {EventName}");
            EventHandler action = ActionEventHandler;
            Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, action.Target, action.Method);
            @event.AddEventHandler(Source, handler);
        }

        ///<inheritdoc/>
        public void SetObject(object d)
        {
            Source = d;
        }

        void IEventCommand.SetParameter(object parameter) => CommandParameter = parameter;

        #endregion Public 方法
    }
}