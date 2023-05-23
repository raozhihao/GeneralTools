using System;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class BlockItemDo
    {
        /// <summary>
        /// 
        /// </summary>
        public string ScriptId { get; set; }
        /// <summary>
        /// 在画面中的位置
        /// </summary>
        public Point CanvasLocation { get; set; }
        /// <summary>
        /// 尺寸
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Size MinSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color BackGround { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color Foreground { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BlockAssmeblyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BlockTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Conent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CanRepeatToCanvas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataContextType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WindowDataContextType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Header { get; set; }
    }
}
