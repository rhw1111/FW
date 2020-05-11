using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.DAL;
using MSLibrary.Transaction;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 工作流资源
    /// 维护参与工作流的资源信息以及流程状态
    /// 资源类型+资源关键字为唯一信息
    /// </summary>
    public class WorkflowResource : EntityBase<IWorkflowResourceIMP>
    {
        private static IFactory<IWorkflowResourceIMP> _workflowResourceIMPFactory;

        public static IFactory<IWorkflowResourceIMP> WorkflowResourceIMPFactory
        {
            set
            {
                _workflowResourceIMPFactory = value;
            }
        }
        public override IFactory<IWorkflowResourceIMP> GetIMPFactory()
        {
            return _workflowResourceIMPFactory;
        }



        /// <summary>
        /// id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }

        /// <summary>
        /// 资源关键字
        /// </summary>
        public string Key
        {
            get
            {
                return GetAttribute<string>("Key");
            }
            set
            {
                SetAttribute<string>("Key", value);
            }
        }

        /// <summary>
        /// 资源当前状态
        /// </summary>
        public int Status
        {
            get
            {
                return GetAttribute<int>("Status");
            }
            set
            {
                SetAttribute<int>("Status", value);
            }
        }

        /// <summary>
        /// 资源初始状态
        /// </summary>
        public int InitStatus
        {
            get
            {
                return GetAttribute<int>("InitStatus");
            }
            set
            {
                SetAttribute<int>("InitStatus", value);
            }
        }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        /// <summary>
        /// 修改资源状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task UpdateStatus(int status)
        {
            await _imp.UpdateStatus(this, status);
        }

        /// <summary>
        /// 为资源增加步骤
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public async Task AddStep(WorkflowStep step)
        {
            await _imp.AddStep(this, step);
        }

        /// <summary>
        /// 根据步骤动作名称和状态获取该资源下的所有该名称的步骤
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetStep(string actionName, int status, Func<WorkflowStep, Task> callback)
        {
            await _imp.GetStep(this, actionName, status, callback);
        }


        /// <summary>
        /// 获取该资源下的所有步骤
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetStep(Func<WorkflowStep, Task> callback)
        {
            await _imp.GetStep(this, callback);
        }

        /// <summary>
        /// 获取该资源下当前状态的所有步骤
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetStepByCurrentStatus(Func<WorkflowStep, Task> callback)
        {
            await _imp.GetStepByCurrentStatus(this, callback);
        }

        /// <summary>
        /// 删除资源下面指定动作名称和状态的工作流步骤
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task DeleteStep(string actionName, int status)
        {
            await _imp.DeleteStep(this, actionName, status);
        }
        /// <summary>
        /// 删除资源下面指定状态和批次的工作流步骤(排除指定的排除动作名称的步骤)
        /// </summary>
        /// <param name="status"></param>
        /// <param name="serialNo"></param>
        /// <param name="excludeActionName"></param>
        /// <returns></returns>
        public async Task DeleteStep(int status, string serialNo, string excludeActionName)
        {
            await _imp.DeleteStep(this, status, serialNo, excludeActionName);
        }

        /// <summary>
        /// 根据步骤动作名称获取该资源下的所有指定动作名称和状态的步骤下面的所有用户动作
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetUserAction(string actionName, int status, Func<WorkflowStepUserAction, Task> callback)
        {
            await _imp.GetUserAction(this, actionName, status, callback);
        }

        /// <summary>
        /// 获取该资源的当前状态下的所有步骤下面的所有用户动作
        /// </summary>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetUserActionByCurrentStatus(Func<WorkflowStepUserAction, Task> callback)
        {
            await _imp.GetUserActionByCurrentStatus(this, callback);
        }




        /// <summary>
        /// 将当前资源退回到前一个状态
        /// </summary>
        /// <param name="returnCallback"></param>
        /// <returns></returns>
        public async Task ReturnToPreStatus(Func<WorkflowStepUserAction, Task> returnCallback)
        {
            await _imp.ReturnToPreStatus(this, returnCallback);
        }

        /// <summary>
        /// 将当前资源退回到指定状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="returnCallback"></param>
        /// <returns></returns>
        public async Task ReturnToStatus(int status, Func<WorkflowStepUserAction, Task> returnCallback)
        {
            await _imp.ReturnToStatus(this, status, returnCallback);
        }

        /// <summary>
        /// 修改指定动作名称和状态的所有步骤的完成状态
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="isComplete"></param>
        /// <param name="returnCallback"></param>
        /// <returns></returns>
        public async Task UpdateStepCompleteStatus(string actionName, int status, bool isComplete, Func<WorkflowStepUserAction, Task> returnCallback)
        {
            await _imp.UpdateStepCompleteStatus(this, actionName, status, isComplete, returnCallback);
        }


        /// <summary>
        /// 指定动作名称和状态的所有步骤增加用户动作
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="userKey"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task AddUserAction(string actionName, int status, string userKey, int result)
        {
            await _imp.AddUserAction(this, actionName, status, userKey, result);
        }

        /// 验证用户是否匹配该资源下指定动作名称的步骤
        /// 只要有一个步骤匹配即返回ture
        public async Task<ValidateResult> ValidateUser(string actionName, string userKey)
        {
            return await _imp.ValidateUser(this, actionName, userKey);
        }

        /// <summary>
        /// 检查指定动作名称和状态的所有步骤是否都已经完成
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> IsStepComplete(string actionName, int status)
        {
            return await _imp.IsStepComplete(this, actionName, status);
        }

        /// <summary>
        /// 获取关联的所有用户动作
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetUserAction(Func<WorkflowStepUserAction, Task> callback)
        {
            await _imp.GetUserAction(this, callback);
        }
    }

    public interface IWorkflowResourceIMP
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task Add(WorkflowResource resource);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task Delete(WorkflowResource resource);
        /// <summary>
        /// 修改资源状态
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task UpdateStatus(WorkflowResource resource, int status);
        /// <summary>
        /// 为资源增加步骤
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        Task AddStep(WorkflowResource resource, WorkflowStep step);
        /// <summary>
        /// 根据步骤动作名称和状态获取该资源下的所有该名称的步骤
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task GetStep(WorkflowResource resource, string actionName, int status, Func<WorkflowStep, Task> callback);
        /// <summary>
        /// 获取该资源下的所有步骤
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetStep(WorkflowResource resource, Func<WorkflowStep, Task> callback);
        /// <summary>
        /// 获取该资源下当前状态的所有步骤
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetStepByCurrentStatus(WorkflowResource resource, Func<WorkflowStep, Task> callback);

        /// <summary>
        /// 删除资源下面指定动作名称和状态的工作流步骤
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task DeleteStep(WorkflowResource resource, string actionName, int status);
        /// <summary>
        /// 删除资源下面指定状态和批次的工作流步骤(排除指定的排除动作名称的步骤)
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="status"></param>
        /// <param name="serialNo"></param>
        /// <param name="excludeActionName"></param>
        /// <param name="excludeStatus"></param>
        /// <returns></returns>
        Task DeleteStep(WorkflowResource resource, int status, string serialNo, string excludeActionName);
        /// <summary>
        /// 根据步骤动作名称获取该资源下的所有指定动作名称和状态的步骤下面的所有用户动作
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetUserAction(WorkflowResource resource, string actionName, int status, Func<WorkflowStepUserAction, Task> callback);
        /// <summary>
        /// 获取当前资源状态下的所有用户动作
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetUserActionByCurrentStatus(WorkflowResource resource, Func<WorkflowStepUserAction, Task> callback);


        /// <summary>
        /// 获取关联的所有用户动作
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetUserAction(WorkflowResource resource, Func<WorkflowStepUserAction, Task> callback);



        /// <summary>
        /// 将当前资源退回到前一个状态
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="returnCallback">被回退的流程中参与步骤的用户动作被移除时触发</param>
        /// <returns></returns>
        Task ReturnToPreStatus(WorkflowResource resource, Func<WorkflowStepUserAction, Task> returnCallback);
        /// <summary>
        /// 将当前资源退回到指定状态
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="status">要退回到的状态</param>
        /// <param name="returnCallback">被回退的流程中参与步骤的用户动作被移除时触发</param>
        /// <returns></returns>
        Task ReturnToStatus(WorkflowResource resource, int status, Func<WorkflowStepUserAction, Task> returnCallback);
        /// <summary>
        /// 修改指定动作名称和状态的所有步骤的完成状态
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="isComplete"></param>
        /// <param name="returnCallback">当每个步骤上面的每个用户动作被移除时触发</param>
        /// <returns></returns>
        Task UpdateStepCompleteStatus(WorkflowResource resource, string actionName, int status, bool isComplete, Func<WorkflowStepUserAction, Task> returnCallback);
        /// <summary>
        /// 指定动作名称和状态的所有步骤增加用户动作
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="userKey"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        Task AddUserAction(WorkflowResource resource, string actionName, int status, string userKey, int result);
        /// <summary>
        /// 验证用户是否匹配该资源下指定动作名称的步骤
        /// 只要有一个步骤匹配即返回ture
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<ValidateResult> ValidateUser(WorkflowResource resource, string actionName, string userKey);
        /// <summary>
        /// 检查指定动作名称和状态的所有步骤是否都已经完成
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<bool> IsStepComplete(WorkflowResource resource, string actionName, int status);
    }


    [Injection(InterfaceType = typeof(IWorkflowResourceIMP), Scope = InjectionScope.Transient)]
    public class WorkflowResourceIMP : IWorkflowResourceIMP
    {
        private IWorkflowResourceStore _workflowResourceStore;
        private IWorkflowStepStore _workflowStepStore;
        private IWorkflowStepUserActionStore _workflowStepUserActionStore;
        private IWorkflowEntityStore _workflowEntityStore;

        public WorkflowResourceIMP(IWorkflowResourceStore workflowResourceStore, IWorkflowStepStore workflowStepStore, IWorkflowStepUserActionStore workflowStepUserActionStore, IWorkflowEntityStore workflowEntityStore)
        {
            _workflowResourceStore = workflowResourceStore;
            _workflowStepStore = workflowStepStore;
            _workflowStepUserActionStore = workflowStepUserActionStore;
            _workflowEntityStore = workflowEntityStore;
        }

        public async Task Add(WorkflowResource resource)
        {
            await _workflowResourceStore.Add(resource);
        }

        public async Task AddStep(WorkflowResource resource, WorkflowStep step)
        {
            //判断当前步骤是否存在，若存在，则不重复创建
            var isExistStep = await _workflowStepStore.IsExistStepByKey(resource.Type, resource.Key, resource.ID, resource.Status, step.ActionName, step.UserType, step.UserKey);

            if (!isExistStep)
            {
                //自动关联资源
                step.ResourceID = resource.ID;
                //自动补上当前资源状态
                step.Status = resource.Status;

                //如果步骤的批次号为null，则需要查找该资源下status=step.Status，createtime最晚的步骤
                //如果找到，则将找到的步骤的批次号赋值给step的批次号
                //如果没找到，则生成一个随机批次号
                var latestStep = await _workflowStepStore.QueryLatestByResource(resource.Type, resource.Key, resource.ID, step.Status);
                if (latestStep != null)
                {
                    step.SerialNo = latestStep.SerialNo;
                }
                else
                {
                    step.SerialNo = Guid.NewGuid().ToString();
                }


                await _workflowStepStore.Add(resource.Type, resource.Key, step);
            }
        }

        public async Task AddUserAction(WorkflowResource resource, string actionName, int status, string userKey, int result)
        {
            await using (var scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 1, 0) }))
            {
                List<WorkflowStep> stepList = new List<WorkflowStep>();
                //找到该资源下面的所有名称为actionName,状态为status的步骤
                await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, actionName, status,
                    async (step) =>
                    {
                        if ((await step.ValidateUser(userKey)).Result)
                        {
                            stepList.Add(step);
                        }
                    }
                    );

                //对所有符合条件的step增加用户动作
                foreach (var item in stepList)
                {
                    WorkflowStepUserAction action = new WorkflowStepUserAction()
                    {
                        StepID = item.ID,
                        Step = item,
                        Result = result,
                        UserKey = userKey
                    };

                    await _workflowStepUserActionStore.Add(resource.Type, resource.Key, action);
                    //判断是否该步骤下面的用户动作数量是否>=该步骤的用户数量，如果是，则需要修改该步骤的完成状态

                    var actionCount = await _workflowStepUserActionStore.QueryCountByStep(resource.Type, resource.Key, item.ID);

                    if (actionCount >= item.UserCount)
                    {
                        await _workflowStepStore.UpdateCompleteStatus(resource.Type, resource.Key, resource.ID, item.ID, true);
                    }
                }


                scope.Complete();
            }

        }

        public async Task Delete(WorkflowResource resource)
        {
            await _workflowResourceStore.Delete(resource.Type, resource.Key, resource.ID);
        }

        public async Task DeleteStep(WorkflowResource resource, string actionName, int status)
        {
            await _workflowStepStore.Delete(resource.Type, resource.Key, resource.ID, actionName, status);
        }

        public async Task DeleteStep(WorkflowResource resource, int status, string serilaNo, string excludeActionName)
        {
            await _workflowStepStore.Delete(resource.Type, resource.Key, resource.ID, status, serilaNo, excludeActionName);
        }

        public async Task GetStep(WorkflowResource resource, string actionName, int status, Func<WorkflowStep, Task> callback)
        {
            await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, actionName, status, callback);
        }

        public async Task GetUserAction(WorkflowResource resource, string actionName, int status, Func<WorkflowStepUserAction, Task> callback)
        {
            await _workflowStepUserActionStore.QueryByResource(resource.Type, resource.Key, resource.ID, actionName, status, callback);
        }

        public async Task<bool> IsStepComplete(WorkflowResource resource, string actionName, int status)
        {
            bool result = true;

            await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, actionName, status, async (step) =>
            {
                if (!step.Complete)
                {
                    result = false;
                }

                await Task.FromResult(0);
            });

            return result;
        }

        public async Task ReturnToPreStatus(WorkflowResource resource, Func<WorkflowStepUserAction, Task> returnCallback)
        {
            using (var scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 1, 0) }))
            {
                //找到最近步骤的前一个步骤
                var preStep = await _workflowStepStore.QueryPreStep(resource.Type, resource.Key, resource.ID);
                if (preStep != null)
                {
                    //获取创建时间大于等于preStep的CreateTime and SerialNo不等于preStep的SerialNo的所有步骤
                    //这些步骤需要被移除
                    List<WorkflowStep> removeSteps = new List<WorkflowStep>();
                    await _workflowStepStore.QueryByCreateTime(resource.Type, resource.Key, resource.ID, preStep.SerialNo, preStep.CreateTime,
                        async (step) =>
                        {
                            removeSteps.Add(step);
                            await Task.FromResult(0);
                        }
                        );

                    foreach (var item in removeSteps)
                    {
                        //每一个需要移除的步骤，都需要获取关联的用户动作，为这些用户动作执行回调
                        await _workflowStepUserActionStore.QueryByStep(resource.Type, resource.Key, item.ID, returnCallback);
                        //删除步骤
                        await _workflowStepStore.Delete(resource.Type, resource.Key, resource.ID, item.ID);
                    }

                    //找到所有SerialNo=preStep.SerialNo的步骤，删除它们下面的所有用户动作，
                    //对这些动作执行回调
                    //再把它们的complete全部改成false
                    await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, preStep.SerialNo,
                        async (step) =>
                        {
                            await _workflowStepUserActionStore.QueryByStep(resource.Type, resource.Key, step.ID,
                                async (action) =>
                                {
                                    await returnCallback(action);
                                }
                                );
                            await _workflowStepUserActionStore.Delete(resource.Type, resource.Key, step.ID);
                            await _workflowStepStore.UpdateCompleteStatus(resource.Type, resource.Key, resource.ID, step.ID, false);
                        }
                        );

                    //修改资源状态为preStep的状态
                    await _workflowResourceStore.UpdateStatus(resource.Type, resource.Key, resource.ID, preStep.Status);
                }
                else
                {
                    //需要退回到最初状态，获取资源下所有步骤，为步骤下面每个用户动作调用回调，
                    //删除所有的步骤，修改资源状态为初始状态
                    List<WorkflowStep> removeSteps = new List<WorkflowStep>();
                    await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, async (step) =>
                    {
                        removeSteps.Add(step);
                        await Task.FromResult(0);
                    });

                    foreach (var item in removeSteps)
                    {
                        //每一个需要移除的步骤，都需要获取关联的用户动作，为这些用户动作执行回调
                        await _workflowStepUserActionStore.QueryByStep(resource.Type, resource.Key, item.ID, returnCallback);
                        //删除步骤
                        await _workflowStepStore.Delete(resource.Type, resource.Key, resource.ID, item.ID);
                    }

                    //修改资源状态为preStep的状态
                    await _workflowResourceStore.UpdateStatus(resource.Type, resource.Key, resource.ID, resource.InitStatus);
                }

                scope.Complete();
            }
        }

        public async Task ReturnToStatus(WorkflowResource resource, int status, Func<WorkflowStepUserAction, Task> returnCallback)
        {
            using (var scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 1, 0) }))
            {

                ////找到资源下指定状态，创建时间最晚的步骤
                var returnStep = await _workflowStepStore.QueryByStatus(resource.Type, resource.Key, resource.ID, status);
                if (returnStep != null)
                {
                    //获取创建时间大于等于returnStep的CreateTime and SerialNo不等于returnStep的SerialNo的所有步骤
                    //这些步骤需要被移除
                    List<WorkflowStep> removeSteps = new List<WorkflowStep>();
                    await _workflowStepStore.QueryByCreateTime(resource.Type, resource.Key, resource.ID, returnStep.SerialNo, returnStep.CreateTime,
                        async (step) =>
                        {
                            removeSteps.Add(step);
                            await Task.FromResult(0);
                        }
                        );

                    foreach (var item in removeSteps)
                    {
                        //每一个需要移除的步骤，都需要获取关联的用户动作，为这些用户动作执行回调
                        await _workflowStepUserActionStore.QueryByStep(resource.Type, resource.Key, item.ID, returnCallback);
                        //删除步骤
                        await _workflowStepStore.Delete(resource.Type, resource.Key, resource.ID, item.ID);
                    }

                    //找到所有SerialNo=preStep.SerialNo的步骤，删除它们下面的所有用户动作，
                    //对这些动作执行回调
                    //再把它们的complete全部改成false
                    await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, returnStep.SerialNo,
                        async (step) =>
                        {
                            await _workflowStepUserActionStore.QueryByStep(resource.Type, resource.Key, step.ID,
                                async (action) =>
                                {
                                    await returnCallback(action);
                                }
                                );
                            await _workflowStepUserActionStore.Delete(resource.Type, resource.Key, step.ID);
                            await _workflowStepStore.UpdateCompleteStatus(resource.Type, resource.Key, resource.ID, step.ID, false);
                        }
                        );

                    //修改资源状态为preStep的状态
                    await _workflowResourceStore.UpdateStatus(resource.Type, resource.Key, resource.ID, returnStep.Status);
                }
                else
                {
                    //需要退回到最初状态，获取资源下所有步骤，为步骤下面每个用户动作调用回调，
                    //删除所有的步骤，修改资源状态为初始状态
                    List<WorkflowStep> removeSteps = new List<WorkflowStep>();
                    await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, async (step) =>
                    {
                        removeSteps.Add(step);
                        await Task.FromResult(0);
                    });

                    foreach (var item in removeSteps)
                    {
                        //每一个需要移除的步骤，都需要获取关联的用户动作，为这些用户动作执行回调
                        await _workflowStepUserActionStore.QueryByStep(resource.Type, resource.Key, item.ID, returnCallback);
                        //删除步骤
                        await _workflowStepStore.Delete(resource.Type, resource.Key, resource.ID, item.ID);
                    }

                    //修改资源状态为preStep的状态
                    await _workflowResourceStore.UpdateStatus(resource.Type, resource.Key, resource.ID, resource.InitStatus);
                }

                scope.Complete();
            }

        }

        public async Task UpdateStatus(WorkflowResource resource, int status)
        {
            await _workflowResourceStore.UpdateStatus(resource.Type, resource.Key, resource.ID, status);
            resource.Status = status;
        }

        public async Task UpdateStepCompleteStatus(WorkflowResource resource, string actionName, int status, bool isComplete, Func<WorkflowStepUserAction, Task> returnCallback)
        {
            await _workflowStepStore.UpdateCompleteStatus(resource.Type, resource.Key, resource.ID, actionName, status, isComplete);
        }

        public async Task<ValidateResult> ValidateUser(WorkflowResource resource, string actionName, string userKey)
        {
            ValidateResult result = new ValidateResult()
            {
                Result = false
            };
            List<WorkflowStep> steps = new List<WorkflowStep>();
            //根据资源的当前状态和actionName获取所有步骤
            await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, actionName, resource.Status,
                async (step) =>
                {
                    steps.Add(step);
                    await Task.FromResult(0);
                }
                );

            StringBuilder strDescript = new StringBuilder();

            if (steps.Count > 0)
            {
                foreach (var item in steps)
                {
                    var itemResult = await item.ValidateUser(userKey);
                    if (itemResult.Result)
                    {
                        result.Result = true;
                        result.Description = null;
                        break;
                    }
                    else
                    {
                        strDescript.Append(itemResult.Description);
                        strDescript.Append("\n\r");
                    }
                }
            }
            else
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundWorkflowStepForAudit,
                    DefaultFormatting = "找不到工作流中需要审核的步骤;查询条件为资源类型:{0},资源关键字:{1},资源Id:{2},动作名称:{3},资源状态:{4}",
                    ReplaceParameters = new List<object>() { resource.Type, resource.Key, resource.ID, actionName, resource.Status }
                };

                throw new UtilityException((int)Errors.NotFoundWorkflowStepForAudit
                    , fragment);
            }

            if (!result.Result)
            {
                result.Description = strDescript.ToString();
            }

            return result;
        }

        public async Task GetStep(WorkflowResource resource, Func<WorkflowStep, Task> callback)
        {
            await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, callback);
        }

        public async Task GetUserActionByCurrentStatus(WorkflowResource resource, Func<WorkflowStepUserAction, Task> callback)
        {
            await _workflowStepUserActionStore.QueryByResource(resource.Type, resource.Key, resource.ID, resource.Status, callback);
        }

        public async Task GetStepByCurrentStatus(WorkflowResource resource, Func<WorkflowStep, Task> callback)
        {
            await _workflowStepStore.QueryByResource(resource.Type, resource.Key, resource.ID, resource.Status, callback);
        }

        public async Task GetUserAction(WorkflowResource resource, Func<WorkflowStepUserAction, Task> callback)
        {
            await _workflowStepUserActionStore.QueryByResource(resource.Type, resource.Key, resource.ID, callback);
        }
    }
}
