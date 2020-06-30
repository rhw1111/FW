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
        Task<TestCaseViewData> Run(TestCaseAddModel model, CancellationToken cancellationToken = default);
        Task<TestCaseViewData> Stop(TestCaseAddModel model, CancellationToken cancellationToken = default);
        Task<TestCaseViewData> IsEngineRun(TestCaseAddModel model, CancellationToken cancellationToken = default);
        Task<TestCaseSlaveHost> AddSlaveHost(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default);
        IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(Guid caseId, CancellationToken cancellationToken = default);
        Task DeleteSlaveHost(Guid slaveHostID, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<TestCaseHistory?> GetHistory(Guid historyID, CancellationToken cancellationToken = default);
        Task DeleteHistory(Guid historyID, CancellationToken cancellationToken = default);
        Task UpdateSlaveHost(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default);
    }
}
