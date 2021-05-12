using System.Windows;

namespace GeneralTool.General.WPFHelper.Events
{
    /// <summary>
    /// 事件宿主集合
    /// </summary>
    public class EventHostCollection : FreezableCollection<EventHost>
    {

        /// <inheritdoc/>
        protected override bool FreezeCore(bool isChecking)
        {
            return !isChecking;
        }

        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore()
        {
            return new EventHostCollection();
        }


    }
}
