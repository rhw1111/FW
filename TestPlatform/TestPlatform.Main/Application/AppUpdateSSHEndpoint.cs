using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary.LanguageTranslate;
using MSLibrary.CommandLine.SSH;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppUpdateSSHEndpoint), Scope = InjectionScope.Singleton)]
    public class AppUpdateSSHEndpoint : IAppUpdateSSHEndpoint
    {
        private readonly ISSHEndpointRepository _sshEndPointRepository;
        public AppUpdateSSHEndpoint(ISSHEndpointRepository sshEndPointRepository)
        {
            _sshEndPointRepository = sshEndPointRepository;
        }
        public async Task<SSHEndPointViewData> Do(SSHEndPointUpdateModel model, CancellationToken cancellationToken = default)
        {
            var sshEndPoint = await _sshEndPointRepository.QueryByID(model.ID, cancellationToken);
            if (sshEndPoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSSHEndPointByID,
                    DefaultFormatting = "找不到SSH终端Id为{0}的SSH终端",
                    ReplaceParameters = new List<object>() { model.ID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSSHEndPointByID, fragment, 1, 0);
            }
            //检查是否有名称重复的
            var newId = await _sshEndPointRepository.QueryByNameNoLock(model.Name, cancellationToken);
            if (newId != null && newId != model.ID)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.ExistSSHEndPointByName,
                    DefaultFormatting = "已经存在名称为{0}的SSH终端",
                    ReplaceParameters = new List<object>() { model.Name }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.ExistSSHEndPointByName, fragment, 1, 0);

            }
            sshEndPoint.Type = model.Type;
            sshEndPoint.Configuration = model.Configuration;
            sshEndPoint.Name = model.Name;
            sshEndPoint.ModifyTime = DateTime.UtcNow;
            await sshEndPoint.Update(cancellationToken);

            SSHEndPointViewData result = new SSHEndPointViewData()
            {
                ID = sshEndPoint.ID,
                Type = sshEndPoint.Type,
                Configuration = sshEndPoint.Configuration,
                Name = sshEndPoint.Name,
                CreateTime = sshEndPoint.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = sshEndPoint.ModifyTime.ToCurrentUserTimeZone()
            };

            return result;
        }
    }
}
