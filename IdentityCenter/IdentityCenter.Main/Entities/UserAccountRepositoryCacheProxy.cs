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
        private readonly ICacheService _cacheService;

        public UserAccountRepositoryCacheProxy(IUserAccountRepository userAccountRepository, ICacheService cacheService)
        {
            _userAccountRepository = userAccountRepository;
            _cacheService = cacheService;
        }
       

        public async Task<UserAccount?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            /*return await _cacheService.Get(
                async(key)=>
                {

                }
                , string.Format(CacheKeyFormats.UserAccountID, id.ToString()));*/
            throw new NotImplementedException();
        }

        public Task<UserAccount?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount?> QueryByThirdParty(string source, string sourceID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
