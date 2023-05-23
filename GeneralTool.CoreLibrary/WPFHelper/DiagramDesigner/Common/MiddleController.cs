using System;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class MiddleController
    {
        /// <summary>
        /// 
        /// </summary>
        public static MiddleController Controller { get; private set; }
        private static readonly Lazy<MiddleController> middle;
        static MiddleController()
        {
            middle = new Lazy<MiddleController>(() => new MiddleController());
            Controller = middle.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        public event Action<DragObject> OnDragBlockItemEvent;
        /// <summary>
        /// 
        /// </summary>
        public void DragBlockItemToCanvas(DragObject dragObject) => OnDragBlockItemEvent?.Invoke(dragObject);
    }
}
