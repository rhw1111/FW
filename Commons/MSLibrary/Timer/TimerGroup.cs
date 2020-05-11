using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Timer
{
    /// <summary>
    /// 定时处理组
    /// 一组可以注册多个定时处理，以同一个时间间隔执行
    /// </summary>
    public abstract class TimerGroup
    {
        /// <summary>
        /// 执行间隔（秒）
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 注册定时器的处理器工厂
        /// </summary>
        /// <param name="handlerFactory"></param>
        public abstract void Register(IFactory<ITimerHandler> handlerFactory);
        /// <summary>
        /// 执行
        /// </summary>
        public abstract Task Execute();
    }
}
