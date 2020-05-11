using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;
using MSLibrary;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    /// <summary>
    /// 认证刷新令牌
    /// </summary>
    public class IdentityRefreshToken : EntityBase<IIdentityRefreshTokenIMP>
    {
        private static IFactory<IIdentityRefreshTokenIMP>? _identityRefreshTokenIMPFactory;

        public static IFactory<IIdentityRefreshTokenIMP>? IdentityRefreshTokenIMPFactory
        {
            set
            {
                _identityRefreshTokenIMPFactory = value;

            }
        }
        public override IFactory<IIdentityRefreshTokenIMP>? GetIMPFactory()
        {
            return _identityRefreshTokenIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        public string Handle
        {
            get
            {

                return GetAttribute<string>(nameof(Handle));
            }
            set
            {
                SetAttribute<string>(nameof(Handle), value);
            }
        }

        public string SubjectId
        {
            get
            {

                return GetAttribute<string>(nameof(SubjectId));
            }
            set
            {
                SetAttribute<string>(nameof(SubjectId), value);
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

        public IdentityToken AccessToken
        {
            get
            {

                return GetAttribute<IdentityToken>(nameof(AccessToken));
            }
            set
            {
                SetAttribute<IdentityToken>(nameof(AccessToken), value);
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
        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }
        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }

        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }

        public async Task<RefreshToken> GenerateRefreshToken( CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateRefreshToken(this, cancellationToken);
        }
    }

    public interface IIdentityRefreshTokenIMP
    {
        Task Add(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default);
        Task Update(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default);
        Task Delete(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken> GenerateRefreshToken(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IIdentityRefreshTokenIMP), Scope = InjectionScope.Transient)]
    public class IdentityRefreshTokenIMP : IIdentityRefreshTokenIMP
    {
        private readonly IIdentityRefreshTokenStore _identityRefreshTokenStore;

        public IdentityRefreshTokenIMP(IIdentityRefreshTokenStore identityRefreshTokenStore)
        {
            _identityRefreshTokenStore = identityRefreshTokenStore;
        }
        public async Task Add(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default)
        {
            await _identityRefreshTokenStore.Add(identityRefreshToken,cancellationToken);
        }

        public async Task Delete(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default)
        {
            await _identityRefreshTokenStore.Delete(identityRefreshToken.ID, cancellationToken);
        }

        public async Task<RefreshToken> GenerateRefreshToken(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                AccessToken = await identityRefreshToken.AccessToken.GenerateRefreshToken(cancellationToken),
               
                CreationTime = identityRefreshToken.CreationTime,
                Lifetime = identityRefreshToken.Lifetime,
                Version = identityRefreshToken.Version
            };
            return refreshToken;
        }

        public async Task Update(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default)
        {
            await _identityRefreshTokenStore.Update(identityRefreshToken, cancellationToken);
        }
    }
}
