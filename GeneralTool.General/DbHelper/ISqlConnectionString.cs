namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// 连接字符串接口
    /// </summary>
    public interface ISqlConnectionString
    {
        /// <summary>
        /// 创建连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateConnectionString();
    }
}
