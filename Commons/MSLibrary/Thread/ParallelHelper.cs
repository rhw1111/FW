using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MSLibrary.Thread.ParallelTaskWrappers;

namespace MSLibrary.Thread
{
    /// <summary>
    /// 并行执行帮助器
    /// 指定最大并行度，保证并发度
    /// </summary>
    public class ParallelHelper
    {
        public static IList<IParallelTaskWrapper> ParallelTaskWrappers { get; } = new List<IParallelTaskWrapper>() { new ParallelTaskWrapperForDBTransactionScope()};
        private Semaphore _semaphore;
        private int _maxParalle;


        public ParallelHelper(int maxParalle)
        {
            _semaphore = new Semaphore(maxParalle, maxParalle);
            _maxParalle = maxParalle;
        }

        private static async Task wrappperRun(Func<Task> action,int index=0)
        {
            if (ParallelTaskWrappers.Count==0)
            {
                await action();
                return;
            }

            var current = ParallelTaskWrappers[index];

            if (index+1<=ParallelTaskWrappers.Count-1)
            {
                var next = ParallelTaskWrappers[index+1];
                await current.Execute(async () =>
                {
                    await wrappperRun(action, index + 1);
                });
            }
            else
            {
                await current.Execute(async () =>
                {
                    await action();
                });
            }
            
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="taskList">
        /// 运行期的任务列表
        /// 传入一个new List<Task>即可
        /// </param>
        /// <param name="errorHandler">错误处理</param>
        /// <param name="callBack">动作处理</param>
        public void Run(List<Task> taskList, Action<Exception> errorHandler, Action callBack)
        {
            _semaphore.WaitOne();

            List<Task> removeList = new List<Task>();

            foreach (var taskItem in taskList)
            {
                if (taskItem.IsCompleted)
                {
                    removeList.Add(taskItem);
                }
            }

            foreach (var taskItem in removeList)
            {
                taskList.Remove(taskItem);
            }

            var t = Task.Run(async () =>
            {
                await wrappperRun(async () =>
                {
                    try
                    {
                        callBack();
                    }
                    catch (Exception ex)
                    {
                        errorHandler(ex);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }

                    await Task.CompletedTask;
                });

            });

            taskList.Add(t);

        }


        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="taskList">
        /// 运行期的任务列表
        /// 传入一个new List<Task>即可
        /// </param>
        /// <param name="errorHandler">错误处理</param>
        /// <param name="callBack">动作处理</param>
        public void Run(List<Task> taskList, Func<Exception, Task> errorHandler, Func<Task> callBack)
        {
            _semaphore.WaitOne();

            List<Task> removeList = new List<Task>();

            foreach (var taskItem in taskList)
            {
                if (taskItem.IsCompleted)
                {
                    removeList.Add(taskItem);
                }
            }

            foreach (var taskItem in removeList)
            {
                taskList.Remove(taskItem);
            }

            var t = Task.Run(async () =>
            {
                await wrappperRun(async () =>
                {
                    try
                    {
                        await callBack().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await errorHandler(ex).ConfigureAwait(false);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                });
            });

            taskList.Add(t);

        }


        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="actionList">
        /// 要运行的任务列表
        /// </param>
        /// <param name="errorHandler">错误处理</param>
        public async Task RunAsync(List<RunAsyncAction> actionList)
        {
            List<RunActionContainer> containerList = new List<RunActionContainer>();
            foreach(var item in actionList)
            {
                containerList.Add(new RunActionContainer() { Use = false, Action = item });
            }


            List<Task> result = new List<Task>();
            int realNumber;
            if (actionList.Count>=_maxParalle)
            {
                realNumber = _maxParalle;
            }
            else
            {
                realNumber = actionList.Count;
            }



            for (var index=0;index<=realNumber-1;index++)
            {
                var currentContainer=containerList[index];
                currentContainer.Use = true;
                var t = Task.Run(async () =>
                {
                    await wrappperRun(async () =>
                    {
                        await RunAsyncInnerAction(containerList, currentContainer);
                    });
                    
                });

                result.Add(t);
            }

            foreach(var resultItem in result)
            {
                await resultItem;
            }
        }

        private async Task RunAsyncInnerAction(List<RunActionContainer> containerList,RunActionContainer container)
        {
            try
            {
                await container.Action.Action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await container.Action.ErrorHandler(ex).ConfigureAwait(false);
            }

            RunActionContainer nextContainer;
            lock (containerList)
            {
                nextContainer = (from item in containerList
                                 where item.Use == false
                                 select item).FirstOrDefault();
                if (nextContainer != null)
                {
                    nextContainer.Use = true;
                }
            }

            if (nextContainer != null)
            {
                await RunAsyncInnerAction(containerList,nextContainer);
            }
        }

        public async Task RunBreakAsync(List<RunBreakAsyncAction> actionList)
        {
            List<RunBreakActionContainer> containerList = new List<RunBreakActionContainer>();
            foreach(var item in actionList)
            {
                containerList.Add(new RunBreakActionContainer() { Use = false, Action = item });
            }

            List<Task<bool>> result = new List<Task<bool>>();
            int realNumber;
            if (actionList.Count>=_maxParalle)
            {
                realNumber = _maxParalle;
            }
            else
            {
                realNumber = actionList.Count;
            }

            for(var index=0;index<=realNumber-1;index++)
            {
                var currentContainer = containerList[index];
                currentContainer.Use = true;
                var t = Task.Run(async () =>
                  {
                      return await RunBreakAsyncInnerAction(containerList, currentContainer);
                  });

                result.Add(t);
            }

            foreach(var resultItem in result)
            {
                if (await resultItem)
                {
                    break;
                }
            }
        }

        private async Task<bool> RunBreakAsyncInnerAction(List<RunBreakActionContainer> containerList,RunBreakActionContainer container)
        {
            bool result;
            try
            {
                result = await container.Action.Action().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                result = await container.Action.ErrorHandler(ex).ConfigureAwait(false);
            }

            if (result)
            {
                return result;
            }

            RunBreakActionContainer nextContainer;
            lock(containerList)
            {
                nextContainer = (from item in containerList
                                 where item.Use == false
                                 select item).FirstOrDefault();
                if (nextContainer!=null)
                {
                    nextContainer.Use = true;
                }
            }

            if (nextContainer!=null)
            {
                return await RunBreakAsyncInnerAction(containerList, nextContainer);
            }
            else
            {
                return false;
            }
        }

        private class RunBreakActionContainer
        {
            public bool Use { get; set; }
            public RunBreakAsyncAction Action { get; set; }
        }

        private class RunActionContainer
        {
            public bool Use { get; set; }
            public RunAsyncAction Action { get; set; }
        }


        /// <summary>
        /// 循环执行，直到callback返回false或者抛出异常
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="errorHandler"></param>
        /// <param name="callBack"></param>
        public void RunCircle(List<Task> taskList, Action<Exception> errorHandler, Func<bool> callBack)
        {
            List<Task> removeList = new List<Task>();

            foreach (var taskItem in taskList)
            {
                if (taskItem.IsCompleted)
                {
                    removeList.Add(taskItem);
                }
            }

            foreach (var taskItem in removeList)
            {
                taskList.Remove(taskItem);
            }

            var t = Task.Run(async() =>
            {
                await wrappperRun(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            _semaphore.WaitOne();

                            var result = callBack();

                            if (!result)
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorHandler(ex);
                            break;
                        }
                        finally
                        {
                            _semaphore.Release();
                        }
                    }
                    await Task.CompletedTask;
                });

            });

