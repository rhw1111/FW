using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 实现基于hash和双向链表存储的缓存控制器
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class HashLinkedCacheController<K,V>
    {
        private  int _cacheSize=200;

        private  HashLinkedCache<K, CacheTimeContainer<V>> _values = new HashLinkedCache<K, CacheTimeContainer<V>>() ;

        public HashLinkedCacheController()
        {
            _values.Length = _cacheSize;
        }

        /// <summary>
        /// 缓存数量
        /// 默认200
        /// </summary>
        public  int CacheSize
        {
            get
            {
                return _cacheSize;
            }
            set
            {
                _cacheSize = value;
                _values.Length = value;
            }
        } 
        /// <summary>
        /// 缓存时间
        /// 默认600秒
        /// </summary>
        public  int CacheTimeout { get; set; } = 600;

        /// <summary>
        /// 获取指定Key的值
        /// </summary>
        /// <param name="creator">创建指定Key值的动作</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public  V GetValue(Func<K,V> creator,K key)
        {
            CacheTimeContainer<V> containerItem = _values.GetValue(key);
            if (containerItem == null || containerItem.Expire())
            {
                var value = creator(key);
                containerItem = new CacheTimeContainer<V>(value, CacheTimeout,0);
                _values.SetValue(key, containerItem);
            }

            return containerItem.Value;
        }

        /// <summary>
        /// 获取指定Key的值（异步）
        /// </summary>
        /// <param name="creator">创建指定Key值的动作</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public  async Task<V> GetValueAsync(Func<K, Task<V>> creator, K key)
        {
            CacheTimeContainer<V> containerItem = _values.GetValue(key);
            if (containerItem == null || containerItem.Expire())
            {
                var value =await creator(key);
                containerItem = new CacheTimeContainer<V>(value, CacheTimeout,0);
                _values.SetValue(key, containerItem);
            }

            return containerItem.Value;
        }

    }
}
