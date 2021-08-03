using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 语言属性模型
    /// </summary>
    public class PropertyLangStruct
    {
        /// <summary>
        /// 默认的值
        /// </summary>
        public object DefaultLabel { get; set; }

        /// <summary>
        /// 其父类控件对象
        /// </summary>
        public DependencyObject Dependency { get; set; }

        /// <summary>
        /// 语言Key
        /// </summary>
        public string LangKey { get; set; }

        /// <summary>
        /// 当前绑定的属性
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="value">值</param>
        public void SetValue(object value)=> this.PropertyInfo.SetValue(this.Dependency, value);

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <returns></returns>
        public object GetValue() => this.PropertyInfo.GetValue(Dependency);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="langStruct"></param>
        /// <returns></returns>
        public bool CompareTo(PropertyLangStruct langStruct)
        {
            return langStruct.Dependency.GetHashCode() == this.GetHashCode();
        }
    }

    /// <summary>
    /// 语言结构对比类
    /// </summary>
    public class LangStructEqualityComparer : IEqualityComparer<PropertyLangStruct>
    {
        /// <inheritdoc/>
        public bool Equals(PropertyLangStruct x, PropertyLangStruct y)
        {
            return x.Dependency.GetHashCode() == y.Dependency.GetHashCode();
        }
        /// <inheritdoc/>
        public int GetHashCode(PropertyLangStruct obj)
        {
            return obj.Dependency.GetHashCode();
        }
    }
}
