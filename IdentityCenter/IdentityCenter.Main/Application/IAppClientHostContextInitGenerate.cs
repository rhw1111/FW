using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Application
{
    public interface IAppClientHostContextInitGenerate
    {
        Task<IClientHostContextInit> Do();
    }

    public interface IClientHostContextInit
    {
        void Init();
    }
}
