using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 通用审批配置初始化动作数据操作
    /// </summary>
    public interface ICommonSignConfigurationRootActionStore
    {
        /// <summary>
        /// 获取指定指定配置下的指定动作名称的初始化动作
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        Task<CommonSignConfigurationRootAction> QueryByActionName(Guid configurationId,string actionName);
        /// <summary>
        /// 获取指定指定配置下的所有初始化动作
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryAll(Guid configurationId,Func<CommonSignConfigurationRootAction,Task> callback);
    }
}
