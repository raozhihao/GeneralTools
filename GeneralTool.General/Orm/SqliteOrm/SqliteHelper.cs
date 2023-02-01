using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;

using GeneralTool.General.DbHelper;

namespace GeneralTool.General.Orm.SqliteOrm
{
    /// <summary>
    /// sqlite帮助类
    /// </summary>
    public class SqliteHelper
    {
        private string erroMsg;
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErroMsg
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.erroMsg))
                {
                    return this.erroMsg;
                }
                return this.Db.ErroMsg;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<Type, string> TypeMapper
        {
            get; set;
        } = new Dictionary<Type, string>()
        {
            {typeof(string),"TEXT" },
            {typeof(Int16),"INTEGER " },
            {typeof(Int32),"INTEGER " },
            {typeof(Int64),"INTEGER " },
            {typeof(uint),"INTEGER " },
            {typeof(UInt16),"INTEGER " },
            {typeof(UInt32),"INTEGER " },
            {typeof(UInt64),"INTEGER " },

            {typeof(DateTime),"TEXT " },
            {typeof(Double),"REAL " },
            {typeof(bool),"INTEGER " },
        };

        /// <summary>
        /// 
        /// </summary>
        public DbManager Db { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public SqliteHelper(DbManager db)
        {
            this.Db = db;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dbProvider"></param>
        public SqliteHelper(string connectionString, DbProviderFactory dbProvider)
        {
            this.Db = new DbManager(connectionString, dbProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool CreateTable<T>()
        {

            var type = typeof(T);

            string tableName = this.GetTableName<T>();

            var queryBuilder = new StringBuilder($"CREAT TABLE [{tableName}](");

            var parList = new List<string>();
            var parameters = type.GetProperties();
            foreach (var item in parameters)
            {
                string proName = item.Name;

                string colType = "";
                object defautlValue = null;
                var proAttr = item.GetCustomAttribute<DataColumnPropertyAttribute>(true);
                if (proAttr != null)
                {
                    proName = proAttr.DataName;

                    colType = proAttr.ColumnType;

                    defautlValue = proAttr.DefaultValue;
                }
                else
                {
                    if (!this.TypeMapper.TryGetValue(item.PropertyType, out colType))
                    {
                        this.erroMsg = $"未能识别出列 [{proName}] 的数据库类型,请使用 {nameof(TypeMapper)} 添加";
                    }
                }


                if (defautlValue != null)
                    parList.Add($"[{proName}] {colType} DEFAULT VALUE {defautlValue}");
                else
                    parList.Add($"[{proName}] {colType}");
            }

            queryBuilder.Append($"{string.Join(",", parList)}");
            queryBuilder.Append(")");
            return this.Db.ExcuteNonQuery(queryBuilder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataColumns"></param>
        /// <returns></returns>
        public bool AddColumn<T>(params DataColumnPropertyAttribute[] dataColumns)
        {
            if (dataColumns.Length == 0)
            {
                return true;
            }

            string tableName = this.GetTableName<T>();

            bool re = false;

            foreach (var item in dataColumns)
            {
                if (string.IsNullOrWhiteSpace(item.ColumnType))
                {

                    this.erroMsg = $"没有为列 [{item.DataName}] 设置数据库类型";
                    re = false;
                    break;

                }
                var query = $"ALTER TABLE [{tableName}] ADD {item.DataName} {item.ColumnType} ";
                if (item.DefaultValue != null)
                    query += $"DEFAULT {item.DefaultValue}";

                re = this.Db.TransctionExcuteNonQuery(query);
                if (!re)
                    break;
            }

            if (re) this.Db.TransctionCommit();
            else this.Db.TransctionRollBack();
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transtion"></param>
        /// <returns></returns>
        public List<T> QueryList<T>(bool transtion = false)
        {
            var type = typeof(T);

            string tableName = this.GetTableName<T>();

            var queryBuilder = new StringBuilder($"SELECT ");

            var parList = new List<string>();
            var parameters = type.GetProperties();
            foreach (var item in parameters)
            {
                string proName = item.Name;
                var proAttr = item.GetCustomAttribute<DataColumnPropertyAttribute>(true);
                if (proAttr != null)
                    proName = proAttr.DataName;

                parList.Add($"[{proName}]");
            }

            queryBuilder.Append($" {string.Join(",", parList)} ");
            queryBuilder.Append($" FROM [{tableName}]");
            return this.Db.QueryList<T>(queryBuilder.ToString(), transtion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transtion"></param>
        /// <returns></returns>
        public bool Insert<T>(T model, bool transtion = false)
        {
            var type = typeof(T);
            var tableName = this.GetTableName<T>();
            var queryBuilder = new StringBuilder($"INSERT INTO [{tableName}](");
            var parameters = type.GetProperties();
            var parList = new List<string>();
            var values = new List<string>();
            foreach (var item in parameters)
            {
                string proName = item.Name;
                var proAttr = item.GetCustomAttribute<DataColumnPropertyAttribute>(true);
                if (proAttr != null)
                    proName = proAttr.DataName;

                //获取值
                if (item.GetMethod != null)
                {
                    var parameterName = $"@{proName}";
                    values.Add(parameterName);
                    parList.Add($"[{proName}]");
                    this.Db.AddParameter(parameterName, item.GetValue(model));
                }
            }

            queryBuilder.Append($"{string.Join(",", parList)}");
            queryBuilder.Append(") VALUES(");
            queryBuilder.Append($"{string.Join(",", values)}");
            queryBuilder.Append(")");

            return transtion ? this.Db.TransctionExcuteNonQuery(queryBuilder.ToString()) : this.Db.ExcuteNonQuery(queryBuilder.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transtion"></param>
        /// <returns></returns>
        public bool Update<T>(T model, bool transtion = false)
        {
            var type = typeof(T);
            var tableName = this.GetTableName<T>();
            var queryBuilder = new StringBuilder($"UPDATE  [{tableName}] SET ");
            var parameters = type.GetProperties();
            var parList = new List<string>(parameters.Length);
            //var values = new List<string>(parameters.Length);

            var idNames = new List<string>(parameters.Length);
            var idValues = new List<object>(parameters.Length);
            foreach (var item in parameters)
            {
                string proName = item.Name;
                var proAttr = item.GetCustomAttribute<DataColumnPropertyAttribute>(true);
                if (proAttr != null)
                    proName = proAttr.DataName;

                var keyAttr = item.GetCustomAttribute<DataKeyAttribute>(true);
                if (keyAttr != null)
                {

                    if (item.GetMethod == null)
                    {
                        this.erroMsg = $"标志Key [{proName}] 的列属性不允许没有Get方法";
                        return false;
                    }
                    var val = item.GetValue(model);
                    if (val == null)
                    {
                        this.erroMsg = $"指定的Key [{proName}] 未有值";
                        return false;
                    }

                    idNames.Add(proName);
                    idValues.Add(val);
                    continue;
                }

                //获取值
                if (item.GetMethod != null)
                {
                    var parameterName = $"@{proName}";
                    //values.Add(parameterName);
                    parList.Add($"[{proName}] = {parameterName}");
                    this.Db.AddParameter(parameterName, item.GetValue(model));
                }
            }

            queryBuilder.Append($"{string.Join(",", parList)}");

            if (idValues.Count > 0)
            {
                queryBuilder.Append(" WHERE  ");
                var queryList = new List<string>(idValues.Count);
                for (int i = 0; i < idValues.Count; i++)
                {
                    queryList.Add($" [{idNames[i]}] = @{idNames[i]}");
                    this.Db.AddParameter(idNames[i], idValues[i]);
                }

                queryBuilder.Append(string.Join(" AND ", queryList));
            }
            return transtion ? this.Db.TransctionExcuteNonQuery(queryBuilder.ToString()) : this.Db.ExcuteNonQuery(queryBuilder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transtion"></param>
        /// <returns></returns>
        public bool Delete<T>(T model, bool transtion = false)
        {
            var type = typeof(T);
            var tableName = this.GetTableName<T>();
            var queryBuilder = new StringBuilder($"DELETE FROM  [{tableName}] ");
            var parameters = type.GetProperties();
            var idNames = new List<string>(parameters.Length);
            var idValues = new List<object>(parameters.Length);
            foreach (var item in parameters)
            {
                string proName = item.Name;
                var proAttr = item.GetCustomAttribute<DataColumnPropertyAttribute>(true);
                if (proAttr != null)
                    proName = proAttr.DataName;

                var keyAttr = item.GetCustomAttribute<DataKeyAttribute>(true);
                if (keyAttr != null)
                {

                    if (item.GetMethod == null)
                    {
                        this.erroMsg = $"标志Key [{proName}] 的列属性不允许没有Get方法";
                        return false;
                    }
                    var val = item.GetValue(model);
                    if (val == null)
                    {
                        this.erroMsg = $"指定的Key [{proName}] 未有值";
                        return false;
                    }

                    idNames.Add(proName);
                    idValues.Add(val);
                    continue;
                }

            }

            if (idValues.Count > 0)
            {
                queryBuilder.Append(" WHERE  ");
                var queryList = new List<string>(idValues.Count);
                for (int i = 0; i < idValues.Count; i++)
                {
                    queryList.Add($" [{idNames[i]}] = @{idNames[i]}");
                    this.Db.AddParameter(idNames[i], idValues[i]);
                }

                queryBuilder.Append(string.Join(" AND ", queryList));
            }
            return transtion ? this.Db.TransctionExcuteNonQuery(queryBuilder.ToString()) : this.Db.ExcuteNonQuery(queryBuilder.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableName<T>()
        {
            var type = typeof(T);

            var tableAttr = type.GetCustomAttribute<DataTableAttribute>(true);
            string tableName = type.Name;
            if (tableAttr != null)
                tableName = tableAttr.DataTableName;

            return tableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapType"></param>
        public void SetTypeMap(Dictionary<Type, string> mapType) => this.TypeMapper = mapType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="colType"></param>
        public void SetTypeMap(Type type, string colType)
        {
            if (!this.TypeMapper.ContainsKey(type))
            {
                this.TypeMapper.Add(type, colType);
            }
            else
            {
                this.TypeMapper[type] = colType;
            }
        }
    }
}
