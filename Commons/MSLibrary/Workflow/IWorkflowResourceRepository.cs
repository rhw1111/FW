using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 工作流资源仓储
    /// </summary>
    public interface IWorkflowResourceRepository
    {
        /// <summary>
        /// 根据关键信息查询
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<WorkflowResource> QueryByKey(string type,string key);
    }
}
