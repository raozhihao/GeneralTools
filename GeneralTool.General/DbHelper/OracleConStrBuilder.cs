namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// Oracle的数据库连接字符串对象
    /// </summary>
    public class OracleConStrBuilder : BaseConStrBuilder, ISqlConnectionString
    {
        /// <summary>
        /// SID
        /// </summary>
        public string Sid { get; set; }
        /// <summary>
        /// 数据库端口号
        /// </summary>
        public new string Port { get; set; }
        /// <summary>
        /// 构造函数 
        /// </summary>
        public OracleConStrBuilder()
        {

        }

        /// <summary>
        /// 构建数据库连接字符串对象
        /// </summary>
        /// <param name="host">数据库IP地址</param>
        /// <param name="port">数据库端口号</param>
        /// <param name="userId">数据库用户名</param>
        /// <param name="password">数据库密码</param>
        /// <param name="sid">SID</param>
        public OracleConStrBuilder(string host, string port, string userId, string password, string sid = "orcl")
        {
            base.Host = host;
            base.Uid = userId;
            base.Pwd = password;
            this.Sid = sid;
            this.Port = port;
        }

        /// <summary>
        /// 返回对象的字符串
        /// </summary>
        /// <returns>返回对象的字符串</returns>
        public override string ToString()
        {
            return $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={base.Host})(PORT={this.Port})))(CONNECT_DATA=(sid ={this.Sid})));User Id={base.Uid};Password={base.Pwd}";
        }

        /// <inheritdoc/>
        public string CreateConnectionString()
        {
            return this.ToString();
        }
    }
}
