﻿using System;
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
    [Injection(InterfaceType = typeof(IAppQuerySingleSSHEndPoint), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleSSHEndPoint : IAppQuerySingleSSHEndPoint
    {
        private readonly ISSHEndpointRepository _sshEndPointRepository;
        public AppQuerySingleSSHEndPoint(ISSHEndpointRepository sshEndPointRepository)
        {
            _sshEndPointRepository = sshEndPointRepository;
        }
        public async Task<SSHEndPointViewData> Do(Guid id, CancellationToken cancellationToken = default)
        {
            var sshEndPoint = await _sshEndPointRepository.QueryByID(id, cancellationToken);
            if (sshEndPoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSSHEndPointByID,
                    DefaultFormatting = "找不到Id为{0}的SSH终结点",
                    ReplaceParameters = new List<object>() { id.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSSHEndPointByID, fragment, 1, 0);
            }
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
