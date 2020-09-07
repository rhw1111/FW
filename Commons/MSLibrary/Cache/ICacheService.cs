using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Cache
{
    public interface ICacheService
    {
        Task<V> Get<K, V>(Func<K, Task<(V,bool)>> creator, K key);
        V GetSync<K, V>(Func<K, (V,bool)> creator, K key);

        Task Set<K, V>( K key, V value);
        void SetSync<K, V>(K key, V value);

        Task Clear<K, V>(K key);
        void ClearSync<K, V>(K key);
    }
}
