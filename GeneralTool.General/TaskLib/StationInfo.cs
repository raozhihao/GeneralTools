namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 站点信息
    /// </summary>
    public class StationInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="station"></param>
        public StationInfo(string ip, int port, Station station)
        {
            Ip = ip;
            Port = port;
            Station = station;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Station Station { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSocketInit { get; set; }

    }
}
