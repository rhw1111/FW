using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using CSRedis;
using Microsoft.Extensions.Azure;

namespace MSLibrary.Redis
{

    public class RedisClientFactory : EntityBase<IRedisClientFactoryIMP>
    {
        private static IFactory<IRedisClientFactoryIMP> _redisClientFactoryIMP;

        public static IFactory<IRedisClientFactoryIMP> RedisClientFactoryIMP
        {
            set
            {
                _redisClientFactoryIMP = value;
            }
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
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }


        /// <summary>
        /// 配置
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
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

        public override IFactory<IRedisClientFactoryIMP> GetIMPFactory()
        {
            return _redisClientFactoryIMP;
        }

        public Task<CSRedisClient> GenerateClient()
        {
            return _imp.GenerateClient(this);
        }
        public CSRedisClient GenerateClientSync()
        {
            return _imp.GenerateClientSync(this);
        }
    }

    public interface IRedisClientFactoryIMP
    {
        Task<CSRedisClient> GenerateClient(RedisClientFactory endpoint);
        CSRedisClient GenerateClientSync(RedisClientFactory endpoint);
    }

    public interface IRedisClientGenerateService
    {
        Task<CSRedisClient> GenerateClient(string configuration);
        CSRedisClient GenerateClientSync(string configuration);
    }

    [Injection(InterfaceType = typeof(IRedisClientFactoryIMP), Scope = InjectionScope.Transient)]
    public class RedisClientFactoryIMP : IRedisClientFactoryIMP
    {
        public static IDictionary<string, IFactory<IRedisClientGenerateService>> RedisClientGenerateServiceFactories { get; } = new Dictionary<string, IFactory<IRedisClientGenerateService>>();

        public async Task<CSRedisClient> GenerateClient(RedisClientFactory endpoint)
        {
            var service = getService(endpoint.Type);
            return await service.GenerateClient(endpoint.Configuration);
        }

        public CSRedisClient GenerateClientSync(RedisClientFactory endpoint)
        {
            var service = getService(endpoint.Type);
            return service.GenerateClientSync(endpoint.Configuration);
        }

        private IRedisClientGenerateService getService(string type)
        {
            if (!RedisClientGenerateServiceFactories.TryGetValue(type,out IFactory<IRedisClientGenerateService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientGenerateServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Redis客户端生成服务,发生位置{1}",
                    ReplaceParameters = new List<object>() { type,$"{this.GetType().FullName}.RedisClientGenerateServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientGenerateServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }
}
