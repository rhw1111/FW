using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;
using MSLibrary.Template;
using MongoDB.Bson;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQueryTestCase), Scope = InjectionScope.Singleton)]
    public class AppQueryFormulas : IAppQueryFormulas
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public AppQueryFormulas(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task<List<FormulasViewData>> Do(CancellationToken cancellationToken = default)
        {
            await _testCaseRepository.QueryByID(new Guid());

            List<FormulasViewData> result = new List<FormulasViewData>();
            

            return result;
        }
    }
}
