using GeneralTool.General.WPFHelper;
using System;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 参数信息
    /// </summary>
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
                this.value = value;
                this.RegisterProperty(ref this.value, value);
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
        /// 参数类型
        /// </summary>
        public Type ParameterType
        {
            get => this.parameterType;
            set => this.RegisterProperty(ref this.parameterType, value);
        }
    }
}
