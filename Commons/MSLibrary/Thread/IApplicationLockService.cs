using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;

namespace MSLibrary.Thread
{
    /// <summary>
    /// 应用程序锁服务接口
    /// 该服务可以用来控制串行化
    /// </summary>
    public interface IApplicationLockService
    {
        /// <summary>
        /// 执行串行化(同步)
        /// </summary>
        /// <param name="lockName">资源名称</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="timeout">超时时间（-1为永不超时）,单位毫秒</param>
        void ExecuteSync(DBConnectionNames connNames,string lockName, Action callBack, int timeout = -1);

        /// <summary>
        /// 执行串行化
        /// </summary>
        /// <param name="lockName">资源名称</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="timeout">超时时间（-1为永不超时），单位毫秒</param>
        Task Execute(DBConnectionNames connNames,string lockName, Func<Task> callBack, int timeout = -1);
    }
}
