namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 处理类型
    /// </summary>
    public enum GenExecuteType
    {
        /// <summary>
        /// 自动处理
        /// </summary>
        Auto,

        /// <summary>
        /// 直接返回字节流数组
        /// </summary>
        Buffer,

        /// <summary>
        /// 触发事件
        /// </summary>
        RequestInfo
    }
}
