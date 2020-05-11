using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 从工作流步骤中的UserKey解析出用户关键字服务
    /// </summary>
    public interface IGetUserInfoFromWorkflowStepService
    {
        /// <summary>
        /// 解析处理
        /// </summary>
        /// <param name="userType">工作流步骤中的用户类型</param>
        /// <param name="userKey">工作流步骤中的用户关键字</param>
        /// <param name="callback">解析后的具体应用中的用户关键字</param>
        /// <returns></returns>
        Task Execute(string userType,string userKey, Func<string, Task> callback);
    }
}
