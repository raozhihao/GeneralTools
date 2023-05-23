using System.Threading;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// 任务基础类
    /// </summary>
    [System.Serializable]
    public class BaseTaskInvoke : BaseNotifyModel
    {
        #region Protected 字段

        /// <summary>
        /// 应当返回给外部socket调用服务错误的说明
        /// </summary>
        protected string erroMsg = "";

        #endregion Protected 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="log">
        /// 日志组件
        /// </param>
        public BaseTaskInvoke(ILog log)
        {
            if (log == null)
                log = new ConsoleLogInfo();

            this.Log = log;
        }

        /// <summary>
        /// </summary>
        public BaseTaskInvoke()
        {
            if (this.Log == null)
                this.Log = new ConsoleLogInfo();
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 返回给外部socket调用的获取错误信息的方法
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetServerErroMsg()
        {
            string _errorMsg = erroMsg;
            erroMsg = "";
            return _errorMsg;
        }

        #endregion Public 方法
    }
}