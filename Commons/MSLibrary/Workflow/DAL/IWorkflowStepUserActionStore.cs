using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 工作流用户动作数据操作
    /// </summary>
    public interface IWorkflowStepUserActionStore
    {
        /// <summary>
        /// 新增用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Add(string resourceType, string resourceKey, WorkflowStepUserAction action);
        /// <summary>
        /// 删除用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        Task Delete(string resourceType, string resourceKey, Guid stepId, Guid actionId);
        /// <summary>
        /// 删除指定步骤下面的所有用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        Task Delete(string resourceType, string resourceKey, Guid stepId);
        /// <summary>
        /// 查询指定步骤下的所有用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByStep(string resourceType, string resourceKey, Guid stepId, Func<WorkflowStepUserAction, Task> callback);

        /// <summary>
        /// 查询指定步骤下的所有用户动作的数量
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task<int> QueryCountByStep(string resourceType, string resourceKey, Guid stepId);


        /// <summary>
        /// 查询指定资源下面的指定动作名称指定状态的所有步骤下面的所有用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, string actionName, int status, Func<WorkflowStepUserAction, Task> callback);

        /// <summary>
        /// 查询指定资源下面的指定状态的所有步骤下面的所有用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, int status, Func<WorkflowStepUserAction, Task> callback);

        /// <summary>
        /// 查询指定资源下面的所有步骤下面的所有用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, Func<WorkflowStepUserAction, Task> callback);


        /// <summary>
        /// 查询指定步骤下面指定用户信息的用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<WorkflowStepUserAction> QueryByStepAndUser(string resourceType, string resourceKey, Guid stepId, string userKey);
    }
}
