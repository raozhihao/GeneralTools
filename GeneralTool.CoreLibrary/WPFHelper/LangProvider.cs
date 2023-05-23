using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GeneralTool.CoreLibrary.WPFHelper
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
        public virtual void AddLangResources(Dictionary<string, ResourceDictionary> langResourceDic)
        {
            this.langResourceDic = langResourceDic;
            if (this.langResourceDic == null)
                throw new Exception("语言集合不可为null");

            if (this.langResourceDic.Count > 0)
                this.CurrentResource = this.langResourceDic.First().Value;
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
        public virtual string GetLangValue(string langKey)
        {
            string value = "";
            if (this.CurrentResource == null)
            {
                //返回默认的
                this.DefaultResource.TryGetValue(langKey, out value);
                return value;
            }

            value = GetValue(langKey, this.CurrentResource);
            if (string.IsNullOrWhiteSpace(value))
            {
                this.DefaultResource.TryGetValue(langKey, out value);
            }
            return value;
        }

        /// <summary>
        /// 获取key所对应的语言,并将其格式化
        /// </summary>
        /// <param name="key">语言key</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        public virtual string GetLangValueFomart(string key, params object[] parmeters)
        {
            var value = this.GetLangValue(key);
            return string.Format(value, parmeters);
        }

        private string GetValue(string key, ResourceDictionary resource)
        {
            //获取第一个key
            key = key.Trim();
            var index = key.IndexOf('.');
            string langKey = "";
            if (index > -1)
                langKey = key.Substring(0, index).Trim();
            else
                langKey = key;

            var value = resource[langKey];
            if (value is ResourceDictionary r)
            {
                //递归处理
                key = key.Remove(0, index + 1);
                return GetValue(key, r);
            }
            else
                return value + "";

        }
    }
}
