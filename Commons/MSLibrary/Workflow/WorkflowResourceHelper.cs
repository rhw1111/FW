using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 工作流资源帮助器
    /// </summary>
    public static class WorkflowResourceHelper
    {
        /// <summary>
        /// 获取指定的工作流资源
        /// 如果存在，则直接返回
        /// 如果不存在，则新建再返回
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="initStatus"></param>
        /// <returns></returns>
        public static async Task<WorkflowResource> GetWorkflowResource(IWorkflowResourceRepository workflowResourceRepository, string type,string key,int initStatus)
        {
            var resource=await workflowResourceRepository.QueryByKey(type,key);
            if (resource == null)
            {
                resource = new WorkflowResource()
                {
                    Type = type,
                    Key = key,
                    InitStatus = initStatus,
                    Status = initStatus
                };

                try
                {
                    await resource.Add();
                }
                catch (UtilityException ex)
                {
                    if (ex.Code == (int)Errors.ExistWorkflowResourceKey)
                    {
                        resource = await workflowResourceRepository.QueryByKey(type, key);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return resource;
        }
    }
}
