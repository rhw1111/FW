using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppQueryAllSSHEndpoint
    {
        Task<List<SSHEndPointViewData>> Do(CancellationToken cancellationToken = default);
    }
}
