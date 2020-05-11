using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Distribute
{
    public interface IApplicationLimitRepository
    {
        Task<ApplicationLimit> QueryByName(string name);

        ApplicationLimit QueryByNameSync(string name);
    }
}
