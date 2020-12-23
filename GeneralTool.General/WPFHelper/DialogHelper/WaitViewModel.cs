using System.Windows;
using System.Windows.Media;

namespace GeneralTool.General.WPFHelper.DialogHelper
{
    /// <summary>
    /// 等待模型
    /// </summary>
    public class WaitViewModel : BaseNotifyModel
    {
        private string title;
        private string caption;
        private bool progressIsIndeterminate = true;
        private double progressValue = 0;
        private double opacity = 0.4;
        private double waitWidth = 300;
        private Brush maskBackGround = Brushes.Black;
        private Brush waitBorderColor = Brushes.OliveDrab;
        private Thickness waitBorderThickness = new Thickness(1);
        private Visibility titleVisible = Visibility.Collapsed;
        private Visibility captionVisible = Visibility.Collapsed;
        private Visibility progressVisible = Visibility.Visible;

        /// <summary>
        /// 设置或获取进度条是否显示
        /// </summary>
        public Visibility ProgressVisible
        {
            get => progressVisible;
            set => RegisterProperty(ref progressVisible, value);
        }

        /// <summary>
        /// 设置文本是否显示
        /// </summary>
        public Visibility CaptionVisible
        {
            get => captionVisible;
            set => RegisterProperty(ref captionVisible, value);
        }

        /// <summary>
        /// 文本标题是否显示设置
        /// </summary>
        public Visibility TitleVisible
        {
            get => titleVisible;
            set => RegisterProperty(ref titleVisible, value);
        }
        /// <summary>
        /// 等待标题
        /// </summary>
        public string Title
        {
            get => title; set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    //如果没有文本,则不显示
                    TitleVisible = Visibility.Collapsed;
                }
                else
                {
                    TitleVisible = Visibility.Visible;
                }
                RegisterProperty(ref title, value);
            }
        }

        /// <summary>
        /// 等待内容
        /// </summary>
        public string Caption
        {
            get => caption; set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    captionVisible = Visibility.Collapsed;
                }
                else
                {
                    captionVisible = Visibility.Visible;
                }
                RegisterProperty(ref caption, value);
            }
        }

        /// <summary>
        /// 是否显示无限期的滚动
        /// </summary>
        public bool ProgressIsIndeterminate
        {
            get => progressIsIndeterminate;
            set
            {
                if (ProgressValue < 0)
                {
                    value = true;
                }

                RegisterProperty(ref progressIsIndeterminate, value);
            }
        }

        /// <summary>
        /// 滚动的当前值,如果设定了 ProgressIsIndeterminate 为 True，则此项不此作用;
        /// 但如果此项设定大于0，则覆盖 ProgressIsIndeterminate
        /// </summary>
        public double ProgressValue
        {
            get => progressValue;
            set
            {
                if (value < 0)
                {
                    value = 0;
                    ProgressIsIndeterminate = true;
                }

                if (value > 0)
                {
                    ProgressIsIndeterminate = false;
                    progressVisible = Visibility.Visible;
                }
                RegisterProperty(ref progressValue, value);
            }
        }

        /// <summary>
        /// 遮罩层透明度
        /// </summary>
        public double Opacity
        {
            get => opacity;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 1)
                {
                    value = 1;
                }

                RegisterProperty(ref opacity, value);
            }
        }

        /// <summary>
        /// 等待区宽度
        /// </summary>
        public double WaitWidth
        {
            get => waitWidth;
            set
            {
                if (value < 0)
                {
                    value = 300;
                }

                RegisterProperty(ref waitWidth, value);
            }
        }

        /// <summary>
        /// 遮罩层背景色
        /// </summary>
        public Brush MaskBackGround { get => maskBackGround; set => RegisterProperty(ref maskBackGround, value); }

        /// <summary>
        /// 等待框边框色
        /// </summary>
        public Brush WaitBorderColor { get => waitBorderColor; set => RegisterProperty(ref waitBorderColor, value); }

        /// <summary>
        /// 等待框边框精细度
        /// </summary>
        public Thickness WaitBorderThickness
        {
            get => waitBorderThickness; set => RegisterProperty(ref waitBorderThickness, value);
        }
    }
}
