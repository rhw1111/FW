using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Entities
{
    public interface IUserAccountCacheProxyService
    {
        Task<bool> HasRoles(UserAccount user, IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
        Task<bool> HasPrivileges(UserAccount user, IEnumerable<string> privilegeCodes, CancellationToken cancellationToken = default);
        Task<bool> HasRole(UserAccount user, string roleName, CancellationToken cancellationToken = default);
        Task<bool> HasPrivilege(UserAccount user, string privilegeCode, CancellationToken cancellationToken = default);
        Task<UserAccount?> QueryByThirdParty(string source, string sourceID, CancellationToken cancellationToken = default);
        Task<UserAccount?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<UserAccount?> QueryByID(Guid id, CancellationToken cancellationToken = default);

        Task ClearAccountCache(Guid userID, CancellationToken cancellationToken = default);
        Task ClearRoleCache(Guid userID, CancellationToken cancellationToken = default);
        Task ClearPrivilegeCache(Guid userID, CancellationToken cancellationToken = default);
        Task ClearAllCache(Guid userID, CancellationToken cancellationToken = default);

    }
}
