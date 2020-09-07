using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Redis;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using CSRedis;
using System.Runtime.CompilerServices;

namespace MSLibrary.VerificationCode.VerificationCodeServices
{
    /// <summary>
    /// 基于Redis的验证码服务
    /// Configuration格式为
    /// {
    ///     "Name":"RedisClientFactory的名称",
    ///     "CodeExpire":验证码的有效时间（毫秒）,
    ///     "KeyPrefix":"Redis中存验证码的key的前缀",
    ///     "CodeRanValue":"验证码中随机数来源，将从该字符串中随机取字符",
    ///     "CodeLen":验证码长度
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(VerificationCodeServiceForRedis), Scope = InjectionScope.Singleton)]
    public class VerificationCodeServiceForRedis : IVerificationCodeService
    {
     
        private readonly IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;

        public VerificationCodeServiceForRedis(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
        }
        public async Task DeleteCode(string configuration, string identity)
        {
            var config = JsonSerializerHelper.Deserialize<configurationData>(configuration);
            var redisClient =await getRedisClient(config.Name);
            redisClient.DelAsync($"{config.KeyPrefix}_{identity}");
            
        }

        public async Task<string> GenerateCode(string configuration)
        {
            var config = JsonSerializerHelper.Deserialize<configurationData>(configuration);
            return config.CodeRanValue.GetRanValue(config.CodeRanValue.Length);
        }

        public async Task<string?> GetLatestCode(string configuration, string identity)
        {
            var config = JsonSerializerHelper.Deserialize<configurationData>(configuration);
            var redisClient = await getRedisClient(config.Name);

            var strValue=await redisClient.GetAsync($"{config.KeyPrefix}_{identity}");
            if (strValue==null)
            {
                return null;
            }

            var codeContainer = JsonSerializerHelper.Deserialize<codeContainer>(strValue);

            return codeContainer.Code;
        }

        public async Task<DateTime?> GetLatestCodeTime(string configuration, string identity)
        {
            var config = JsonSerializerHelper.Deserialize<configurationData>(configuration);
            var redisClient = await getRedisClient(config.Name);

            var strValue = await redisClient.GetAsync($"{config.KeyPrefix}_{identity}");
            if (strValue == null)
            {
                return null;
            }

            var codeContainer = JsonSerializerHelper.Deserialize<codeContainer>(strValue);

            return codeContainer.Time;
        }

        public async Task<bool> SaveCode(string configuration, string identity, string code)
        {
            var config = JsonSerializerHelper.Deserialize<configurationData>(configuration);
            var redisClient = await getRedisClient(config.Name);
            string strValue = JsonSerializerHelper.Serializer(new codeContainer()
            {
                 Code= code,
                 Time=DateTime.UtcNow
            });
            return await redisClient.SetAsync($"{config.KeyPrefix}_{identity}", strValue, config.CodeExpire, RedisExistence.Nx);
        }

        private async Task<CSRedisClient> getRedisClient(string name)
        {
            var clientFactory=await _redisClientFactoryRepositoryCacheProxy.QueryByName(name);
            if (clientFactory==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            return await clientFactory.GenerateClient();
        }

        

        [DataContract]
        private class configurationData
        {
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public int CodeExpire { get; set; }
            [DataMember]
            public string KeyPrefix { get; set; }
            [DataMember]
            public string CodeRanValue { get; set; }
            [DataMember]
            public int CodeLen { get; set; }
        }
        
        [DataContract]
        private class codeContainer
        {
            [DataMember]
            public string Code { get; set; }
            [DataMember]
            public DateTime Time { get; set; }
        }
    }

    
}
