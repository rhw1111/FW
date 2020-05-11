using IdentityCenter.Main.DTOModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary;
using MSLibrary.DI;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;
using IdentityCenter.Main.IdentityServer.ClientBindings;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppGetAllIdentityClientBindings), Scope = InjectionScope.Singleton)]
    public class AppGetAllIdentityClientBindings : IAppGetAllIdentityClientBindings
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityClientBindingRepository _identityClientBindingRepository;

        public AppGetAllIdentityClientBindings(ISystemConfigurationService systemConfigurationService, IIdentityClientBindingRepository identityClientBindingRepository)
        {
            _systemConfigurationService = systemConfigurationService;
            _identityClientBindingRepository = identityClientBindingRepository;
        }
        public async Task<(IdentityClientHostInfoModel,IList<(IdentityClientBindingInfoModel, IInitIdentityClientBindingOptions)>)> Do(CancellationToken cancellationToken = default)
        {
            IdentityClientHostInfoModel hostInfo = new IdentityClientHostInfoModel();
            List<(IdentityClientBindingInfoModel, IInitIdentityClientBindingOptions)> listResult = new List<(IdentityClientBindingInfoModel, IInitIdentityClientBindingOptions)>();
            var clientAppName = await _systemConfigurationService.GetIdentityClientApplicationName(cancellationToken);

            Dictionary<string,string> corsOrgins = new Dictionary<string,string>();
            var bindings = _identityClientBindingRepository.QueryAll(cancellationToken);



            await foreach(var binding in bindings)
            {
                string bindingType = IdentityClientBindingTypes.None;
                switch (binding)
                {
                    case IdentityClientOpenIDBinding openid:
                        bindingType = IdentityClientBindingTypes.OpenID;
                        break;
                }


                foreach (var item in binding.AllowedCorsOrigins)
                {
                    corsOrgins[item] = item;
                }

                listResult.Add(
                    (
                    new IdentityClientBindingInfoModel()
                    {
                        BindingName = binding.Name,
                        BindingType = bindingType
                    },
                    new InitIdentityClientBindingOptionsDefault(await binding.InitOptions())
                    )); ;
            }
                
                                           
            hostInfo.AllowedCorsOrigins = corsOrgins.Values.ToList();

            return (hostInfo, listResult);
        }
    }

    public class InitIdentityClientBindingOptionsDefault : IInitIdentityClientBindingOptions
    {
        private readonly IIdentityClientBindingOptionsInit _optionsInit;

        public InitIdentityClientBindingOptionsDefault(IIdentityClientBindingOptionsInit optionsInit)
        {
            _optionsInit = optionsInit;
        }
        public void Init<T>(T options)
        {
            _optionsInit.Init(options);
        }
    }
}