            taskList.Add(t);
        }


        /// <summary>
        /// 循环执行，直到callback返回false或者抛出异常
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="errorHandler"></param>
        /// <param name="callBack"></param>
        public void RunCircle(List<Task> taskList, Func<Exception, Task> errorHandler, Func<Task<bool>> callBack)
        {
            List<Task> removeList = new List<Task>();

            foreach (var taskItem in taskList)
            {
                if (taskItem.IsCompleted)
                {
                    removeList.Add(taskItem);
                }
            }

            foreach (var taskItem in removeList)
            {
                taskList.Remove(taskItem);
            }

            var t = Task.Run(async () =>
            {
                await wrappperRun(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            _semaphore.WaitOne();

                            var result = await callBack().ConfigureAwait(false);

                            if (!result)
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            await errorHandler(ex).ConfigureAwait(false);
                            break;
                        }
                        finally
                        {
                            _semaphore.Release();
                        }
                    }
                });
             

            });

            taskList.Add(t);
        }


        public static async Task ForEach<T>(IEnumerable<T> source, int maxDegree, Func<T, Task> body)
        {
            using (SemaphoreSlim _lock = new SemaphoreSlim(1, 1))
            {
                var sourceEnumerator = source.GetEnumerator();

                List<Task> tasks = new List<Task>();
                //按指定并行度并行执行
                for (var index = 0; index <= maxDegree - 1; index++)
                {
                    tasks.Add(
                        Task.Run(async () =>
                        {
                            await wrappperRun(async () =>
                            {
                                ///每个任务完成当前数据源项后，再从数据源获取下一个项
                                ///充分利用资源，不会因为任务项执行时间的长短不同发生等待
                                while (true)
                                {
                                    T data = default(T);
                                    //使用信号量控制数据源移动
                                    await _lock.WaitAsync();
                                    try
                                    {
                                        if (sourceEnumerator.MoveNext())
                                        {
                                            data = sourceEnumerator.Current;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    finally
                                    {
                                        _lock.Release();
                                    }

                                    await body(data);
                                }
                            });

                        })
                        );

                }

                //等待最终所有任务完成
                foreach (var item in tasks)
                {
                    await item;
                }
            }
        }

        /// <summary>
        /// 按指定的并行度，不间断从源中获取数据处理，直到源中数据走到最后
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="maxDegree"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static async Task ForEach<T>(IAsyncEnumerable<T> source, int maxDegree, Func<T, Task> body)
        {
            var sourceEnumerator = source.GetAsyncEnumerator();

            using (SemaphoreSlim _lock = new SemaphoreSlim(1, 1))
            {
                List<Task> tasks = new List<Task>();

                for (var index = 0; index <= maxDegree - 1; index++)
                {
                    tasks.Add(
                        Task.Run(async () =>
                        {
                            await wrappperRun(async () =>
                            {
                                while (true)
                                {
                                    T data = default(T);
                                    await _lock.WaitAsync();
                                    try
                                    {
                                        if (await sourceEnumerator.MoveNextAsync())
                                        {
                                            data = sourceEnumerator.Current;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    finally
                                    {
                                        _lock.Release();
                                    }

                                    await body(data);
                                }
                            });

                        })
                        );

                }


                //等待最终所有任务完成
                foreach (var item in tasks)
                {
                    await item;
                }
            }



        }



        /// <summary>
        /// 按照指定的并行度并行执行action，
        /// action的第一个参数起始值为0，累加，直到全部action都返回false，执行完毕
        /// </summary>
        /// <param name="maxParalle"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task RunCircle(int maxParalle,Func<int,Task<bool>> action)
        {
            object lockObj = new object();
            int num = 0;
            List<Task> taskList = new List<Task>();
            for(var index=0;index<=maxParalle-1;index++)
            {
                Task newTask = Task.Run(async () =>
                  {
                      await wrappperRun(async () =>
                      {
                          int actionIndex;
                          while (true)
                          {
                              lock (lockObj)
                              {
                                  actionIndex = num;
                                  num++;
                              }
                              if (!await action(actionIndex))
                              {
                                  break;
                              }
                          }
                      });

                  });
                taskList.Add(newTask);
            }

            foreach (var item in taskList)
            {
                await item;
            }
        }

        /// <summary>
        /// 并行向前执行
        /// 按照指定的并行度，获取资源向前执行，获取资源时，起始位置为0，每次调用累加1，直到返回元组中第二个值为false
        /// 资源中的IEnumerable<T>依次交给action执行，直到当前资源完全执行完成，则再次获取下一批资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="getSourceAction"></param>
        /// <returns></returns>
        public async Task RunForward<T>(Func<T,Task> action,Func<int,Task<(IEnumerable<T>,bool)>> getSourceAction)
        {
            int currentParallel = 0;
            int sourceIndex = 0;
            object lockParallel = new object();
            object lockSource = new object();
            object lockGetSource = new object();
            Exception exception=null;

            List<Task> currentTasks = new List<Task>();

            bool getSourceResult = false;
            bool getSourceBit = false;
            IEnumerator<T> sourceEnumerator = null;

            Func<int, Task> getNextSourceEnumerator = async (i) =>
            {
                IEnumerable<T> sources;
                (sources, getSourceResult) = await getSourceAction(i);

                sourceEnumerator = sources.GetEnumerator();

            };

            await getNextSourceEnumerator(sourceIndex);



            Func<(T, bool)> getNextSourceItem = () =>
            {
                T itemResult = default(T);
                bool hasResult = true;
                lock (lockSource)
                {
                    hasResult = sourceEnumerator.MoveNext();
                    if (hasResult)
                    {
                        itemResult = sourceEnumerator.Current;
                    }
                }

                return (itemResult, hasResult);
            };

            Action innerAction = null;

            Action refreashAction = () =>
              {
                      var number = _maxParalle - currentParallel;
                      for (var index = 0; index <= number - 1; index++)
                      {
                          innerAction();
                      }                  
              };

            innerAction = () =>
               {
                   Task task = null;
                   task = new Task(async()=>
                   {
                       await wrappperRun(async () =>
                       {
                           IEnumerator<T> tempSourceEnumerator = null;
                           try
                           {

                               T item;
                               bool itemResult;
                               while (true)
                               {
                                   tempSourceEnumerator = sourceEnumerator;

                                   (item, itemResult) = getNextSourceItem();
                                   if (itemResult)
                                   {
                                       await action(item);
                                   }
                                   else
                                   {
                                       if (!getSourceResult)
                                       {
                                           break;
                                       }

                                       bool canDo = false;
                                       if (!getSourceBit)
                                       {
                                           lock (lockGetSource)
                                           {
                                               if (!getSourceBit)
                                               {
                                                   sourceIndex++;
                                                   getSourceBit = true;
                                                   canDo = true;
                                               }
                                           }
                                       }

                                       if (canDo)
                                       {
                                           if (tempSourceEnumerator == sourceEnumerator)
                                           {
                                               try
                                               {
                                                   await getNextSourceEnumerator(sourceIndex);
                                               }
                                               finally
                                               {
                                                   getSourceBit = false;
                                               }

                                               refreashAction();
                                           }
                                       }
                                       else
                                       {
                                           break;
                                       }

                                   }
                               }
                           }
                           catch (Exception ex)
                           {
                               exception = ex;
                           }
                           finally
                           {
                               lock (currentTasks)
                               {
                                   currentTasks.Remove(task);
                               }

                               lock (lockParallel)
                               {
                                   currentParallel--;
                               }

                               if (tempSourceEnumerator != sourceEnumerator)
                               {
                                   refreashAction();
                               }
                           }
                       });


                   });

                   bool addResult = false;
                   if (currentParallel<_maxParalle)
                   {
                       lock(lockParallel)
                       {
                           if (currentParallel < _maxParalle)
                           {
                               currentParallel++;
                               addResult = true;
                           }
                       }
                   }

 

                   if (addResult)
                   {
                       task.Start();

                       lock (currentTasks)
                       {
                           //var completeTasks
                           currentTasks.Add(task);
                       }
                   }
                   
               };




            refreashAction();

            while(currentParallel>0)
            {
                if (exception!=null)
                {
                    throw exception;
                }

                Task actionTask=null;
                lock(currentTasks)
                {
                    actionTask = currentTasks.FirstOrDefault();
                }

                if (actionTask != null)
                {
                    await actionTask;
                }
                else
                {
                    if (currentParallel > 0)
                    {
                        await Task.Delay(10);
                    }
                }
            }

            if (exception != null)
            {
                throw exception;
            }

        }


    }


    public class RunAsyncAction
    {
        public Func<Task> Action { get; set; }

        public Func<Exception, Task> ErrorHandler { get; set; }
    }

    public class RunBreakAsyncAction
    {
        public Func<Task<bool>> Action { get; set; }

        public Func<Exception, Task<bool>> ErrorHandler { get; set; }
    }

    public interface IParallelTaskWrapper
    {
        Task Execute(Func<Task> action);
    }
}
