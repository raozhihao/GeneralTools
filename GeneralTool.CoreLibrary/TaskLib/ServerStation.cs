using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.SocketLib.Models;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerStation : ServerStationBase
    {

        #region Public 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonConvert"></param>
        /// <param name="log"></param>
        public ServerStation(IJsonConvert jsonConvert = null, ILog log = null) : base(log)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();

            SocketServer = new GenServerStation<ReceiveState>(jsonConvert, log, null);
        }

        #endregion Public 构造函数

        #region Public 属性
        /// <summary>
        /// 
        /// </summary>
        public GenServerStation<ReceiveState> SocketServer { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            _ = SocketServer.Close();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public override bool Start(string ip, int port)
        {
            return SocketServer.Start(ip, port);
        }

        #endregion Public 方法

    }
}