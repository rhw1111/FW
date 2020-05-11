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
    public class IdentityAuthorizationCode : EntityBase<IIdentityAuthorizationCodeIMP>
    {
        private static IFactory<IIdentityAuthorizationCodeIMP>? _identityAuthorizationCodeIMPFactory;

        public static IFactory<IIdentityAuthorizationCodeIMP> IdentityAuthorizationCodeIMPFactory
        {
            set
            {
                _identityAuthorizationCodeIMPFactory = value;
            }
        }

        public override IFactory<IIdentityAuthorizationCodeIMP>? GetIMPFactory()
        {
            return _identityAuthorizationCodeIMPFactory;
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

        /// <summary>
        /// Code
        /// </summary>
        public string Code
        {
            get
            {

                return GetAttribute<string>(nameof(Code));
            }
            set
            {
                SetAttribute<string>(nameof(Code), value);
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

        public string SubjectData
        {
            get
            {

                return GetAttribute<string>(nameof(SubjectData));
            }
            set
            {
                SetAttribute<string>(nameof(SubjectData), value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
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

        /// <summary>
        /// 有效期（秒）
        /// </summary>
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

        public bool IsOpenId
        {
            get
            {

                return GetAttribute<bool>(nameof(IsOpenId));
            }
            set
            {
                SetAttribute <bool>(nameof(IsOpenId), value);
            }
        }

        public string[] RequestedScopes
        {
            get
            {

                return GetAttribute<string[]>(nameof(RequestedScopes));
            }
            set
            {
                SetAttribute<string[]>(nameof(RequestedScopes), value);
            }
        }

        public string RedirectUri
        {
            get
            {

                return GetAttribute<string>(nameof(RedirectUri));
            }
            set
            {
                SetAttribute<string>(nameof(RedirectUri), value);
            }
        }
        
        public string Nonce
        {
            get
            {

                return GetAttribute<string>(nameof(Nonce));
            }
            set
            {
                SetAttribute<string>(nameof(Nonce), value);
            }
        }

        public string StateHash
        {
            get
            {

                return GetAttribute<string>(nameof(StateHash));
            }
            set
            {
                SetAttribute<string>(nameof(StateHash), value);
            }
        }

        public bool WasConsentShown
        {
            get
            {

                return GetAttribute<bool>(nameof(WasConsentShown));
            }
            set
            {
                SetAttribute<bool>(nameof(WasConsentShown), value);
            }
        }

        public string SessionId
        {
            get
            {

                return GetAttribute<string>(nameof(SessionId));
            }
            set
            {
                SetAttribute<string>(nameof(SessionId), value);
            }
        }

        public string CodeChallenge
        {
            get
            {

                return GetAttribute<string>(nameof(CodeChallenge));
            }
            set
            {
                SetAttribute<string>(nameof(CodeChallenge), value);
            }
        }
        public string CodeChallengeMethod
        {
            get
            {

                return GetAttribute<string>(nameof(CodeChallengeMethod));
            }
            set
            {
                SetAttribute<string>(nameof(CodeChallengeMethod), value);
            }
        }

        public Dictionary<string,string> Properties
        {
            get
            {

                return GetAttribute<Dictionary<string, string>>(nameof(Properties));
            }
            set
            {
                SetAttribute<Dictionary<string, string>>(nameof(Properties), value);
            }
        }

        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }
        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }


        /// <summary>
        /// 生成AuthorizationCode
        /// </summary>
        /// <returns></returns>
        public async Task<AuthorizationCode> GenerateAuthorizationCode(CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateAuthorizationCode(this, cancellationToken);
        }
    }

    public interface IIdentityAuthorizationCodeIMP
    {
        Task Add(IdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);
        Task Delete(IdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);
        Task<AuthorizationCode> GenerateAuthorizationCode(IdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IIdentityAuthorizationCodeIMP), Scope = InjectionScope.Transient)]
    public class IdentityAuthorizationCodeIMP : IIdentityAuthorizationCodeIMP
    {
        private readonly IIdentityAuthorizationCodeStore _identityAuthorizationCodeStore;

        public IdentityAuthorizationCodeIMP(IIdentityAuthorizationCodeStore identityAuthorizationCodeStore)
        {
            _identityAuthorizationCodeStore = identityAuthorizationCodeStore;
        }
        public async Task Add(IdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
        {
            await _identityAuthorizationCodeStore.Add(authorizationCode, cancellationToken);
        }

        public async Task Delete(IdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
        {
            await _identityAuthorizationCodeStore.Delete(authorizationCode.ID, cancellationToken);
        }

        public async Task<AuthorizationCode> GenerateAuthorizationCode(IdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
        {
            AuthorizationCode code = new AuthorizationCode();
            code.ClientId = authorizationCode.ClientId;
            code.CodeChallenge = authorizationCode.CodeChallenge;
            code.CodeChallengeMethod = authorizationCode.CodeChallengeMethod;
            code.CreationTime = authorizationCode.CreationTime;
            code.IsOpenId = authorizationCode.IsOpenId;
            code.Lifetime = authorizationCode.Lifetime;
            code.Nonce = authorizationCode.Nonce;
            code.Properties = authorizationCode.Properties;
            code.RedirectUri = authorizationCode.RedirectUri;
            code.RequestedScopes = authorizationCode.RequestedScopes;
            code.SessionId = authorizationCode.SessionId;
            code.StateHash = authorizationCode.StateHash;
            code.Subject =await ClaimsPrincipalExtension.CreateFromBinaryData(authorizationCode.SubjectData);
            code.WasConsentShown = authorizationCode.WasConsentShown;

            return await Task.FromResult(code);
        }
    }
}
