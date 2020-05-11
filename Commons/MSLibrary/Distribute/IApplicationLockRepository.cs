using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Distribute
{
    public interface IApplicationLockRepository
    {
        Task<ApplicationLock> QueryByName(string name);

        ApplicationLock QueryByNameSync(string name);
    }
}
