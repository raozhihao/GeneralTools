using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_MoveThumb", Type = typeof(MoveThumb))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_Resize", Type = typeof(Control))]
    [TemplatePart(Name = "PART_Connectors", Type = typeof(Control))]
    public partial class BlockItem : Control, IDisposable
    {
        static BlockItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BlockItem), new FrameworkPropertyMetadata(typeof(BlockItem)));

            PropertyMetadata md = new PropertyMetadata()
            {
                DefaultValue = Guid.NewGuid(),
            };
            IDProperty = DependencyProperty.Register(nameof(ID), typeof(Guid), typeof(BlockItem), md);
        }

        /// <summary>
        /// 
        /// </summary>
        public BlockItem()
        {
            ID = Guid.NewGuid();
        }

        #region 字段

        //private Point? prevPoint;

        #endregion

        #region 附加属性

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(BlockItem), new PropertyMetadata(ContentChanged));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsBreakPointProperty = DependencyProperty.Register(nameof(IsBreakPoint), typeof(bool), typeof(BlockItem), new PropertyMetadata(false));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsShowConnectorProperty = DependencyProperty.Register(nameof(ConnectorVisibility), typeof(Visibility), typeof(BlockItem), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CanRepeatToCanvasProperty = DependencyProperty.Register(nameof(CanRepeatToCanvas), typeof(bool), typeof(BlockItem), new PropertyMetadata(true));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsStartProperty = DependencyProperty.Register(nameof(IsStart), typeof(bool), typeof(BlockItem));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsEndProperty = DependencyProperty.Register(nameof(IsEnd), typeof(bool), typeof(BlockItem));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IDProperty;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(BlockItem));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsInCanvasProperty = DependencyProperty.Register(nameof(IsInCanvas), typeof(bool), typeof(BlockItem), new PropertyMetadata(false));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(BlockItem));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ContentVisibilityProperty = DependencyProperty.Register(nameof(ContentVisibility), typeof(Visibility), typeof(BlockItem), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ResizeVisibilityProperty = DependencyProperty.Register(nameof(ResizeVisibility), typeof(Visibility), typeof(BlockItem), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(nameof(HeaderVisibility), typeof(Visibility), typeof(BlockItem));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HeaderCornerRadiusProperty = DependencyProperty.Register(nameof(HeaderCornerRadius), typeof(CornerRadius), typeof(BlockItem));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsBreakBlockProperty = DependencyProperty.Register(nameof(IsBreakBlock), typeof(bool), typeof(BlockItem));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ResultVaraibleProperty = DependencyProperty.Register(nameof(ResultVaraible), typeof(string), typeof(BlockItem), new PropertyMetadata(ResultChanged));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ResultVisibilityProperty = DependencyProperty.Register(nameof(ResultVisibility), typeof(Visibility), typeof(BlockItem), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsDebugModeProperty = DependencyProperty.Register(nameof(IsDebugMode), typeof(bool), typeof(BlockItem), new PropertyMetadata(false));

        private static void ResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BlockItem b)
            {
                if (string.IsNullOrWhiteSpace(e.NewValue + ""))
                {
                    b.ResultVisibility = Visibility.Collapsed;
                    b.HeaderCornerRadius = new CornerRadius(10);//更新边框
                }
                else
                {
                    b.ResultVisibility = Visibility.Visible;
                    b.HeaderCornerRadius = new CornerRadius(10, 10, 0, 0);//更新边框
                }

                //b.ConnectorVisibility = Visibility.Visible;
                double x = Canvas.GetLeft(b);
                double y = Canvas.GetTop(b);
                Point point = new Point(x, y);
                b.Width = double.NaN;
                b.Height = double.NaN;

                b.RaiseResizeChanged(point, b.DesiredSize);
            }
        }

        private static void ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BlockItem b)
            {
                if (string.IsNullOrWhiteSpace(e.NewValue + ""))
                {
                    b.ContentVisibility = Visibility.Collapsed;
                }
                else
                {
                    b.ContentVisibility = Visibility.Visible;
                    b.HeaderCornerRadius = new CornerRadius(10, 10, 0, 0);//更新边框
                }

                //b.ConnectorVisibility = Visibility.Visible;
                double x = Canvas.GetLeft(b);
                double y = Canvas.GetTop(b);
                Point point = new Point(x, y);
                b.Width = double.NaN;
                b.Height = double.NaN;

                b.RaiseResizeChanged(point, b.DesiredSize);
            }
        }

        #endregion

        #region 公共属性

        /// <summary>
        /// 
        /// </summary>
        public bool IsBreakPoint
        {
            get => (bool)GetValue(IsBreakPointProperty);
            set => SetValue(IsBreakPointProperty, value);
        }

        /// <summary>
        /// 是否调试模式
        /// </summary>
        public bool IsDebugMode
        {
            get => (bool)GetValue(IsDebugModeProperty);
            set => SetValue(IsDebugModeProperty, value);
        }

        /// <summary>
        /// ID,唯一标识符
        /// </summary>
        public Guid ID
        {
            get => (Guid)GetValue(IDProperty);
            set => SetValue(IDProperty, value);
        }

        /// <summary>
        /// 是否第一个
        /// </summary>
        public bool IsStart
        {
            get => (bool)GetValue(IsStartProperty);
            set => SetValue(IsStartProperty, value);
        }

        /// <summary>
        /// 是否最后一个
        /// </summary>
        public bool IsEnd
        {
            get => (bool)GetValue(IsEndProperty);
            set => SetValue(IsEndProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBreakBlock
        {
            get => (bool)GetValue(IsBreakBlockProperty);
            set => SetValue(IsBreakBlockProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public string ResultVaraible
        {
            get => GetValue(ResultVaraibleProperty) + "";
            set => SetValue(ResultVaraibleProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility ResultVisibility
        {
            get => (Visibility)GetValue(ResultVisibilityProperty);
            set => SetValue(ResultVisibilityProperty, value);
        }
        /// <summary>
        /// 是否可以多次拖入画布
        /// </summary>
        public bool CanRepeatToCanvas
        {
            get => (bool)GetValue(CanRepeatToCanvasProperty);
            set => SetValue(CanRepeatToCanvasProperty, value);
        }

        /// <summary>
        /// 内容
        /// </summary>
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// 是否显示点
        /// </summary>
        public Visibility ConnectorVisibility
        {
            get => (Visibility)GetValue(IsShowConnectorProperty);
            set => SetValue(IsShowConnectorProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility ContentVisibility
        {
            get => (Visibility)GetValue(ContentVisibilityProperty);
            set => SetValue(ContentVisibilityProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility ResizeVisibility
        {
            get => (Visibility)GetValue(ResizeVisibilityProperty);
            set => SetValue(ResizeVisibilityProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public CornerRadius HeaderCornerRadius
        {
            get => (CornerRadius)GetValue(HeaderCornerRadiusProperty);
            set => SetValue(HeaderCornerRadiusProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility HeaderVisibility
        {
            get => (Visibility)GetValue(HeaderVisibilityProperty);
            set => SetValue(HeaderVisibilityProperty, value);
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Header
        {
            get => GetValue(HeaderProperty) + "";
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        /// <summary>
        /// 本块向下挂接的目标块集合
        /// </summary>
        public List<BlockItem> SinkItems
        {
            get; set;
        } = new List<BlockItem>();

        /// <summary>
        /// 
        /// </summary>
        public bool IsInCanvas
        {
            get => (bool)GetValue(IsInCanvasProperty);
            set => SetValue(IsInCanvasProperty, value);
        }

        ///// <summary>
        ///// 本块被挂接的源集合
        ///// </summary>
        //public List<BlockItem> SourceItems
        //{
        //    get; set;
        //} = new List<BlockItem>();

        /// <summary>
        /// 连接点集合
        /// </summary>
        public ConnectorThumbCollection ConnectorThumbs { get; set; }

        /// <summary>
        /// 父画布
        /// </summary>
        public DesignerCanvas ParentCanvas { get; private set; }

        #endregion

    }
}
