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
    [Injection(InterfaceType = typeof(IAppAddSSHEndPoint), Scope = InjectionScope.Singleton)]
    public class AppAddSSHEndPoint : IAppAddSSHEndPoint
    {
        private readonly ISSHEndpointRepository _sshEndPointRepository;
        public AppAddSSHEndPoint(ISSHEndpointRepository sshEndPointRepository)
        {
            _sshEndPointRepository = sshEndPointRepository;
        }
        public async Task<SSHEndPointViewData> Do(SSHEndPointAddModel model, CancellationToken cancellationToken = default)
        {
            //检查是否有名称重复的
            var newId = await _sshEndPointRepository.QueryByNameNoLock(model.Name, cancellationToken);
            if (newId != null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.ExistSSHEndPointByName,
                    DefaultFormatting = "已经存在名称为{0}的SSH终结点",
                    ReplaceParameters = new List<object>() { model.Name }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.ExistSSHEndPointByName, fragment, 1, 0);

            }
            SSHEndpoint sshEndPoint = new SSHEndpoint()
            {
                Name = model.Name,
                Configuration = model.Configuration,
                Type = model.Type
            };
            await sshEndPoint.Add(cancellationToken);

            return new SSHEndPointViewData()
            {
                ID = sshEndPoint.ID,
                Name = sshEndPoint.Name,
                Configuration = sshEndPoint.Configuration,
                Type = sshEndPoint.Type,
                CreateTime = sshEndPoint.CreateTime,
                ModifyTime = sshEndPoint.ModifyTime
            };
        }
    }
}
