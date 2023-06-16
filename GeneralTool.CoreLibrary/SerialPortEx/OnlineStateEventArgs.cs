using System;

namespace GeneralTool.CoreLibrary.SerialPortEx
{
    /// <summary>
    /// 串口在线事件附加信息
    /// </summary>
    public class OnlineStateEventArgs : EventArgs
    {
        /// <summary>
        /// 在线标志
        /// </summary>
        public OnlineState OnlineState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlineState"></param>
        public OnlineStateEventArgs(OnlineState onlineState)
        {
            OnlineState = onlineState;
        }
    }

    /// <summary>
    /// 在线枚举
    /// </summary>
    public enum OnlineState
    {
        /// <summary>
        /// 在线
        /// </summary>
        Online,
        /// <summary>
        /// 不在线
        /// </summary>
        Unline
    }
}
