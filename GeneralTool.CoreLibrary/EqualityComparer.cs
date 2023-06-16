using System;
using System.Collections.Generic;

namespace GeneralTool.CoreLibrary
{
    /// <summary>
    /// 对类进行同值判断
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class EqualityComparerExpand<T> : IEqualityComparer<T>
    {
        #region Private 字段

        private readonly Func<T, T, bool> predicate;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="predicate">
        /// </param>
        public EqualityComparerExpand(Func<T, T, bool> predicate = null)
        {
            this.predicate = predicate;
        }

        #endregion Public 构造函数

        #region Public 方法

        /// <summary>
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Equals(T x, T y)
        {
            return predicate != null ? predicate(x, y) : x.Equals(y);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }

        #endregion Public 方法
    }
}