using System;

namespace GeneralTool.CoreLibrary.ExceptionHelper
{
    /// <summary>
    /// </summary>
    public class SerializeException : Exception
    {
        #region Public 构造函数

        ///<inheritdoc/>
        public SerializeException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public SerializeException(string message, Exception innerException) : base(message + ",详情请查看内部信息", innerException)
        {
        }

        #endregion Public 构造函数
    }
}