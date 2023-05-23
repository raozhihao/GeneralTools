using System;

namespace GeneralTool.CoreLibrary.WebExtensioins
{
    /// <summary>
    /// 页面相关帮助类
    /// </summary>
    public static class PageHelper
    {
        #region Public 方法

        /// <summary>
        /// 以数据量和每页数量计算页码数量
        /// </summary>
        /// <param name="dataCount">
        /// 数据量
        /// </param>
        /// <param name="pageSize">
        /// 页码数量
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetPageLength(int dataCount, int pageSize) => (int)Math.Ceiling(dataCount / pageSize * 1.0);

        /// <summary>
        /// 计算按当前页码及最大生成页条数在总页数时生成的两个下标值, 例如pageIndex=4,当前数据一共有7页(pageLength),
        /// 而需要生成5页(pageCount),那么最终会生成两个值2和6, 前端在使用时只用以2开始以6结尾生成对应的页码标签即可
        /// </summary>
        /// <param name="pageIndex">
        /// 当前页码
        /// </param>
        /// <param name="pageSum">
        /// 所有页码数量
        /// </param>
        /// <param name="pageCount">
        /// 要生成的页码数量
        /// </param>
        /// <returns>
        /// 第一页的页码,最后一页的页码
        /// </returns>
        public static Tuple<int, int> GetPages(int pageIndex, int pageSum, int pageCount = 5)
        {
            int left = 1, right = 1;

            //针对要显示的页码数量比总页码数量还要大的情况
            if (pageSum < pageCount)
            {
                pageCount = pageSum;
            }

            //如果当前页码为1
            if (pageIndex <= 1)
            {
                left = 1;
                right = pageCount;//最终页码则为当前的页码数
                return new Tuple<int, int>(left, right);
            }

            //如果当前页码为最大页码数量
            if (pageIndex == pageSum)
            {
                left = pageSum - pageCount + 1;
                if (left < 1)
                {
                    left = 1;
                }
                right = pageSum;
                return new Tuple<int, int>(left, right);
            }

            if (pageSum == pageCount)
            {
                //如果当前要显示的页码数量与最大页码数量一致
                right = pageSum;
                left = 1;
                return new Tuple<int, int>(left, right);
            }

            //余下为不一定充足的量
            int leftSub, rightSub;
            //计算充足量
            if (pageCount % 2 == 0)
            {
                //当前要显示的最大页码量为偶数,则左边会多一点,右边少一位
                leftSub = pageCount / 2;
                rightSub = leftSub - 1;
            }
            else
            {
                //为奇数,则为一致
                leftSub = rightSub = pageCount / 2;
            }

            //计算左边是否充足
            if (pageIndex - leftSub >= 1)
            {
                //左边是充足的
                left = pageIndex - leftSub;
            }
            else
            {
                //左边不充足
                left = 1;
            }

            //查看右边是否充足
            if (pageIndex + rightSub <= pageSum)
            {
                //右边也充足
                right = pageIndex + rightSub;
                //查看最终页码数量是否达到预期数量
                if (right - left + 1 < pageCount)
                {
                    //没有达到,查看缺少多少
                    var sum = pageCount - (right + left - 1);
                    right += sum;
                }
            }
            else
            {
                //右边不充足
                right = pageSum;
                //查看最终页码数量是否达到预期数量
                var sum = right - left + 1;//当前总条数
                if (sum < pageCount)
                {
                    //没有达到,查看缺少多少
                    var tmpLeft = pageCount - sum;
                    if (left - tmpLeft > 0)
                    {
                        left -= tmpLeft;
                    }
                    else
                    {
                        left = 1;
                    }
                }
            }

            return new Tuple<int, int>(left, right);
        }

        /// <summary>
        /// 计算按当前页码及最大生成页条数在总页数时生成的两个下标值, 例如pageIndex=4,当前数据一共有7页(pageLength),
        /// 而需要生成5页(pageCount),那么最终会生成两个值2和6, 前端在使用时只用以2开始以6结尾生成对应的页码标签即可
        /// </summary>
        /// <param name="pageIndex">
        /// 当前页码
        /// </param>
        /// <param name="dataCount">
        /// 当前数据总条数
        /// </param>
        /// <param name="pageSize">
        /// 每页数量条数
        /// </param>
        /// <param name="pageCount">
        /// 要生成的页码数量
        /// </param>
        /// <returns>
        /// 第一页的页码,最后一页的页码
        /// </returns>
        public static Tuple<int, int> GetPages(int pageIndex, int dataCount, int pageSize, int pageCount = 5) => GetPages(pageIndex, GetPageLength(dataCount, pageSize), pageCount);

        /// <summary>
        /// 根据每页条数及当前页码计算偏移起始值, 在Mysql或Pgsql中使用例:SELECT * FROM TEST LIMIT {pageSize} OFFSET PageOffset(pageSize,1)
        /// </summary>
        /// <param name="pageSize">
        /// 每页条数
        /// </param>
        /// <param name="pageIndex">
        /// 当前页码
        /// </param>
        /// <returns>
        /// </returns>
        public static int PageOffset(int pageSize, int pageIndex) => pageSize * (pageIndex - 1);

        #endregion Public 方法
    }
}