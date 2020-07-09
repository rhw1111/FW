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
            //if (sshEndPoint == null)
            //{
            //    var fragment = new TextFragment()
            //    {
            //        Code = TestPlatformTextCodes.NotFoundSSHEndPointByID,
            //        DefaultFormatting = "找不到SSH终端Id为{0}的SSH终端",
            //        ReplaceParameters = new List<object>() { id.ToString() }
            //    };

            //    throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSSHEndPointByID, fragment, 1, 0);
            //}
            //return new SSHEndPointViewData()
            //{
            //    ID = sshEndPoint.ID,
            //    Name = sshEndPoint.Name,
            //    Configuration = sshEndPoint.Configuration,
            //    Type = sshEndPoint.Type,
            //    CreateTime = sshEndPoint.CreateTime,
            //    ModifyTime = sshEndPoint.ModifyTime
            //};
        }
    }
}
