using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppExecuteCopy
    {
        Task<bool> Do(ExecuteCopyModel model, CancellationToken cancellationToken = default);
    }
}
