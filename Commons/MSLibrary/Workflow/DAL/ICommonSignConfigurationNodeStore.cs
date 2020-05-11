using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 通用审批配置节点数据操作
    /// </summary>
    public interface ICommonSignConfigurationNodeStore
    {

        /// <summary>
        /// 根据配置id，获取指定名称的节点
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<CommonSignConfigurationNode> QueryByConfigurationName(Guid configurationId, string name);
        Task Lock(string lockName, Func<Task> callBack, int timeout = -1);
    }
}
