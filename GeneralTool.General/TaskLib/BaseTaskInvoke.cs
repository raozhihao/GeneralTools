using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 任务基础类
    /// </summary>
    public class BaseTaskInvoke
    {
        /// <summary>
        /// 应当返回给外部socket调用服务错误的说明
        /// </summary>
        protected string erroMsg = "";

        /// <summary>
        /// 返回给外部socket调用的获取错误信息的方法
        /// </summary>
        /// <returns></returns>
        public string GetServerErroMsg()
        {
            string _errorMsg = erroMsg;
            erroMsg = "";
            return _errorMsg;
        }

        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log">日志组件</param>
        public BaseTaskInvoke(ILog log)
        {
            if (log == null)
                log = new ConsoleLogInfo();

            this.Log = log;
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseTaskInvoke() : this(null)
        {

        }
    }
}
