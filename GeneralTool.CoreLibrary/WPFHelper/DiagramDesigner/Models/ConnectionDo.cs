using System;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models
{
    /// <summary>
    /// 连接线
    /// </summary>
    public class ConnectionDo
    {
        /// <summary>
        /// 
        /// </summary>
        public string ScriptId { get; set; }
        /// <summary>
        /// 源方向
        /// </summary>
        public Direction SourceDirection { get; set; }
        /// <summary>
        /// 目标方向
        /// </summary>
        public Direction SinkDirection { get; set; }
        /// <summary>
        /// 源ID
        /// </summary>
        public Guid SourceBlockId { get; set; }
        /// <summary>
        /// 目标ID
        /// </summary>
        public Guid SinkBlockId { get; set; }
    }
}
