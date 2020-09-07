using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Cache
{
    public class KVCacheVisitor : EntityBase<IKVCacheVisitorIMP>
    {
        private static IFactory<IKVCacheVisitorIMP> _kvCacheVisitorIMPFactory;

        public static IFactory<IKVCacheVisitorIMP> KVCacheVisitorIMPFactory
        {
            set
            {
                _kvCacheVisitorIMPFactory = value;
            }
        }
        public override IFactory<IKVCacheVisitorIMP> GetIMPFactory()
        {
            return _kvCacheVisitorIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }

        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheType
        {
            get
            {
                return GetAttribute<string>("CacheType");
            }
            set
            {
                SetAttribute<string>("CacheType", value);
            }
        }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public string CacheConfiguration
        {
            get
            {
                return GetAttribute<string>("CacheConfiguration");
            }
            set
            {
                SetAttribute<string>("CacheConfiguration", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        public async Task<(V,bool)> Get<K, V>( Func<K, Task<(V, bool)>> creator, K key)
        {
            return await _imp.Get<K, V>(this,creator, key);
        }
        public  (V,bool) GetSync<K, V>( Func<K, (V, bool)> creator, K key)
        {
            return _imp.GetSync<K, V>(this, creator, key);
        }

        public async Task Set<K, V>( K key, V value)
        {
            await _imp.Set(this, key, value);
        }

        public void SetSync<K, V>(K key, V value)
        {
            _imp.SetSync(this,key, value);
        }

        public async Task Clear<K, V>(K key)
        {
            await _imp.Clear<K, V>(this, key);
        }
        public void ClearSync<K, V>( K key)
        {
            _imp.ClearSync<K, V>(this, key);
        }

    }

    public interface IKVCacheVisitorIMP
    {
        Task<(V,bool)> Get<K,V>(KVCacheVisitor visito,Func<K,Task<(V,bool)>> creator,K key);
        (V,bool) GetSync<K, V>(KVCacheVisitor visitor,Func<K, (V, bool)> creator, K key);

        Task Set<K, V>(KVCacheVisitor visitor, K key, V value);
        void SetSync<K, V>(KVCacheVisitor visitor, K key, V value);

        Task Clear<K,V>(KVCacheVisitor visitor, K key);
        void ClearSync<K,V>(KVCacheVisitor visitor, K key);

    }

    [Injection(InterfaceType = typeof(IKVCacheVisitorIMP), Scope = InjectionScope.Transient)]
    public class KVCacheVisitorIMP : IKVCacheVisitorIMP
    {
        private static Dictionary<string, IFactory<IRealKVCacheVisitService>> _realKVCacheVisitServiceFactories = new Dictionary<string, IFactory<IRealKVCacheVisitService>>();

        /// <summary>
        /// 实际KV缓存访问服务工厂键值对
        /// 键为缓存类型
        /// </summary>
        public static IDictionary<string, IFactory<IRealKVCacheVisitService>> RealKVCacheVisitServiceFactories
        {
            get
            {
                return _realKVCacheVisitServiceFactories;
            }
        }

        public async Task<(V,bool)> Get<K, V>(KVCacheVisitor visitor,Func<K, Task<(V, bool)>> creator, K key)
        {
            var prefix=GetPrefix(visitor.Name, typeof(K), typeof(V));
            var realService = getRealService(visitor.CacheType);

            return await realService.Get(visitor.CacheConfiguration,
                async () =>
                {
                    return await creator(key);
                }
            ,prefix,key);
          
        }

        public (V,bool) GetSync<K, V>(KVCacheVisitor visitor,Func<K, (V, bool)> creator, K key)
        {
            var prefix = GetPrefix(visitor.Name, typeof(K), typeof(V));
            var realService = getRealService(visitor.CacheType);

            return realService.GetSync(visitor.CacheConfiguration,
                 () =>
                {
                    return creator(key);
                }
            , prefix, key);
        }

        private string GetPrefix(string name, Type keyType,Type valueType)
        {
            return $"{name}_{keyType.FullName}_{valueType.FullName}";
        }

        public IRealKVCacheVisitService getRealService(string type)
        {
            if (!_realKVCacheVisitServiceFactories.TryGetValue(type, out IFactory<IRealKVCacheVisitService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRealKVCacheVisitServiceByCacheType,
                    DefaultFormatting = "找不到缓存类型为{0}的实际KV缓存访问服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.RealKVCacheVisitServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundRealKVCacheVisitServiceByCacheType, fragment);
            }

            return serviceFactory.Create();

        }

        public async Task Set<K, V>(KVCacheVisitor visitor, K key, V value)
        {
            var prefix = GetPrefix(visitor.Name, typeof(K), typeof(V));
            var realService = getRealService(visitor.CacheType);
            await realService.Set(visitor.CacheConfiguration, prefix, key, value);
        }

        public void SetSync<K, V>(KVCacheVisitor visitor, K key, V value)
        {
            var prefix = GetPrefix(visitor.Name, typeof(K), typeof(V));
            var realService = getRealService(visitor.CacheType);
            realService.SetSync(visitor.CacheConfiguration, prefix, key, value);
        }

        public async Task Clear<K,V>(KVCacheVisitor visitor, K key)
        {
            var prefix = GetPrefix(visitor.Name, typeof(K), typeof(V));
            var realService = getRealService(visitor.CacheType);
            await realService.Clear<K,V>(visitor.CacheConfiguration, prefix, key);
        }

        public void ClearSync<K,V>(KVCacheVisitor visitor, K key)
        {
            var prefix = GetPrefix(visitor.Name, typeof(K), typeof(V));
            var realService = getRealService(visitor.CacheType);
            realService.ClearSync<K, V>(visitor.CacheConfiguration, prefix, key);
        }
    }


    public interface IRealKVCacheVisitService
    {
        Task<(V,bool)> Get<K,V>(string cacheConfiguration,Func<Task<(V, bool)>> creator,string prefix, K key);
        (V,bool) GetSync<K,V>(string cacheConfiguration,Func<(V, bool)> creator,string prefix, K key);

        Task Set<K, V>(string cacheConfiguration,string prefix, K key,V value);
        void SetSync<K, V>(string cacheConfiguration, string prefix, K key,V value);

        Task Clear<K,V>(string cacheConfiguration, string prefix, K key);
        void ClearSync<K,V>(string cacheConfiguration, string prefix, K key);
    }
}
