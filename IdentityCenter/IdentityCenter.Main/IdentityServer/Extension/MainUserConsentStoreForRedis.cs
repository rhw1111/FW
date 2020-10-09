using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Redis;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using IdentityCenter.Main.Entities;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using CSRedis;

namespace IdentityCenter.Main.IdentityServer.Extension
{
    public class MainUserConsentStoreForRedis : IUserConsentStore
    {

        public static string RedisPrefix { get; set; } = "UserConsent_";

        private readonly IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;
        private readonly IIdentityConsentFactory _identityConsentFactory;

        public MainUserConsentStoreForRedis(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy, IIdentityConsentFactory identityConsentFactory)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
            _identityConsentFactory = identityConsentFactory;
        }
        public async Task<Consent> GetUserConsentAsync(string subjectId, string clientId)
        {
            var redisClient = await getRedisClient();
            var strData = await redisClient.GetAsync($"{RedisPrefix}{clientId}_{subjectId}");

            if (strData == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityConsentBySubjectAndClient,
                    DefaultFormatting = "找不到SubjectId为{0}、ClientId为{1}的认证确认",
                    ReplaceParameters = new List<object>() { subjectId, clientId }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityConsentBySubjectAndClient, fragment, 1, 0);
            }
            

            var consent = await _identityConsentFactory.Create(strData);
            return await consent.GenerateConsent();
        }

        public async Task RemoveUserConsentAsync(string subjectId, string clientId)
        {
            var redisClient = await getRedisClient();
            await redisClient.DelAsync($"{RedisPrefix}{clientId}_{subjectId}");
        }

        public async Task StoreUserConsentAsync(Consent consent)
        {
            IdentityConsent identityConsent = new IdentityConsent()
            {
                ClientId = consent.ClientId,
                SubjectId = consent.SubjectId,
                ID = Guid.NewGuid(),
                CreationTime = consent.CreationTime,
                Expiration = consent.Expiration,
                Scopes = consent.Scopes.ToArray()
            };
            var redisClient = await getRedisClient();
            int expire = -1;
            if (consent.Expiration!=null)
            {
                expire = ((int)(consent.Expiration.Value - DateTime.UtcNow).TotalSeconds) + 10;
            }
            await redisClient.SetAsync($"{RedisPrefix}{consent.ClientId}_{consent.SubjectId}",await identityConsent.GetSerializeData(), expire);

        }

        private async Task<CSRedisClient> getRedisClient()
        {
            var redisClientFactory = await _redisClientFactoryRepositoryCacheProxy.QueryByName(ExtensionConsts.IdentityServerRedisName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { ExtensionConsts.IdentityServerRedisName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            return await redisClientFactory.GenerateClient();
        }


    }
}
