using MSLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 与工作流相关的连接字符串工厂
    /// </summary>
    public interface IWorkflowConnectionFactory
    {
        /// <summary>
        /// 创建有关工作流的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForWorkflow();
        /// <summary>
        /// 创建有关工作流的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForWorkflow();
    }
}
