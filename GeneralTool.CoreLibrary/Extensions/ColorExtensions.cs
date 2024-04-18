using System;

namespace GeneralTool.CoreLibrary.Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color ToDrawingColor(this string color)
        {
            return (System.Drawing.Color)new System.Drawing.ColorConverter().ConvertFromString(color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color ToWindowColor(this string color)
        {
            return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color ToDrawingColor(System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color ToWindowColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToHexString(this System.Drawing.Color color)
        {
            return ToHexString(color.R, color.G, color.B);
        }

        public static string ToHexString(this System.Windows.Media.Color color)
        {
            return ToHexString(color.R, color.G, color.B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToHexString(byte r, byte g, byte b)
        {
            string R = Convert.ToString(r, 16);
            if (R == "0")
                R = "00";
            string G = Convert.ToString(g, 16);
            if (G == "0")
                G = "00";
            string B = Convert.ToString(b, 16);
            if (B == "0")
                B = "00";
            string HexColor = "#" + R + G + B;
            return HexColor.ToUpper();
        }

       
    }
}
