using System;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppQueryChild
    {
        Task<TreeEntityViewModel?> Do(Guid? parentId, string name, CancellationToken cancellationToken = default);
    }
}
