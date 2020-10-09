using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityConsentFactory
    {
        Task<IdentityConsent> Create(string serializeData);
    }
}
