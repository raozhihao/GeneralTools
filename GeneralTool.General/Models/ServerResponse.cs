using System;

using GeneralTool.General.Enums;
using GeneralTool.General.WPFHelper;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 服务返回
    /// </summary>
    [Serializable]
    public class ServerResponse
    {
        #region Public 属性

        /// <summary>
        /// </summary>
        public byte[] AttachDatas { get; set; }

        /// <summary>
        /// </summary>
        public int AttachDatasLength
        {
            get
            {
                if (this.AttachDatas != null)
                {
                    return this.AttachDatas.Length;
                }
                return 0;
            }
        }

        /// <summary>
        /// </summary>
        public AttachDataType AttachDataType { get; set; }

        /// <summary>
        /// </summary>
        public string ErroMsg { get; set; }

        /// <summary>
        /// </summary>
        public bool RequestSuccess { get; set; } = true;

        /// <summary>
        /// 请求url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public string ReturnTypeString { get; set; }

        /// <summary>
        /// 结果字符串
        /// </summary>
        public string ResultString { get; set; }

        /// <summary>
        /// 将结果转换为指定类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object ToResult(Type type)
        {
            return new StringConverter().Convert(this.ResultString, type, null, null);
        }

        /// <summary>
        /// 将结果转换为指定类型
        /// </summary>
        public T ToResult<T>() => (T)new StringConverter().Convert(this.ResultString, typeof(T), null, null);

        /// <summary>
        /// </summary>
        public RequestStateCode StateCode { get; set; }

        #endregion Public 属性
    }
}