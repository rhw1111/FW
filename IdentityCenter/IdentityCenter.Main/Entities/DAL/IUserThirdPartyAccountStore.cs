using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Entities.DAL
{
    public interface IUserThirdPartyAccountStore
    {
        Task Add(UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default);
        Task Update(UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default);
        Task Delete(Guid partyAccountID, CancellationToken cancellationToken = default);
        Task<UserThirdPartyAccount?> QueryByID(Guid accountID, Guid partyAccountID, CancellationToken cancellationToken = default);
        Task<UserThirdPartyAccount?> QueryBySource(Guid accountID, string source,string sourceID, CancellationToken cancellationToken = default);

        Task<Guid> QueryFirstIDNolockBySource(string source, string sourceID, CancellationToken cancellationToken = default);
    }
}
