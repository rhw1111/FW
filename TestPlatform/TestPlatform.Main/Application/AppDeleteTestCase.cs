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
    [Injection(InterfaceType = typeof(IAppDeleteTestCase), Scope = InjectionScope.Singleton)]
    public class AppDeleteTestCase : IAppDeleteTestCase
    { 
        public async Task Do(Guid id, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = id
            };
            await source.Delete(cancellationToken);
        }

    }
}
