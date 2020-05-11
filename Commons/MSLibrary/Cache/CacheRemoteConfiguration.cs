using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Cache.DAL;
using MSLibrary.DI;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 远程缓存设置
    /// 用于分布式缓存管理
    /// </summary>
    public class CacheRemoteConfiguration : EntityBase<ICacheRemoteConfigurationIMP>
    {
        private static IFactory<ICacheRemoteConfigurationIMP> _cacheRemoteConfigurationIMPFactory;

        public static IFactory<ICacheRemoteConfigurationIMP> CacheRemoteConfigurationIMPFactory
        {
            set
            {
                _cacheRemoteConfigurationIMPFactory = value;
            }
        }

        public override IFactory<ICacheRemoteConfigurationIMP> GetIMPFactory()
        {
            return _cacheRemoteConfigurationIMPFactory;
        }

        /// <summary>
        /// id
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
        /// 远程地址列表（以;分隔）
        /// </summary>
        public string RemoteAddresses
        {
            get
            {
                return GetAttribute<string>("RemoteAddresses");
            }
            set
            {
                SetAttribute<string>("RemoteAddresses", value);
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

        public async Task Add()
        {
            await _imp.Add(this);
        }


        public async Task Update()
        {
            await _imp.Update(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }
    }


    public interface ICacheRemoteConfigurationIMP
    {
        Task Add(CacheRemoteConfiguration configuration);
        Task Update(CacheRemoteConfiguration configuration);
        Task Delete(CacheRemoteConfiguration configuration);
    }

    [Injection(InterfaceType = typeof(ICacheRemoteConfigurationIMP), Scope = InjectionScope.Transient)]
    public class CacheRemoteConfigurationIMP : ICacheRemoteConfigurationIMP
    {
        private ICacheRemoteConfigurationStore _cacheRemoteConfigurationStore;
        public CacheRemoteConfigurationIMP(ICacheRemoteConfigurationStore cacheRemoteConfigurationStore)
        {
            _cacheRemoteConfigurationStore = cacheRemoteConfigurationStore;
        }
        public async Task Add(CacheRemoteConfiguration configuration)
        {
            await _cacheRemoteConfigurationStore.Add(configuration);
        }

        public async Task Delete(CacheRemoteConfiguration configuration)
        {
            await _cacheRemoteConfigurationStore.Delete(configuration.ID);
        }

        public async Task Update(CacheRemoteConfiguration configuration)
        {
            await _cacheRemoteConfigurationStore.Update(configuration);
        }
    }
}
