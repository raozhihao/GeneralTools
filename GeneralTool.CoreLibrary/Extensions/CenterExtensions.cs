namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 求解中心点
    /// </summary>
    public static class CenterExtensions
    {
        /// <summary>
        /// 获取中心点
        /// </summary>
        public static System.Windows.Point Center(double x, double y, double width, double height)
            => new System.Windows.Point(x + width / 2, y + height / 2);

        /// <summary>
        /// 获取中心点
        /// </summary>
        public static System.Windows.Point Center(this System.Windows.Rect rect)
            => Center(rect.X, rect.Y, rect.Width, rect.Height);


        /// <summary>
        /// 获取中心点
        /// </summary>
        public static System.Windows.Point Center(this System.Windows.Int32Rect rect)
            => Center(rect.X, rect.Y, rect.Width, rect.Height);

        /// <summary>
        /// 获取中心点
        /// </summary>
        public static System.Windows.Point Center(this System.Drawing.Rectangle rect)
            => Center(rect.X, rect.Y, rect.Width, rect.Height);

        /// <summary>
        /// 获取中心点
        /// </summary>
        public static System.Windows.Point Center(this System.Drawing.RectangleF rect)
            => Center(rect.X, rect.Y, rect.Width, rect.Height);

    }
}
