using System;
using System.Linq;

namespace GeneralTool.CoreLibrary.Extensions
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
        /// <returns></returns>
        public static decimal ToDecimal(this object obj) => Convert.ToDecimal(obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryToDecimal(this object obj, out decimal result) => decimal.TryParse(obj + "", out result);

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

        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="consturctorInvoke">是否执行构造函数,只会执行无参构造函数</param>
        /// <returns></returns>
        public static T Copy<T>(this T obj, bool consturctorInvoke = true)
        {
            return obj == null ? default : (T)obj.CopyObject(consturctorInvoke);
        }

        /// <summary>
        /// 将右值复制到左值中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Copy<T>(this T left, T right)
        {
            if (left == null || right == null) return;
            Type type = right.GetType();
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                System.Reflection.PropertyInfo property = properties[i];
                if (property.GetMethod != null && property.SetMethod != null)
                {
                    object value = property.GetMethod.Invoke(right, null);
                    property.SetValue(left, value);
                }
            }

        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="consturctorInvoke">是否先执行构造函数,只会执行无参构造函数</param>
        public static object CopyObject(this object obj, bool consturctorInvoke = true)
        {
            if (obj == null) return obj;
            object objInstance = obj.Serialize().Desrialize();
            //查看构造函数,找无参构造函数
            System.Reflection.ConstructorInfo constructor = objInstance.GetType().GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);

            if (constructor != null && !consturctorInvoke)
                _ = constructor.Invoke(objInstance, null);

            return objInstance;
        }
    }
}
