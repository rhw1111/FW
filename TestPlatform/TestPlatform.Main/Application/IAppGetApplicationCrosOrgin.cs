using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppGetApplicationCrosOrgin
    {
        string[] Do(CancellationToken cancellationToken = default);
    }
}
