using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.ExpressionCalculate;

namespace MSLibrary.Workflow.Description
{
    public interface IWorkflowContext
    {
        string ResourceType { get; }
        string ResourceInfo { get; }
        string UserInfo { get; }
        Guid CurrentDescription { get; set; } 
        Guid CurrentNode { get; set; }
        Guid CurrentAction { get; set; }
        Guid CurrentBlock { get; set; }
        Guid CurrentActivity { get; set; }
        ExpressionCalculator ExpressionCalculator { get; }

        IDictionary<Guid, WorkflowActivityBlock> ActionBlocks { get; }
        IDictionary<string, object> Paremeters { get; }
        IDictionary<string, object> Items { get; }

        IWorkflowTraceService TraceService { get; }

        IWorkflowContext CreateNew();
    }

    /// <summary>
    /// 工作流跟踪服务
    /// 用于记录工作流执行情况
    /// </summary>
    public interface IWorkflowTraceService
    {
        Task Trace(string resourceType,string resourceID,string userInfo,Guid descriptionID,Guid nodeID,Guid actionID,Guid blockID,Guid activityID,string message);
    }
}
