using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace GeneralTool.CoreLibrary.WPFHelper
{
    /// <summary>
    /// 表示一个异步线程动态数据集合，它可在添加、删除项目或刷新整个列表时提供通知。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        //获取当前线程的SynchronizationContext对象
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        /// <inheritdoc/>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {

            if (SynchronizationContext.Current == _synchronizationContext)
            {
                //如果操作发生在同一个线程中，不需要进行跨线程执行         
                RaiseCollectionChanged(e);
            }
            else
            {
                //如果不是发生在同一个线程中
                //准确说来，这里是在一个非UI线程中，需要进行UI的更新所进行的操作         
                _synchronizationContext.Post(RaiseCollectionChanged, e);
            }
        }
        private void RaiseCollectionChanged(object param)
        {
            // 执行         
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the PropertyChanged event on the current thread             
                RaisePropertyChanged(e);
            }
            else
            {
                // Post the PropertyChanged event on the creator thread             
                _synchronizationContext.Post(RaisePropertyChanged, e);
            }
        }
        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly         
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}
