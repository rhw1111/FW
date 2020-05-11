using IdentityCenter.Main.DTOModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppGetLoggedOutView), Scope = InjectionScope.Singleton)]
    public class AppGetLoggedOutView : IAppGetLoggedOutView
    {
        public Task<LoggedOutViewModel> Do(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
