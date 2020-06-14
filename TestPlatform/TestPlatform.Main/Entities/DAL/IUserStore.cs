using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Entities.DAL
{
    public interface IUserStore
    {
        Task Add(User user, CancellationToken cancellationToken = default);
        Task Update(User user, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
    }
}
