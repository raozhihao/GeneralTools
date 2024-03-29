﻿using System;
using System.IO;
#if NET6_0
using System.Text.Json;
#endif
using System.Xml.Serialization;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 序列化扩展类
    /// </summary>
    public static class SerializeExtensions
    {
        #region Public 方法

        /// <summary>
        /// 将字符串反序列化回对象
        /// </summary>
        /// <param name="jsonStr">
        /// </param>
        /// <returns>
        /// </returns>
        public static T DeserializeJsonToObject<T>(this string jsonStr)
        {
#if NET452
            System.Web.Script.Serialization.JavaScriptSerializer serialize = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue, RecursionLimit = int.MaxValue };
            return serialize.Deserialize<T>(jsonStr);
#else
            return JsonSerializer.Deserialize<T>(jsonStr);
#endif
        }

        /// <summary>
        /// 将字符串反序列化回对象
        /// </summary>
        /// <param name="jsonStr">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static object DeserializeJsonToObject(this string jsonStr, Type type)
        {
#if NET452
            System.Web.Script.Serialization.JavaScriptSerializer serialize = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue, RecursionLimit = int.MaxValue };
            return serialize.Deserialize(jsonStr, type);
#else
            return JsonSerializer.Deserialize(jsonStr, type);
#endif
        }

        /// <summary>
        /// 将字节数组转换回指定对象,使用该方法的前提是使用 <see cref="Serialize(object)"/> 方法转换的字节数组
        /// </summary>
        /// <typeparam name="T">
        /// 类型
        /// </typeparam>
        /// <param name="bytes">
        /// </param>
        /// <returns>
        /// </returns>
        public static T Desrialize<T>(this byte[] bytes)
        {
            return new SerializeHelpers().Desrialize<T>(bytes);
        }

        /// <summary>
        /// 将字节数组转换回指定对象,使用该方法的前提是使用 <see cref="Serialize(object)"/> 方法转换的字节数组
        /// </summary>
        /// <param name="bytes">
        /// </param>
        /// <returns>
        /// </returns>
        public static object Desrialize(this byte[] bytes)
        {
            return new SerializeHelpers().Desrialize(bytes);
        }

        /// <summary>
        /// 将字节数组写入到文件中
        /// </summary>
        /// <param name="bytes">
        /// </param>
        /// <param name="savePath">
        /// </param>
        public static void SaveToPath(this byte[] bytes, string savePath)
        {
            System.IO.File.WriteAllBytes(savePath, bytes);
        }

        /// <summary>
        /// 序列化对象为字节数组,使用该方法后需要使用对应 <see cref="Desrialize{T}(byte[])"/> 反序列化回原始对象
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] Serialize(this object obj)
        {
            return new SerializeHelpers().Serialize(obj);
        }

        /// <summary>
        /// 将对象序列化为字符串
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public static string SerializeToJsonString(this object obj)
        {
#if NET452
            System.Web.Script.Serialization.JavaScriptSerializer serialize = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = int.MaxValue, RecursionLimit = int.MaxValue };
            return serialize.Serialize(obj);
#else
            return JsonSerializer.Serialize(obj);
#endif
        }

        /// <summary>
        /// 将对象序列化后写入到指定文件中
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <param name="savePath">
        /// </param>
        public static void SerializeToPath(this object obj, string savePath)
        {
            System.IO.File.WriteAllBytes(savePath, obj.Serialize());
        }

        /// <summary>
        /// XmlSerializer反序列化
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="serializer">
        /// </param>
        /// <param name="stream">
        /// </param>
        /// <returns>
        /// </returns>
        public static T XMlDeserialize<T>(this XmlSerializer serializer, Stream stream)
        {
            return (T)serializer.Deserialize(stream);
        }

        #endregion Public 方法
    }
}