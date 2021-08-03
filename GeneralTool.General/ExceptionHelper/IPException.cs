using System;

namespace GeneralTool.General.ExceptionHelper
{
    /// <summary>
    /// </summary>
    public class IPException : Exception
    {
        #region Public 字段

        /// <summary>
        /// </summary>
        public string ErrorIP = "";

        #endregion Public 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="errorip">
        /// </param>
        public IPException(string errorip) : base("ip错误，不是正确的ip")
        {
            this.ErrorIP = errorip;
        }

        #endregion Public 构造函数
    }
}