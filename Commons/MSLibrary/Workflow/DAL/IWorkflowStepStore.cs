using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 工作流步骤数据操作
    /// </summary>
    public interface IWorkflowStepStore
    {
        /// <summary>
        /// 新增工作流步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        Task Add(string resourceType, string resourceKey, WorkflowStep step);
        /// <summary>
        /// 删除工作流步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(string resourceType, string resourceKey, Guid resourceId, Guid id);

        /// <summary>
        /// 删除指定动作名称和状态的工作流步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="actionName"></param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        Task Delete(string resourceType, string resourceKey, Guid resourceId, string actionName, int status);

        /// <summary>
        /// 删除资源下面指定状态和批次的工作流步骤(排除指定的排除动作名称的步骤)
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="status"></param>
        /// <param name="serialNo"></param>
        /// <param name="excludeActionName"></param>
        /// <returns></returns>
        Task Delete(string resourceType, string resourceKey, Guid resourceId, int status, string serialNo, string excludeActionName);


        /// <summary>
        /// 修改资源下指定动作名称和状态的所有步骤的完成状态
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="completeStatus"></param>
        /// <returns></returns>
        Task UpdateCompleteStatus(string resourceType, string resourceKey, Guid resourceId, string actionName, int status, bool completeStatus);

        /// <summary>
        /// 修改资源下指定id的步骤的完成状态
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="id"></param>
        /// <param name="completeStatus"></param>
        /// <returns></returns>
        Task UpdateCompleteStatus(string resourceType, string resourceKey, Guid resourceId, Guid id, bool completeStatus);

        /// <summary>
        /// 指定步骤是否存在
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="status"></param>
        /// <param name="actionName"></param>
        /// <param name="userType"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<bool> IsExistStepByKey(string resourceType, string resourceKey, Guid resourceId, int status,string actionName, string userType, string userKey);

        /// <summary>
        /// 查询资源下面的所有步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, Func<WorkflowStep, Task> callback);

        /// <summary>
        /// 查询资源下面状态等于指定状态，创建时间最晚的步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<WorkflowStep> QueryLatestByResource(string resourceType, string resourceKey, Guid resourceId, int status);



        /// <summary>
        /// 查询资源下面指定动作名称和状态的所有步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, string actionName, int status, Func<WorkflowStep, Task> callback);


        /// <summary>
        /// 查询资源下面指定状态的所有步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByResource(string resourceType, string resourceKey, Guid resourceId,  int status, Func<WorkflowStep, Task> callback);


        /// <summary>
        /// 查询资源下面指定批次号的所有步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="serialNo"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, string serialNo, Func<WorkflowStep, Task> callback);

        /// <summary>
        /// 查询资源下面前一个步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<WorkflowStep> QueryPreStep(string resourceType, string resourceKey, Guid resourceId);
        /// <summary>
        /// 查询资源下指定状态、创建时间最晚的步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<WorkflowStep> QueryByStatus(string resourceType, string resourceKey, Guid resourceId, int status);
        /// <summary>
        /// 查询资源下面创建时间大于等于指定时间 and 批次号不等于指定状批次号 的所有步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="status"></param>
        /// <param name="createtime"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByCreateTime(string resourceType, string resourceKey, Guid resourceId, string serialNo, DateTime createtime, Func<WorkflowStep, Task> callback);
        /// <summary>
        /// 查询资源下面动作名称等于指定名称、状态等于指定状态、完成状态等于指定状态的所有步骤
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="completeStatus"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByCompleteStatus(string resourceType, string resourceKey, Guid resourceId, string actionName, int status, bool completeStatus, Func<WorkflowStep, Task> callback);
    }
}
