namespace GeneralTool.CoreLibrary.DbHelper
{
    /// <summary>
    /// 提供数据库连接字符串对象的基类型
    /// </summary>
    public abstract class BaseConStrBuilder
    {
        /// <summary>
        /// 数据库IP地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 数据库端口号
        /// </summary>
        protected string Port { get; set; }
        /// <summary>
        /// 数据库用户名
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 数据库密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 连接的数据库名称
        /// </summary>
        protected string DataBase { get; set; }
    }
}
