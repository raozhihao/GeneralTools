namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ArraryInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ArraryInfo()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public ArraryInfo(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Bottom = bottom;
            Right = right;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ColIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Bottom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return $"{(int)this.Left} - {(int)this.Top} - {(int)this.Right} - {(int)this.Bottom}";
        }
    }
}
