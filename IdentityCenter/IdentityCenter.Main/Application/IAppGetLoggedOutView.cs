using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityCenter.Main.DTOModel;

namespace IdentityCenter.Main.Application
{
    public interface IAppGetLoggedOutView
    {
        Task<LoggedOutViewModel> Do(CancellationToken cancellationToken = default);
    }
}
