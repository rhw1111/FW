using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Coroutine
{
    /// <summary>
    /// 协程容器服务
    /// </summary>
    public interface ICoroutineService
    {
        /// <summary>
        /// 向容器加入动作
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task ApplyAction( Func<IEnumerator<Task>> action);
        /// <summary>
        /// 运行容器
        /// </summary>
        /// <returns></returns>
        Task Run();
    }
}
