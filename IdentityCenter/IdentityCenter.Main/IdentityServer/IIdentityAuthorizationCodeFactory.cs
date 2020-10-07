using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityAuthorizationCodeFactory
    {
        Task<IdentityAuthorizationCode> Create(string serializeData);
    }
}
