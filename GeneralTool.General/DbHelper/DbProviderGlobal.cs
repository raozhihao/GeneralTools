using System;
using System.Data.Common;

namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// 全局的注册DbProvider
    /// </summary>
    public static class DbProviderGlobal
    {
        internal static DbProviderFactory ProviderFactory { get { return dbProvider; } }

        internal static Boolean HaveFactory { get { return dbProvider != null; } }

        private static DbProviderFactory dbProvider;

        /// <summary>
        /// 注册DbProviderFactory实例,调用方式如:DbProviderGlobal.Register(System.Data.SqlClient.SqlClientFactory.Instance)
        /// </summary>
        /// <param name="providerFactory">对应数据库工厂实例</param>
        /// <remarks>调用方式如:DbProviderGlobal.Register(System.Data.SqlClient.SqlClientFactory.Instance)</remarks>
        public static void Register(DbProviderFactory providerFactory)
        {
            dbProvider = providerFactory;
        }
    }
}
