using GeneralTool.General.Extensions;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// 特性扩展类
    /// </summary>
    public static class AttributeExtensions
    {
        #region Public 方法

        /// <summary>
        /// 获取对象上的特性
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public static T GetAttributeByClass<T>(this object value)
        {
            T t = default;
            object[] customAttributes = value.GetType().GetCustomAttributes(inherit: false);

            customAttributes.Foreach(a =>
            {
                if (a is T t1)
                {
                    t = t1;
                    return;
                }
            });

            return t;
        }

        #endregion Public 方法
    }
}