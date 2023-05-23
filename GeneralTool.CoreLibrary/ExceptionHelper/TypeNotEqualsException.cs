using System;

namespace GeneralTool.CoreLibrary.ExceptionHelper
{
    /// <summary>
    /// 类型不一致异常
    /// </summary>
    public class TypeNotEqualsException : Exception
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        public TypeNotEqualsException(string message) : base(message)
        {
        }

        #endregion Public 构造函数
    }
}