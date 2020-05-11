using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Redis.DAL
{
    /// <summary>
    /// Redis客户端工厂数据操作
    /// </summary>
    public interface IRedisClientFactoryStore
    {
        Task<RedisClientFactory> QueryByName(string name);

        RedisClientFactory QueryByNameSync(string name);
    }
}
