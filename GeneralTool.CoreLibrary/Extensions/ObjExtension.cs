using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Documents;

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

        private static int count = 1;
        private static List<object> cacheObjs = new List<object>();
        /// <summary>
        /// 深克隆,可以无视循环引用,但对于设置属性时属性设置方法中内部抛异常无能为力
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object CopyDeep(this object obj)
        {
            count++;
            if (obj == null) return null;

            var type = obj.GetType();
            var properties = type.GetProperties();

            var instance = FormatterServices.GetUninitializedObject(type);

            if (properties.Length == 0)
            {
                instance = obj;
            }
            else
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var propery = properties[i];
                    if (propery.SetMethod != null && propery.GetMethod != null)
                    {
                        var proObj = propery.GetMethod.Invoke(obj, null);

                        if (cacheObjs.Contains(proObj))
                        {
                            propery.SetValue(instance, proObj);
                            continue;
                        }
                        if (proObj != null)
                        {
                            cacheObjs.Add(proObj);
                            proObj = proObj.CopyDeep();

                        }

                        propery.SetValue(instance, proObj);
                    }
                }

            }

            count--;
            if (count == 1) cacheObjs.Clear();
            return instance;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object To<T>(this object obj) where T : IConvertible
        {
            return Convert.ChangeType(obj, typeof(T));
        }
    }
}
