using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Coroutine
{
    /// <summary>
    /// 协程仓储接口
    /// </summary>
    public interface ICoroutineContainerRepository
    {
        Task<CoroutineContainer> QueryByName(string name);
    }
}
