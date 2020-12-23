namespace GeneralTool.General.Models
{
    /// <summary>
    /// 返回Http服务器消息对象
    /// </summary>
    [System.Serializable]
    public class ResponseCommand
    {
        /// <summary>
        /// 方法是否调用成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 方法返回错误信息 
        /// </summary>
        public string Messages { get; set; }


        /// <summary>
        /// 方法返回的对象
        /// </summary>
        public object ResultObject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResponseCommand() : this(false, "", null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sucess"></param>
        /// <param name="msg"></param>
        /// <param name="resultObj"></param>
        public ResponseCommand(bool sucess, string msg, object resultObj)
        {
            Success = sucess;
            Messages = msg;
            ResultObject = resultObj;
        }

        /// <summary>
        /// 获取实际对象
        /// </summary>
        /// <typeparam name="T">需要转换的对象类型</typeparam>
        /// <returns>返回所转换的实际对象</returns>
        public T GetResultObj<T>()
        {
            return (T)ResultObject;
        }

        /// <summary>
        /// 获取默认的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Default<T>()
        {
            return default;
        }
    }
}
