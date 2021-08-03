using System;

namespace GeneralTool.General.Interfaces
{
    /// <summary>
    /// Json转换接口
    /// </summary>
    public interface IJsonConvert
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
        T DeserializeObject<T>(string value);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        object DeserializeObject(string value, Type type);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serverResponse">
        /// </param>
        /// <returns>
        /// </returns>
        string SerializeObject(object serverResponse);

        #endregion Public 方法
    }
}