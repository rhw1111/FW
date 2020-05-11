using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Redis;
using MSLibrary.LanguageTranslate;
using CSRedis;

namespace MSLibrary.Distribute.ApplicationLockServices
{
    /// <summary>
    /// 基于Redis实现的应用程序锁服务
    /// 配置格式为
    /// {
    ///     "RedisClientFactoryName":"客户端工厂名称",
    ///     "WaitPollingMillisecond":等待锁时轮询的毫秒数
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(ApplicationLockServiceForRedis), Scope = InjectionScope.Singleton)]
    public class ApplicationLockServiceForRedis : IApplicationLockService
    {
        /// <summary>
        /// Redis客户端工厂仓储缓存代理
        /// </summary>
        private IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;

        public ApplicationLockServiceForRedis(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
        }
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="configuration">配置信息</param>
        /// <param name="lockName">资源名称</param>
        /// <param name="expireMillisecond">超时毫秒数</param>
        /// <param name="action">动作</param>
        /// <returns></returns>
        public async Task Lock(string configuration, string lockName, int expireMillisecond, int maxLockMillisecond, Func<Task> action)
        {
            var configurationObj = getConfiguration(configuration);
            var redisClientFactory = await _redisClientFactoryRepositoryCacheProxy.QueryByName(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }
            //获取Redis客户端
            var redisClient = await redisClientFactory.GenerateClient();
            //生成唯一锁内容
            string lockValue = Guid.NewGuid().ToString();
            int totalTime = 0;
            while (!await redisClient.SetAsync(lockName, lockValue, maxLockMillisecond / 1000, RedisExistence.Nx))
            {
                if (expireMillisecond >= 0)
                {
       
                    if (totalTime >= expireMillisecond)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AcquireRedisApplicationLockExpire,
                            DefaultFormatting = "请求获取Redis应用程序锁超时，请求的锁名称为{0}，Redis客户端工厂名称为{1}",
                            ReplaceParameters = new List<object>() { lockName, configurationObj.RedisClientFactoryName }
                        };

                        throw new UtilityException((int)Errors.AcquireRedisApplicationLockExpire, fragment);
                    }
                }


                await Task.Delay(configurationObj.WaitPollingMillisecond);
                if (expireMillisecond >= 0)
                {
                    totalTime += configurationObj.WaitPollingMillisecond;

                }
            }

            try
            {
                await action();
            }
            finally
            {

                var existValue = await redisClient.GetAsync(lockName);

                if (existValue == lockValue)
                {
                    await redisClient.DelAsync(lockName);
                }

            }



        }

        public void LockSync(string configuration, string lockName, int expireMillisecond, int maxLockMillisecond, Action action)
        {
            var configurationObj = getConfiguration(configuration);
            var redisClientFactory = _redisClientFactoryRepositoryCacheProxy.QueryByNameSync(configurationObj.RedisClientFactoryName);

            var redisClient = redisClientFactory.GenerateClientSync();
            string lockValue = Guid.NewGuid().ToString();
            int totalTime = 0;
            while (redisClient.Set(lockName, lockValue, maxLockMillisecond / 1000, RedisExistence.Nx))
            {              
                if (expireMillisecond >= 0)
                {
                    
                    if (totalTime >= expireMillisecond)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.AcquireRedisApplicationLockExpire,
                            DefaultFormatting = "请求获取Redis应用程序锁超时，请求的锁名称为{0}，Redis客户端工厂名称为{1}",
                            ReplaceParameters = new List<object>() { lockName, configurationObj.RedisClientFactoryName }
                        };

                        throw new UtilityException((int)Errors.AcquireRedisApplicationLockExpire, fragment);
                    }
                }


                System.Threading.Thread.Sleep(configurationObj.WaitPollingMillisecond);
                if (expireMillisecond >= 0)
                {
                    totalTime += configurationObj.WaitPollingMillisecond;

                }

            }

            try
            {
                action();
            }
            finally
            {

                var existValue = redisClient.Get(lockName);

                if (existValue == lockValue)
                {
                    redisClient.Del(lockName);
                }

            }

        }

        private Configuration getConfiguration(string configuration)
        {
            var configurationObj=JsonSerializerHelper.Deserialize<Configuration>(configuration);
            return configurationObj;
        }

        [DataContract]
        private class Configuration
        {
            /// <summary>
            /// redis客户端工厂名称
            /// </summary>
            [DataMember]
            public string RedisClientFactoryName { get; set; }
            /// <summary>
            /// 等待锁时轮询的毫秒数
            /// </summary>
            [DataMember]
            public int WaitPollingMillisecond { get; set; } = 200;
        }
    }
}
