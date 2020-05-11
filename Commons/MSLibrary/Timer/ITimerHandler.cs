using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Timer
{
    /// <summary>
    /// 定时处理接口
    /// </summary>
    public interface ITimerHandler
    {
        Task Do();
    }
}
