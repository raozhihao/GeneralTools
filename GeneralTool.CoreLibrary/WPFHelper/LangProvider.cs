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
        protected internal ResourceDictionary CurrentResource { get; set; }

        /// <summary>
        /// 界面默认资源库
        /// </summary>
        protected internal Dictionary<string, string> DefaultResource { get; set; } = new Dictionary<string, string>();
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
                CurrentResource = this.langResourceDic.First().Value;
        }

        /// <summary>
        /// 更改语言
        /// </summary>
        /// <param name="key">要更改的语言key</param>
        public virtual void ChangeLang(string key)
        {
            //查看是否传入的是默认的key
            if (key == DefaultLang)
            {
                //是默认的中文key,则移除之前的
                if (CurrentResource != null)
                {
                    _ = Application.Current.Resources.MergedDictionaries.Remove(CurrentResource);
                }
                CurrentResource = null;
                LangChanged?.Invoke(CurrentResource);
                return;
            }

            //非默认key查看是否已经添加了
            bool re = langResourceDic.TryGetValue(key, out ResourceDictionary chooseLangResx);
            if (!re)
                return;//没有添加过,则返回

            //不一致,先清除
            _ = Application.Current.Resources.MergedDictionaries.Remove(CurrentResource);
            Application.Current.Resources.MergedDictionaries.Add(chooseLangResx);
            CurrentResource = chooseLangResx;
            LoadLang();
        }

        /// <summary>
        /// 通知加载语言
        /// </summary>
        public virtual void LoadLang()
        {
            LangChanged?.Invoke(CurrentResource);
        }

        /// <summary>
        /// 通过语言key获取对应的语言版本描述
        /// </summary>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public virtual string GetLangValue(string langKey)
        {
            string value;
            if (CurrentResource == null)
            {
                //返回默认的
                _ = DefaultResource.TryGetValue(langKey, out value);
                return value;
            }

            value = GetValue(langKey, CurrentResource);
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = DefaultResource.TryGetValue(langKey, out value);
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
            string value = GetLangValue(key);
            return string.Format(value, parmeters);
        }

        private string GetValue(string key, ResourceDictionary resource)
        {
            //获取第一个key
            key = key.Trim();
            int index = key.IndexOf('.');
            string langKey = index > -1 ? key.Substring(0, index).Trim() : key;
            object value = resource[langKey];
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
