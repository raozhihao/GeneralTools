namespace GeneralTool.General.SerialPortEx
{
    /// <summary>
    /// 返回类
    /// </summary>
    public class SerialResponse
    {
        /// <summary>
        /// 请求
        /// </summary>
        public SerialRequest Request
        {
            get;
            private set;
        }

        /// <summary>
        /// 源数据
        /// </summary>
        public byte[] SourceDatas
        {
            get;
            private set;
        }

        /// <summary>
        /// 用户数据
        /// </summary>
        public byte[] UserDatas
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="datas"></param>
        /// <param name="userDatas"></param>
        public SerialResponse(SerialRequest request, byte[] datas, byte[] userDatas = null)
        {
            Request = request;
            SourceDatas = datas;
            UserDatas = userDatas;
        }
    }
}
