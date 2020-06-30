using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Configuration;
using System.Linq;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppUpdateTestCase), Scope = InjectionScope.Singleton)]
    public class AppUpdateTestCase : IAppUpdateTestCase
    {
        public async Task Do(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = model.ID,
                Name = model.Name,
                OwnerID = model.OwnerID,
                EngineType = model.EngineType,
                MasterHostID = model.MasterHostID,
                Configuration = model.Configuration,
                Status = model.Status
            };
            await source.Update(cancellationToken);
        }

    }
}
