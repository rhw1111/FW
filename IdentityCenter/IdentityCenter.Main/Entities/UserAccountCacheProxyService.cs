using IdentityCenter.Main.Entities.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;
using MSLibrary.Cache;
using MSLibrary.Thread;

namespace IdentityCenter.Main.Entities
{
    [Injection(InterfaceType = typeof(IUserAccountCacheProxyService), Scope = InjectionScope.Singleton)]
    public class UserAccountCacheProxyService : IUserAccountCacheProxyService
    {
        private readonly ICacheKeyRelationService _cacheKeyRelationService;
        private readonly ICacheVersionService _cacheVersionService;
        private readonly ICacheVersionRepository _cacheVersionRepository;
        private readonly ICacheService _cacheService;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserAccountFactory _userAccountFactory;


        public UserAccountCacheProxyService(ICacheKeyRelationService cacheKeyRelationService, ICacheVersionService cacheVersionService, ICacheVersionRepository cacheVersionRepository, ICacheService cacheService,
            IUserAccountRepository userAccountRepository, IUserAccountFactory userAccountFactory
            )
        {
            _cacheKeyRelationService = cacheKeyRelationService;
            _cacheVersionService = cacheVersionService;
            _cacheVersionRepository = cacheVersionRepository;
            _cacheService = cacheService;
            _userAccountRepository = userAccountRepository;
            _userAccountFactory = userAccountFactory;
        }
        public async Task ClearAllCache(Guid userID, CancellationToken cancellationToken = default)
        {
            await ClearPrivilegeCache(userID, cancellationToken);
            await ClearRoleCache(userID, cancellationToken);
            await ClearAccountCache(userID, cancellationToken);
        }

        public async Task ClearPrivilegeCache(Guid userID, CancellationToken cancellationToken = default)
        {          
            await _cacheService.Clear<string, bool>(string.Format(CacheKeyFormats.UserPrivilege, userID.ToString()));
        }

        public async Task ClearRoleCache(Guid userID, CancellationToken cancellationToken = default)
        {
            await _cacheService.Clear<string, bool>(string.Format(CacheKeyFormats.UserRole,userID.ToString()));
        }

        public async Task<bool> HasPrivilege(UserAccount user, string privilegeCode, CancellationToken cancellationToken = default)
        {
            await privilegeVersionCheck(user, cancellationToken);
            var result = await hasPrivilege(user, privilegeCode, cancellationToken);
            return result;
        }

        public async Task<bool> HasPrivileges(UserAccount user, IEnumerable<string> privilegeCodes, CancellationToken cancellationToken = default)
        {
            await privilegeVersionCheck(user, cancellationToken);
            bool result = true;
            await ParallelHelper.ForEach(privilegeCodes, 5, async (code) =>
              {
                  var iResult = await HasPrivilege(user, code, cancellationToken);
                  if (result && !iResult)
                  {
                      result = false;
                  }
              });

            return result;
        }

        public async Task<bool> HasRole(UserAccount user, string roleName, CancellationToken cancellationToken = default)
        {
            await roleVersionCheck(user, cancellationToken);
            var result = await hasRole(user, roleName, cancellationToken);
            return result;
        }

        public async Task<bool> HasRoles(UserAccount user, IEnumerable<string> roleNames, CancellationToken cancellationToken = default)
        {
            await roleVersionCheck(user, cancellationToken);
            bool result = true;
            await ParallelHelper.ForEach(roleNames, 5, async (name) =>
            {
                var iResult = await HasRole(user, name, cancellationToken);
                if (result && !iResult)
                {
                    result = false;
                }
            });

            return result;
        }


        private async Task privilegeVersionCheck(UserAccount user, CancellationToken cancellationToken = default)
        {
            await _cacheVersionService.Execute(CacheVersionNames.Privilege,
                 async (name) =>
                 {
                     var version = await _cacheVersionRepository.QueryByName(name);
                     if (version == null)
                     {
                         var fragment = new TextFragment()
                         {
                             Code = TextCodes.NotFoundCacheVersionByName,
                             DefaultFormatting = "找不到名称为{0}的缓存版本号记录",
                             ReplaceParameters = new List<object>() { name }
                         };

                         throw new UtilityException((int)Errors.NotFoundCacheVersionByName, fragment);
                     }

                     return version.Version;

                 },
                 async () =>
                 {
                     await ClearPrivilegeCache(user.ID, cancellationToken);
                 }
                 );
        }
        
