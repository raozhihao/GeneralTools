﻿using System;

using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Models
{
    /// <summary>
    /// 复制属性集
    /// </summary>
    public class BlockCopyArgs : EventArgs
    {
        /// <summary>
        /// 指示是否进行下一步的处理,如果为true则不进行下一步
        /// </summary>
        public bool Block { get; set; }

        /// <summary>
        /// 当前被复制的对象
        /// </summary>
        public DragObject DragItem { get; internal set; }

        /// <summary>
        /// 生成的目标块
        /// </summary>
        public BlockItem DestBlock { get; internal set; }

        /// <summary>
        /// 是否发送接下来的事件
        /// </summary>
        public bool OnCreateToCanvas { get; set; } = true;
    }
}
