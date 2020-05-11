using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Redis
{
    public interface IRedisClientFactoryRepository
    {
        Task<RedisClientFactory> QueryByName(string name);
        RedisClientFactory QueryByNameSync(string name);

    }
}
