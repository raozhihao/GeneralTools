﻿using GeneralTool.General.Enums;
using System;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 服务返回
    /// </summary>
    [Serializable]
    public class ServerResponse
    {
        /// <summary>
        /// 请求url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RequestStateCode StateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AttachDataType AttachDataType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] AttachDatas { get; set; }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public string ErroMsg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RequestSuccess { get; set; } = true;
    }
}