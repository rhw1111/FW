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
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppDeleteTestHosts), Scope = InjectionScope.Singleton)]
    public class AppDeleteTestHosts : IAppDeleteTestHosts
    {
        private readonly ITestHostRepository _testHostRepository;
        public AppDeleteTestHosts(ITestHostRepository testHostRepository)
        {
            _testHostRepository = testHostRepository;
        }
        public async Task Do(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await _testHostRepository.DeleteTestHosts(ids, cancellationToken);
        }
    }
}
