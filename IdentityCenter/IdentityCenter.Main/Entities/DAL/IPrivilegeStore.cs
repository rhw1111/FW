using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;

namespace IdentityCenter.Main.Entities.DAL
{
    public interface IPrivilegeStore
    {
        Task Add(Privilege privilege, CancellationToken cancellationToken = default);
        Task Update(Privilege privilege, CancellationToken cancellationToken = default);
        Task Delete(Guid privilegeID, CancellationToken cancellationToken = default);
        Task<Privilege?> QueryByID(Guid privilegeID, CancellationToken cancellationToken = default);
        Task<Privilege?> QueryByCode(string code, CancellationToken cancellationToken = default);
        Task<Guid?> QueryByCodeNoLock(string code, CancellationToken cancellationToken = default);
        Task<QueryResult<Privilege>> QueryByPage(string matchCode, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<QueryResult<Privilege>> QueryByRolePage(Guid roleID, int page, int pageSize, CancellationToken cancellationToken = default);

    }
}
