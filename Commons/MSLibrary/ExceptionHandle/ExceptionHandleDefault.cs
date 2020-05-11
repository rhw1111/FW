using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.ExceptionHandle
{
    public class ExceptionHandleDefault : IExceptionHandle
    {
        public static IList<ExceptionRetryCheckHandleContainer> RetryCheckHandles { get; } = new List<ExceptionRetryCheckHandleContainer>();


        public void Handle(int maxRetry, int waitMillisecond, Action action)
        {
            int retryNumber = maxRetry;

            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception ex)
                {
                    if (retryNumber-- <= 0)
                    {
                        throw;
                    }

                    var type = ex.GetType();

                    bool isHandle = false;
                    foreach(var item in RetryCheckHandles)
                    {
                        if (item.Type.IsInstanceOfType(ex))
                        {
                            isHandle = true;
                            var handle = item.HandleFactory.Create();

                            if (!handle.CheckSync(ex))
                            {
                                throw;
                            }
                            continue;
                        }
                    }

                    if (!isHandle)
                    {
                        throw;
                    }


                    System.Threading.Thread.Sleep(waitMillisecond);
                }
            }
        }

        public async Task HandleAsync(int maxRetry, int waitMillisecond, Func<Task> action)
        {
            int retryNumber = maxRetry;

            while (true)
            {
                try
                {
                    await action();
                    break;
                }
                catch (Exception ex)
                {
                    if (retryNumber-- <= 0)
                    {
                        throw;
                    }

                    var type = ex.GetType();

                    bool isHandle = false;
                    foreach (var item in RetryCheckHandles)
                    {
                        if (item.Type.IsInstanceOfType(ex))
                        {
                            isHandle = true;
                            var handle = item.HandleFactory.Create();

                            if (!await handle.Check(ex))
                            {
                                throw;
                            }
                            continue;
                        }
                    }

                    if (!isHandle)
                    {
                        throw;
                    }


                    await Task.Delay(waitMillisecond);
                }
            }
        }

    }

    public class ExceptionRetryCheckHandleContainer
    {
        public Type Type { get; set; }
        public IFactory<IExceptionRetryCheckHandle> HandleFactory { get; set; }
    }
}
