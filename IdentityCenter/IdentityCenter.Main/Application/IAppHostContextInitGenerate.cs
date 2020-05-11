using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Application
{
    public interface IAppHostContextInitGenerate
    {
        Task<IHostContextInit> Do();
    }

    public interface IHostContextInit
    {
        void Init();
    }
}
