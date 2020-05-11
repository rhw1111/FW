using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Distribute.DAL
{
    /// <summary>
    /// 应用程序锁数据操作
    /// </summary>
    public interface IApplicationLockStore
    {
        Task<ApplicationLock> QueryByName(string name);

        ApplicationLock QueryByNameSync(string name);
    }
}
