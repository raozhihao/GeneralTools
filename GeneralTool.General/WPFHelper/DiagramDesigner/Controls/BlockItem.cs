using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using GeneralTool.General.WPFHelper.DiagramDesigner.Models;
using GeneralTool.General.WPFHelper.DiagramDesigner.Thumbs;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Controls
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

            var md = new PropertyMetadata()
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
            this.ID = Guid.NewGuid();
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
                var x = Canvas.GetLeft(b);
                var y = Canvas.GetTop(b);
                var point = new Point(x, y);
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
                var x = Canvas.GetLeft(b);
                var y = Canvas.GetTop(b);
                var point = new Point(x, y);
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
            get => (bool)this.GetValue(IsBreakPointProperty);
            set => this.SetValue(IsBreakPointProperty, value);
        }

        /// <summary>
        /// 是否调试模式
        /// </summary>
        public bool IsDebugMode
        {
            get => (bool)this.GetValue(IsDebugModeProperty);
            set => this.SetValue(IsDebugModeProperty, value);
        }

        /// <summary>
        /// ID,唯一标识符
        /// </summary>
        public Guid ID
        {
            get => (Guid)this.GetValue(IDProperty);
            set => this.SetValue(IDProperty, value);
        }

        /// <summary>
        /// 是否第一个
        /// </summary>
        public bool IsStart
        {
            get => (bool)this.GetValue(IsStartProperty);
            set => this.SetValue(IsStartProperty, value);
        }

        /// <summary>
        /// 是否最后一个
        /// </summary>
        public bool IsEnd
        {
            get => (bool)this.GetValue(IsEndProperty);
            set => this.SetValue(IsEndProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBreakBlock
        {
            get => (bool)this.GetValue(IsBreakBlockProperty);
            set => this.SetValue(IsBreakBlockProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public string ResultVaraible
        {
            get => this.GetValue(ResultVaraibleProperty) + "";
            set => this.SetValue(ResultVaraibleProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility ResultVisibility
        {
            get => (Visibility)this.GetValue(ResultVisibilityProperty);
            set => this.SetValue(ResultVisibilityProperty, value);
        }
        /// <summary>
        /// 是否可以多次拖入画布
        /// </summary>
        public bool CanRepeatToCanvas
        {
            get => (bool)this.GetValue(CanRepeatToCanvasProperty);
            set => this.SetValue(CanRepeatToCanvasProperty, value);
        }

        /// <summary>
        /// 内容
        /// </summary>
        public object Content
        {
            get => this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        /// <summary>
        /// 是否显示点
        /// </summary>
        public Visibility ConnectorVisibility
        {
            get => (Visibility)this.GetValue(IsShowConnectorProperty);
            set => this.SetValue(IsShowConnectorProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility ContentVisibility
        {
            get => (Visibility)this.GetValue(ContentVisibilityProperty);
            set => this.SetValue(ContentVisibilityProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility ResizeVisibility
        {
            get => (Visibility)this.GetValue(ResizeVisibilityProperty);
            set => this.SetValue(ResizeVisibilityProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public CornerRadius HeaderCornerRadius
        {
            get => (CornerRadius)this.GetValue(HeaderCornerRadiusProperty);
            set => this.SetValue(HeaderCornerRadiusProperty, value);
        }



        /// <summary>
        /// 
        /// </summary>
        public Visibility HeaderVisibility
        {
            get => (Visibility)this.GetValue(HeaderVisibilityProperty);
            set => this.SetValue(HeaderVisibilityProperty, value);
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Header
        {
            get => this.GetValue(HeaderProperty) + "";
            set => this.SetValue(HeaderProperty, value);
        }



        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get => (bool)this.GetValue(IsSelectedProperty);
            set => this.SetValue(IsSelectedProperty, value);
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
            get => (bool)this.GetValue(IsInCanvasProperty);
            set => this.SetValue(IsInCanvasProperty, value);
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
