using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Cache
{
    [Injection(InterfaceType = typeof(ICacheService), Scope = InjectionScope.Singleton)]
    public class CacheService : ICacheService
    {
        public static string? KVCacheVisitorName { get; set; } = null;

        private readonly IKVCacheVisitorRepositoryCacheProxy _kvCacheVisitorRepositoryCacheProxy;

        public CacheService(IKVCacheVisitorRepositoryCacheProxy kvCacheVisitorRepositoryCacheProxy)
        {
            _kvCacheVisitorRepositoryCacheProxy = kvCacheVisitorRepositoryCacheProxy;
        }
        public async Task Clear<K, V>(K key)
        {
            if (KVCacheVisitorName != null)
            {
                var kv = await getVisitor();
                await kv.Clear<K, V>(key);
            }
        }

        public void ClearSync<K, V>(K key)
        {
            if (KVCacheVisitorName != null)
            {
                var kv = getVisitorSync();
                kv.ClearSync<K, V>(key);
            }
        }

        public async Task<V> Get<K, V>(Func<K, Task<(V,bool)>> creator, K key)
        {
            if (KVCacheVisitorName == null)
            {
                return (await creator(key)).Item1;
            }

            var kv = await getVisitor();
            return (await kv.Get<K, V>(creator, key)).Item1;

        }

        public V GetSync<K, V>(Func<K, (V,bool)> creator, K key)
        {
            if (KVCacheVisitorName == null)
            {
                return  creator(key).Item1;
            }

            var kv =  getVisitorSync();
            return (kv.GetSync<K, V>(creator, key)).Item1;
        }

        public async Task Set<K, V>(K key, V value)
        {
            if (KVCacheVisitorName != null)
            {
                var kv = await getVisitor();
                await kv.Set(key, value);
            }
        }

        public void SetSync<K, V>(K key, V value)
        {
            if (KVCacheVisitorName != null)
            {
                var kv =  getVisitorSync();
                kv.SetSync(key, value);
            }
        }

        private async Task<KVCacheVisitor> getVisitor()
        {
            var kv = await _kvCacheVisitorRepositoryCacheProxy.QueryByName(KVCacheVisitorName);
            if (kv==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKVCacheVisitorByName,
                    DefaultFormatting = "找不到名称为{0}的KV缓存访问者",
                    ReplaceParameters = new List<object>() { KVCacheVisitorName }
                };

                throw new UtilityException((int)Errors.NotFoundKVCacheVisitorByName, fragment, 1, 0);
            }
            return kv;
        }
        private KVCacheVisitor getVisitorSync()
        {
            var kv =  _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(KVCacheVisitorName);
            if (kv == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKVCacheVisitorByName,
                    DefaultFormatting = "找不到名称为{0}的KV缓存访问者",
                    ReplaceParameters = new List<object>() { KVCacheVisitorName }
                };

                throw new UtilityException((int)Errors.NotFoundKVCacheVisitorByName, fragment, 1, 0);
            }
            return kv;
        }

        public async Task Clear<K, V>(IEnumerable<K> keys)
        {
            if (KVCacheVisitorName != null)
            {
                var kv = await getVisitor();
                await kv.Clear<K, V>(keys);
            }
        }

        public void ClearSync<K, V>(IEnumerable<K> keys)
        {
            if (KVCacheVisitorName != null)
            {
                var kv = getVisitorSync();
                kv.ClearSync<K, V>(keys);
            }
        }

        public async Task<V> GetHash<K, V>(Func<K, string, Task<(V, bool)>> creator, K key, string hashKey)
        {
            if (KVCacheVisitorName == null)
            {
                return (await creator(key,hashKey)).Item1;
            }

            var kv = getVisitorSync();
            return (await kv.GetHash<K, V>(creator, key,hashKey)).Item1;
        }

        public V GetHashSync<K, V>(Func<K, string, (V, bool)> creator, K key, string hashKey)
        {
            if (KVCacheVisitorName == null)
            {
                return creator(key, hashKey).Item1;
            }

            var kv = getVisitorSync();
            return  kv.GetHashSync<K, V>(creator, key, hashKey).Item1;
        }

        public async Task SetHash<K, V>(K key, IDictionary<string, V> values)
        {
            if (KVCacheVisitorName != null)
            {
                var kv = await getVisitor();
                await kv.SetHash(key, values);
            }
        }

        public void SetHashSync<K, V>(K key, IDictionary<string, V> values)
        {
            if (KVCacheVisitorName != null)
            {
                var kv = getVisitorSync();
                kv.SetHashSync(key, values);
            }
        }
    }
}
