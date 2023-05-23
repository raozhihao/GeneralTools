using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GeneralTool.General.Extensions
{
    /// <summary>
    /// DataTable扩展类
    /// </summary>
    public static class DataTableExtensions
    {
        #region Public 方法

        /// <summary>
        /// 使用指定的条件清除表格中的重复行数
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="predicate">
        /// 清除条件
        /// </param>
        public static void ClearSame(this DataTable table, Func<DataRow, bool> predicate)
        {
            int count = table.Rows.Count;
            for (int i = count - 1; i > -1; i--)
            {
                DataRow row = table.Rows[i];
                if (predicate != null)
                {
                    if (predicate(row))
                    {
                        table.Rows.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 使用指定的条件清除表格中的重复行数
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="equality">
        /// 清除条件
        /// </param>
        public static void ClearSame(this DataTable table, IEqualityComparer<DataRow> equality)
        {
            int count = table.Rows.Count;
            for (int i = count - 1; i > -1; i--)
            {
                DataRow row1 = table.Rows[i];
                for (int j = i - 1; j > -1; j--)
                {
                    DataRow row2 = table.Rows[j];
                    if (equality != null)
                    {
                        if (equality.Equals(row1, row2))
                        {
                            table.Rows.RemoveAt(j);
                            i = --count;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 指示以自定义的比较方法来确定指定的列值是否存在
        /// </summary>
        /// <param name="rowCollection">
        /// 行集合
        /// </param>
        /// <param name="columnName">
        /// 列名称
        /// </param>
        /// <param name="value">
        /// 要搜索的列值
        /// </param>
        /// <param name="predicate">
        /// 比较的方法(参数1:当前循环到的行列值,参数2:当前传入要搜索的 <paramref name="value"/>)
        /// </param>
        /// <returns>
        /// 返回列值是否存在
        /// </returns>
        public static bool Contains(this DataRowCollection rowCollection, string columnName, object value, Func<object, object, bool> predicate = null)
        {
            if (rowCollection.Count == 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                return false;
            }

            foreach (DataRow row in rowCollection)
            {
                object rowValue = row[columnName];
                if (predicate(rowValue, value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 指示以默认的比较方法来确定指定的列值是否存在
        /// </summary>
        /// <param name="rowCollection">
        /// 行集合
        /// </param>
        /// <param name="column">
        /// 列名称
        /// </param>
        /// <param name="value">
        /// 要搜索的列值
        /// </param>
        /// <returns>
        /// 返回列值是否存在
        /// </returns>
        public static bool Contains(this DataRowCollection rowCollection, DataColumn column, object value)
        {
            return rowCollection.Contains(column.ColumnName, value);
        }

        /// <summary>
        /// 指示以默认的比较方法来确定指定的列值是否存在
        /// </summary>
        /// <param name="table">
        /// 表格
        /// </param>
        /// <param name="columnName">
        /// 列名称
        /// </param>
        /// <param name="value">
        /// 要搜索的列值
        /// </param>
        /// <returns>
        /// 返回列值是否存在
        /// </returns>
        public static bool Contains(this DataTable table, string columnName, object value)
        {
            return table.Contains(columnName, value);
        }

        /// <summary>
        /// 指示以自定义的比较方法来确定指定的列值是否存在
        /// </summary>
        /// <param name="table">
        /// 表格
        /// </param>
        /// <param name="column">
        /// 列
        /// </param>
        /// <param name="value">
        /// 要搜索的列值
        /// </param>
        /// <param name="predicate">
        /// 比较的方法(参数1:当前循环到的行列值,参数2:当前传入要搜索的 <paramref name="value"/>)
        /// </param>
        /// <returns>
        /// 返回列值是否存在
        /// </returns>
        public static bool Contains(this DataTable table, DataColumn column, object value, Func<object, object, bool> predicate = null)
        {
            return table.Rows.Contains(column.ColumnName, value, predicate);
        }

        /// <summary>
        /// 在DataTable上对DataRow循环迭代计算
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="action">
        /// </param>
        public static void Foreach(this DataTable table, Action<DataRow> action)
        {
            foreach (DataRow row in table.Rows)
            {
                action?.Invoke(row);
            }
        }

        /// <summary>
        /// 在DataTable上对DataColumn循环迭代计算
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="action">
        /// </param>
        public static void Foreach(this DataTable table, Action<DataColumn> action)
        {
            foreach (DataColumn column in table.Columns)
            {
                action?.Invoke(column);
            }
        }

        /// <summary>
        /// 按指定键升序排序 System.Data.EnumerableRowCollection 的行。
        /// </summary>
        /// <typeparam name="TKey">
        /// </typeparam>
        /// <param name="dt">
        /// </param>
        /// <param name="keySelector">
        /// 应用于每个元素的转换函数
        /// </param>
        /// <returns>
        /// </returns>
        public static OrderedEnumerableRowCollection<DataRow> OrderBy<TKey>(this DataTable dt, Func<DataRow, TKey> keySelector)
        {
            DataTable dtTmp = dt.Copy();
            dtTmp.AcceptChanges();

            OrderedEnumerableRowCollection<DataRow> resultTmp = dtTmp.AsEnumerable().OrderBy(keySelector);
            dtTmp.Dispose();
            return resultTmp;
        }

        /// <summary>
        /// 使用指定的条件删除行
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="predicate">
        /// </param>
        public static void RemoveWhere(this DataTable table, Predicate<DataRow> predicate)
        {
            int count = table.Rows.Count;
            for (int i = count - 1; i > -1; i--)
            {
                if (predicate(table.Rows[i]))
                {
                    table.Rows.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 使用指定的条件删除列
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="predicate">
        /// </param>
        public static void RemoveWhere(this DataTable table, Predicate<DataColumn> predicate)
        {
            int count = table.Columns.Count;
            for (int i = count - 1; i > -1; i--)
            {
                if (predicate(table.Columns[i]))
                {
                    table.Rows.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 将 System.Data.EnumerableRowCollection`1 中的每个元素投影到新窗体。
        /// </summary>
        /// <typeparam name="S">
        /// </typeparam>
        /// <param name="dt">
        /// </param>
        /// <param name="selector">
        /// 应用于每个元素的转换函数。
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<S> Select<S>(this DataTable dt, Func<DataRow, S> selector)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    yield return selector(row);
                }
            }
        }

        /// <summary>
        /// 按照指定的列进行排序
        /// </summary>
        /// <param name="dt">
        /// </param>
        /// <param name="columnName">
        /// 指定排序的列名
        /// </param>
        /// <returns>
        /// 返回新的表格
        /// </returns>
        public static DataTable Sort(this DataTable dt, string columnName)
        {
            DataView view = dt.AsDataView();
            view.Sort = columnName;
            return view.ToTable();
        }

        /// <summary>
        /// 将xml文档转为DataTable
        /// </summary>
        /// <param name="xml">
        /// xml文档
        /// </param>
        /// <returns>
        /// </returns>
        public static DataTable ToDataTable(this XmlDocument xml)
        {
            return xml.OuterXml.ToDataTable();
        }

        /// <summary>
        /// 将xml文档转为DataTable
        /// </summary>
        /// <param name="xmlString">
        /// xml字符串
        /// </param>
        /// <returns>
        /// </returns>
        public static DataTable ToDataTable(this string xmlString)
        {
            StringReader reader = new StringReader(xmlString);
            XmlTextReader xr = new XmlTextReader(reader);
            DataTable table = new DataTable();
            table.ReadXml(xr);
            reader.Dispose();
            xr.Dispose();
            return table;
        }

        /// <summary>
        /// 将DataTable转为xml文档
        /// </summary>
        /// <param name="dt">
        /// 要转换的表格
        /// </param>
        /// <returns>
        /// </returns>
        public static XmlDocument ToXmlDocument(this DataTable dt)
        {
            //检查Table的名称是否为空
            if (string.IsNullOrWhiteSpace(dt.TableName))
            {
                //如果为空,则给一个命名,一定要有名称
                dt.TableName = "table";
            }

            //检查Table的命名空间是否为空
            if (!string.IsNullOrWhiteSpace(dt.Namespace))
            {
                //如果不为空,则一定要删除它的命名空间
                dt.Namespace = null;
            }
            var stream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(dt.GetType());

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                CheckCharacters = true,
                CloseOutput = true,
                ConformanceLevel = ConformanceLevel.Auto,
                Encoding = new UTF8Encoding(false),
                DoNotEscapeUriAttributes = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = false,
                OmitXmlDeclaration = false,
                WriteEndDocumentOnClose = true
            };
            using (var writer = XmlWriter.Create(stream, settings))
            {
                xs.Serialize(stream, dt);
            }

            string xmlStr = Encoding.UTF8.GetString(stream.ToArray());
            stream.Dispose();

            //下面还需要做一些其它的事情,所以装载到xmldocument中
            var xml = new XmlDocument();
            xml.LoadXml(xmlStr);

            //给根路径添加命名空间,这是必须的
            xml.DocumentElement.SetAttribute("xmlns", "http://schemas.datacontract.org/2004/07/System.Data");
            //创建一个空的命名空间,这也是必须的
            var attr = xml.CreateAttribute("xmlns");
            attr.Value = "";
            //找到第一个子节点下的DocumentElement子节点(因为DataTable序列化后是固定的,所以直接下标获取)
            var element = xml.DocumentElement.ChildNodes[1].ChildNodes[0];
            //设置命名空间
            element.Attributes.SetNamedItem(attr);
            //找到所有的diffgr:before节点下的子节点
            var elements = xml.DocumentElement.ChildNodes[1].ChildNodes[1].ChildNodes;
            foreach (XmlNode node in elements)
            {
                //循环添加空命名空间,这是必须的
                node.Attributes.SetNamedItem(attr);
            }

            return xml;
        }

        /// <summary>
        /// 将DataTable转为xml字符串
        /// </summary>
        /// <param name="dt">
        /// 要转换的表格
        /// </param>
        /// <returns>
        /// </returns>
        public static string ToXmlString(this DataTable dt)
        {
            return dt.ToXmlDocument().OuterXml;
        }

        /// <summary>
        /// 按指定谓词筛选行序列。
        /// </summary>
        /// <param name="dt">
        /// </param>
        /// <param name="predicate">
        /// 用于测试每个元素是否满足条件的函数。
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<DataRow> Where(this DataTable dt, Func<DataRow, bool> predicate)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState != DataRowState.Deleted && predicate(row))
                {
                    yield return row;
                }
            }
        }

        /// <summary>
        /// 将 DataTable 转化为 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> values = new List<T>();

            T t = default;

            int count = dt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                t = ToModel<T>(dt, i);
                values.Add(t);
            }
            return values;
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="T">需要获取的对象</typeparam>
        /// <param name="dt">对象的表</param>
        /// <param name="rowIndex">当前需要获取的表中的行索引</param>
        /// <returns></returns>
        public static T ToModel<T>(this DataTable dt, int rowIndex)
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
                    string currentName = item.Name.ToLower();

                    //如果当前循环到的对象属性名称与当前数据表中的列名一致
                    if (currentName.Equals(dt.Columns[i].ColumnName.ToLower()))
                    {
                        //将当前的行列值设置到对象中
                        var value = dt.Rows[rowIndex][i];
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
                                value += "";
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

        /// <summary>
        /// 将集合对象转为DataTable
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="collection">对象集合</param>
        /// <returns>返回一个DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable() { TableName = typeof(T).Name };
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    var tempList = new ArrayList();
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
        #endregion Public 方法
    }
}