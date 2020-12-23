using System;
using System.Windows.Input;

namespace GeneralTool.General.WPFHelper
{
    /// <summary>
    /// 简单命令
    /// </summary>
    public class SimpleCommand<T> : ICommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="canExecute">指示是否可以执行命令</param>
        /// <param name="execute">执行命令的委托</param>
        public SimpleCommand(Func<T, bool> canExecute = null, Action<T> execute = null)
        {
            CanExecuteDelegate = canExecute;
            ExecuteDelegate = execute;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="execute">执行命令的委托</param>
        public SimpleCommand(Action<T> execute) : this(null, execute)
        {

        }
        /// <summary>
        /// 是否可以执行事件
        /// </summary>
        public Func<T, bool> CanExecuteDelegate { get; set; }
        /// <summary>
        /// 执行事件
        /// </summary>
        public Action<T> ExecuteDelegate { get; set; }
        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            Func<T, bool> canExecute = CanExecuteDelegate;
            if (canExecute == null)
            {
                return true;
            }
            return canExecute((T)parameter);
        }
        /// <summary>
        /// 是否可以执行改变
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            ExecuteDelegate?.Invoke((T)parameter);
        }
    }

    /// <summary>
    /// 简单命令
    /// </summary>
    public class SimpleCommand : SimpleCommand<object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute"></param>
        public SimpleCommand(Action execute) : base(null, new Action<object>(o => execute())) { }
    }
}
