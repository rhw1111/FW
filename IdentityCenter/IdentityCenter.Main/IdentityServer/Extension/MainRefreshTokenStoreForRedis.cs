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
    public class MainRefreshTokenStoreForRedis : IRefreshTokenStore
    {
        /// <summary>
        /// 以IdentityRefreshToken中的Handle存储，存储格式为Hash
        /// 字段如下
        /// Value：序列化的值
        /// Subject:所属的Subject的键
        /// </summary>
        public static string RedisIDPrefix { get; set; } = "RefreshTokenID_";
        /// <summary>
        /// 以IdentityRefreshToken中的SubjectID+ClientID存储，存储格式为Set
        /// 值为每个Handle的键
        /// </summary>
        public static string RedisSubjectPrefix { get; set; } = "RefreshTokenSubject_";

        private readonly IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;
        private readonly IIdentityRefreshTokenFactory _identityRefreshTokenFactory;

        public MainRefreshTokenStoreForRedis(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy, IIdentityRefreshTokenFactory identityRefreshTokenFactory)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
            _identityRefreshTokenFactory = identityRefreshTokenFactory;
        }


        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshTokenHandle)
        {
            var redisClient = await getRedisClient();
            var strRefreshTokenData =await redisClient.HGetAsync($"{RedisIDPrefix}{refreshTokenHandle}","Value");
            
            if (strRefreshTokenData == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityRefreshTokenByHandle,
                    DefaultFormatting = "找不到Handle为{0}的认证刷新令牌",
                    ReplaceParameters = new List<object>() { refreshTokenHandle }
                };
                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityAuthorizationCodeByCode, fragment, 1, 0);
            }

            var refreshToken=await _identityRefreshTokenFactory.Create(strRefreshTokenData);
            return await refreshToken.GenerateRefreshToken();
        }

        public async Task RemoveRefreshTokenAsync(string refreshTokenHandle)
        {
            var redisClient = await getRedisClient();
            var subjectID = await redisClient.HGetAsync($"{RedisIDPrefix}{refreshTokenHandle}", "Subject");
            if (subjectID!=null)
            {
                await redisClient.SRemAsync($"{RedisSubjectPrefix}{subjectID}", refreshTokenHandle);
            }
            await redisClient.DelAsync($"{RedisIDPrefix}{refreshTokenHandle}");
        }

        public async Task RemoveRefreshTokensAsync(string subjectId, string clientId)
        {
            var redisClient = await getRedisClient();
            await redisClient.DelAsync($"{RedisSubjectPrefix}{clientId}_{subjectId}");
        }

        public async Task<string> StoreRefreshTokenAsync(RefreshToken refreshToken)
        {
            var handle = Guid.NewGuid().ToString();
            IdentityRefreshToken identityRefreshToken = new IdentityRefreshToken()
            {
                ID = Guid.NewGuid(),
                CreationTime = refreshToken.CreationTime,
                Handle = handle,
                Lifetime = refreshToken.Lifetime,
                Version = refreshToken.Version,
                ConsumedTime = refreshToken.ConsumedTime,
            };

            if (refreshToken.AccessToken != null)
            {
                identityRefreshToken.AccessToken = new IdentityToken()
                {
                    ClientId = refreshToken.AccessToken.ClientId,
                    Issuer = refreshToken.AccessToken.Issuer,
                    CreationTime = refreshToken.AccessToken.CreationTime,
                    Lifetime = refreshToken.AccessToken.Lifetime,
                    Type = refreshToken.AccessToken.Type,
                    Version = refreshToken.AccessToken.Version,
                    AccessTokenType = refreshToken.AccessToken.AccessTokenType,
                    Confirmation = refreshToken.AccessToken.Confirmation

                };

                if (refreshToken.AccessToken.Claims != null)
                {
                    List<string> strClaims = new List<string>();
                    foreach (var item in refreshToken.AccessToken.Claims)
                    {
                        strClaims.Add(await item.GetBinaryData());
                    }
                    identityRefreshToken.AccessToken.Claims = strClaims;
                }
                else
                {
                    identityRefreshToken.AccessToken.Claims = new List<string>();
                }

                if (refreshToken.AccessToken.Audiences != null)
                {
                    identityRefreshToken.AccessToken.Audiences = refreshToken.AccessToken.Audiences.ToList();
                }
                else
                {
                    identityRefreshToken.AccessToken.Audiences = new List<string>();
                }

                if (refreshToken.AccessToken.AllowedSigningAlgorithms != null)
                {
                    identityRefreshToken.AccessToken.AllowedSigningAlgorithms = refreshToken.AccessToken.AllowedSigningAlgorithms.ToList();
                }
                else
                {
                    identityRefreshToken.AccessToken.AllowedSigningAlgorithms = new List<string>();
                }



            }

            var strData = await identityRefreshToken.GetSerializeData();
            var redisClient = await getRedisClient();
            await redisClient.HMSetAsync($"{RedisIDPrefix}{handle}", new string[] { "Value", strData }, new string[] { "Subject", $"{refreshToken.ClientId}_{refreshToken.SubjectId}" }, new string[] { "CreateTime", DateTime.UtcNow.Ticks.ToString() });
            await redisClient.ExpireAsync($"{RedisIDPrefix}{handle}", refreshToken.Lifetime+10);

            return handle;
        }

        public async Task UpdateRefreshTokenAsync(string handle, RefreshToken refreshToken)
        {

            IdentityRefreshToken identityRefreshToken = new IdentityRefreshToken()
            {
                ID = Guid.NewGuid(),
                CreationTime = refreshToken.CreationTime,
                Handle = handle,
                Lifetime = refreshToken.Lifetime,
                Version = refreshToken.Version,
                ConsumedTime = refreshToken.ConsumedTime,
            };

            if (refreshToken.AccessToken != null)
            {
                identityRefreshToken.AccessToken = new IdentityToken()
                {
                    ClientId = refreshToken.AccessToken.ClientId,
                    Issuer = refreshToken.AccessToken.Issuer,
                    CreationTime = refreshToken.AccessToken.CreationTime,
                    Lifetime = refreshToken.AccessToken.Lifetime,
                    Type = refreshToken.AccessToken.Type,
                    Version = refreshToken.AccessToken.Version,
                    AccessTokenType = refreshToken.AccessToken.AccessTokenType,
                    Confirmation = refreshToken.AccessToken.Confirmation

                };

                if (refreshToken.AccessToken.Claims != null)
                {
                    List<string> strClaims = new List<string>();
                    foreach (var item in refreshToken.AccessToken.Claims)
                    {
                        strClaims.Add(await item.GetBinaryData());
                    }
                    identityRefreshToken.AccessToken.Claims = strClaims;
                }
                else
                {
                    identityRefreshToken.AccessToken.Claims = new List<string>();
                }

                if (refreshToken.AccessToken.Audiences != null)
                {
                    identityRefreshToken.AccessToken.Audiences = refreshToken.AccessToken.Audiences.ToList();
                }
                else
                {
                    identityRefreshToken.AccessToken.Audiences = new List<string>();
                }

                if (refreshToken.AccessToken.AllowedSigningAlgorithms != null)
                {
                    identityRefreshToken.AccessToken.AllowedSigningAlgorithms = refreshToken.AccessToken.AllowedSigningAlgorithms.ToList();
                }
                else
                {
                    identityRefreshToken.AccessToken.AllowedSigningAlgorithms = new List<string>();
                }



            }

            var strData=await identityRefreshToken.GetSerializeData();
            var redisClient = await getRedisClient();
            await redisClient.HMSetAsync($"{RedisIDPrefix}{handle}", new string[] { "Value", strData }, new string[] { "Subject", $"{refreshToken.ClientId}_{refreshToken.SubjectId}" },new string[] { "CreateTime",DateTime.UtcNow.Ticks.ToString()});
            await redisClient.ExpireAsync($"{RedisIDPrefix}{handle}", refreshToken.Lifetime);

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
