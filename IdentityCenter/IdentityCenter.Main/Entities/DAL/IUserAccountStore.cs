using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Entities.DAL
{
    public interface IUserAccountStore
    {
        Task Add(UserAccount account,CancellationToken cancellationToken=default);
        Task Delete(Guid id, CancellationToken cancellationToken= default);
        Task UpdateActive(Guid id,bool active, CancellationToken cancellationToken = default);
        Task Update(UserAccount account, CancellationToken cancellationToken = default);
        Task UpdatePassword(Guid id,string password, CancellationToken cancellationToken = default);
        Task<UserAccount?> QueryByThirdParty(string source, string sourceID, CancellationToken cancellationToken = default);
        Task<UserAccount?> QueryByName(string name, CancellationToken cancellationToken = default);
        
        Task<UserAccount?> QueryByID(Guid id, CancellationToken cancellationToken = default);

        Task<Guid> QueryFirstIDNolockByName(string name, CancellationToken cancellationToken = default);

    }
}
