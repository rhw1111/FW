using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 合并键值对
        /// 将mergeSource中的键值对合并到source中的键值对
        /// 如果键相等，则覆盖
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="mergeSource"></param>
        public static void Merge<K,V>(this Dictionary<K, V> source, Dictionary<K, V> mergeSource)
        {
            foreach(var item in mergeSource)
            {
                source[item.Key] = item.Value;
            }
        }
    }
}
