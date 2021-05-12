using System;
using System.Windows.Input;

namespace GeneralTool.General.WPFHelper.Events
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEventCommand : ICommand
    {
        /// <summary>
        /// 设置引发事件的源对象
        /// </summary>
        /// <param name="d">引发事件的源对象</param>
        void SetObject(Object d);
        /// <summary>
        /// 设置或获取事件的名称
        /// </summary>
        string EventName { get; set; }
        /// <summary>
        /// 设置事件的自定义参数
        /// </summary>
        /// <param name="parameter"></param>
        void SetParameter(object parameter);
        /// <summary>
        /// 获取事件的委托
        /// </summary>
        EventHandler ActionEventHandler { get; }
    }
}
