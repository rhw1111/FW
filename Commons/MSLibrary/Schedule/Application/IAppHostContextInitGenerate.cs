using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Schedule.Application
{
    public interface IAppHostContextInitGenerate
    {
        Task<IHostContextInit> Do(CancellationToken cancellationToken);
    }

    public interface IHostContextInit
    {
        void Init();
    }
}
