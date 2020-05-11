using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 通用审批配置节点动作数据操作
    /// </summary>
    public interface ICommonSignConfigurationNodeActionStore
    {
        /// <summary>
        /// 查询指定节点下的所有动作
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByNode(Guid nodeId,Func<CommonSignConfigurationNodeAction,Task> callback);
        /// <summary>
        /// 查询指定节点下指定动作名称的节点动作
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        Task<CommonSignConfigurationNodeAction> QueryByNodeAndActionName(Guid nodeId,string actionName);
    }
}
