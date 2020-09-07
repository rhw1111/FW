using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;

namespace IdentityCenter.Main.Entities.DAL
{
    public interface IRoleStore
    {
        Task Add(Role role, CancellationToken cancellationToken = default);
        Task Update(Role role, CancellationToken cancellationToken = default);
        Task Delete(Guid roleID, CancellationToken cancellationToken = default);
        Task<Role?> QueryByID(Guid roleID, CancellationToken cancellationToken = default);
        Task<Role?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default);
        Task<QueryResult<Role>> QueryByPage(string matchName,int page,int pageSize, CancellationToken cancellationToken = default);
        Task AddRolePrivilege(Guid roleID,Guid privilegeID, CancellationToken cancellationToken = default);
        Task AddRolePrivilege(IEnumerable<(Guid,Guid)> rps, CancellationToken cancellationToken = default);
        Task DeleteRolePrivilege(Guid roleID, Guid privilegeID, CancellationToken cancellationToken = default);
        Task DeleteRolePrivilege(IEnumerable<(Guid, Guid)> rps, CancellationToken cancellationToken = default);
    }
}
