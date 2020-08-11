using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.SerialNo.DAL
{
    public interface ISerialNoConfigurationStore
    {
        Task Add(SerialNoConfiguration configuration, CancellationToken cancellationToken = default);
        Task Update(SerialNoConfiguration configuration, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<SerialNoConfiguration> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<SerialNoConfiguration?> QueryFirstNolockByName(string name, CancellationToken cancellationToken = default);

    }
}
