using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using GeneralTool.General.Models;

namespace GeneralTool.General.WPFHelper.WPFControls
{
    /// <summary>
    /// 为日志显示
    /// </summary>
    public class LogParargraph : Paragraph
    {
        #region Public 字段

        /// <summary>
        /// Debug日志前景色
        /// </summary>
        public static readonly DependencyProperty DebugForegroundProperty = DependencyProperty.RegisterAttached(nameof(DebugForeground), typeof(Brush), typeof(LogParargraph));

        /// <summary>
        /// Debug 日志是否显示
        /// </summary>
        public static readonly DependencyProperty DebugVisibleProperty = DependencyProperty.RegisterAttached(nameof(DebugVisible), typeof(bool), typeof(LogParargraph), new PropertyMetadata(true));

        /// <summary>
        /// 错误日志前景色
        /// </summary>
        public static readonly DependencyProperty ErrorForegroundProperty = DependencyProperty.RegisterAttached(nameof(ErrorForeground), typeof(Brush), typeof(LogParargraph));

        /// <summary>
        /// Erro 日志是否显示
        /// </summary>
        public static readonly DependencyProperty ErroVisibleProperty = DependencyProperty.RegisterAttached(nameof(ErroVisible), typeof(bool), typeof(LogParargraph), new PropertyMetadata(true));

        /// <summary>
        /// Fail 日志前景色
        /// </summary>
        public static readonly DependencyProperty FailForegroundProperty = DependencyProperty.RegisterAttached(nameof(FailForeground), typeof(Brush), typeof(LogParargraph));

        /// <summary>
        /// Fail 日志是否显示
        /// </summary>
        public static readonly DependencyProperty FailVisibleProperty = DependencyProperty.RegisterAttached(nameof(FailVisible), typeof(bool), typeof(LogParargraph), new PropertyMetadata(true));

        /// <summary>
        /// Info 日志前景色
        /// </summary>
        public static readonly DependencyProperty InfoForegroundProperty = DependencyProperty.RegisterAttached(nameof(InfoForeground), typeof(Brush), typeof(LogParargraph));

        /// <summary>
        /// Info 日志是否显示
        /// </summary>
        public static readonly DependencyProperty InfoVisibleProperty = DependencyProperty.RegisterAttached(nameof(InfoVisible), typeof(bool), typeof(LogParargraph), new PropertyMetadata(true));

        /// <summary>
        /// 附加 Text
        /// </summary>
        public static readonly DependencyProperty LinesProperty = DependencyProperty.RegisterAttached(nameof(Lines), typeof(ObservableCollection<LogMessageInfo>), typeof(LogParargraph), new PropertyMetadata(LinesChanged));



        /// <summary>
        /// 最大数量
        /// </summary>
        public static readonly DependencyProperty MaxLineCountProperty = DependencyProperty.RegisterAttached(nameof(MaxLineCount), typeof(int), typeof(LogParargraph));

        /// <summary>
        /// Waring 日志前景色
        /// </summary>
        public static readonly DependencyProperty WaringForegroundProperty = DependencyProperty.RegisterAttached(nameof(WaringForeground), typeof(Brush), typeof(LogParargraph));

        /// <summary>
        /// Waring 日志是否显示
        /// </summary>
        public static readonly DependencyProperty WaringVisibleProperty = DependencyProperty.RegisterAttached(nameof(WaringVisible), typeof(bool), typeof(LogParargraph), new PropertyMetadata(true));

        #endregion Public 字段

