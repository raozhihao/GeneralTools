using System.Windows;

namespace GeneralTool.CoreLibrary.WPFHelper.Events
{
    /// <summary>
    /// 事件宿主集合
    /// </summary>
    public class EventHostCollection : FreezableCollection<EventHost>
    {
        #region Protected 方法

        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore()
        {
            return new EventHostCollection();
        }

        /// <inheritdoc/>
        protected override bool FreezeCore(bool isChecking)
        {
            return !isChecking;
        }

        #endregion Protected 方法
    }
}