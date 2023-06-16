using System.Drawing;

namespace GeneralTool.CoreLibrary.MVS
{
    /// <summary>
    /// 相机信息
    /// </summary>
    public struct CameraRectangleInfo
    {
        /// <summary>
        /// 最大宽度
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// 宽度步幅
        /// </summary>
        public int WidthInc { get; set; }

        /// <summary>
        /// 最大高度
        /// </summary>
        public int MaxHeight { get; set; }

        /// <summary>
        /// 高度步幅
        /// </summary>
        public int HeightInc { get; set; }

        /// <summary>
        /// 当前画面宽度
        /// </summary>
        public int CurrentWidth { get; set; }

        /// <summary>
        /// 当前画面高度
        /// </summary>
        public int CurrentHeight { get; set; }

        /// <summary>
        /// 当前X裁剪坐标
        /// </summary>
        public int OffsetX { get; set; }

        /// <summary>
        /// X步幅
        /// </summary>
        public int OffXInc { get; set; }

        /// <summary>
        /// 当前Y裁剪坐标
        /// </summary>
        public int OffsetY { get; set; }

        /// <summary>
        /// Y步幅
        /// </summary>
        public int OffYInc { get; set; }

        /// <summary>
        /// 是否为空画幅
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return CurrentHeight == 0 && CurrentWidth == 0;
            }
        }

        internal Rectangle ToRectangle()
        {
            return new Rectangle(OffsetX, OffsetY, CurrentWidth, CurrentHeight);
        }
    }
}