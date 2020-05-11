using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Entities
{
    public interface IUserAccountRepository
    {
        Task<UserAccount?> QueryByThirdParty(string source,string sourceID, CancellationToken cancellationToken = default);
        Task<UserAccount?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<UserAccount?> QueryByID(Guid id, CancellationToken cancellationToken = default);
    }
}
