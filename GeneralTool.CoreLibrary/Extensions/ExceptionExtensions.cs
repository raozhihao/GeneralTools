
using System;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 异常信息扩展类
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Public 方法

        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <param name="exception">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetInnerExceptionMessage(this Exception exception)
        {
            return exception.InnerException != null
                ? exception.Message + Environment.NewLine + exception.InnerException.GetInnerExceptionMessage()
                : exception.Message;
        }

        #endregion Public 方法
    }
}