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
        private readonly IUserAccountFactory _userAccountFactory;
        private readonly ICacheKeyRelationService _cacheKeyRelationService;

        public UserAccountRepositoryCacheProxy(IUserAccountRepository userAccountRepository, ICacheService cacheService, IUserAccountFactory userAccountFactory, ICacheKeyRelationService cacheKeyRelationService)
        {
            _userAccountRepository = userAccountRepository;
            _cacheService = cacheService;
            _userAccountFactory = userAccountFactory;
            _cacheKeyRelationService = cacheKeyRelationService;
        }
       

        public async Task<UserAccount?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            var accountData= await _cacheService.Get<string,string?>(
                async(key)=>
                {
                    var account = await _userAccountRepository.QueryByID(id, cancellationToken);
                    if (account==null)
                    {
                        return (null, false);
                    }
                    else
                    {
                        return (await account.GetSerializeData(), true);
                    }
                }
                , string.Format(CacheKeyFormats.UserAccountID, id.ToString()));

            if (accountData==null)
            {
                return null;
            }

            return await _userAccountFactory.Create(accountData);
        }

        public async Task<UserAccount?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            var accountID = await _cacheService.Get<string, Guid?>(
                    async (key) =>
                    {
                        var account = await _userAccountRepository.QueryByName(name, cancellationToken);
                        if (account==null)
                        {
                            return (null, false);
                        }
                        else
                        {
                            await _cacheKeyRelationService.AddOTN(CacheKeyRelationNames.UserAccountIDName, string.Format(CacheKeyFormats.UserAccountID, account.ID.ToString()), string.Format(CacheKeyFormats.UserAccountName, name));
                            return (account.ID, true);
                        }
                    }
                    , string.Format(CacheKeyFormats.UserAccountName, name));

            if (accountID==null)
            {
                return null;
            }

            return await QueryByID(accountID.Value, cancellationToken);
        }

        public async Task<UserAccount?> QueryByThirdParty(string source, string sourceID, CancellationToken cancellationToken = default)
        {
            var accountID = await _cacheService.Get<string, Guid?>(
                    async (key) =>
                    {
                        var account = await _userAccountRepository.QueryByThirdParty(source, sourceID, cancellationToken);
                        if (account == null)
                        {
                            return (null, false);
                        }
                        else
                        {
                            await _cacheKeyRelationService.AddOTN(CacheKeyRelationNames.UserAccountIDThirdParty, string.Format(CacheKeyFormats.UserAccountID, account.ID.ToString()), string.Format(CacheKeyFormats.UserAccountThirdParty, source,sourceID));
                            return (account.ID, true);
                        }
                    }
                    , string.Format(CacheKeyFormats.UserAccountName, source, sourceID));

            if (accountID == null)
            {
                return null;
            }

            return await QueryByID(accountID.Value, cancellationToken);
        }
    }
}
