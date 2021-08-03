using System;
using System.Collections.Generic;
using System.Windows;

namespace GeneralTool.General.WPFHelper
{
    /// <summary>
    /// 语言提供器
    /// </summary>
    public class LangProvider
    {
        /// <summary>
        /// 
        /// </summary>
        protected LangProvider()
        {

        }
        private static readonly Lazy<LangProvider> langProvider = new Lazy<LangProvider>(() => new LangProvider());

        /// <summary>
        /// 获取当前语言对象
        /// </summary>
        public static LangProvider LangProviderInstance { get => langProvider.Value; }

        /// <summary>
        /// 语言切换事件
        /// </summary>
        public event Action<ResourceDictionary> LangChanged;

        /// <summary>
        /// 默认的语言名称
        /// </summary>
        public string DefaultLang { get; set; }

        private Dictionary<string, ResourceDictionary> langResourceDic;

        /// <summary>
        /// 当前的语言资源
        /// </summary>
        internal protected ResourceDictionary CurrentResource { get; set; }

        /// <summary>
        /// 界面默认资源库
        /// </summary>
        internal protected Dictionary<string, string> DefaultResource { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 添加语言包资源
        /// </summary>
        /// <param name="langResourceDic"></param>
        public void AddLangResources(Dictionary<string, ResourceDictionary> langResourceDic)
        {
            this.langResourceDic = langResourceDic;
        }

        /// <summary>
        /// 更改语言
        /// </summary>
        /// <param name="key">要更改的语言key</param>
        public virtual void ChangeLang(string key)
        {
            //查看是否传入的是默认的key
            if (key == this.DefaultLang)
            {
                //是默认的中文key,则移除之前的
                if (this.CurrentResource != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(this.CurrentResource);
                }
                this.CurrentResource = null;
                this.LangChanged?.Invoke(this.CurrentResource);
                return;
            }

            //非默认key查看是否已经添加了
            var re = this.langResourceDic.TryGetValue(key, out var chooseLangResx);
            if (!re)
                return;//没有添加过,则返回

            ////已经有了,则与上次的对比下
            //if (this.currentResource == chooseLangResx)
            //{
            //    //一致,则不管,直接返回
            //    return;
            //}

            //不一致,先清除
            Application.Current.Resources.MergedDictionaries.Remove(this.CurrentResource);
            Application.Current.Resources.MergedDictionaries.Add(chooseLangResx);
            this.CurrentResource = chooseLangResx;
            this.LoadLang();
        }

        /// <summary>
        /// 通知加载语言
        /// </summary>
        public virtual void LoadLang()
        {
            this.LangChanged?.Invoke(this.CurrentResource);
        }

        /// <summary>
        /// 通过语言key获取对应的语言版本描述
        /// </summary>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public string GetLangValue(string langKey)
        {
            if (this.CurrentResource==null)
            {
                //返回默认的
                this.DefaultResource.TryGetValue(langKey,out var value);
                return value;
            }
            var val =this.CurrentResource[langKey]+"";
            if (string.IsNullOrWhiteSpace(val))
                this.DefaultResource.TryGetValue(langKey, out  val);
            return val;
        }
    }
}
