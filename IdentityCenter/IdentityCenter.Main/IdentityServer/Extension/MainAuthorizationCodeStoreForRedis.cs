using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.Serialization;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using MSLibrary.Redis;
using MSLibrary.Serializer;
using CSRedis;


namespace IdentityCenter.Main.IdentityServer.Extension
{
    public class MainAuthorizationCodeStoreForRedis : IAuthorizationCodeStore
    {
        public static string RedisPrefix { get; set; } = "AuthorizationCode_";

        private readonly IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;
        private readonly IIdentityAuthorizationCodeFactory _identityAuthorizationCodeFactory;
        public MainAuthorizationCodeStoreForRedis(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy, IIdentityAuthorizationCodeFactory identityAuthorizationCodeFactory)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
            _identityAuthorizationCodeFactory = identityAuthorizationCodeFactory;
        }

        public async Task<AuthorizationCode> GetAuthorizationCodeAsync(string code)
        {
            var redisClient = await getRedisClient();
            var strData = await redisClient.GetAsync($"{RedisPrefix}{code}");
            if (strData == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityAuthorizationCodeByCode,
                    DefaultFormatting = "找不到Code为{0}的IdentityAuthorizationCode",
                    ReplaceParameters = new List<object>() { code }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityAuthorizationCodeByCode, fragment, 1, 0);
            }

            var identityAuthorizationCode=await _identityAuthorizationCodeFactory.Create(strData);

            return await identityAuthorizationCode.GenerateAuthorizationCode();
        }

        public async Task RemoveAuthorizationCodeAsync(string code)
        {
            var redisClient = await getRedisClient();
            await redisClient.DelAsync($"{RedisPrefix}{code}");
        }

        public async Task<string> StoreAuthorizationCodeAsync(AuthorizationCode code)
        {
            string strCode = Guid.NewGuid().ToString();

            var properties = new Dictionary<string, string>();
            foreach (var item in code.Properties)
            {
                properties[item.Key] = item.Value;
            }

            IdentityAuthorizationCode authorizationCode = new IdentityAuthorizationCode()
            {
                ClientId = code.ClientId,
                CodeChallenge = code.CodeChallenge,
                CodeChallengeMethod = code.CodeChallengeMethod,
                CreationTime = code.CreationTime,
                IsOpenId = code.IsOpenId,
                Lifetime = code.Lifetime,
                Nonce = code.Nonce,
                Properties = properties,
                RedirectUri = code.RedirectUri,
                RequestedScopes = code.RequestedScopes.ToArray(),
                SessionId = code.SessionId,
                StateHash = code.StateHash,
                SubjectData = await code.Subject.GetBinaryData(),
                WasConsentShown = code.WasConsentShown
            };

            var redisClient = await getRedisClient();
            await redisClient.SetAsync($"{RedisPrefix}{strCode}", await authorizationCode.GetSerializeData(), code.Lifetime + 10);
            return strCode;
        }

        private async Task<CSRedisClient> getRedisClient()
        {
            var redisClientFactory =await _redisClientFactoryRepositoryCacheProxy.QueryByName(ExtensionConsts.IdentityServerRedisName);
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
