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

namespace MSLibrary.Distribute.ApplicationLimitServices
{
    /// <summary>
    /// 基于Redis令牌桶实现的应用程序限流服务
    /// 配置格式为
    /// {
    ///     "RedisClientFactoryName":"客户端工厂名称",
    ///     "PerWaitMillisecond":当没有令牌时，每次等待的时间间隔
    ///     "TokenDurationMillisecond":令牌生成周期，达到该周期才会生成令牌，
    ///     "TokenDurationNumber":令牌周期数量，每周期生成的令牌数,
    ///     "TokenMax":令牌桶中最大容量
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(ApplicationLimitServiceForRedisToken), Scope = InjectionScope.Singleton)]
    public class ApplicationLimitServiceForRedisToken : IApplicationLimitService
    {
        private const string _datetimeField = "Datetime";
        private const string _count = "Count";
        private const string _version = "Version";

        /// <summary>
        /// Redis客户端工厂仓储缓存代理
        /// </summary>
        private IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;

        public ApplicationLimitServiceForRedisToken(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
        }

        public async Task Acquire(string configuration, string resourceName, int expireMillisecond)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);
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

            while (true)
            {
                //尝试在Redis生成Hash键，确保Redis中有值
                await redisClient.HSetNxAsync(resourceName, _datetimeField, DateTime.UtcNow.Ticks);
                await redisClient.HSetNxAsync(resourceName, _count, configurationObj.TokenMax);
                await redisClient.HSetNxAsync(resourceName, _version, Guid.NewGuid().ToString());

                //计算当前时刻令牌桶中应该有的数量
                var dict = await redisClient.HGetAllAsync(resourceName);
                var dictDatetime = new DateTime(long.Parse(dict[_datetimeField]));
                var dictCount = int.Parse(dict[_count]);
                var now = DateTime.UtcNow;
                var shouldCount = ((long)(now - dictDatetime).TotalMilliseconds / configurationObj.TokenDurationMillisecond) * configurationObj.TokenDurationNumber;
                if (shouldCount + dictCount > configurationObj.TokenMax)
                {
                    shouldCount = configurationObj.TokenMax - dictCount;
                }

                string strLua = getLua();


                var execResult = (int)await redisClient.EvalAsync(strLua, resourceName, (int)shouldCount, now.Ticks, dict[_version], Guid.NewGuid().ToString());

                int waitMillisecond = 0;
                if (execResult == 1)
                {
                    break;
                }
                else
                {
                    if (expireMillisecond>0)
                    {                  
                        if (waitMillisecond>= expireMillisecond)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.AcquireRedisLimitTokenExpire,
                                DefaultFormatting = "从Redis限流令牌桶中获取令牌超时，请求的令牌桶名称为{0}，Redis客户端工厂名称为{1}",
                                ReplaceParameters = new List<object>() { resourceName, configurationObj.RedisClientFactoryName }
                            };

                            throw new UtilityException((int)Errors.AcquireRedisLimitTokenExpire, fragment);
                        }
                    }
                    
                    await Task.Delay(expireMillisecond);

                    if (expireMillisecond > 0)
                    {
                        waitMillisecond += expireMillisecond;
                    }
                }
            }

        }

        public void AcquireSync(string configuration, string resourceName, int expireMillisecond)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);
            var redisClientFactory = _redisClientFactoryRepositoryCacheProxy.QueryByNameSync(configurationObj.RedisClientFactoryName);
            //获取Redis客户端
            var redisClient =  redisClientFactory.GenerateClientSync();

            while (true)
            {
                //尝试在Redis生成Hash键，确保Redis中有值
                 redisClient.HSetNx(resourceName, _datetimeField, DateTime.UtcNow.Ticks);
                 redisClient.HSetNx(resourceName, _count, configurationObj.TokenMax);
                 redisClient.HSetNx(resourceName, _version, Guid.NewGuid().ToString());

                //计算当前时刻令牌桶中应该有的数量
                var dict =  redisClient.HGetAll(resourceName);
                var dictDatetime = new DateTime(long.Parse(dict[_datetimeField]));
                var dictCount = int.Parse(dict[_count]);
                var now = DateTime.UtcNow;
                var shouldCount = ((long)(now - dictDatetime).TotalMilliseconds / configurationObj.TokenDurationMillisecond) * configurationObj.TokenDurationNumber;
                if (shouldCount + dictCount > configurationObj.TokenMax)
                {
                    shouldCount = configurationObj.TokenMax - dictCount;
                }

                string strLua = getLua();


                var execResult = (int) redisClient.Eval(strLua, resourceName, (int)shouldCount, now.Ticks, dict[_version], Guid.NewGuid().ToString());

                int waitMillisecond = 0;
                if (execResult == 1)
                {
                    break;
                }
                else
                {
                    if (expireMillisecond > 0)
                    {
                        if (waitMillisecond >= expireMillisecond)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.AcquireRedisLimitTokenExpire,
                                DefaultFormatting = "从Redis限流令牌桶中获取令牌超时，请求的令牌桶名称为{0}，Redis客户端工厂名称为{1}",
                                ReplaceParameters = new List<object>() { resourceName, configurationObj.RedisClientFactoryName }
                            };

                            throw new UtilityException((int)Errors.AcquireRedisLimitTokenExpire, fragment);
                        }
                    }

                    System.Threading.Thread.Sleep(expireMillisecond);

                    if (expireMillisecond > 0)
                    {
                        waitMillisecond += expireMillisecond;
                    }
                }
            }
        }


        private string getLua()
        {

            string strLua = @$"
                    local version= redis.call('HGET',KEYS[1],'{_version}')
                    local ccount= redis.call('HGET',KEYS[1],'{_count}')
                    local cversion=ARGV[3]
                    local count=ARGV[1]
                    if (version==cversion)
                    then
                        if (count>0)
                        then
                            redis.call('HMSET',KEYS[1],'{_datetimeField}',ARGV[2],'{_count}',ccount+count-1,'{_version}',ARGV[4]) 
                            return 1
                        end                                  
                    end
                    
                    if (ccount>0)
                    then
                            redis.call('HMSET',KEYS[1],'{_count}',ccount-1) 
                            return 1   
                    else
                            return 0
                    end
                ";
            return strLua;
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
            /// 令牌生成周期
            /// </summary>
            [DataMember]
            public int TokenDurationMillisecond { get; set; }

            /// <summary>
            ///令牌周期数量
            /// </summary>
            [DataMember]
            public int TokenDurationNumber { get; set; }

            /// <summary>
            ///令牌桶中最大容量
            /// </summary>
            [DataMember]
            public int TokenMax { get; set; }

        }
    }
}
