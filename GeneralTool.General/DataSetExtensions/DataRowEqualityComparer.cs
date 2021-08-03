using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GeneralTool.General.DataSetExtensions
{
    /// <summary>
    /// DataRow比较器
    /// </summary>
    public class DataRowEqualityComparer : IEqualityComparer<DataRow>
    {
        #region Private 字段

        private readonly string[] _columnNames;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="columnNames">
        /// 要比较的列名称
        /// </param>
        public DataRowEqualityComparer(params string[] columnNames)
        {
            _columnNames = columnNames;
        }

        #endregion Public 构造函数

        #region Public 方法

        /// <inheritdoc/>
        public bool Equals(DataRow x, DataRow y)
        {
            if (_columnNames.Length == 0)
            {
                return false;
            }
            else
            {
                return _columnNames.Any(colName => x[colName].Equals(y[colName]));
            }
        }

        /// <inheritdoc/>
        public int GetHashCode(DataRow obj)
        {
            return obj.ToString().GetHashCode();
        }

        #endregion Public 方法
    }
}