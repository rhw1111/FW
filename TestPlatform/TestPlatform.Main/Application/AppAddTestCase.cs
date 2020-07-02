using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using System.Linq;
using System.Diagnostics.Tracing;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestCase), Scope = InjectionScope.Singleton)]
    public class AppAddTestCase : IAppAddTestCase
    {
        public async Task<TestCaseViewData> Do(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCaseViewData result;
            TestCase source = new TestCase()
            {
                Name = model.Name,
                EngineType = model.EngineType,
                MasterHostID = model.MasterHostID,
                Configuration = model.Configuration,
                Status = TestCaseStatus.NoRun
            };               
            await source.Add();

            result = new TestCaseViewData()
            {
                ID = source.ID,
                EngineType = source.EngineType,
                Configuration = source.Configuration,
                Name = source.Name,
                Status = source.Status,
                MasterHostID = source.MasterHostID,
                CreateTime = source.CreateTime.ToCurrentUserTimeZone()
            };
            return result;
        }
    }
}
