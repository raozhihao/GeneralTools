using System;

using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 参数信息
    /// </summary>
    [Serializable]
    public class ParameterItem : BaseNotifyModel
    {
        #region Private 字段

        private string parameterName = "";

        private Type parameterType;
        private object value = "";

        private string waterMark;
        private int index;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="parameterName">
        /// </param>
        /// <param name="value">
        /// </param>
        public ParameterItem(string parameterName, object value)
        {
            ParameterName = parameterName;
            Value = value;
        }

        /// <summary>
        /// </summary>
        public ParameterItem()
        {
        }

        #endregion Public 构造函数

        #region Public 事件

        /// <summary>
        /// 值更改事件
        /// </summary>
        public event Action ValueChanged;

        #endregion Public 事件

        #region Public 属性

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
                RegisterProperty(ref parameterName, value);
            }
        }

        /// <summary>
        /// 参数类型
        /// </summary>
        public Type ParameterType
        {
            get => parameterType;
            set => RegisterProperty(ref parameterType, value);
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
                RegisterProperty(ref this.value, value);
                ValueChanged?.Invoke();
            }
        }

        /// <summary>
        /// 水印信息
        /// </summary>
        public string WaterMark
        {
            get => waterMark;
            set => RegisterProperty(ref waterMark, value);
        }

        /// <summary>
        /// 坐标
        /// </summary>
        public int Index
        {
            get => index;
            set => RegisterProperty(ref index, value);
        }

        #endregion Public 属性
    }
}