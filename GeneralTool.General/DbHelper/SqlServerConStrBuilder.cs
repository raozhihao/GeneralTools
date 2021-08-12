namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// SqlServer的数据库连接字符串对象
    /// </summary>
    public class SqlServerConStrBuilder : BaseConStrBuilder, ISqlConnectionString
    {
        /// <summary>
        /// 是否使用本地模式
        /// </summary>
        public bool UseLocal;
        /// <summary>
        /// 连接的数据库名称
        /// </summary>
        public new string DataBase { get; set; }
        /// <summary>
        /// 构建数据库连接字符串对象
        /// </summary>
        /// <param name="host">数据库IP地址</param>
        /// <param name="userId">数据库用户名</param>
        /// <param name="password">数据库密码</param>
        /// <param name="dataBase">连接的数据库名称</param>
        /// <param name="useLocal">是否使用本地模式</param>
        public SqlServerConStrBuilder(string host, string dataBase, string userId = "", string password = "", bool useLocal = true)
        {
            base.Host = host;
            this.DataBase = dataBase;
            base.Uid = userId;
            base.Pwd = password;
            this.UseLocal = useLocal;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public SqlServerConStrBuilder() { }

        /// <summary>
        /// 返回对应的字符串类型
        /// </summary>
        /// <returns>返回对象的字符串</returns>
        public override string ToString()
        {
            if (UseLocal)
            {
                return $"Data Source={base.Host};Initial Catalog={this.DataBase};Integrated Security=True";
            }
            else
            {
                return $"Data Source = {base.Host}; Initial Catalog = {this.DataBase}; User Id = {base.Uid}; Password = {base.Pwd}";
            }

        }

        /// <inheritdoc/>
        public string CreateConnectionString()
        {
            return this.ToString();
        }
    }
}
