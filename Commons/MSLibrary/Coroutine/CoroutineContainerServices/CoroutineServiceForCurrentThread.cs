using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Coroutine.CoroutineContainerServices
{
    /// <summary>
    /// 基于当前线程的协程容器
    /// 由当前线程统一调度执行所有加入的动作,不会额外创建工作线程
    /// </summary>
    [Injection(InterfaceType = typeof(CoroutineServiceForCurrentThread), Scope = InjectionScope.Transient)]
    public class CoroutineServiceForCurrentThread : ICoroutineService
    {
        private Dictionary<string,Func<IEnumerator<Task>>> _actionList = new Dictionary<string,Func<IEnumerator<Task>>>();
        
        public CoroutineServiceForCurrentThread()
        {
            CoroutineLocalContainer.Init();
        }

        public async Task ApplyAction(Func<IEnumerator<Task>> action)
        {
            lock (_actionList)
            {
                var name = Guid.NewGuid().ToString();
                _actionList.Add(name, action);

                CoroutineLocalContainer.Generate(name);
            }
            await Task.FromResult(0);
        }

        public async Task Run()
        {
            Dictionary<string ,IEnumerator<Task>> taskList = new Dictionary<string, IEnumerator<Task>>();


            //获取每个动作的迭代器，加入任务列表中
            lock (_actionList)
            {
                foreach(var item in _actionList)
                {
                    taskList.Add(item.Key, item.Value());
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
                    if (taskItem.Value.Current != null)
                    {
                        if (!taskItem.Value.Current.IsCompleted && !taskItem.Value.Current.IsCanceled && !taskItem.Value.Current.IsFaulted)
                        {
                            waitTasks.Add(taskItem.Value.Current);
                            needMoveNext = false;
                        }
                        else
                        {
                            if (taskItem.Value.Current.Exception != null)
                            {
                                taskList.Remove(taskItem.Key);
                                lock (_actionList)
                                {
                                    _actionList.Remove(taskItem.Key);
                                }

                                CoroutineLocalContainer.Remove(taskItem.Key);

                                throw taskItem.Value.Current.Exception;
                            }
                        }
                    }

                    if (needMoveNext)
                    {
                        bool result;
                        try
                        {
                            //运行到下一个断点
                            result = taskItem.Value.MoveNext();
                        }
                        catch
                        {
                            taskList.Remove(taskItem.Key);
                            lock (_actionList)
                            {
                                _actionList.Remove(taskItem.Key);
                            }
                            CoroutineLocalContainer.Remove(taskItem.Key);

                            throw;
                        }

                        //如果已经运行到结束了，则从列表中移除
                        if (!result)
                        {
                            taskList.Remove(taskItem.Key);
                            lock (_actionList)
                            {
                                _actionList.Remove(taskItem.Key);
                            }

                            CoroutineLocalContainer.Remove(taskItem.Key);

                            break;
                        }
                    }

                }





                if (taskList.Count==0)
                {
                    break;
                }


                if (waitTasks.Count>0)
                {
                    Task.WaitAny(waitTasks.ToArray());
                }

            }

            await Task.FromResult(0);
        }
    }
}
