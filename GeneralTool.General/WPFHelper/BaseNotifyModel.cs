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
        /// <summary>
        /// 该事件在更改组件上的属性时触发
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 向组件注册属性更改
        /// <example>
        /// <code>
        /// <para>private int _id;</para> 
        /// <para>public int Id</para> 
        /// <para>{</para> 
        /// <para>    get => this.id;</para> 
        /// <para>    set => RegisterProperty(ref this.id, value);</para> 
        /// <para>}</para> 
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">注册的属性类型</typeparam>
        /// <param name="fieldValue">注册的属性的字段</param>
        /// <param name="value">注册的属性的实际值</param>
        /// <param name="propertyName">注册的属性的名称(可忽略不指定,默认生成)</param>

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

            fieldValue = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
