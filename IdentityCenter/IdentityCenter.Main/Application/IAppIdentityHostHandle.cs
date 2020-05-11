using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityCenter.Main.DTOModel;
using IdentityServer4.Configuration;

namespace IdentityCenter.Main.Application
{
    public interface IAppIdentityHostHandle
    {
        Task<(IdentityHostHandleResult, IIdentityServiceOptionsInitController)> Do();
    }

    public interface IIdentityServiceOptionsInitController
    {
        void Init(IdentityServerOptions options);
    }
}
