using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GeneralTool.General.DataSetExtensions
{
    /// <summary>
    /// DataTable扩展类
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// 指示以自定义的比较方法来确定指定的列值是否存在
        /// </summary>
        /// <param name="rowCollection">行集合</param>
        /// <param name="columnName">列名称</param>
        /// <param name="value">要搜索的列值</param>
        /// <param name="predicate">比较的方法(参数1:当前循环到的行列值,参数2:当前传入要搜索的 <paramref name="value"/>)</param>
        /// <returns>返回列值是否存在</returns>
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
        /// <param name="rowCollection">行集合</param>
        /// <param name="column">列名称</param>
        /// <param name="value">要搜索的列值</param>
        /// <returns>返回列值是否存在</returns>
        public static bool Contains(this DataRowCollection rowCollection, DataColumn column, object value)
        {
            return rowCollection.Contains(column.ColumnName, value);
        }

        /// <summary>
        /// 指示以默认的比较方法来确定指定的列值是否存在
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="columnName">列名称</param>
        /// <param name="value">要搜索的列值</param>
        /// <returns>返回列值是否存在</returns>
        public static bool Contains(this DataTable table, string columnName, object value)
        {
            return table.Contains(columnName, value);
        }

        /// <summary>
        /// 指示以自定义的比较方法来确定指定的列值是否存在
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="column">列</param>
        /// <param name="value">要搜索的列值</param>
        /// <param name="predicate">比较的方法(参数1:当前循环到的行列值,参数2:当前传入要搜索的 <paramref name="value"/>)</param>
        /// <returns>返回列值是否存在</returns>
        public static bool Contains(this DataTable table, DataColumn column, object value, Func<object, object, bool> predicate = null)
        {
            return table.Rows.Contains(column.ColumnName, value, predicate);
        }

        /// <summary>
        /// 在DataTable上对DataRow循环迭代计算
        /// </summary>
        /// <param name="table"></param>
        /// <param name="action"></param>
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
        /// <param name="table"></param>
        /// <param name="action"></param>
        public static void Foreach(this DataTable table, Action<DataColumn> action)
        {
            foreach (DataColumn column in table.Columns)
            {
                action?.Invoke(column);
            }
        }

        /// <summary>
        /// 使用指定的条件清除表格中的重复行数
        /// </summary>
        /// <param name="table"></param>
        /// <param name="predicate">清除条件</param>
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
        /// <param name="table"></param>
        /// <param name="equality">清除条件</param>
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
        /// 使用指定的条件删除行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="predicate"></param>
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
        /// <param name="table"></param>
        /// <param name="predicate"></param>
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
        ///  将 System.Data.EnumerableRowCollection`1 中的每个元素投影到新窗体。
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="dt"></param>
        /// <param name="selector">应用于每个元素的转换函数。</param>
        /// <returns></returns>
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
        /// 按指定谓词筛选行序列。
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数。</param>
        /// <returns></returns>
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
        /// 按照指定的列进行排序
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName">指定排序的列名</param>
        /// <returns>返回新的表格</returns>
        public static DataTable Sort(this DataTable dt, string columnName)
        {
            DataView view = dt.AsDataView();
            view.Sort = columnName;
            return view.ToTable();
        }

        /// <summary>
        /// 按指定键升序排序 System.Data.EnumerableRowCollection 的行。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="dt"></param>
        /// <param name="keySelector">应用于每个元素的转换函数</param>
        /// <returns></returns>
        public static OrderedEnumerableRowCollection<DataRow> OrderBy<TKey>(this DataTable dt, Func<DataRow, TKey> keySelector)
        {
            DataTable dtTmp = dt.Copy();
            dtTmp.AcceptChanges();

            OrderedEnumerableRowCollection<DataRow> resultTmp = dtTmp.AsEnumerable().OrderBy(keySelector);
            dtTmp.Dispose();
            return resultTmp;
        }
    }
}
