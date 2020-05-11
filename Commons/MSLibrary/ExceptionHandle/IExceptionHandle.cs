using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.ExceptionHandle
{
    /// <summary>
    /// 异常处理接口
    /// </summary>
    public interface IExceptionHandle
    {
        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="maxRetry">最大重试次数</param>
        /// <param name="waitMillisecond">重试时等待的毫秒数</param>
        /// <param name="action">业务操作</param>
        void Handle(int maxRetry, int waitMillisecond, Action action);
        /// <summary>
        /// 处理异常（异步）
        /// </summary>
        /// <param name="maxRetry">最大重试次数</param>
        /// <param name="waitMillisecond">重试时等待的毫秒数</param>
        /// <param name="action">业务操作</param>
        Task HandleAsync(int maxRetry, int waitMillisecond, Func<Task> action);
    }
}
