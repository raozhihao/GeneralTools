using System;

namespace GeneralTool.General
{
    /// <summary>
    /// 随机生成器扩展
    /// </summary>
    public static class RandomEx
    {
        private static Random _global = new Random();

        [ThreadStatic]
        private static Random _local;

        /// <summary>
        /// 获取线程安全随机对象
        /// </summary>
        public static Random RandomLocal
        {
            get
            {
                var inst = _local;
                if (inst == null)
                {
                    int seed;
                    lock (_global) seed = _global.Next();
                    _local = inst = new Random(seed);
                }
                return inst;
            }
        }

        /// <summary>
        /// 返回一个非负随机整数
        /// </summary>
        /// <returns></returns>
        public static int Next() => RandomLocal.Next();

        /// <summary>
        /// 返回指定范围内的任意整数
        /// </summary>
        /// <param name="minValue">返回随机数的上界(随机数可取该上界值)</param>
        /// <param name="maxValue">返回随机数的下界(随机数不取该下界值)</param>
        /// <returns></returns>
        public static int Next(int minValue, int maxValue) => RandomLocal.Next(minValue, maxValue);

        /// <summary>
        /// 返回大于或等于0.0且小于1.0的随机浮点数
        /// </summary>
        /// <returns></returns>
        public static double NextDouble() => RandomLocal.NextDouble();

        /// <summary>
        /// 返回一个小于所指定的最大值的非负随机整数
        /// </summary>
        /// <param name="maxValue">返回随机数的下界(随机数不取该下界值)</param>
        /// <returns></returns>
        public static int Next(int maxValue) => RandomLocal.Next(maxValue);
    }
}
