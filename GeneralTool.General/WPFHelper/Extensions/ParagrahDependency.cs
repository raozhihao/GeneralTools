using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using GeneralTool.General.Models;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 为 Paragrah 注册属性
    /// </summary>
    public class ParagrahDependency : DependencyObject
    {
        #region Public 字段

        /// <summary>
        /// Debug日志前景色
        /// </summary>
        public static readonly DependencyProperty DebugForegroundProperty = DependencyProperty.RegisterAttached("DebugForeground", typeof(Brush), typeof(ParagrahDependency));

        /// <summary>
        /// Debug 日志是否显示
        /// </summary>
        public static readonly DependencyProperty DebugVisibleProperty = DependencyProperty.RegisterAttached("DebugVisible", typeof(bool), typeof(ParagrahDependency), new PropertyMetadata(true));

        /// <summary>
        /// 错误日志前景色
        /// </summary>
        public static readonly DependencyProperty ErrorForegroundProperty = DependencyProperty.RegisterAttached("ErrorForeground", typeof(Brush), typeof(ParagrahDependency));

        /// <summary>
        /// Erro 日志是否显示
        /// </summary>
        public static readonly DependencyProperty ErroVisibleProperty = DependencyProperty.RegisterAttached("ErroVisible", typeof(bool), typeof(ParagrahDependency), new PropertyMetadata(true));

        /// <summary>
        /// Fail 日志前景色
        /// </summary>
        public static readonly DependencyProperty FailForegroundProperty = DependencyProperty.RegisterAttached("FailForeground", typeof(Brush), typeof(ParagrahDependency));

        /// <summary>
        /// Fail 日志是否显示
        /// </summary>
        public static readonly DependencyProperty FailVisibleProperty = DependencyProperty.RegisterAttached("FailVisible", typeof(bool), typeof(ParagrahDependency), new PropertyMetadata(true));

        /// <summary>
        /// Info 日志前景色
        /// </summary>
        public static readonly DependencyProperty InfoForegroundProperty = DependencyProperty.RegisterAttached("InfoForeground", typeof(Brush), typeof(ParagrahDependency));

        /// <summary>
        /// Info 日志是否显示
        /// </summary>
        public static readonly DependencyProperty InfoVisibleProperty = DependencyProperty.RegisterAttached("InfoVisible", typeof(bool), typeof(ParagrahDependency), new PropertyMetadata(true));

        /// <summary>
        /// 附加 Text
        /// </summary>
        public static readonly DependencyProperty LinesProperty = DependencyProperty.RegisterAttached("Lines", typeof(ObservableCollection<LogMessageInfo>), typeof(ParagrahDependency), new PropertyMetadata(LinesChanged));

        /// <summary>
        /// 最大数量
        /// </summary>
        public static readonly DependencyProperty MaxLineCountProperty = DependencyProperty.RegisterAttached("MaxLineCount", typeof(int), typeof(ParagrahDependency));

        /// <summary>
        /// Waring 日志前景色
        /// </summary>
        public static readonly DependencyProperty WaringForegroundProperty = DependencyProperty.RegisterAttached("WaringForeground", typeof(Brush), typeof(ParagrahDependency));

        /// <summary>
        /// Waring 日志是否显示
        /// </summary>
        public static readonly DependencyProperty WaringVisibleProperty = DependencyProperty.RegisterAttached("WaringVisible", typeof(bool), typeof(ParagrahDependency), new PropertyMetadata(true));

        #endregion Public 字段

        #region Private 字段

        private static Paragraph paragraph;

        #endregion Private 字段

        #region Public 方法

        /// <summary>
        ///获取Debug日志前景色
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static Brush GetDebugForeground(Paragraph paragraph) => paragraph.GetValue(DebugForegroundProperty) as Brush;

        /// <summary>
        /// 获取 Debug 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        public static bool GetDebugVisible(Paragraph paragraph) => (bool)paragraph.GetValue(DebugVisibleProperty);

        /// <summary>
        ///获取错误日志前景色
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static Brush GetErrorForeground(Paragraph paragraph) => paragraph.GetValue(ErrorForegroundProperty) as Brush;

        /// <summary>
        /// 获取 Erro 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        public static bool GetErroVisible(Paragraph paragraph) => (bool)paragraph.GetValue(ErroVisibleProperty);

        /// <summary>
        ///获取 Fail 日志前景色
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static Brush GetFailForeground(Paragraph paragraph) => paragraph.GetValue(FailForegroundProperty) as Brush;

        /// <summary>
        /// 获取 Fail 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        public static bool GetFailVisible(Paragraph paragraph) => (bool)paragraph.GetValue(FailVisibleProperty);

        /// <summary>
        ///获取 Info 日志前景色
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static Brush GetInfoForeground(Paragraph paragraph) => paragraph.GetValue(InfoForegroundProperty) as Brush;

        /// <summary>
        /// 获取 Info 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        public static bool GetInfoVisible(Paragraph paragraph) => (bool)paragraph.GetValue(InfoVisibleProperty);

        /// <summary>
        /// 获取日志集合
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <returns>
        /// </returns>
        public static ObservableCollection<LogMessageInfo> GetLines(Paragraph paragraph)
        {
            return (ObservableCollection<LogMessageInfo>)paragraph.GetValue(LinesProperty);
        }

        /// <summary>
        /// 获取最大数量
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetMaxCount(DependencyObject obj)
        {
            return Convert.ToInt32(obj.GetValue(MaxLineCountProperty));
        }

        /// <summary>
        ///获取 Waring 日志前景色
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static Brush GetWaringForeground(Paragraph paragraph) => paragraph.GetValue(WaringForegroundProperty) as Brush;

        /// <summary>
        /// 获取 Waring 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        public static bool GetWaringVisible(Paragraph paragraph) => (bool)paragraph.GetValue(WaringVisibleProperty);

        /// <summary>
        /// 设置Debug日志前景色
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="brush">
        /// </param>
        public static void SetDebugForeground(Paragraph paragraph, Brush brush) => paragraph.SetValue(DebugForegroundProperty, brush);

        /// <summary>
        /// 设置 Debug 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="visible">
        /// </param>
        public static void SetDebugVisible(Paragraph paragraph, bool visible) => paragraph.SetValue(DebugVisibleProperty, visible);

        /// <summary>
        /// 设置错误日志前景色
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="brush">
        /// </param>
        public static void SetErrorForeground(Paragraph paragraph, Brush brush) => paragraph.SetValue(ErrorForegroundProperty, brush);

        /// <summary>
        /// 设置 Erro 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="visible">
        /// </param>
        public static void SetErroVisible(Paragraph paragraph, bool visible) => paragraph.SetValue(ErroVisibleProperty, visible);

        /// <summary>
        /// 设置 Fail 日志前景色
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="brush">
        /// </param>
        public static void SetFailForeground(Paragraph paragraph, Brush brush) => paragraph.SetValue(FailForegroundProperty, brush);

        /// <summary>
        /// 设置 Fail 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="visible">
        /// </param>
        public static void SetFailVisible(Paragraph paragraph, bool visible) => paragraph.SetValue(FailVisibleProperty, visible);

        /// <summary>
        /// 设置 Info 日志前景色
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="brush">
        /// </param>
        public static void SetInfoForeground(Paragraph paragraph, Brush brush) => paragraph.SetValue(InfoForegroundProperty, brush);

        /// <summary>
        /// 设置 Info 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="visible">
        /// </param>
        public static void SetInfoVisible(Paragraph paragraph, bool visible) => paragraph.SetValue(InfoVisibleProperty, visible);

        /// <summary>
        /// 设置日志集合
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="values">
        /// </param>
        public static void SetLines(Paragraph paragraph, ObservableCollection<LogMessageInfo> values)
        {
            paragraph.SetValue(LinesProperty, values);
        }

        /// <summary>
        /// 设置最大数量
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <param name="value">
        /// </param>
        public static void SetMaxLineCount(DependencyObject obj, int value)
        {
            obj.SetValue(MaxLineCountProperty, value);
        }

        /// <summary>
        /// 设置 Waring 日志前景色
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="brush">
        /// </param>
        public static void SetWaringForeground(Paragraph paragraph, Brush brush) => paragraph.SetValue(WaringForegroundProperty, brush);

        /// <summary>
        /// 设置 Waring 日志是否显示
        /// </summary>
        /// <param name="paragraph">
        /// </param>
        /// <param name="visible">
        /// </param>
        public static void SetWaringVisible(Paragraph paragraph, bool visible) => paragraph.SetValue(WaringVisibleProperty, visible);

        #endregion Public 方法

        #region Private 方法

        private static void AddItems(System.Collections.IList addItems, NotifyCollectionChangedAction action)
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
                            case Enums.LogType.Info when GetInfoVisible(paragraph):
                                brush = GetInfoForeground(paragraph);
                                visible = true;
                                break;

                            case Enums.LogType.Debug when GetDebugVisible(paragraph):
                                brush = GetDebugForeground(paragraph);
                                visible = true;
                                break;

                            case Enums.LogType.Error when GetErroVisible(paragraph):
                                brush = GetErrorForeground(paragraph);
                                visible = true;
                                break;

                            case Enums.LogType.Waring when GetWaringVisible(paragraph):
                                brush = GetWaringForeground(paragraph);
                                visible = true;
                                break;

                            case Enums.LogType.Fail when GetFailVisible(paragraph):
                                brush = GetFailForeground(paragraph);
                                visible = true;
                                break;
                        }
                        break;
                }
                if (brush != null)
                    run.Foreground = brush;
                if (visible)
                    paragraph.Inlines.Add(run);
            }
        }

        private static readonly object Locker = new object();
        private static void Dp_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            paragraph.Dispatcher.BeginInvoke(new Action(() =>
            {
                lock (Locker)
                {
                    var list = sender as ObservableCollection<LogMessageInfo>;
                    if (paragraph.Inlines.Count > GetMaxCount(paragraph))
                    {
                        list.Clear();
                        paragraph.Inlines.Clear();
                    }
                    AddItems(e.NewItems, e.Action);
                }
            }));
        }

        private static void LinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Paragraph p)
            {
                if (e.NewValue != null)
                {
                    paragraph = p;
                    (e.NewValue as ObservableCollection<LogMessageInfo>).CollectionChanged += Dp_CollectionChanged;
                }
            }
        }

        #endregion Private 方法
    }
}