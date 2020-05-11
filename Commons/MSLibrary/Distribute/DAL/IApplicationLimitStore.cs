using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Distribute.DAL
{
    /// <summary>
    /// 应用程序限流数据操作
    /// </summary>
    public interface IApplicationLimitStore
    {
        Task<ApplicationLimit> QueryByName(string name);

        ApplicationLimit QueryByNameSync(string name);
    }
}
