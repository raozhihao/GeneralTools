using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace GeneralTool.CoreLibrary.DbHelper
{
    /// <summary>
    /// 接口数据
    /// </summary>
    public interface IDbManager
    {
        /// <summary>
        /// 设置或获取连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 数据源适配对象
        /// </summary>
        DbProviderFactory DbProvider { get; set; }

        /// <summary>
        /// 返回最后一次获取到的错误信息
        /// </summary>
        string ErroMsg { get; }

        /// <summary>
        /// 添加参数,请使用当前确切的参数类型
        /// </summary>
        /// <param name="parameter">参数对象</param>
        void AddParameter(DbParameter parameter);

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        void AddParameter(string name, object value);

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">类型</param>
        void AddParameter(string name, object value, ParameterDirection direction);

        /// <summary>
        /// 添加参数集合
        /// </summary>
        /// <param name="parameters">参数对象集合</param>
        void AddParameters(IEnumerable<DbParameter> parameters);

        /// <summary>
        /// 添加参数集合
        /// </summary>
        /// <param name="list">参数对象集合</param>
        void AddParameters(params ParameterHelper[] list);

        /// <summary>
        /// 添加参数集合
        /// </summary>
        /// <param name="parameters">参数对象集合</param>
        void AddParameters(params DbParameter[] parameters);

        /// <summary>
        /// 更改连接字符串
        /// </summary>
        /// <param name="stringBuilder">连接字符串对象</param>
        void ChangeConnectionStr(ISqlConnectionString stringBuilder);

        /// <summary>
        /// 更改连接字符串
        /// </summary>
        /// <param name="newConnectionStr">新连接字符串</param>
        void ChangeConnectionStr(string newConnectionStr);

        /// <summary>
        /// 创建参数对象集合
        /// </summary>
        /// <param name="list">参数对象集合</param>
        void AddParameters(IEnumerable<ParameterHelper> list);

        /// <summary>
        /// 增加,删除,更新
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回正确与否</returns>
        bool ExcuteNonQuery(string query, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 调用有无返回值的存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        void ExcuteProcedure(string proName);

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="obj">输出最后获取</param>
        /// <returns>返回调用之后的返回值</returns>
        bool ExcuteProcedure(string proName, out object obj, CommandType commandType);

        /// <summary>
        /// 获取单个的值
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="obj">输出最后获取</param>
        /// <returns>返回查询结果</returns>
        bool ExcuteSacler(string query, out object obj, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="query">sql查询语句</param>
        /// <param name="dt">返回的数据集</param>
        /// <param name="tableName">为数据集命名</param>
        /// <param name="columnUpper">是否将列名转换为大写</param>
        /// <returns>返回成功与否</returns>
        bool GetDataTable(string query, out DataTable dt, string tableName = "", bool columnUpper = true);



        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>返回对象集合</returns>
        List<object[]> GetObjects(string sql);

        /// <summary>
        /// 获取单行的值,无返回值时返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        List<T> GetSingleColumnValue<T>(string query);

        /// <summary>
        /// 获取指定对象集合
        /// </summary>
        /// <typeparam name="T">要获取的对象</typeparam>
        /// <param name="query">sql语句</param>
        /// <param name="transtion"></param>
        /// <returns></returns>
        List<T> QueryList<T>(string query, bool transtion = false);

        /// <summary>
        /// 将集合对象转为DataTable
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="collection">对象集合</param>
        DataTable ToDataTable<T>(IEnumerable<T> collection);

        /// <summary>
        /// 根据DataTable创建一个强类型对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns>如果失败,将返回null</returns>
        List<T> ToList<T>(DataTable dt);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="query">sql查询语句</param>
        /// <param name="dt">返回的数据集</param>
        /// <param name="tableName">为数据集命名</param>
        /// <param name="columnUpper">是否将列名转换为大写</param>
        /// <returns>返回成功与否</returns>
        bool TransactionGetDataTable(string query, out DataTable dt, string tableName = "", bool columnUpper = true);

        /// <summary>
        /// 事务更新
        /// </summary>
        /// <param name="dt">更新表</param>
        /// <param name="query">查询语句</param>
        /// <param name="setAllValues"></param>
        /// <param name="conflictOption"></param>
        /// <returns></returns>
        bool TransactionUpdateDataTable(ref DataTable dt, string query, bool setAllValues = false, ConflictOption conflictOption = ConflictOption.OverwriteChanges);

        /// <summary>
        /// 提交事务
        /// </summary>
        void TransctionCommit();

        /// <summary>
        /// 事务内增加,删除,更新
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        bool TransctionExcuteNonQuery(string query, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 事务内调用有无返回值的存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        void TransctionExcuteProcedure(string proName);

        /// <summary>
        /// 事务内调用存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="obj">输出最后获取</param>
        /// <returns>返回调用之后的返回值</returns>
        object TransctionExcuteProcedure(string proName, out object obj, CommandType commandType);

        /// <summary>
        /// 事务内获取单个的值
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="obj">输出最后获取</param>
        /// <returns>返回查询结果</returns>
        bool TransctionExcuteSacler(string query, out object obj, CommandType commandType = CommandType.Text);

        /// <summary>
        /// 回滚事务
        /// </summary>
        void TransctionRollBack();

        /// <summary>
        /// 更新表格
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="dt">需要更新的表</param>
        /// <returns>返回正确与否</returns>
        bool UpdateTable(string query, DataTable dt);

        /// <summary>
        /// 更新表格
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="dt">需要更新的表</param>
        /// <param name="setAllValues">指定 update 语句中是包含所有列值还是仅包含更改的列值。</param>
        /// <param name="conflictOption">指定将如何检测和解决对数据源的相互冲突的更改。</param>
        /// <returns>对表进行更新操作</returns>
        bool UpdateTable(string query, DataTable dt, bool setAllValues, ConflictOption conflictOption);
    }
}
