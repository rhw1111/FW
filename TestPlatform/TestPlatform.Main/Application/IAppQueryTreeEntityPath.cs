using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppQueryTreeEntityPath
    {
        Task<List<string>> Do(Guid treeEntityId, CancellationToken cancellationToken = default);
    }
}
