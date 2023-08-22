namespace GeneralTool.CoreLibrary.DbHelper
{
    /// <summary>
    /// sqlite连接字符串对象
    /// </summary>
    public class SqliteConStrBuilder : ISqlConnectionString
    {
        /// <summary>
        /// 数据库文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }

        /// <inheritdoc/>
        public string CreateConnectionString()
        {
            return $"Data Source = {FileName}; Version = {Version};";
        }

    }
}
