using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;

namespace MSLibrary.Thread.DAL
{
    /// <summary>
    /// 应用程序锁存储操作接口
    /// </summary>
    public interface IApplicationLockStore
    {
        Task Lock(DBConnectionNames connNames,string lockName, int timeout);
        Task UnLock(DBConnectionNames connNames,string lockName);

        void LockSync(DBConnectionNames connNames,string lockName, int timeout);
        void UnLockSync(DBConnectionNames connNames,string lockName);
    }
}
