using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 缓存容器接口
    /// </summary>
    public interface ICache<TKey, TValue>
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        TValue GetValue(TKey key);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        void SetValue(TKey key, TValue value);
        /// <summary>
        /// 最大容量
        /// </summary>
        int Length { get; set; }

        void Remove(TKey key);

        void Clear();
        /// <summary>
        /// 获取当前缓存长度
        /// </summary>
        int CurrentLength { get; }
    }
}
