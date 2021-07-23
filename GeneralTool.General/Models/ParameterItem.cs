﻿using GeneralTool.General.WPFHelper;
using System;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 参数信息
    /// </summary>
    [Serializable]
    public class ParameterItem : BaseNotifyModel
    {
        private string parameterName = "";

        private object value = "";

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName
        {
            get
            {
                return parameterName;
            }
            set
            {
                this.RegisterProperty(ref this.parameterName, value);
            }
        }

        /// <summary>
        /// 值更改事件
        /// </summary>
        public event Action ValueChanged;
        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.RegisterProperty(ref this.value, value);
                this.ValueChanged?.Invoke();
            }
        }

        private string waterMark;
        /// <summary>
        /// 水印信息
        /// </summary>
        public string WaterMark
        {
            get => this.waterMark;
            set => this.RegisterProperty(ref this.waterMark, value);
        }

        private Type parameterType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public ParameterItem(string parameterName, object value)
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public ParameterItem()
        {

        }

        /// <summary>
        /// 参数类型
        /// </summary>
        public Type ParameterType
        {
            get => this.parameterType;
            set => this.RegisterProperty(ref this.parameterType, value);
        }
    }
}