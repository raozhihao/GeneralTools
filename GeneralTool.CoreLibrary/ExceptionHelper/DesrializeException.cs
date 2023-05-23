using System;

namespace GeneralTool.CoreLibrary.ExceptionHelper
{
    /// <summary>
    /// </summary>
    public class DesrializeException : Exception
    {
        #region Public 构造函数

        ///<inheritdoc/>
        public DesrializeException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public DesrializeException(string message, Exception innerException) : base(message + ",详情请查看内部信息", innerException)
        {
        }

        #endregion Public 构造函数
    }
}