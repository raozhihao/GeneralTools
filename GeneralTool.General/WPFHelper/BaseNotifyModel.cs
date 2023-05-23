using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GeneralTool.General.WPFHelper
{
    /// <summary>
    /// 用作WPF属性通知模型类的基类
    /// </summary>
    [System.Serializable]
    public abstract class BaseNotifyModel : INotifyPropertyChanged
    {
        #region Public 事件

        /// <summary>
        /// 该事件在更改组件上的属性时触发
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性正在更新中事件
        /// </summary>
        [field: NonSerialized]
        public event Func<PropertyChangArg, object> PropertyChaningEvent;

        #endregion Public 事件

        #region Public 方法

        /// <summary>
        /// 向组件注册属性更改 <example>
        /// <code><para>private int _id;</para><para>public int Id</para><para>{</para><para>    get =&gt; this.id;</para><para>    set =&gt; RegisterProperty(ref this.id, value);</para><para>}</para></code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">
        /// 注册的属性类型
        /// </typeparam>
        /// <param name="fieldValue">
        /// 注册的属性的字段
        /// </param>
        /// <param name="value">
        /// 注册的属性的实际值
        /// </param>
        /// <param name="propertyName">
        /// 注册的属性的名称(可忽略不指定,默认生成)
        /// </param>
        public void RegisterProperty<T>(ref T fieldValue, T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                fieldValue = value;
                return;
            }

            if (fieldValue != null && fieldValue.Equals(value))
            {
                return;
            }

            if (this.PropertyChaningEvent != null)
            {
                value = (T)this.PropertyChaningEvent(new PropertyChangArg(fieldValue, value, propertyName));
            }

            if (fieldValue != null && fieldValue.Equals(value))
            {
                return;
            }

            fieldValue = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Public 方法


    }



    /// <summary>
    /// 属性更新信息
    /// </summary>
    public class PropertyChangArg : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        public PropertyChangArg(object oldValue, object newValue, string propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// 值
        /// </summary>
        public object OldValue { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object NewValue { get; set; }
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }
    }
}