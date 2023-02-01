using System;
using System.Windows;
using System.Windows.Media;

using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DragObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Type DragType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Brush BackGround { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Brush ForceGround { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double FontSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CanRepeatToCanvas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Visibility LeftVisibility { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Visibility TopVisibility { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Visibility RightVisibility { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Visibility BottomVisibility { get; set; }
        /// <summary>
        /// 可连接目标点数量(一个点可连接的目标点数量)
        /// </summary>
        public int SinkConnectorCount { get; set; }
        /// <summary>
        /// 指示是否由双击添加
        /// </summary>
        public bool IsDoubleClickAdd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Header { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsStart { get; set; }

        /// <summary>
        /// 被拖动的对象
        /// </summary>
        public BlockItem DragItem { get; internal set; }
    }
}
