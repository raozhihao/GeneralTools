using System;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;

namespace GeneralTool.CoreLibrary.SerialPortEx
{
    public static class SerialControlExtensions
    {
        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="serialControl"></param>
        /// <param name="keyword">关键字</param>
        /// <param name="data">数据</param>
        /// <param name="connectAction">连接函数</param>
        /// <param name="closeAction">关闭函数</param>
        /// <param name="validateAction">校验函数</param>
        /// <param name="log">日志对象</param>
        /// <returns></returns>
        public static byte[] SimpleBatchCommand(this SerialControl serialControl, byte keyword, byte[] data, Action connectAction = null, Action closeAction = null, Action<byte, byte[]> validateAction = null, ILog log = null)
        {
            if (log == null) log = new ConsoleLogInfo();

            connectAction?.Invoke();

            SerialRequest request = serialControl.CreateRequest();
            if (data == null || data.Length == 0) data = new byte[] { };
            _ = request.SetData(keyword, data);

            string sendDataStr = request.ToSendDatas().FomartDatas();
            log.Debug($"发送命令[{keyword} - {keyword}],发送命令数据:{sendDataStr}");

            SerialResponse reponse = serialControl.Send(request);

            byte[] recDatas = reponse.UserDatas;

            if (recDatas != null)
            {
                string reponseDataStr = recDatas.FomartDatas();
                log.Debug($"命令[{keyword} - {keyword}] 返回:{reponseDataStr}");
            }
            else
            {
                log.Debug($"命令[{keyword} - {keyword}] 返回:null");
            }

            try
            {
                ValidateResult(keyword, recDatas, validateAction);
            }
            finally
            {
                closeAction?.Invoke();
            }
            return recDatas;
        }


        public static void ValidateResult(byte keyword, byte[] result, Action<byte, byte[]> validateAction = null)
        {
            if (result == null)
                throw new Exception("没有接收到任何数据,请检查端口连接");

            if (result.Length == 0)
                throw new Exception("接收到空的数据");

            validateAction?.Invoke(keyword, result);
        }
    }
}
