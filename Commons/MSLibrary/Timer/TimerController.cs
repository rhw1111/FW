using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSLibrary.Logger;

namespace MSLibrary.Timer
{
    /// <summary>
    /// 定时处理器
    /// 并行执行多个定时组
    /// </summary>
    public static class TimerController
    {
        private static ConcurrentBag<TimerGroup> _groups = new ConcurrentBag<TimerGroup>();
        private static bool _run = false;
        public static void Register(TimerGroup timerItem)
        {
            lock (_groups)
            {
                _groups.Add(timerItem);
            }
        }

        public static void Execute()
        {
            if (_run)
            {
                return;
            }

            lock (_groups)
            {
                if (!_run)
                {
                   
                    Parallel.ForEach(_groups, async (item) =>
                    {
                        try
                        {
                            await item.Execute();
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.GetLogger((logger) =>
                            {
                                logger.LogError($"TimerController error:{ex.ToString()}" );
                            });
                        }

                        await Task.Delay(1);
                        while (true)
                        {
                            System.Threading.Thread.Sleep(item.Interval * 1000);

                            try
                            {
                                await item.Execute();
                            }
                            catch (Exception ex)
                            {
                                LoggerHelper.GetLogger((logger) =>
                                {
                                    logger.LogError($"TimerController error:{ex.ToString()}");
                                });
                            }
                        }
                    });


                    _run = true;
                }
            }
        }
    }
}
