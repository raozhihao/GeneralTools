using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using GeneralTool.General.ExceptionHelper;

namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// 提供对数据库访问的上下文类
    /// 本类为短连接
    /// </summary>
    public class DbManager : IDbManager
    {
        #region 私有字段

        /// <summary>
        /// 存储参数
        /// </summary>
        protected List<DbParameter> parameters;

        /// <summary>
        /// 事务命令对象
        /// </summary>
        private DbCommand transCommand;


        #endregion

        #region 公共属性

        /// <summary>
        /// 返回最后一次获取到的错误信息
        /// </summary>
        public string ErroMsg { get; protected set; }

        /// <summary>
        /// 数据源适配对象
        /// </summary>
        public DbProviderFactory DbProvider { get; set; }

        /// <summary>
        /// 设置或获取连接字符串
        /// </summary>
        public string ConnectionString { get; set; }


        #endregion

        #region 构造器

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStr">连接字符串</param>
        /// <param name="dbProvider">用于创建提供程序对数据源类的实现的实例</param>
        public DbManager(string connectionStr, DbProviderFactory dbProvider)
        {
            this.ConnectionString = connectionStr;
            parameters = new List<DbParameter>();

            if (dbProvider == null)
            {
                throw new ArgumentException($"未提供数据库访问实例..");
            }
            else
            {
                this.DbProvider = dbProvider;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStrBuilder">连接字符串对象</param>
        /// <param name="dbProvider">用于创建提供程序对数据源类的实现的实例</param>
        public DbManager(ISqlConnectionString conStrBuilder, DbProviderFactory dbProvider) : this(conStrBuilder.CreateConnectionString(), dbProvider)
        {

        }

        /// <summary>
        /// 构造器,该构造函数默认使用SqlServer数据源
        /// </summary>
        /// <param name="conStrBuilder">连接字符串对象</param>
        public DbManager(ISqlConnectionString conStrBuilder) : this(conStrBuilder.CreateConnectionString(), System.Data.SqlClient.SqlClientFactory.Instance)
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
            this.ConnectionString = newConnectionStr;
        }

        /// <summary>
        /// 添加参数,请使用当前确切的参数类型
        /// </summary>
        /// <param name="parameter">参数对象</param>
        public virtual void AddParameter(DbParameter parameter)
        {
            this.parameters.Add(parameter);
        }

        /// <summary>
        /// 创建参数对象集合
        /// </summary>
        /// <param name="list">参数对象集合</param>
        public virtual void CreateParameters(IEnumerable<ParameterHelper> list)
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
        public virtual void AddParameter(string name, object value, ParameterDirection direction)
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
        public virtual void AddParameter(string name, object value)
        {
            this.AddParameter(name, value, ParameterDirection.Input);
        }


        /// <summary>
        /// 添加参数集合
        /// </summary>
        /// <param name="parameters">参数对象集合</param>
        public virtual void AddParameters(params DbParameter[] parameters)
        {
            foreach (var item in parameters)
            {
                AddParameter(item);
            }
        }

        /// <summary>
        /// 添加参数集合
        /// </summary>
        /// <param name="parameters">参数对象集合</param>
        public virtual void AddParameters(IEnumerable<DbParameter> parameters)
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
            DbCommand command = null;
            try
            {
                command = CreateCommand(query, commandType);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.GetInnerExceptionMessage();
                return false;
            }
            finally
            {
                parameters.Clear();
                DisposeCommand(command);

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
                ErroMsg = exc.GetInnerExceptionMessage();
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
        /// <param name="obj">输出最后获取</param>
        /// <returns>返回查询结果</returns>
        public bool ExcuteSacler(string query, out object obj, CommandType commandType = CommandType.Text)
        {
            DbCommand command = null;
            obj = null;
            try
            {
                command = CreateCommand(query, commandType);
                obj = command.ExecuteScalar();
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.GetInnerExceptionMessage();

                return false;
            }
            finally
            {
                parameters.Clear();
                DisposeCommand(command);
            }
        }

        /// <summary>
        /// 事务内获取单个的值
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="obj">输出最后获取</param>
        /// <returns>返回查询结果</returns>
        public bool TransctionExcuteSacler(string query, out object obj, CommandType commandType = CommandType.Text)
        {
            obj = null;
            try
            {
                SetTransCommand(query, commandType);
                obj = transCommand.ExecuteScalar();
                return true;
            }
            catch (Exception exc)
            {
                ErroMsg = exc.GetInnerExceptionMessage();
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
            this.ExcuteSacler(proName, out _, CommandType.StoredProcedure);
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
        /// <param name="obj">输出最后获取</param>
        /// <returns>返回调用之后的返回值</returns>
        public bool ExcuteProcedure(string proName, out object obj, CommandType commandType)
        {
            return this.ExcuteSacler(proName, out obj, commandType);
        }

        /// <summary>
        /// 事务内调用存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="obj">输出最后获取</param>
        /// <returns>返回调用之后的返回值</returns>
        public object TransctionExcuteProcedure(string proName, out object obj, CommandType commandType)
        {
            return this.TransctionExcuteSacler(proName, out obj, commandType);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="query">sql查询语句</param>
        /// <param name="dt">返回的数据集</param>
        /// <param name="tableName">为数据集命名</param>
        /// <param name="columnUpper">是否将列名转换为大写</param>
        /// <returns>返回成功与否</returns>
        public bool GetDataTable(string query, out DataTable dt, string tableName = "", bool columnUpper = true)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = "dt";
            }
            dt = new DataTable(tableName);
            DbDataAdapter adapter = null;
            DbCommand command = null;
            try
            {
                command = CreateCommand(query, CommandType.Text);
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
                ErroMsg = ex.GetInnerExceptionMessage();
                return false;
            }
            finally
            {
                parameters.Clear();
                DisposeCommand(command);
            }
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="query">sql查询语句</param>
        /// <param name="dt">返回的数据集</param>
        /// <param name="tableName">为数据集命名</param>
        /// <param name="columnUpper">是否将列名转换为大写</param>
        /// <returns>返回成功与否</returns>
        public bool TransactionGetDataTable(string query, out DataTable dt, string tableName = "", bool columnUpper = true)
        {
            bool result;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = "dt";
            }
            dt = new DataTable(tableName);
            DbDataAdapter adapter = null;
            try
            {
                SetTransCommand(query);
                adapter = DbProvider.CreateDataAdapter();
                adapter.SelectCommand = this.transCommand;

                adapter.Fill(dt);
                if (columnUpper)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colName = dt.Columns[i].ColumnName;
                        dt.Columns[i].ColumnName = colName.ToUpper();
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                this.ErroMsg = ex.GetInnerExceptionMessage();
                this.TransctionRollBack();
            }
            finally
            {
                parameters.Clear();
            }
            return result;
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
            DbCommand command = null;
            try
            {
                command = CreateCommand(query, CommandType.Text);
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
                ErroMsg = ex.GetInnerExceptionMessage();
                return false;
            }
            finally
            {
                this.parameters.Clear();
                DisposeCommand(command);
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
            }
            catch (Exception ex)
            {
                result = false;
                this.ErroMsg = ex.GetInnerExceptionMessage();
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
        [Obsolete("该方法性能上有所损耗,请使用QueryList")]
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
                ErroMsg = ex.GetInnerExceptionMessage();
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
            DbCommand command = null;
            try
            {
                command = CreateCommand(sql, CommandType.Text);
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
                ErroMsg = ex.GetInnerExceptionMessage();
            }
            finally
            {
                reader.Close();
                DisposeCommand(command);
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
                transCommand.Dispose();
                transCommand = null;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void TransctionRollBack()
        {
            if (null != transCommand)
            {
                transCommand.Transaction.Rollback();
                transCommand.Dispose();
                transCommand = null;
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
        /// 获取指定对象集合
        /// </summary>
        /// <typeparam name="T">要获取的对象</typeparam>
        /// <param name="query">sql语句</param>
        /// <returns></returns>
        public List<T> QueryList<T>(string query)
        {
            DbCommand cmd = null;
            try
            {
                cmd = CreateCommand(query, CommandType.Text);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        return new List<T>();
                    }

                    List<T> list = new List<T>();
                    while (reader.Read())
                    {
                        T tobj = Activator.CreateInstance<T>();//创建一个对象
                        PropertyInfo[] pr = tobj.GetType().GetProperties();//获取对象的所有公共属性

                        foreach (PropertyInfo property in pr)
                        {
                            //判断当前的属性是否实现了对应的特性
                            DataPropertyAttribute attr = null;
                            var attrs = property.GetCustomAttributes(typeof(DataPropertyAttribute), false);
                            if (null != attrs && attrs.Length > 0)
                            {
                                attr = (DataPropertyAttribute)attrs[0];
                            }


                            string currentName = property.Name.ToLower();
                            if (attr != null)
                            {
                                currentName = attr.DataName;
                            }

                            object value = null;
                            try
                            {
                                value = reader[currentName];
                            }
                            catch
                            {
                                continue;//没有这一列,直接略过
                            }

                            //获取当前列类型
                            Type colType = value.GetType();
                            //获取当前字段类型
                            Type itemType = property.PropertyType;
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
                                        this.ErroMsg = $"{value} 的类型为 {colType},但在希望将其转成 {itemType} 时无法转换,请更正!";
                                        return null;
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
                                property.SetValue(tobj, value, null);
                            }
                            else
                            {
                                property.SetValue(tobj, null, null);
                            }
                        }
                        list.Add(tobj);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                this.ErroMsg = ex.GetInnerExceptionMessage();
                return null;
            }
            finally
            {
                this.DisposeCommand(cmd);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 释放command的所有资源
        /// </summary>
        /// <param name="command"></param>
        protected void DisposeCommand(DbCommand command)
        {
            if (command != null)
            {
                command.Dispose();
                command = null;
            }
        }

        /// <summary>
        /// 创建一个DbCommand
        /// </summary>
        /// <param name="query"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected DbCommand CreateCommand(string query, CommandType text)
        {
            ErroMsg = "";
            DbCommand command = null;
            DbConnection connection = null;
            try
            {
                connection = DbProvider.CreateConnection();
                connection.ConnectionString = this.ConnectionString;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.CommandType = text;
                if (parameters.Count > 0)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddRange(parameters.ToArray());
                }
                return command;
            }
            catch (Exception ex)
            {
                DisposeCommand(command);
                parameters.Clear();
                throw ex;
            }
        }


        /// <summary>
        /// 设置事务属性
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        protected void SetTransCommand(string sql, CommandType commandType = CommandType.Text)
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
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="T">需要获取的对象</typeparam>
        /// <param name="dt">对象的表</param>
        /// <param name="index">当前需要获取的表中的行索引</param>
        /// <returns></returns>
        protected T GetObjectInfo<T>(DataTable dt, int index)
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
                    DataPropertyAttribute attr = null;
                    var attrs = item.GetCustomAttributes(typeof(DataPropertyAttribute), false);
                    if (null != attrs && attrs.Length > 0)
                    {
                        attr = (DataPropertyAttribute)attrs[0];
                    }

                    // var attr = item.GetCustomAttribute<DataPropertyAttribute>();
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


        #endregion

    }
}
