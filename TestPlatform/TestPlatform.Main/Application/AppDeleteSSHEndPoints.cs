using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary.CommandLine.SSH;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppDeleteSSHEndPoints), Scope = InjectionScope.Singleton)]
    public class AppDeleteSSHEndPoints : IAppDeleteSSHEndPoints
    {
        private readonly ISSHEndpointRepository _sshEndPointRepository;
        public AppDeleteSSHEndPoints(ISSHEndpointRepository sshEndPointRepository)
        {
            _sshEndPointRepository = sshEndPointRepository;
        }
        public async Task Do(List<Guid> ids, CancellationToken cancellationToken = default)
        {
             await _sshEndPointRepository.DeleteMutiple(ids, cancellationToken);
        }
    }
}
