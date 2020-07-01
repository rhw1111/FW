using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary.Serializer;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySingleTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestCaseHistory : IAppQuerySingleTestCaseHistory
    {
        public async Task<TestCaseHistoryViewModel> Do(Guid caseId, Guid historyId, CancellationToken cancellationToken = default)
        {
            TestCaseHistoryViewModel viewHistory = new TestCaseHistoryViewModel();
            TestCase source = new TestCase()
            {
                ID = caseId
            };
            var history = await source.GetHistory(historyId, cancellationToken);
            if (history != null)
            {
                TestCaseHistorySummyAddModel addModel = JsonSerializerHelper.Deserialize<TestCaseHistorySummyAddModel>(history.Summary);
                viewHistory.AvgDuration = addModel.AvgDuration;
                viewHistory.ConnectCount = addModel.ConnectCount;
                viewHistory.AvgQPS = addModel.AvgQPS;
                viewHistory.CaseID = addModel.CaseID;
                viewHistory.MaxDuration = addModel.MaxDuration;
                viewHistory.MinDurartion = addModel.MinDurartion;
                viewHistory.MinDurartion = addModel.MaxQPS;
                viewHistory.MinQPS = addModel.MinQPS;
                viewHistory.ReqFailCount = addModel.ReqFailCount;
                viewHistory.ReqCount = addModel.ReqCount;
                viewHistory.CreateTime = history.CreateTime;
                viewHistory.ModifyTime = history.ModifyTime;
                viewHistory.ID = history.ID;
            }

            return viewHistory;
        }
    }
}
