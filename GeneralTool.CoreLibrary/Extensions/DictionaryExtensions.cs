using System.Collections.Generic;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 字典扩展
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 添加新项或更新
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> keyValues, TKey key, TValue value)
        {
            if (keyValues.ContainsKey(key))
                keyValues[key] = value;
            else
                keyValues.Add(key, value);
        }

        public static void TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> keyValues, TKey key)
        {
            if (!keyValues.ContainsKey(key)) return;
            keyValues.Remove(key);
        }

        /// <summary>
        /// 根据指定的Key获取其值或默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> keyValues, TKey key)
        {
            return keyValues.ContainsKey(key) ? keyValues[key] : default;
        }

        /// <summary>
        /// 根据指定的Key获取其值或默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue> GetKeyValuePairOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> keyValues, TKey key)
        {
            return keyValues.ContainsKey(key) ? new KeyValuePair<TKey, TValue>(key, keyValues[key]) : default;
        }
    }
}
