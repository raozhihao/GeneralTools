namespace GeneralTool.General.SocketLib.Models
{
    /// <summary>
    /// 固定头部长度数据包
    /// </summary>
    public class FixedHeadRecevieState : ReceiveState
    {
        /// <summary>
        /// 接收到的数据长度
        /// </summary>
        public int ReceiveNeedLen { get; set; }

    }
}
