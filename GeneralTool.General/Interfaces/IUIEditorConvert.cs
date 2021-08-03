using System.Reflection;
using System.Windows.Controls;

namespace GeneralTool.General.Interfaces
{
    /// <summary>
    /// UI编辑器接口
    /// </summary>
    public interface IUIEditorConvert
    {
        #region Public 方法

        /// <summary>
        /// 将新的UI内容添加到 <paramref name="gridParent"/>
        /// </summary>
        /// <param name="gridParent">
        /// 上级Grid控件
        /// </param>
        /// <param name="instance">
        /// 需要编辑的对象
        /// </param>
        /// <param name="propertyInfo">
        /// 当前属性
        /// </param>
        /// <param name="sortAsc">
        /// 排序方式
        /// </param>
        /// <param name="Row">
        /// 当前应该处于上级Grid控件中的行号
        /// </param>
        /// <param name="header">
        /// 可选的添加Header
        /// </param>
        void ConvertTo(Grid gridParent, object instance, PropertyInfo propertyInfo, bool? sortAsc, ref int Row, string header = null);

        #endregion Public 方法
    }
}