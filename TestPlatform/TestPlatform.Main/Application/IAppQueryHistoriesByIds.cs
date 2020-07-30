using FW.TestPlatform.Main.DTOModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppQueryHistoriesByIds
    {
        Task<List<TestCaseHistoryDetailViewData>> Do(Guid caseId, List<Guid> historyIds, CancellationToken cancellationToken = default);
    }
}
