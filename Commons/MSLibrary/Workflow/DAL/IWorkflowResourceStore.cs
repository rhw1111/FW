using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 工作流资源数据操作
    /// </summary>
    public interface IWorkflowResourceStore
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
        /// <param name="resourceKey"></param>
        /// <param name="resourceType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(string resourceType, string resourceKey, Guid id);
        /// <summary>
        /// 修改指定资源的状态
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task UpdateStatus(string resourceType, string resourceKey, Guid id, int status);

        /// <summary>
        /// 根据工作流资源关键字查询
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<WorkflowResource> QueryByKey(string type, string key);
    }
}
