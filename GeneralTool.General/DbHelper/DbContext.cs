using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// 提供对数据库访问的上下文类,该类使用长连接
    /// </summary>
    public class DbContext : IDisposable
    {
        #region 私有字段

        /// <summary>
        /// 存储参数
        /// </summary>
        private readonly List<DbParameter> parameters;

        /// <summary>
        /// 数据源适配对象
        /// </summary>
        private readonly DbProviderFactory DbProvider;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection connection;

        /// <summary>
        /// 数据库命令对象
        /// </summary>
        private DbCommand command;


        /// <summary>
        /// 事务命令对象
        /// </summary>
        private DbCommand transCommand;


        /// <summary>
        /// 连接字符串
        /// </summary>
        private string connectionStr;


        #endregion

        #region 公共属性

        /// <summary>
        /// 返回最后一次获取到的错误信息
        /// </summary>
        public string ErroMsg { get; private set; }


        /// <summary>
        /// 获取当前的DbConnection
        /// </summary>
        public DbConnection DbConnection
        {
            get
            {
                if (this.connection == null)
                {
                    connection = DbProvider.CreateConnection();
                    connection.ConnectionString = connectionStr;
                }
                return connection;
            }
        }

        /// <summary>
        /// 获取当前的DbCommand
        /// </summary>
        public DbCommand DbCommand
        {
            get
            {
                SetCommand(null, CommandType.Text);
                return this.command;
            }
        }

        /// <summary>
        /// 设置或获取连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.connectionStr;
            }
            set
            {
                if (this.connectionStr != value)
                {
                    this.ChangeConnectionStr(value);
                }
                this.connectionStr = value;
            }
        }



        #endregion

        #region 构造器

        /// <summary>
        /// 获取DbContext的实例,如要使用此种,请预先使用DbProviderGlobal注册
        /// </summary>
        public static DbContext Instance
        {
            get
            {
                if (!DbProviderGlobal.HaveFactory)
                {
                    throw new ArgumentException($"未注册 DbProviderGlobal 实例,请先调用 DbProviderGlobal.Register(DbProviderFactory providerFactory)..");
                }
                return new DbContext("");
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStr">连接字符串</param>
        public DbContext(string connectionStr)
        {
            this.connectionStr = connectionStr;
            this.DbProvider = DbProviderGlobal.ProviderFactory;
            parameters = new List<DbParameter>();
            command = null;
            transCommand = null;
            ErroMsg = "";
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="conStrBuilder">连接字符串对象</param>
        public DbContext(ISqlConnectionString conStrBuilder) : this(conStrBuilder.CreateConnectionString())
        {
        }



        #endregion

        #region 公共方法


        /// <summary>
        /// 更改连接字符串
        /// </summary>
        /// <param name="stringBuilder">连接字符串对象</param>
        public void ChangeConnectionStr(ISqlConnectionString stringBuilder)
        {
            ChangeConnectionStr(stringBuilder.CreateConnectionString());
        }

        /// <summary>
        /// 更改连接字符串
        /// </summary>
        /// <param name="newConnectionStr">新连接字符串</param>
        public void ChangeConnectionStr(string newConnectionStr)
        {
            try
            {
                if (connection == null)
                {
                    connection = this.DbProvider.CreateConnection();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.ConnectionString = newConnectionStr;
            }
            catch
            {
                throw new ArgumentException(ErroMsg);
            }
        }

        /// <summary>
        /// 添加参数,请使用当前确切的参数类型
        /// </summary>
        /// <param name="parameter">参数对象</param>
        public void AddParameter(DbParameter parameter)
        {
            this.parameters.Add(parameter);
        }

        /// <summary>
        /// 创建参数对象集合
        /// </summary>
        /// <param name="list">参数对象集合</param>
        public void CreateParameters(IEnumerable<ParameterHelper> list)
        {
            if (list == null)
            {
                return;
            }

            foreach (var item in list)
            {
                AddParameter(item.Name, item.Value);
            }
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">类型</param>
        public void AddParameter(string name, object value, ParameterDirection direction)
        {
            var par = DbProvider.CreateParameter();
            par.ParameterName = name;
            par.Value = value;
            par.Direction = direction;
            this.parameters.Add(par);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string name, object value)
        {

            this.AddParameter(name, value, ParameterDirection.Input);
        }


        /// <summary>
        /// 添加参数集合
        /// </summary>
        /// <param name="parameters">参数对象集合</param>
        public void AddParameters(params DbParameter[] parameters)
        {
            foreach (var item in parameters)
            {
                AddParameter(item);
            }
        }

        /// <summary>
        /// 增加,删除,更新
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回正确与否</returns>
        public bool ExcuteNonQuery(string query, CommandType commandType = CommandType.Text)
        {
            try
            {
                SetCommand(query, commandType);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {

                ErroMsg = ex.Message;
                return false;
            }
            finally
            {
                parameters.Clear();
            }
        }

        /// <summary>
        /// 事务内增加,删除,更新
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        public bool TransctionExcuteNonQuery(string query, CommandType commandType = CommandType.Text)
        {
            try
            {
                SetTransCommand(query, commandType);
                transCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception exc)
            {
                ErroMsg = exc.Message;
                TransctionRollBack();
                return false;
            }
            finally
            {
                parameters.Clear();
            }
        }

        /// <summary>
        /// 获取单个的值
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果</returns>
        public object ExcuteSacler(string query, CommandType commandType = CommandType.Text)
        {
            try
            {
                SetCommand(query, commandType);
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return null;
            }
            finally
            {
                parameters.Clear();
            }
        }

        /// <summary>
        /// 事务内获取单个的值
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果</returns>
        public object TransctionExcuteSacler(string query, CommandType commandType = CommandType.Text)
        {
            try
            {
                SetTransCommand(query, commandType);
                return transCommand.ExecuteScalar();
            }
            catch (Exception exc)
            {
                ErroMsg = exc.Message;
                TransctionRollBack();
                return false;
            }
            finally
            {
                parameters.Clear();
            }
        }

        /// <summary>
        /// 调用有无返回值的存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        public void ExcuteProcedure(string proName)
        {
            this.ExcuteSacler(proName, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 事务内调用有无返回值的存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        public void TransctionExcuteProcedure(string proName)
        {
            this.TransctionExcuteNonQuery(proName, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回调用之后的返回值</returns>
        public object ExcuteProcedure(string proName, CommandType commandType)
        {
            return this.ExcuteSacler(proName, commandType);
        }

        /// <summary>
        /// 事务内调用存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回调用之后的返回值</returns>
        public object TransctionExcuteProcedure(string proName, CommandType commandType)
        {
            return this.TransctionExcuteSacler(proName, commandType);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="query">sql查询语句</param>
        /// <param name="dt">返回的数据集</param>
        /// <param name="tableName">为数据集命名</param>
        /// <param name="columnUpper">是否将列名转换为大写</param>
        /// <returns>返回正确与否</returns>
        public bool GetDataTable(string query, out DataTable dt, string tableName = "", bool columnUpper = true)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = "dt";
            }
            dt = new DataTable(tableName);
            DbDataAdapter adapter = null;
            try
            {
                SetCommand(query, CommandType.Text);
                adapter = DbProvider.CreateDataAdapter();

                adapter.SelectCommand = command;

                adapter.Fill(dt);
                if (columnUpper)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colName = dt.Columns[i].ColumnName;
                        dt.Columns[i].ColumnName = colName.ToUpper();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return false;
            }
            finally
            {
                parameters.Clear();
                adapter?.Dispose();
            }

        }



        /// <summary>
        /// 更新表格
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="dt">需要更新的表</param>
        /// <returns>返回正确与否</returns>
        public bool UpdateTable(string query, DataTable dt)
        {
            return UpdateTable(query, dt, false, ConflictOption.OverwriteChanges);
        }

        /// <summary>
        /// 更新表格
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="dt">需要更新的表</param>
        /// <param name="setAllValues">指定 update 语句中是包含所有列值还是仅包含更改的列值。</param>
        /// <param name="conflictOption">指定将如何检测和解决对数据源的相互冲突的更改。</param>
        /// <returns>对表进行更新操作</returns>
        public bool UpdateTable(string query, DataTable dt, bool setAllValues, ConflictOption conflictOption)
        {
            DbDataAdapter adapter = null;
            try
            {
                SetCommand(query, CommandType.Text);
                adapter = DbProvider.CreateDataAdapter();
                adapter.SelectCommand = command;
                DbCommandBuilder builder = DbProvider.CreateCommandBuilder();
                builder.DataAdapter = adapter;
                builder.SetAllValues = setAllValues;
                builder.ConflictOption = conflictOption;
                adapter.Update(dt);
                dt.AcceptChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return false;
            }
            finally
            {
                this.parameters.Clear();
                adapter?.Dispose();
            }
        }


        /// <summary>
        /// 事务更新
        /// </summary>
        /// <param name="dt">更新表</param>
        /// <param name="query">查询语句</param>
        /// <param name="setAllValues"></param>
        /// <param name="conflictOption"></param>
        /// <returns></returns>
        public bool TransactionUpdateDataTable(ref DataTable dt, string query, bool setAllValues = false, ConflictOption conflictOption = ConflictOption.OverwriteChanges)
        {
            bool result;
            try
            {
                SetTransCommand(query);
                DbDataAdapter adapter = DbProvider.CreateDataAdapter();
                adapter.SelectCommand = this.transCommand;
                DbCommandBuilder builder = DbProvider.CreateCommandBuilder();
                builder.DataAdapter = adapter;
                builder.SetAllValues = setAllValues;
                builder.ConflictOption = conflictOption;
                adapter.Update(dt);
                result = true;
                adapter.Dispose();
            }
            catch (Exception ex)
            {
                result = false;
                this.ErroMsg = ex.Message;
                this.TransctionRollBack();
            }
            return result;
        }
        /// <summary>
        /// 获取指定对象集合
        /// </summary>
        /// <typeparam name="T">需要获取的对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="values">返回的强类型集合</param>
        /// <returns>返回正确与否</returns>
        public bool GetList<T>(string sql, out List<T> values)
        {
            values = new List<T>();
            bool re = GetDataTable(sql, out DataTable dt, typeof(T).Name);
            if (re)
            {

                values = ToList<T>(dt);
                re = values != null;
            }

            return re;
        }

        /// <summary>
        /// 根据DataTable创建一个强类型对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns>如果失败,将返回null</returns>
        public List<T> ToList<T>(DataTable dt)
        {
            List<T> values = new List<T>();

            try
            {
                T t = default;

                int count = dt.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    t = GetObjectInfo<T>(dt, i);
                    values.Add(t);
                }
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                values = null;
            }
            return values;
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>返回对象集合</returns>
        public List<object[]> GetObjects(string sql)
        {
            DbDataReader reader = null;
            List<object[]> values = new List<object[]>();
            try
            {
                SetCommand(sql, CommandType.Text);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object[] objs = new object[reader.FieldCount];

                        reader.GetValues(objs);
                        values.Add(objs);
                    }
                }
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
            }
            finally
            {
                reader.Close();
            }
            return values;
        }


        /// <summary>
        /// 将集合对象转为DataTable
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="collection">对象集合</param>
        /// <returns>返回一个DataTable</returns>
        public DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }


        /// <summary>
        /// 提交事务
        /// </summary>
        public void TransctionCommit()
        {
            if (null != transCommand)
            {
                transCommand.Transaction.Commit();
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void TransctionRollBack()
        {
            if (null != transCommand)
            {
                transCommand.Transaction?.Rollback();
            }
        }



        #endregion

        #region 私有方法

        /// <summary>
        /// 设置Command对象的一些值
        /// </summary>
        /// <param name="query"></param>
        /// <param name="text"></param>
        private void SetCommand(string query, CommandType text)
        {
            ErroMsg = "";
            if (command == null)
            {
                command = DbProvider.CreateCommand();
                command.Connection = connection;
            }
            if (connection.State == ConnectionState.Closed)
            {
                connection.ConnectionString = this.connectionStr;
                connection.Open();
            }
            command.CommandType = text;
            command.CommandText = query;
            if (parameters.Count > 0)
            {
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters.ToArray());
            }
        }


        /// <summary>
        /// 设置事务属性
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        private void SetTransCommand(string sql, CommandType commandType = CommandType.Text)
        {
            ErroMsg = "";
            if (transCommand == null)
            {
                var connection = DbProvider.CreateConnection();
                connection.ConnectionString = this.ConnectionString;
                try
                {
                    connection.Open();
                    transCommand = connection.CreateCommand();
                    transCommand.CommandType = commandType;

                    transCommand.Transaction = transCommand.Connection.BeginTransaction();
                }
                catch (Exception ex)
                {
                    ErroMsg = ex.Message;
                    connection?.Close();
                    connection?.Dispose();
                    transCommand?.Dispose();
                    transCommand = null;
                    parameters.Clear();
                    throw ex;
                }
            }
            transCommand.CommandText = sql;
            if (parameters.Count > 0)
            {
                transCommand.Parameters.Clear();
                transCommand.Parameters.AddRange(parameters.ToArray());
            }
        }

        /// <summary>
        /// 获取单行的值,无返回值时返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> GetSingleColumnValue<T>(string query)
        {
            List<T> list = new List<T>();
            bool re = this.GetDataTable(query, out DataTable dt);
            if (!re)
            {
                return null;
            }

            var rows = dt.Rows;
            int count = rows.Count;
            if (count == 0)
            {
                return list;
            }

            for (int i = 0; i < count; i++)
            {
                T t = default;
                t = (T)rows[i][0];
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="T">需要获取的对象</typeparam>
        /// <param name="dt">对象的表</param>
        /// <param name="index">当前需要获取的表中的行索引</param>
        /// <returns></returns>
        private T GetObjectInfo<T>(DataTable dt, int index)
        {
            T t = default;
            T tobj = Activator.CreateInstance<T>();//创建一个对象
            PropertyInfo[] pr = tobj.GetType().GetProperties();//获取对象的所有公共属性
            int columnCount = dt.Columns.Count;
            //循环获取
            foreach (PropertyInfo item in pr)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    //判断当前的属性是否实现了对应的特性
                    //var attr = item.GetCustomAttribute<DataPropertyAttribute>();
                    var attr = (DataPropertyAttribute)(item.GetCustomAttributes(typeof(DataPropertyAttribute), false)[0]);
                    string currentName = item.Name.ToLower();
                    if (attr != null)
                    {
                        currentName = attr.DataName;
                    }
                    //如果当前循环到的对象属性名称与当前数据表中的列名一致
                    if (currentName.Equals(dt.Columns[i].ColumnName.ToLower()))
                    {
                        //将当前的行列值设置到对象中
                        var value = dt.Rows[index][i];
                        //获取当前列类型
                        Type colType = dt.Columns[i].DataType;
                        //获取当前字段类型
                        Type itemType = item.PropertyType;
                        //如果两个类型不一致
                        if (colType.ToString() != itemType.ToString())
                        {
                            //如果值实现了IConvertible接口
                            if (value is IConvertible)
                            {
                                //直接将当前值转化成用户传进来的T类型
                                try
                                {
                                    value = Convert.ChangeType(value, itemType);
                                }
                                catch
                                {

                                    throw new FormatException($"{value} 的类型为 {colType},但在希望将其转成 {itemType} 时无法转换,请更正!");
                                }

                            }
                            else
                            {
                                //没有实现,直接返回其字符串形式
                                value = value.ToString();
                            }

                        }

                        if (value != DBNull.Value)
                        {
                            item.SetValue(tobj, value, null);
                        }
                        else
                        {
                            item.SetValue(tobj, null, null);
                        }

                    }
                }
            }
            t = tobj;
            return t;
        }


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    parameters.Clear();

                    ErroMsg = null;
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                transCommand?.Dispose();
                command?.Dispose();
                connection?.Close();
                connection?.Dispose();
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DbContext() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        /// <summary>
        /// 释放对应资源
        /// </summary>
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
        #endregion

    }
}
