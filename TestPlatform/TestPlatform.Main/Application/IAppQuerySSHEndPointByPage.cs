using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppQuerySSHEndPointByPage
    {
        Task<QueryResult<SSHEndPointViewData>> Do(string matchName, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
