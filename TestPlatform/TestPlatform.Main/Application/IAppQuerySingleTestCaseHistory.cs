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
    public interface IAppQuerySingleTestCaseHistory
    {
        Task<TestCaseHistoryViewModel> Do(Guid caseId, Guid historyId, CancellationToken cancellationToken = default);
    }
}
