using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.Logger;

namespace MSLibrary.Thread
{
    /// <summary>
    /// 轮询处理帮助器
    /// </summary>
    public static class PollingHelper
    {
        /// <summary>
        /// 执行轮询任务
        /// 该方法为非阻塞方法
        /// </summary>
        /// <param name="parallel"></param>
        /// <param name="configurations"></param>
        /// <returns></returns>
        public static IAsyncPollingResult Polling(List<PollingConfiguration> configurations,Func<Exception,Task> exceptionHandler)
        {
            List<Task> tasks = new List<Task>();

            AsyncPollingResultDefault result = new AsyncPollingResultDefault(
                async()=>
                {
                    //等待最终所有任务完成
                    foreach (var item in tasks)
                    {
                        await item;
                    }
                }
                );

            foreach (var item in configurations)
            {
                tasks.Add(
                    Task.Run(
                        async () =>
                        {
                            while (true)
                            {
                                if (result.IsStop)
                                {
                                    break;
                                }

                                try
                                {
                                    await item.Action();
                                }
                                catch (Exception ex)
                                {
                                    await exceptionHandler(ex);
                                    //LoggerHelper.LogError(null, $"PollingHelper Execute Error,ErrorMessage:{ex.Message},StackTrace:{ex.StackTrace}");
                                }

                                await Task.Delay(item.Interval);
                            }
                        }
                        )
                    );
            }

            return result;
        }


        /// <summary>
        /// 按指定的并行度，不间断从源中获取数据处理，当每次数据源取完后,执行perCompleteAction，wait指定的interval毫秒数，再次轮询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="maxDegree"></param>
        /// <param name="interval"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static async Task<IAsyncPollingResult> Polling<T>(Func<Task<IAsyncEnumerable<T>>> sourceGereratorFun, int maxDegree, int interval, Func<T, Task> body,Func<Task> perCompleteAction,Func<Exception,Task> exceptionHandler)
        {
            SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

            var sourceEnumerator = (await sourceGereratorFun()).GetAsyncEnumerator();

            List<Task> tasks = new List<Task>();

            AsyncPollingResultDefault result = new AsyncPollingResultDefault(
                async () =>
                {
                    //等待最终所有任务完成
                    foreach (var item in tasks)
                    {
                        await item;
                    }
                }
                );


            for (var index = 0; index <= maxDegree - 1; index++)
            {
                tasks.Add(
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            if (result.IsStop)
                            {
                                break;
                            }
                            var tempSourceEnumerator = sourceEnumerator;


                            await _lock.WaitAsync();
                            T data = default(T);
                            bool isError = false;
                            try
                            {
                                if (await sourceEnumerator.MoveNextAsync())
                                {
                                    data = sourceEnumerator.Current;
                                }
                                else
                                {
                                    if (perCompleteAction!=null)
                                    {
                                        await perCompleteAction();
                                    }
                                    await Task.Delay(interval);

                                    if (tempSourceEnumerator != sourceEnumerator)
                                    {
                                        continue;
                                    }
                                    sourceEnumerator = (await sourceGereratorFun()).GetAsyncEnumerator();
                                }
                            }
                            catch (Exception ex)
                            {
                                isError = true;
                                await exceptionHandler(ex);
                            }
                            finally
                            {
                                _lock.Release();
                            }

                            if (!isError)
                            {
                                try
                                {
                                    await body(data);
                                }
                                catch (Exception ex)
                                {
                                    await exceptionHandler(ex);
                                }
                            }

                        }
                    })
                    );

            }

            return result;

        }



    }

    /// <summary>
    /// 轮询执行结果
    /// </summary>
    public class PollingResult
    {
        private SemaphoreSlim _semaphere;
        private bool _stop;
        public PollingResult()
        {
            _semaphere = new SemaphoreSlim(1, 1);
            _stop = false;
        }

        /// <summary>
        /// 停止轮询
        /// 阻塞到当前所有任务执行完成
        /// </summary>
        public void Stop()
        {
            _stop = true;
            _semaphere.Wait();
        }

        internal bool IsStop
        {
            get
            {
                return _stop;
            }
        }

        internal SemaphoreSlim Semaphere
        {
            get
            {
                return _semaphere;
            }
        }
    }

    /// <summary>
    /// 轮询配置
    /// </summary>
    public class PollingConfiguration
    {
        /// <summary>
        /// 轮询的间隔周期（毫秒）
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 要执行的动作
        /// </summary>
        public Func<Task> Action { get; set; }
    }

    /// <summary>
    /// 轮询动作
    /// </summary>
    internal class PollingAction
    {
        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextTime { get; set; }
        /// <summary>
        /// 轮询配置
        /// </summary>
        public PollingConfiguration PollingConfiguration { get; set; }
        /// <summary>
        /// 是否已经完成
        /// </summary>
        public bool Complete { get; set; }
    }


    public interface IAsyncPollingResult
    {
        Task Stop();
    }

    public class AsyncPollingResultDefault : IAsyncPollingResult
    {
        private bool _stop = false;
        private Func<Task> _completeAction;

        public AsyncPollingResultDefault(Func<Task> completeAction)
        {
            _completeAction = completeAction;
        }

        public async Task Stop()
        {
            _stop = true;
            if (_completeAction!=null)
            {
                await _completeAction();
            }
            await Task.CompletedTask;
        }

        public bool IsStop
        {
            get
            {
                return _stop;
            }
        }
    }
}
