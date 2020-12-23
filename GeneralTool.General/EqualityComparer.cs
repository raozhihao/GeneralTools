using System;
using System.Collections.Generic;

namespace GeneralTool.General
{
    /// <summary>
    /// 对类进行同值判断
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EqualityComparerExpand<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> predicate;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        public EqualityComparerExpand(Func<T, T, bool> predicate = null)
        {
            this.predicate = predicate;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            if (predicate != null)
            {
                return predicate(x, y);
            }
            return x.Equals(y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