        private async Task<bool> hasPrivilege(UserAccount user, string privilegeCode, CancellationToken cancellationToken = default)
        {
            var result = await _cacheService.GetHash<string, bool>(
                async (key,hKey) =>
                {
                    var result = await user.HasPrivilege(privilegeCode, cancellationToken);
                    return (result, true);
                },
                string.Format(CacheKeyFormats.UserPrivilege, user.ID.ToString()),
                privilegeCode
                );

            return result;
        }

        private async Task roleVersionCheck(UserAccount user, CancellationToken cancellationToken = default)
        {
            await _cacheVersionService.Execute(CacheVersionNames.Role,
                 async (name) =>
                 {
                     var version = await _cacheVersionRepository.QueryByName(name);
                     if (version == null)
                     {
                         var fragment = new TextFragment()
                         {
                             Code = TextCodes.NotFoundCacheVersionByName,
                             DefaultFormatting = "找不到名称为{0}的缓存版本号记录",
                             ReplaceParameters = new List<object>() { name }
                         };

                         throw new UtilityException((int)Errors.NotFoundCacheVersionByName, fragment);
                     }

                     return version.Version;

                 },
                 async () =>
                 {
                     await ClearRoleCache(user.ID, cancellationToken);
                 }
                 );
        }

        private async Task<bool> hasRole(UserAccount user, string roleName, CancellationToken cancellationToken = default)
        {
            var result = await _cacheService.GetHash<string, bool>(
                async (key,hKey) =>
                {                   
                    var result = await user.HasRole(roleName, cancellationToken);
                    return (result, true);
                },
                string.Format(CacheKeyFormats.UserRole, user.ID.ToString())
                ,roleName
                );

            return result;
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
                            await _cacheKeyRelationService.AddOTN(CacheKeyRelationNames.UserAccountIDThirdParty, string.Format(CacheKeyFormats.UserAccountID, account.ID.ToString()), string.Format(CacheKeyFormats.UserAccountThirdParty, source, sourceID));
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

        public async Task<UserAccount?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            var accountID = await _cacheService.Get<string, Guid?>(
                    async (key) =>
                    {
                        var account = await _userAccountRepository.QueryByName(name, cancellationToken);
                        if (account == null)
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

            if (accountID == null)
            {
                return null;
            }

            return await QueryByID(accountID.Value, cancellationToken);
        }

        public async Task<UserAccount?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            var accountData = await _cacheService.Get<string, string?>(
                async (key) =>
                {
                    var account = await _userAccountRepository.QueryByID(id, cancellationToken);
                    if (account == null)
                    {
                        return (null, false);
                    }
                    else
                    {
                        return (await account.GetSerializeData(), true);
                    }
                }
                , string.Format(CacheKeyFormats.UserAccountID, id.ToString()));

            if (accountData == null)
            {
                return null;
            }

            return await _userAccountFactory.Create(accountData);
        }

        public async Task ClearAccountCache(Guid userID, CancellationToken cancellationToken = default)
        {
            var nameCacheKeys = await _cacheKeyRelationService.GetNKeys(CacheKeyRelationNames.UserAccountIDName, string.Format(CacheKeyFormats.UserAccountID, userID.ToString()));
            await _cacheService.Clear<string, Guid?>(nameCacheKeys);
            await _cacheKeyRelationService.Delete(CacheKeyRelationNames.UserAccountIDName, string.Format(CacheKeyFormats.UserAccountID, userID.ToString()));

            var thirdPartyCacheKeys = await _cacheKeyRelationService.GetNKeys(CacheKeyRelationNames.UserAccountIDThirdParty, string.Format(CacheKeyFormats.UserAccountID, userID.ToString()));
            await _cacheService.Clear<string, Guid?>(thirdPartyCacheKeys);
            await _cacheKeyRelationService.Delete(CacheKeyRelationNames.UserAccountIDThirdParty, string.Format(CacheKeyFormats.UserAccountID, userID.ToString()));

            await _cacheService.Clear<string, string?>(string.Format(CacheKeyFormats.UserAccountID, userID.ToString()));
        }
    }
}
