using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Serializer;
using IdentityCenter.Main.IdentityServer.DAL;
using System.Runtime.Serialization;

namespace IdentityCenter.Main.IdentityServer
{
    public class IdentityConsent : EntityBase<IIdentityConsentIMP>
    {
        private static IFactory<IIdentityConsentIMP>? _identityConsentIMPFactory;

        public static IFactory<IIdentityConsentIMP> IdentityConsentIMPFactory
        {
            set
            {
                _identityConsentIMPFactory = value;
            }
        }
        public override IFactory<IIdentityConsentIMP>? GetIMPFactory()
        {
            return _identityConsentIMPFactory;
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


        public string[] Scopes
        {
            get
            {

                return GetAttribute<string[]>(nameof(Scopes));
            }
            set
            {
                SetAttribute<string[]>(nameof(Scopes), value);
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

        public DateTime? Expiration
        {
            get
            {

                return GetAttribute<DateTime?>(nameof(Expiration));
            }
            set
            {
                SetAttribute<DateTime?>(nameof(Expiration), value);
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
        public async Task<Consent> GenerateConsent(CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateConsent(this, cancellationToken);
        }

        public async Task<string> GetSerializeData()
        {
            return await _imp.GetSerializeData(this);
        }
    }

    public interface IIdentityConsentIMP
    {
        Task Add(IdentityConsent identityConsent, CancellationToken cancellationToken = default);
        Task Delete(IdentityConsent identityConsent, CancellationToken cancellationToken = default);
        Task<Consent> GenerateConsent(IdentityConsent identityConsent, CancellationToken cancellationToken = default);
        Task<string> GetSerializeData(IdentityConsent identityConsent);
    }


    [Injection(InterfaceType = typeof(IIdentityConsentIMP), Scope = InjectionScope.Transient)]
    public class IdentityConsentIMP : IIdentityConsentIMP
    {
        private readonly IIdentityConsentStore _identityConsentStore;

        public IdentityConsentIMP(IIdentityConsentStore identityConsentStore)
        {
            _identityConsentStore = identityConsentStore;
        }
        public async Task Add(IdentityConsent identityConsent, CancellationToken cancellationToken = default)
        {
            await _identityConsentStore.Add(identityConsent, cancellationToken);
        }

        public async Task Delete(IdentityConsent identityConsent, CancellationToken cancellationToken = default)
        {
            await _identityConsentStore.Delete(identityConsent.ID, cancellationToken);
        }

        public async Task<Consent> GenerateConsent(IdentityConsent identityConsent, CancellationToken cancellationToken = default)
        {
            Consent consent = new Consent()
            {
                ClientId = identityConsent.ClientId,
                CreationTime = identityConsent.CreationTime,
                Expiration = identityConsent.Expiration,
                Scopes = identityConsent.Scopes,
                SubjectId = identityConsent.SubjectId
            };
            return await Task.FromResult(consent);
        }

        public async Task<string> GetSerializeData(IdentityConsent identityConsent)
        {
            IdentityConsentData data = new IdentityConsentData()
            {
                ID = identityConsent.ID,
                ClientId = identityConsent.ClientId,
                SubjectId = identityConsent.SubjectId,
                CreationTime = identityConsent.CreationTime,
                Expiration = identityConsent.Expiration,
                Scopes = identityConsent.Scopes

            };
            return await Task.FromResult(JsonSerializerHelper.Serializer(data));
        }
    }

    [DataContract]
    public class IdentityConsentData
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string[] Scopes { get; set; } = null!;
        [DataMember]
        public DateTime CreationTime { get; set; }
        [DataMember]
        public DateTime? Expiration { get; set; }
        [DataMember]
        public string SubjectId { get; set; } = null!;
        [DataMember]
        public string ClientId { get; set; } = null!;
    }
}