        private static void LinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LogParargraph p)
            {
                if (e.NewValue != null)
                {
                    p.RaiseChanged((e.NewValue as ObservableCollection<LogMessageInfo>));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessageInfos"></param>
        public void RaiseChanged(ObservableCollection<LogMessageInfo> logMessageInfos)
        {
            logMessageInfos.CollectionChanged += Dp_CollectionChanged;
        }


        private void AddItems(System.Collections.IList addItems, NotifyCollectionChangedAction action)
        {
            if (addItems == null)
                return;
            for (int index = 0; index < addItems.Count; index++)
            {
                var msg = (LogMessageInfo)addItems[index];
                var run = new Run(msg.Msg + Environment.NewLine);
                Brush brush = null;
                var visible = false;
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        switch (msg.LogType)
                        {
                            case Enums.LogType.Info when this.InfoVisible:
                                brush = this.InfoForeground;
                                visible = true;
                                break;

                            case Enums.LogType.Debug when this.DebugVisible:
                                brush = this.DebugForeground;
                                visible = true;
                                break;

                            case Enums.LogType.Error when ErroVisible:
                                brush = this.ErrorForeground;
                                visible = true;
                                break;

                            case Enums.LogType.Waring when this.WaringVisible:
                                brush = this.WaringForeground;
                                visible = true;
                                break;

                            case Enums.LogType.Fail when this.FailVisible:
                                brush = this.FailForeground;
                                visible = true;
                                break;
                        }
                        break;
                }
                if (brush != null)
                    run.Foreground = brush;
                if (visible)
                    this.Inlines.Add(run);
            }
        }

        private readonly object Locker = new object();
        private void Dp_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                lock (Locker)
                {
                    var list = sender as ObservableCollection<LogMessageInfo>;
                    if (this.Inlines.Count > this.MaxLineCount)
                    {
                        list.Clear();
                        this.Inlines.Clear();
                    }
                    else if (list.Count == 0)
                        this.Inlines.Clear();
                    AddItems(e.NewItems, e.Action);
                }
            }));
        }


        #region Public 属性

        /// <summary>
        ///获取Debug日志前景色
        /// </summary>
        /// <returns></returns>
        public Brush DebugForeground
        {
            get => this.GetValue(DebugForegroundProperty) as Brush;
            set => this.SetValue(DebugVisibleProperty, value);
        }

        /// <summary>
        /// 获取 Debug 日志是否显示
        /// </summary>
        public bool DebugVisible
        {
            get => (bool)this.GetValue(DebugVisibleProperty);
            set => this.SetValue(DebugVisibleProperty, value);
        }

        /// <summary>
        ///获取错误日志前景色
        /// </summary>
        /// <returns></returns>
        public Brush ErrorForeground
        {
            get => this.GetValue(ErrorForegroundProperty) as Brush;
            set => this.SetValue(ErrorForegroundProperty, value);
        }

        /// <summary>
        /// 获取 Erro 日志是否显示
        /// </summary>
        public bool ErroVisible
        {
            get => (bool)GetValue(ErroVisibleProperty);
            set => this.SetValue(ErroVisibleProperty, value);
        }

        /// <summary>
        ///获取 Fail 日志前景色
        /// </summary>
        public Brush FailForeground
        {
            get => this.GetValue(FailForegroundProperty) as Brush;
            set => this.SetValue(FailForegroundProperty, value);
        }

        /// <summary>
        /// 获取 Fail 日志是否显示
        /// </summary>
        public bool FailVisible
        {
            get => (bool)this.GetValue(FailVisibleProperty);
            set => this.SetValue(FailVisibleProperty, value);
        }

        /// <summary>
        ///获取 Info 日志前景色
        /// </summary>
        public Brush InfoForeground
        {
            get => this.GetValue(InfoForegroundProperty) as Brush;
            set => this.SetValue(InfoForegroundProperty, value);
        }

        /// <summary>
        /// 获取 Info 日志是否显示
        /// </summary>
        public bool InfoVisible
        {
            get => (bool)this.GetValue(InfoVisibleProperty);
            set => this.SetValue(InfoVisibleProperty, value);
        }

        /// <summary>
        /// 获取日志集合
        /// </summary>
        /// <returns>
        /// </returns>
        public ObservableCollection<LogMessageInfo> Lines
        {
            get => this.GetValue(LinesProperty) as ObservableCollection<LogMessageInfo>;
            set => this.SetValue(LinesProperty, value);
        }

        /// <summary>
        /// 获取最大数量
        /// </summary>
        /// <returns>
        /// </returns>
        public int MaxLineCount
        {
            get => Convert.ToInt32(this.GetValue(MaxLineCountProperty));
            set => this.SetValue(MaxLineCountProperty, value);
        }

        /// <summary>
        ///获取 Waring 日志前景色
        /// </summary>
        /// <returns></returns>
        public Brush WaringForeground
        {
            get => this.GetValue(WaringForegroundProperty) as Brush;
            set => this.SetValue(WaringForegroundProperty, value);
        }

        /// <summary>
        /// 获取 Waring 日志是否显示
        /// </summary>
        /// 
        public bool WaringVisible
        {
            get => (bool)this.GetValue(WaringVisibleProperty);
            set => this.SetValue(WaringVisibleProperty, value);
        }


        #endregion Public 方法
    }
}
