using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppGoBackPrevious
    {
        Task<QueryResult<TreeEntityViewModel>> Do(Guid treeEntityId, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
