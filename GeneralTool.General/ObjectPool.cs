using System;
using System.Collections.Concurrent;

namespace GeneralTool.General
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool<T> : IDisposable
    {
        private bool isDisposed;

        private readonly ConcurrentBag<T> Objects = new ConcurrentBag<T>();

        private readonly int maxCount;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="objectFunc">对象创建函数</param>
        /// <param name="maxCount">池中对象最大数量</param>
        public ObjectPool(Func<T> objectFunc, int maxCount = 10)
        {
            if (maxCount <= 0) throw new Exception("池中最大值不能小于或等于0");
            this.maxCount = maxCount;
            for (int i = 0; i < maxCount; i++)
            {
                this.Objects.Add(objectFunc.Invoke());
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public T Item
        {
            get
            {
                if (this.isDisposed)
                    throw new ObjectDisposedException(nameof(ObjectPool<T>));

                T item = default;
                while (!this.Objects.TryTake(out item))
                {
                    //System.Diagnostics.Trace.WriteLine("等待新的进入");
                }

                return item;
            }
        }

        /// <summary>
        /// 返回对象
        /// </summary>
        /// <param name="item"></param>
        public void Resolve(T item)
        {
            if (this.isDisposed)
                throw new ObjectDisposedException(this.GetType().Name);
            //如果当前池是满的,则先不添加,等待有空位

            while (this.Objects.Count >= this.maxCount)
            {
                // System.Diagnostics.Trace.WriteLine("池满了");
            }

            this.Objects.Add(item);
            // System.Diagnostics.Trace.WriteLine("归还");
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            //System.Diagnostics.Trace.WriteLine($"Dispose count {this.Objects.Count}");
            isDisposed = true;
            while (this.Objects.Count > 0)
            {
                this.Objects.TryTake(out var result);
                if (result is IDisposable d) d.Dispose();
            }
        }
    }
}
