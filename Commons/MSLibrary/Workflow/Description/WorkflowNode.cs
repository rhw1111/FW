using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.Description
{
    /// <summary>
    /// 工作流节点
    /// 一个状态一个节点，每个节点有多个工作流动作
    /// </summary>
    public class WorkflowNode : EntityBase<IWorkflowNodeIMP>
    {
        private static IFactory<IWorkflowNodeIMP> _workflowNodeIMPFactory;

        public static IFactory<IWorkflowNodeIMP> WorkflowNodeIMPFactory
        {
            set
            {
                _workflowNodeIMPFactory = value;
            }
        }
        public override IFactory<IWorkflowNodeIMP> GetIMPFactory()
        {
            return _workflowNodeIMPFactory;
        }



        /// <summary>
        /// id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        public int Status
        {
            get
            {
                return GetAttribute<int>("Status");
            }
            set
            {
                SetAttribute<int>("Status", value);
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return GetAttribute<string>("Description");
            }
            set
            {
                SetAttribute<string>("Description", value);
            }
        }

        /// <summary>
        /// 动作键值对
        /// </summary>
        public Dictionary<string,WorkflowAction> Actions
        {
            get
            {
                return GetAttribute<Dictionary<string, WorkflowAction>>("Actions");
            }
            set
            {
                SetAttribute<Dictionary<string, WorkflowAction>>("Actions", value);
            }
        }
    }

    public interface IWorkflowNodeIMP
    {
        Task<IList<WorkflowAction>> GetAuthorizeActions(WorkflowNode node,string userInfo);
        Task<WorkflowAction> GetAction(WorkflowNode node,string actionName);
    }
}
