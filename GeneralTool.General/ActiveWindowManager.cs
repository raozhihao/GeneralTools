using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace GeneralTool.General
{
    /// <summary>
    /// 窗体管理
    /// </summary>
    public class ActiveWindowManager<T> where T : class
    {
        private static Lazy<ActiveWindowManager<T>> manager = new Lazy<ActiveWindowManager<T>>(() => new ActiveWindowManager<T>());

        /// <summary>
        /// 管理类实例
        /// </summary>
        public static ActiveWindowManager<T> Manager
        {
            get
            {
                return manager.Value;
            }
        }

        private ActiveWindowManager()
        {

        }

        /// <summary>
        /// 存储所有加入的激活窗体
        /// </summary>
        private List<WindowEx<T>> lastWindows = new List<WindowEx<T>>();

        /// <summary>
        /// 获取所有当前存活的窗体
        /// </summary>
        public List<T> WindowIces
        {
            get
            {
                var list = new List<T>();
                foreach (var item in this.lastWindows)
                {
                    list.Add(item.Item);
                }
                return list;
            }
        }

        /// <summary>
        /// 将窗体添加到管理中
        /// </summary>
        /// <param name="window">要添加的窗体</param>
        public void AddActive(T window)
        {
            var w = new WindowEx<T>(window);
            w.Actived += W_Actived;
            w.Closeing += W_Closeing;
        }


        private void W_Closeing(WindowEx<T> obj)
        {
            Remove(obj);
        }

        private void Remove(WindowEx<T> obj)
        {
            //关闭,则从队列中移除
            if (this.lastWindows.Contains(obj))
            {
                this.lastWindows.Remove(obj);
            }
        }

        private void W_Actived(WindowEx<T> obj)
        {
            //先移除,再添加
            this.Remove(obj);
            this.lastWindows.Add(obj);
        }

        /// <summary>
        /// 关闭最后一个激活的窗体
        /// </summary>
        public void CloseActive()
        {
            //获取最后一个,直接关闭
            var window = this.lastWindows.LastOrDefault();
            if (window != null)
            {
                window.Close();
            }
        }


        /// <summary>
        /// 获取最后一个激活的窗体
        /// </summary>
        public T ActiveWindow
        {
            get
            {
                var last = this.lastWindows.LastOrDefault();
                if (last != null)
                    return last.Item;
                return null;
            }
        }

        /// <summary>
        /// 关闭所有已激活的窗体
        /// </summary>
        public void CloseAll()
        {
            for (int i = this.lastWindows.Count - 1; i > -1; i--)
            {
                this.lastWindows[i].Close();
            }
        }
    }

    /// <summary>
    /// Window
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class WindowEx<T> where T : class
    {
        public event Action<WindowEx<T>> Actived;
        public event Action<WindowEx<T>> Closeing;
        public T Item { get; set; }
        public WindowEx(T item)
        {
            this.Item = item;
            //如果是Form的话
            if (typeof(T) == typeof(Form))
            {
                var f = (this.Item as Form);
                f.Activated += F_Activated;
                f.FormClosing += F_FormClosing;
            }
            else if (typeof(T) == typeof(Window))
            {
                var w = this.Item as Window;
                w.Activated += W_Activated;
                w.Closing += W_Closing;
            }
        }

        private void W_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Closeing?.Invoke(this);
        }

        private void W_Activated(object sender, EventArgs e)
        {
            this.Actived?.Invoke(this);
        }

        private void F_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Closeing?.Invoke(this);
        }

        private void F_Activated(object sender, EventArgs e)
        {
            this.Actived?.Invoke(this);
        }

        public void Close()
        {
            this.Item.GetType().GetMethod("Close").Invoke(this.Item, null);
        }

        public override bool Equals(object obj)
        {
            if (obj is WindowEx<T> item)
            {
                return item.Item.Equals(this.Item);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
