using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;
using MSLibrary.ExceptionHandle.CheckHandles;

namespace MSLibrary.ExceptionHandle
{
    /// <summary>
    /// 异常处理静态类
    /// </summary>
    public static class ExceptionHandleHepler
    {
        public static IExceptionHandle ExceptionHandle
        {
            private get;
            set;
        } = new ExceptionHandleDefault();

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="maxRetry">最大重试次数</param>
        /// <param name="waitMillisecond">重试时等待的毫秒数</param>
        /// <param name="action">业务操作</param>
        public static void Handle(int maxRetry, int waitMillisecond, Action action)
        {
            ExceptionHandle.Handle(maxRetry, waitMillisecond, action);
        }
        /// <summary>
        /// 处理异常（异步）
        /// </summary>
        /// <param name="maxRetry">最大重试次数</param>
        /// <param name="waitMillisecond">重试时等待的毫秒数</param>
        /// <param name="action">业务操作</param>
        public static async Task HandleAsync(int maxRetry, int waitMillisecond, Func<Task> action)
        {
            await ExceptionHandle.HandleAsync(maxRetry, waitMillisecond, action);
        }

    }

    /// <summary>
    /// 异常重试检查处理
    /// </summary>
    public interface IExceptionRetryCheckHandle
    {
        Task<bool> Check(Exception ex);
        bool CheckSync(Exception ex);
    }



}
