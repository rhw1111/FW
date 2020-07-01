using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppExecuteTestCase
    {
        Task Run(Guid id, CancellationToken cancellationToken = default);
        Task Stop(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsEngineRun(Guid id, CancellationToken cancellationToken = default);
        Task<string> GetMasterLog(Guid caseId, CancellationToken cancellationToken = default);
        Task<string> GetSlaveLog(Guid caseId, Guid slaveHostId, CancellationToken cancellationToken = default);
        Task<TestCaseSlaveHost> AddSlaveHost(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default);
        IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(Guid caseId, CancellationToken cancellationToken = default);
        Task DeleteSlaveHost(Guid slaveHostID, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<TestCaseHistory?> GetHistory(Guid historyID, CancellationToken cancellationToken = default);
        Task DeleteHistory(Guid historyID, CancellationToken cancellationToken = default);
        Task UpdateSlaveHost(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default);
    }
}
