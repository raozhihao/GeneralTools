using System;

using GeneralTool.General.Extensions;
using GeneralTool.General.Interfaces;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 基础Json转换器
    /// </summary>
    public class BaseJsonCovert : IJsonConvert
    {
        #region Public 方法

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public T DeserializeObject<T>(string value)
        {
            return value.DeserializeJsonToObject<T>();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public object DeserializeObject(string value, Type type)
        {
            return value.DeserializeJsonToObject(type);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serverResponse">
        /// </param>
        /// <returns>
        /// </returns>
        public string SerializeObject(object serverResponse)
        {
            return serverResponse.SerializeToJsonString();
        }

        #endregion Public 方法
    }
}