using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;
using MSLibrary;
using MSLibrary.DI;

namespace IdentityCenter.Main.IdentityServer
{
    /// <summary>
    /// 认证令牌
    /// </summary>
    public class IdentityToken : EntityBase<IIdentityTokenIMP>
    {
        private static IFactory<IIdentityTokenIMP>? _identityTokenIMPFactory;

        public static IFactory<IIdentityTokenIMP> IdentityTokenIMPFactory
        {
            set
            {
                _identityTokenIMPFactory = value;
            }
        }
        public override IFactory<IIdentityTokenIMP>? GetIMPFactory()
        {
            return _identityTokenIMPFactory;
        }


        public List<string> Audiences
        {
            get
            {

                return GetAttribute<List<string>>(nameof(Audiences));
            }
            set
            {
                SetAttribute<List<string>> (nameof(Audiences), value);
            }
        }

        public string Issuer
        {
            get
            {

                return GetAttribute<string>(nameof(Issuer));
            }
            set
            {
                SetAttribute<string>(nameof(Issuer), value);
            }
        }

        public DateTime CreationTime
        {
            get
            {

                return GetAttribute<DateTime>(nameof(CreationTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreationTime), value);
            }
        }

        public int Lifetime
        {
            get
            {

                return GetAttribute<int>(nameof(Lifetime));
            }
            set
            {
                SetAttribute<int>(nameof(Lifetime), value);
            }
        }

        public string Type
        {
            get
            {

                return GetAttribute<string>(nameof(Type));
            }
            set
            {
                SetAttribute<string>(nameof(Type), value);
            }
        }

        public string ClientId
        {
            get
            {

                return GetAttribute<string>(nameof(ClientId));
            }
            set
            {
                SetAttribute<string>(nameof(ClientId), value);
            }
        }

        public List<string> Claims
        {
            get
            {

                return GetAttribute<List<string>>(nameof(Claims));
            }
            set
            {
                SetAttribute<List<string>>(nameof(Claims), value);
            }
        }

        public int Version
        {
            get
            {

                return GetAttribute<int>(nameof(Version));
            }
            set
            {
                SetAttribute<int>(nameof(Version), value);
            }
        }

        public async Task<Token> GenerateRefreshToken(CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateRefreshToken(this, cancellationToken);
        }

    }

    public interface IIdentityTokenIMP
    {
        Task<Token> GenerateRefreshToken(IdentityToken identityToken, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IIdentityTokenIMP), Scope = InjectionScope.Transient)]
    public class IdentityTokenIMP : IIdentityTokenIMP
    {
        public async Task<Token> GenerateRefreshToken(IdentityToken identityToken, CancellationToken cancellationToken = default)
        {
            List<Claim> claims = new List<Claim>();
            foreach(var item in  identityToken.Claims)
            {
                claims.Add(await ClaimExtension.CreateFromBinaryData(item));
            }

            Token token = new Token()
            {
                AccessTokenType = AccessTokenType.Jwt,
                Audiences = identityToken.Audiences,
                Claims = claims,
                ClientId = identityToken.ClientId,
                CreationTime = identityToken.CreationTime,
                Issuer = identityToken.Issuer,
                Lifetime = identityToken.Lifetime,
                Type = identityToken.Type,
                Version = identityToken.Version

            };

            return token;
        }
    }
}
