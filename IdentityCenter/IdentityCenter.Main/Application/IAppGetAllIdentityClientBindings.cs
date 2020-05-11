using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityCenter.Main.DTOModel;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.Application
{
    public interface IAppGetAllIdentityClientBindings
    {
        Task<(IdentityClientHostInfoModel, IList<(IdentityClientBindingInfoModel, IInitIdentityClientBindingOptions)>)> Do(CancellationToken cancellationToken = default);
    }

    public interface IInitIdentityClientBindingOptions
    {
        void Init<T>(T options);
    }


}
