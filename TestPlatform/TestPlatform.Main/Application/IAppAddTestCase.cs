using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppAddTestCase
    {
        Task<TestCaseViewData> Do(TestCaseAddModel model, CancellationToken cancellationToken = default);
        Task<TestCaseViewData> Update(TestCaseAddModel model, CancellationToken cancellationToken = default);
        Task<TestCaseViewData> Delete(TestCase model, CancellationToken cancellationToken = default);
        Task AddHistory(TestCaseHistorySummyAddModel model, CancellationToken cancellationToken = default);
        Task DeleteMutiple(List<TestCaseAddModel> list, CancellationToken cancellationToken = default);
    }
}
