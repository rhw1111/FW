using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MSLibrary.Collections
{
    /// <summary>
    /// 定时加载
    /// 获取数据时，检查缓存中的数据时间，如果与当前时间相差大于指定的秒数，
    /// 则重新执行数据加载动作，并保存到缓存中去
    /// </summary>
    public class TimeLoad<K,V>
    {
        /// <summary>
        /// 数据超时时间：秒
        /// </summary>
        private int _timeout;
        /// <summary>
        /// 缓存列表最大长度
        /// </summary>
        private int _limit;
        /// <summary>
        /// 数据生成动作(异步)
        /// </summary>
        private Func<K, Task<V>> _dataGenerateActionAsync;
        /// <summary>
        /// 数据生成动作(同步)
        /// </summary>
        private Func<K, V> _dataGenerateAction;

        private ConcurrentDictionary<K, TimeLoadContainer<V>> _datas = new ConcurrentDictionary<K, TimeLoadContainer<V>>();

        public TimeLoad(Func<K, Task<V>> dataGenerateActionAsync, Func<K, V> dataGenerateAction,int timeout,int limit)
        {
            _dataGenerateActionAsync = dataGenerateActionAsync;
            _dataGenerateAction = dataGenerateAction;
            _timeout = timeout;
            _limit = limit;
        }

        /// <summary>
        /// 获取数据(异步)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<V> GetDataAsync(K key)
        {
            V value = default(V);
            bool needNew = true;
            if (_datas.TryGetValue(key,out TimeLoadContainer<V> contanier))
            {
                if ((DateTime.UtcNow-contanier.CreateTime).TotalSeconds< _timeout)
                {
                    value = contanier.Data;
                    needNew = false;
                }
            }
            
            if (needNew)
            {
                value = await _dataGenerateActionAsync(key);
                contanier = new TimeLoadContainer<V>() { Data = value, CreateTime = DateTime.UtcNow };
                _datas[key] = contanier;

                var dataCount = _datas.Count;
                if (dataCount > _limit)
                {
                    lock(_datas)
                    {
                        dataCount = _datas.Count;
                        if (dataCount > _limit)
                        {
                            var deleteItems = (from item in _datas
                                               orderby item.Value.CreateTime
                                               select item).Take(dataCount - _limit).ToList();
                            foreach(var deleteItem in deleteItems)
                            {
                                _datas.TryRemove(deleteItem.Key,out TimeLoadContainer<V> newItem);
                            }
                        }
                    }

                }
            }

            return value;
        }

        /// <summary>
        /// 获取数据(同步)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V GetData(K key)
        {
            V value = default(V);
            bool needNew = true;
            if (_datas.TryGetValue(key, out TimeLoadContainer<V> contanier))
            {
                if ((DateTime.UtcNow - contanier.CreateTime).TotalSeconds < _timeout)
                {
                    value = contanier.Data;
                    needNew = false;
                }
            }

            if (needNew)
            {
                value = _dataGenerateAction(key);
                contanier = new TimeLoadContainer<V>() { Data = value, CreateTime = DateTime.UtcNow };
                _datas[key] = contanier;

                var dataCount = _datas.Count;
                if (dataCount > _limit)
                {
                    lock (_datas)
                    {
                        dataCount = _datas.Count;
                        if (dataCount > _limit)
                        {
                            var deleteItems = (from item in _datas
                                               orderby item.Value.CreateTime
                                               select item).Take(dataCount - _limit).ToList();
                            foreach (var deleteItem in deleteItems)
                            {
                                _datas.TryRemove(deleteItem.Key, out TimeLoadContainer<V> newItem);
                            }
                        }
                    }

                }
            }

            return value;
        }

        private class TimeLoadContainer<TV>
        {
            public TV Data { get; set; }
            public DateTime CreateTime { get; set; }
        }
    }
}
