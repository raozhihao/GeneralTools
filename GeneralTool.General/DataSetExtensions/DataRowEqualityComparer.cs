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
        private readonly string[] _columnNames;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="columnNames">要比较的列名称</param>
        public DataRowEqualityComparer(params string[] columnNames)
        {
            _columnNames = columnNames;
        }


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
    }
}
