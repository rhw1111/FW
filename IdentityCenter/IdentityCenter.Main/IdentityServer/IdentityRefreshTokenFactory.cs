using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace IdentityCenter.Main.IdentityServer
{

    [Injection(InterfaceType = typeof(IIdentityRefreshTokenFactory), Scope = InjectionScope.Singleton)]
    public class IdentityRefreshTokenFactory : IIdentityRefreshTokenFactory
    {
        public async Task<IdentityRefreshToken> Create(string serializeData)
        {
            var refreshTokenData = JsonSerializerHelper.Deserialize<RefreshTokenData>(serializeData);

            IdentityRefreshToken refreshToken = new IdentityRefreshToken()
            {
                ID = refreshTokenData.ID,
                ConsumedTime = refreshTokenData.ConsumedTime,
                CreationTime = refreshTokenData.CreationTime,
                Handle = refreshTokenData.Handle,
                Lifetime = refreshTokenData.Lifetime,
                Version = refreshTokenData.Version,
                AccessToken = new IdentityToken()
                {
                    ClientId = refreshTokenData.AccessToken.ClientId,
                    Claims = refreshTokenData.AccessToken.Claims,
                    Audiences = refreshTokenData.AccessToken.Audiences,
                    Confirmation = refreshTokenData.AccessToken.Confirmation,
                    AllowedSigningAlgorithms = refreshTokenData.AccessToken.AllowedSigningAlgorithms,
                    AccessTokenType = refreshTokenData.AccessToken.AccessTokenType,
                    CreationTime = refreshTokenData.AccessToken.CreationTime,
                    Issuer = refreshTokenData.AccessToken.Issuer,
                    Lifetime = refreshTokenData.AccessToken.Lifetime,
                    Type = refreshTokenData.AccessToken.Type,
                    Version = refreshTokenData.AccessToken.Version

                }
            };

            return await Task.FromResult(refreshToken);
        }
    }
}
