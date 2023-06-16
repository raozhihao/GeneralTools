using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls
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
        /// 最大数量
        /// </summary>
        public static readonly DependencyProperty MaxLineCountProperty = DependencyProperty.RegisterAttached(nameof(MaxLineCount), typeof(int), typeof(LogParargraph), new PropertyMetadata(1000));

        /// <summary>
        /// Waring 日志前景色
        /// </summary>
        public static readonly DependencyProperty WaringForegroundProperty = DependencyProperty.RegisterAttached(nameof(WaringForeground), typeof(Brush), typeof(LogParargraph));

        /// <summary>
        /// Waring 日志是否显示
        /// </summary>
        public static readonly DependencyProperty WaringVisibleProperty = DependencyProperty.RegisterAttached(nameof(WaringVisible), typeof(bool), typeof(LogParargraph), new PropertyMetadata(true));

        #endregion Public 字段

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessageInfos"></param>
        public void RaiseChanged(ObservableCollection<LogMessageInfo> logMessageInfos)
        {
            logMessageInfos.CollectionChanged -= Dp_CollectionChanged;
            logMessageInfos.CollectionChanged += Dp_CollectionChanged;
        }

        private void AddItems(System.Collections.IList addItems, NotifyCollectionChangedAction action)
        {
            if (addItems == null)
                return;
            for (int index = 0; index < addItems.Count; index++)
            {
                LogMessageInfo msg = (LogMessageInfo)addItems[index];
                Run run = new Run(msg.Msg + Environment.NewLine)
                {
                    Tag = msg
                };
                Brush brush = null;
                bool visible = false;
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        switch (msg.LogType)
                        {
                            case Enums.LogType.Info when InfoVisible:
                                brush = InfoForeground;
                                visible = true;
                                break;

                            case Enums.LogType.Debug when DebugVisible:
                                brush = DebugForeground;
                                visible = true;
                                break;

                            case Enums.LogType.Error when ErroVisible:
                                brush = ErrorForeground;
                                visible = true;
                                break;

                            case Enums.LogType.Waring when WaringVisible:
                                brush = WaringForeground;
                                visible = true;
                                break;

                            case Enums.LogType.Fail when FailVisible:
                                brush = FailForeground;
                                visible = true;
                                break;
                        }
                        break;
                }
                if (brush != null)
                    run.Foreground = brush;
                if (visible)
                    Inlines.Add(run);
            }
        }

        private readonly object Locker = new object();
        private void Dp_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                lock (Locker)
                {
                    ObservableCollection<LogMessageInfo> list = sender as ObservableCollection<LogMessageInfo>;
                    if (Inlines.Count > MaxLineCount)
                    {
                        list.CollectionChanged -= Dp_CollectionChanged;
                        list.Clear();
                        Inlines.Clear();

                        list.CollectionChanged += Dp_CollectionChanged;
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        list.CollectionChanged -= Dp_CollectionChanged;
                        RemoveItem(e.OldItems);
                        list.CollectionChanged += Dp_CollectionChanged;
                    }
                    else if (list.Count == 0)
                        Inlines.Clear();
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        AddItems(e.NewItems, e.Action);
                    }

                }
            }));
        }

        private void RemoveItem(IList oldItems)
        {
            if (oldItems == null)
            {
                return;
            }

            for (int i = 0; i < oldItems.Count; i++)
            {
                if (!(oldItems[i] is  LogMessageInfo item)) continue;
                Inline run = Inlines.FirstOrDefault(r => ((Run)r).Tag == item);
                if (run != null)
                    _ = Inlines.Remove(run);
            }

        }

        #region Public 属性

        /// <summary>
        ///获取Debug日志前景色
        /// </summary>
        /// <returns></returns>
        public Brush DebugForeground
        {
            get => GetValue(DebugForegroundProperty) as Brush;
            set => SetValue(DebugVisibleProperty, value);
        }

        /// <summary>
        /// 获取 Debug 日志是否显示
        /// </summary>
        public bool DebugVisible
        {
            get => (bool)GetValue(DebugVisibleProperty);
            set => SetValue(DebugVisibleProperty, value);
        }

        /// <summary>
        ///获取错误日志前景色
        /// </summary>
        /// <returns></returns>
        public Brush ErrorForeground
        {
            get => GetValue(ErrorForegroundProperty) as Brush;
            set => SetValue(ErrorForegroundProperty, value);
        }

        /// <summary>
        /// 获取 Erro 日志是否显示
        /// </summary>
        public bool ErroVisible
        {
            get => (bool)GetValue(ErroVisibleProperty);
            set => SetValue(ErroVisibleProperty, value);
        }

        /// <summary>
        ///获取 Fail 日志前景色
        /// </summary>
        public Brush FailForeground
        {
            get => GetValue(FailForegroundProperty) as Brush;
            set => SetValue(FailForegroundProperty, value);
        }

        /// <summary>
        /// 获取 Fail 日志是否显示
        /// </summary>
        public bool FailVisible
        {
            get => (bool)GetValue(FailVisibleProperty);
            set => SetValue(FailVisibleProperty, value);
        }

        /// <summary>
        ///获取 Info 日志前景色
        /// </summary>
        public Brush InfoForeground
        {
            get => GetValue(InfoForegroundProperty) as Brush;
            set => SetValue(InfoForegroundProperty, value);
        }

        /// <summary>
        /// 获取 Info 日志是否显示
        /// </summary>
        public bool InfoVisible
        {
            get => (bool)GetValue(InfoVisibleProperty);
            set => SetValue(InfoVisibleProperty, value);
        }

        /// <summary>
        /// 获取日志集合
        /// </summary>
        /// <returns>
        /// </returns>
        public ObservableCollection<LogMessageInfo> Lines
        {
            get => GetValue(LinesProperty) as ObservableCollection<LogMessageInfo>;
            set => SetValue(LinesProperty, value);
        }

        /// <summary>
        /// 获取最大数量
        /// </summary>
        /// <returns>
        /// </returns>
        public int MaxLineCount
        {
            get => Convert.ToInt32(GetValue(MaxLineCountProperty));
            set => SetValue(MaxLineCountProperty, value);
        }

        /// <summary>
        ///获取 Waring 日志前景色
        /// </summary>
        /// <returns></returns>
        public Brush WaringForeground
        {
            get => GetValue(WaringForegroundProperty) as Brush;
            set => SetValue(WaringForegroundProperty, value);
        }

        /// <summary>
        /// 获取 Waring 日志是否显示
        /// </summary>
        /// 
        public bool WaringVisible
        {
            get => (bool)GetValue(WaringVisibleProperty);
            set => SetValue(WaringVisibleProperty, value);
        }

        #endregion Public 方法
    }
}
