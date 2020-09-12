using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.Cache;
using MSLibrary.DI;

namespace IdentityCenter.Main.Entities
{
    [Injection(InterfaceType = typeof(IUserAccountRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class UserAccountRepositoryCacheProxy : IUserAccountRepositoryCacheProxy
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserAccountCacheProxyService _userAccountCacheProxyService;

        public UserAccountRepositoryCacheProxy(IUserAccountRepository userAccountRepository, IUserAccountCacheProxyService userAccountCacheProxyService)
        {
            _userAccountRepository = userAccountRepository;
            _userAccountCacheProxyService = userAccountCacheProxyService;
        }
       

        public async Task<UserAccount?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _userAccountCacheProxyService.QueryByID(id, cancellationToken);


        }

        public async Task<UserAccount?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _userAccountCacheProxyService.QueryByName(name, cancellationToken);
        }

        public async Task<UserAccount?> QueryByThirdParty(string source, string sourceID, CancellationToken cancellationToken = default)
        {
            return await _userAccountCacheProxyService.QueryByThirdParty(source, sourceID, cancellationToken);
        }
    }
}
