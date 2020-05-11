using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using System.Linq;

namespace IdentityCenter.Main.IdentityServer
{
    public class MainRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IIdentityRefreshTokenRepository _identityRefreshTokenRepository;

        public MainRefreshTokenStore(IIdentityRefreshTokenRepository identityRefreshTokenRepository)
        {
            _identityRefreshTokenRepository = identityRefreshTokenRepository;
        }
        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshTokenHandle)
        {            
            var identityRefreshToken = await _identityRefreshTokenRepository.QueryByHandle(refreshTokenHandle);
            if (identityRefreshToken==null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityRefreshTokenByHandle,
                    DefaultFormatting = "找不到Handle为{0}的认证刷新令牌",
                    ReplaceParameters = new List<object>() { refreshTokenHandle }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityRefreshTokenByHandle, fragment, 1, 0);
            }

            return await identityRefreshToken.GenerateRefreshToken();
        }

        public async Task RemoveRefreshTokenAsync(string refreshTokenHandle)
        {
            var identityRefreshToken = await _identityRefreshTokenRepository.QueryByHandle(refreshTokenHandle);
            if (identityRefreshToken != null)
            {
                await identityRefreshToken.Delete();
            }
        }

        public async Task RemoveRefreshTokensAsync(string subjectId, string clientId)
        {            
            int size = 500;
            int count = size;
            while(count==size)
            {
                var tokenList=await _identityRefreshTokenRepository.QueryBySubjectClient(subjectId, clientId, size);
                count = tokenList.Count;

                var idList = (from item in tokenList
                              select item.ID).ToList();
                if (idList.Count>0)
                {
                    await _identityRefreshTokenRepository.Delete(idList);
                }
            }
        }

        public async Task<string> StoreRefreshTokenAsync(RefreshToken refreshToken)
        {
            string strHandle = Guid.NewGuid().ToString();
            IdentityRefreshToken identityRefreshToken = new IdentityRefreshToken()
            {
                ID = Guid.NewGuid(),
                ClientId = refreshToken.ClientId,
                SubjectId = refreshToken.SubjectId,
                CreationTime = refreshToken.CreationTime,
                Handle = strHandle,
                Lifetime = refreshToken.Lifetime,
                Version = refreshToken.Version
            };

            if (refreshToken.AccessToken!=null)
            {
                identityRefreshToken.AccessToken = new IdentityToken()
                {
                    ClientId = refreshToken.AccessToken.ClientId,
                    Issuer = refreshToken.AccessToken.Issuer,
                    CreationTime = refreshToken.AccessToken.CreationTime,
                    Lifetime = refreshToken.AccessToken.Lifetime,
                    Type = refreshToken.AccessToken.Type,
                    Version = refreshToken.AccessToken.Version

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

                if (refreshToken.AccessToken.Audiences!=null)
                {
                    identityRefreshToken.AccessToken.Audiences = refreshToken.AccessToken.Audiences.ToList();
                }
            }

            await identityRefreshToken.Add();
            return strHandle;
        }

        public async Task UpdateRefreshTokenAsync(string handle, RefreshToken refreshToken)
        {
            var identityRefreshToken=await _identityRefreshTokenRepository.QueryByHandle(handle);
            if (identityRefreshToken!=null)
            {
 
                    identityRefreshToken.AccessToken = new IdentityToken()
                    {
                        ClientId = refreshToken.AccessToken.ClientId,
                        Issuer = refreshToken.AccessToken.Issuer,
                        CreationTime = refreshToken.AccessToken.CreationTime,
                        Lifetime = refreshToken.AccessToken.Lifetime,
                        Type = refreshToken.AccessToken.Type,
                        Version = refreshToken.AccessToken.Version

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

                    if (refreshToken.AccessToken.Audiences != null)
                    {
                        identityRefreshToken.AccessToken.Audiences = refreshToken.AccessToken.Audiences.ToList();
                    }
                


                identityRefreshToken.ClientId = refreshToken.ClientId;
                identityRefreshToken.SubjectId = refreshToken.SubjectId;
                identityRefreshToken.CreationTime = refreshToken.CreationTime;
                identityRefreshToken.Lifetime = refreshToken.Lifetime;
                identityRefreshToken.Version = refreshToken.Version;

                await identityRefreshToken.Update();
            }

        }
    }
}
