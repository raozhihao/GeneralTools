namespace GeneralTool.CoreLibrary.SerialPortEx
{
    /// <summary>
    /// 返回类
    /// </summary>
    public class SerialResponse
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <param name="datas">
        /// </param>
        /// <param name="userDatas">
        /// </param>
        public SerialResponse(SerialRequest request, byte[] datas, byte[] userDatas = null)
        {
            Request = request;
            SourceDatas = datas;
            UserDatas = userDatas;
        }

        #endregion Public 构造函数

        #region Public 属性

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

        #endregion Public 属性
    }
}