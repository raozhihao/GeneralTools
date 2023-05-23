
namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// Rect
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.Rectangle ToRectangle(double x, double y, double width, double height)
            => new System.Drawing.Rectangle(x.ToInt32(), y.ToInt32(), width.ToInt32(), height.ToInt32());


        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.RectangleF ToRectangleF(double x, double y, double width, double height)
            => new System.Drawing.RectangleF(x.ToFloat(), y.ToFloat(), width.ToFloat(), height.ToFloat());

        /// <summary>
        /// 
        /// </summary>
        public static System.Windows.Rect ToRect(double x, double y, double width, double height)
            => new System.Windows.Rect(x, y, width, height);


        /// <summary>
        /// 
        /// </summary>
        public static System.Windows.Int32Rect ToInt32Rect(double x, double y, double width, double height)
            => new System.Windows.Int32Rect(x.ToInt32(), y.ToInt32(), width.ToInt32(), height.ToInt32());

        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.Rectangle ToRectangle(this System.Drawing.RectangleF rectangleF)
            => ToRectangle(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);

        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.Rectangle ToRectangle(this System.Windows.Rect rect)
            => ToRectangle(rect.X, rect.Y, rect.Width, rect.Height);

        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.Rectangle ToRectangle(this System.Windows.Int32Rect rect32)
            => ToRectangle(rect32.X, rect32.Y, rect32.Width, rect32.Height);


        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.RectangleF ToRectangleF(this System.Drawing.Rectangle rectangleF)
            => ToRectangle(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);

        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.RectangleF ToRectangleF(this System.Windows.Rect rect)
            => ToRectangle(rect.X, rect.Y, rect.Width, rect.Height);

        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.RectangleF ToRectangleF(this System.Windows.Int32Rect rect32)
            => ToRectangle(rect32.X, rect32.Y, rect32.Width, rect32.Height);


        /// <summary>
        /// 
        /// </summary>
        public static System.Windows.Rect ToRect(this System.Drawing.Rectangle rectangle)
            => ToRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        /// <summary>
        /// 
        /// </summary>
        public static System.Windows.Rect ToRect(this System.Drawing.RectangleF rectangleF)
            => ToRect(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);

        /// <summary>
        /// 
        /// </summary>
        public static System.Windows.Rect ToRect(this System.Windows.Int32Rect rect32)
            => ToRect(rect32.X, rect32.Y, rect32.Width, rect32.Height);


        /// <summary>
        /// 
        /// </summary>
        public static System.Windows.Int32Rect ToInt32Rect(this System.Drawing.Rectangle rectangle)
            => ToInt32Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        /// <summary>
        /// 
        /// </summary>
        public static System.Windows.Int32Rect ToInt32Rect(this System.Drawing.RectangleF rectangleF)
            => ToInt32Rect(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);

        /// <summary>
        /// 
        /// </summary>
        public static System.Windows.Int32Rect ToInt32Rect(this System.Windows.Rect rect)
            => ToInt32Rect(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
