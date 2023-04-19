using System;

namespace GeneralTool.General.Extensions
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class ObjExtension
    {
        /// <summary>
        /// 转为Int32
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt32(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryToInt32(this object obj, out int result) => int.TryParse(obj.ToString(), out result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj) => Convert.ToDouble(obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryToDouble(this object obj, out double result) => double.TryParse(obj.ToString(), out result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float ToFloat(this object obj) => Convert.ToSingle(obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryToFloat(this object obj, out float result) => float.TryParse(obj.ToString(), out result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBool(this object obj) => Convert.ToBoolean(obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryToBool(this object obj, out bool result) => bool.TryParse(obj.ToString(), out result);


    }
}
