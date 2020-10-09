using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using IdentityServer4.Models;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Serializer;
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

        public DateTime? ConsumedTime
        {
            get
            {

                return GetAttribute<DateTime?>(nameof(ConsumedTime));
            }
            set
            {
                SetAttribute<DateTime?>(nameof(ConsumedTime), value);
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

        public async Task<RefreshToken> GenerateRefreshToken(CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateRefreshToken(this, cancellationToken);
        }

        public async Task<string> GetSerializeData()
        {
            return await _imp.GetSerializeData(this);
        }

    }

    public interface IIdentityRefreshTokenIMP
    {
        Task Add(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default);
        Task Update(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default);
        Task Delete(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken> GenerateRefreshToken(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default);

        Task<string> GetSerializeData(IdentityRefreshToken identityRefreshToken);
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
                ConsumedTime = identityRefreshToken.ConsumedTime,
                CreationTime = identityRefreshToken.CreationTime,
                Lifetime = identityRefreshToken.Lifetime,
                Version = identityRefreshToken.Version
            };
            return refreshToken;
        }

        public async Task<string> GetSerializeData(IdentityRefreshToken identityRefreshToken)
        {
            RefreshTokenData data = new RefreshTokenData()
            {
                ID = identityRefreshToken.ID,
                Handle = identityRefreshToken.Handle,
                ConsumedTime = identityRefreshToken.ConsumedTime,
                Lifetime = identityRefreshToken.Lifetime,
                CreationTime = identityRefreshToken.CreationTime,
                Version = identityRefreshToken.Version,
                AccessToken = new TokenData()
                {
                    AccessTokenType = identityRefreshToken.AccessToken.AccessTokenType,
                    AllowedSigningAlgorithms = identityRefreshToken.AccessToken.AllowedSigningAlgorithms,
                    Audiences = identityRefreshToken.AccessToken.Audiences,
                    Claims = identityRefreshToken.AccessToken.Claims,
                    ClientId = identityRefreshToken.AccessToken.ClientId,
                    Confirmation = identityRefreshToken.AccessToken.Confirmation,
                    CreationTime = identityRefreshToken.AccessToken.CreationTime,
                    Issuer = identityRefreshToken.AccessToken.Issuer,
                    Lifetime = identityRefreshToken.AccessToken.Lifetime,
                    Type = identityRefreshToken.AccessToken.Type,
                    Version = identityRefreshToken.AccessToken.Version
                }

            };

            return await Task.FromResult(JsonSerializerHelper.Serializer(data));
        }

        public async Task Update(IdentityRefreshToken identityRefreshToken, CancellationToken cancellationToken = default)
        {
            await _identityRefreshTokenStore.Update(identityRefreshToken, cancellationToken);
        }
    }

    [DataContract]
    public class TokenData
    {
        [DataMember]
        public int Version { get; set; }
        [DataMember]
        public List<string> Claims { get; set; } = null!;
        [DataMember]
        public AccessTokenType AccessTokenType { get; set; }
        [DataMember]
        public string ClientId { get; set; } = null!;

        [DataMember]
        public string Type { get; set; } = null!;

        [DataMember]
        public int Lifetime { get; set; }
        [DataMember]
        public DateTime CreationTime { get; set; }
        [DataMember]
        public string Issuer { get; set; } = null!;
        [DataMember]
        public List<string> Audiences { get; set; } = null!;
        [DataMember]
        public string Confirmation { get; set; } = null!;
        [DataMember]
        public List<string> AllowedSigningAlgorithms { get; set; } = null!;

    }
    [DataContract]
    public class RefreshTokenData
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Handle { get; set; } = null!;
        [DataMember]
        public DateTime CreationTime { get; set; }
        [DataMember]
        public int Lifetime { get; set; }
        [DataMember]
        public DateTime? ConsumedTime { get; set; }
        [DataMember]
        public TokenData AccessToken { get; set; } = null!;
        [DataMember]
        public int Version { get; set; }
    }

}
