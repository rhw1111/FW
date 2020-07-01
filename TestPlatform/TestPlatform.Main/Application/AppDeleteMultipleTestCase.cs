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
    [Injection(InterfaceType = typeof(IAppDeleteMultipleTestCase), Scope = InjectionScope.Singleton)]
    public class AppDeleteMultipleTestCase : IAppDeleteMultipleTestCase
    {
        public async Task Do(List<Guid> list, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase();
            List<TestCase> array = new List<TestCase>();
            foreach (Guid id in list)
            {
                TestCase tCase = new TestCase()
                {
                    ID = id
                };
                array.Add(tCase);
            }
            await source.DeleteMultiple(array.ToList(), cancellationToken);
        }

    }
}
