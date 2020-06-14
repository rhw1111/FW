using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSLibrary.Logger;

namespace MSLibrary.Coroutine.CoroutineContainerServices
{
    /// <summary>
    /// 基于多线程的协程服务
    /// 应用程序维护全局多个线程，动作随机加入线程处理
    /// </summary>
    public class CoroutineServiceForMultipleThread : ICoroutineService,IDisposable
    {
        
        private List<TaskItem> _taskItems = new List<TaskItem>();
        private static string _errorCategoryName;
        private ILoggerFactory _loggerFactory;

        /// <summary>
        /// 错误日志名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string ErrorCategoryName
        {
            set
            {
                _errorCategoryName = value;
            }
        }


        public CoroutineServiceForMultipleThread(ILoggerFactory loggerFactory, int threadCount=1)
        {
            _loggerFactory = loggerFactory;

            for(var index=1;index<=threadCount;index++)
            {
                Action<SemaphoreSlim, List<Func<IEnumerator<Task>>>> doAction = (sem,actions) =>
                  {

                          _taskItems.Add(new TaskItem()
                          {
                              ActionList = actions,
                              Task = new Task(() =>
                              {
                                  CoroutineLocalContainer.Init();

                                  while (true)
                                  {
                                      sem.Wait();

                                      Dictionary<string, KeyValuePair<Func<IEnumerator<Task>>, IEnumerator<Task>>> taskList = new Dictionary<string, KeyValuePair<Func<IEnumerator<Task>>, IEnumerator<Task>>>();

                                  //获取每个动作的迭代器，加入任务列表中
                                  lock (actions)
                                      {
                                          foreach (var item in actions)
                                          {
                                              var name = Guid.NewGuid().ToString();
                                              CoroutineLocalContainer.Generate(name);
                                              taskList.Add(name, new KeyValuePair<Func<IEnumerator<Task>>, IEnumerator<Task>>(item, item()));
                                          }
                                      }

                                      while (true)
                                      {
                                          List<Task> waitTasks = new List<Task>();
                                      //针对每个任务做处理
                                      foreach (var taskItem in taskList)
                                          {
                                              CoroutineLocalContainer.SetCurrentCoroutineName(taskItem.Key);

                                              bool needMoveNext = true;
                                              if (taskItem.Value.Value.Current != null)
                                              {
                                                  if (!taskItem.Value.Value.Current.IsCompleted && !taskItem.Value.Value.Current.IsCanceled && !taskItem.Value.Value.Current.IsFaulted)
                                                  {
                                                      waitTasks.Add(taskItem.Value.Value.Current);
                                                      needMoveNext = false;
                                                  }
                                                  else
                                                  {
                                                      if (taskItem.Value.Value.Current.Exception != null)
                                                      {
                                                          taskList.Remove(taskItem.Key);
                                                          lock (actions)
                                                          {
                                                              actions.Remove(taskItem.Value.Key);
                                                          }

                                                          CoroutineLocalContainer.Remove(taskItem.Key);

                                                          LoggerHelper.LogError(_errorCategoryName, $"CoroutineServiceForMultipleThread error,message:{taskItem.Value.Value.Current.Exception.Message},stacktrace:{taskItem.Value.Value.Current.Exception.StackTrace}");
                                                          break;
                                                      }
                                                  }
                                              }

                                              if (needMoveNext)
                                              {
                                                  bool result;
                                                  try
                                                  {
                                                  //运行到下一个断点
                                                  result = taskItem.Value.Value.MoveNext();
                                                  }
                                                  catch (Exception ex)
                                                  {
                                                      taskList.Remove(taskItem.Key);
                                                      lock (actions)
                                                      {
                                                          actions.Remove(taskItem.Value.Key);
                                                      }

                                                      CoroutineLocalContainer.Remove(taskItem.Key);

                                                      LoggerHelper.LogError(_errorCategoryName, $"CoroutineServiceForMultipleThread error,message:{ex.Message},stacktrace:{ex.StackTrace}");
                                                      break;
                                                  }

                                              //如果已经运行到结束了，则从列表中移除
                                              if (!result)
                                                  {
                                                      taskList.Remove(taskItem.Key);
                                                      lock (actions)
                                                      {
                                                          actions.Remove(taskItem.Value.Key);
                                                      }

                                                      CoroutineLocalContainer.Remove(taskItem.Key);

                                                      break;
                                                  }
                                              }

                                          }





                                          if (taskList.Count == 0)
                                          {
                                              break;
                                          }


                                          if (waitTasks.Count > 0)
                                          {
                                              Task.WaitAny(waitTasks.ToArray());
                                          }

                                      }
                                  }

                              }
                                ,
                                TaskCreationOptions.LongRunning
                              ),
                              Semaphore = sem
                          });
                      
                  };

                doAction(new SemaphoreSlim(0,1), new List<Func<IEnumerator<Task>>>());





            }
        }
        public async Task ApplyAction(Func<IEnumerator<Task>> action)
        {
            Random ran = new Random(DateTime.Now.Millisecond);
            var index = ran.Next(0, _taskItems.Count - 1);

            var taskItem = _taskItems[index];

            lock(taskItem.ActionList)
            {
                taskItem.ActionList.Add(action);
            }

            lock (taskItem.Semaphore)
            {
                if (taskItem.Semaphore.CurrentCount == 0)
                {
                    try
                    {
                        taskItem.Semaphore.Release();
                    }
                    catch (SemaphoreFullException)
                    {

                    }
                }
            }


            await Task.FromResult(0);
        }

        public async Task Run()
        {
            foreach (var item in _taskItems)
            {
                if (!item.Run)
                {
                    item.Task.Start();
                    item.Run = true;
                }
            }

            await Task.FromResult(0);
        }

        public void Dispose()
        {
            foreach(var item in _taskItems)
            {
                item.Semaphore.Dispose();
            }
        }

        /// <summary>
        /// 任务项
        /// </summary>
        private class TaskItem
        {
            public TaskItem()
            {
                Run = false;
            }

            /// <summary>
            /// 待处理动作列表
            /// </summary>
            public bool Run
            {
                get; set;
            }

            /// <summary>
            /// 待处理动作列表
            /// </summary>
            public List<Func<IEnumerator<Task>>> ActionList
            {
                get;set;
            }

            /// <summary>
            /// 负责处理动作的任务
            /// </summary>
            public Task Task
            {
                get;set;
            }
            /// <summary>
            /// 锁定信号量
            /// </summary>
            public SemaphoreSlim Semaphore
            {
                get;set;
            }
        }
    }
}
