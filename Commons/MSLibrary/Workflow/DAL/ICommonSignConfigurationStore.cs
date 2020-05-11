using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 通用审批配置数据操作
    /// </summary>
    public interface ICommonSignConfigurationStore
    {
        /// <summary>
        /// 根据工作流资源类型查询
        /// </summary>
        /// <param name="workflowResourceType"></param>
        /// <returns></returns>
        Task<CommonSignConfiguration> QueryByWorkflowResourceType(string workflowResourceType);
        /// <summary>
        /// 根据实体类型查询
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByEntityType(string entityType, Func<CommonSignConfiguration, Task> callback);

        Task Lock(string lockName, Func<Task> callBack, int timeout = -1);
    }
}
