﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FW.TestPlatform.Main.Entities.DAL
{
    public interface ITestCaseSlaveHostStore
    {
        Task Add(TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default);
        Task Update(TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        IAsyncEnumerable<TestCaseSlaveHost> QueryByCase(Guid caseID, CancellationToken cancellationToken = default);
        Task<TestCaseSlaveHost?> QueryByCase(Guid caseID, Guid slaveHostID, CancellationToken cancellationToken = default);
        Task<TestCaseSlaveHost?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<Guid?> QueryByNameNoLock(Guid caseId, string name, CancellationToken cancellationToken = default);

        Task<List<TestCaseSlaveHost>> QueryByCaseIdAndSlaveHostIds(Guid caseId, List<Guid> ids, CancellationToken cancellationToken = default);
        Task DeleteSlaveHosts(List<Guid> ids, CancellationToken cancellationToken = default);
    }
}
