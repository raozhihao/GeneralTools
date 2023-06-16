using System;
using System.Windows;
using System.Windows.Controls;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls
{
    /// <summary>
    /// 动态列面板
    /// </summary>
    public class UniformColPanel : Panel
    {

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ColsProperty;

        /// <summary>
        /// 需要的列,如果为0,则为1
        /// </summary>
        public int Cols
        {
            get => (int)GetValue(ColsProperty);
            set => SetValue(ColsProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RowHeightProperty;
        /// <summary>
        /// 行高
        /// </summary>
        public GridLength RowHeight
        {
            get => (GridLength)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }

        private int rowCount = 1;
        private int colCount = 1;

        /// <summary>
        /// 
        /// </summary>
        static UniformColPanel()
        {
            ColsProperty = DependencyProperty.Register(nameof(Cols), typeof(int), typeof(UniformColPanel), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
            RowHeightProperty = DependencyProperty.Register(nameof(RowHeight), typeof(GridLength), typeof(UniformColPanel), new FrameworkPropertyMetadata(GridLength.Auto, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            //判断每个元素能够适合的大小
            double maxWidth = availableSize.Width;
            double maxHeight = availableSize.Height;

            if (double.IsInfinity(maxWidth))
            {
                if (double.IsInfinity(ActualWidth))
                    return base.MeasureOverride(availableSize);

                maxWidth = ActualWidth;
            }

            if (double.IsInfinity(maxHeight))
            {
                if (double.IsInfinity(ActualHeight))
                    return base.MeasureOverride(availableSize);

                maxHeight = ActualHeight;
            }

            colCount = Cols;
            if (colCount < 1) colCount = 1;

            //看看能分多少列
            int childCount = base.InternalChildren.Count;
            if (childCount == 0) return base.MeasureOverride(availableSize);

            //如果子元素数量与列数不匹配
            if (childCount < colCount)
            {
                colCount = childCount;
            }
            double colWidth = Math.Floor(maxWidth / colCount);
            rowCount = 1;
            double rowHeight;
            //行高平均分配,得到可以分到的行数
            rowCount = (int)Math.Ceiling(childCount * 1.0 / colCount);
            if (RowHeight == GridLength.Auto)
            {
                rowHeight = Math.Floor(maxHeight / rowCount);
            }
            else if (RowHeight.IsStar)
            {
                //按星区分
                rowHeight = Math.Floor(maxHeight * RowHeight.Value * 0.1);
            }
            else
            {
                rowHeight = RowHeight.Value;
            }

            Size size = new Size(colWidth, rowHeight);
            //多余的就往下放了
            foreach (UIElement element in base.InternalChildren)
            {
                element.Measure(size);
            }

            return new Size(colCount * colWidth, rowCount * rowHeight);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Rect finalRect = RowHeight == GridLength.Auto
                ? new Rect(0.0, 0.0, arrangeSize.Width / colCount, arrangeSize.Height / rowCount)
                : RowHeight.IsStar
                    ? new Rect(0.0, 0.0, arrangeSize.Width / colCount, Math.Floor(arrangeSize.Height * RowHeight.Value * 0.1))
                    : new Rect(0.0, 0.0, arrangeSize.Width / colCount, RowHeight.Value);
            double width = finalRect.Width;
            double num = arrangeSize.Width - 1.0;
            // finalRect.X += finalRect.Width * (double)FirstColumn;
            foreach (UIElement internalChild in base.InternalChildren)
            {
                internalChild.Arrange(finalRect);
                if (internalChild.Visibility != Visibility.Collapsed)
                {
                    finalRect.X += width;
                    if (finalRect.X >= num)
                    {
                        finalRect.Y += finalRect.Height;
                        finalRect.X = 0.0;
                    }
                }
            }

            return arrangeSize;
        }
    }
}
