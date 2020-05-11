using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Workflow.Description
{
    /// <summary>
    /// 工作流动作
    /// 每个动作包含多个工作流块，其中一个是起始块，只执行起始块
    /// 工作流动作是执行入口
    /// 工作流动作的职责包括：
    /// 验证执行权限
    /// 执行动作
    /// </summary>
    public class WorkflowAction : EntityBase<IWorkflowActionIMP>
    {
        private static IFactory<IWorkflowActionIMP> _workflowActionIMPFactory;

        public static IFactory<IWorkflowActionIMP> WorkflowActionIMPFactory
        {
            set
            {
                _workflowActionIMPFactory = value;
            }
        }
        public override IFactory<IWorkflowActionIMP> GetIMPFactory()
        {
            return _workflowActionIMPFactory;
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

        /// <summary>
        /// 动作名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
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
        /// 授权类型
        /// </summary>
        public string AuthorizeType
        {
            get
            {
                return GetAttribute<string>("AuthorizeType");
            }
            set
            {
                SetAttribute<string>("AuthorizeType", value);
            }
        }

        /// <summary>
        /// 授权表达式
        /// 通过该表达式计算出匹配授权类型的授权关键字
        /// </summary>
        public string AuthorizeExpression
        {
            get
            {
                return GetAttribute<string>("AuthorizeExpression");
            }
            set
            {
                SetAttribute<string>("AuthorizeExpression", value);
            }
        }

        /// <summary>
        /// 活动块列表
        /// </summary>
        public Dictionary<Guid,WorkflowActivityBlock> Blocks
        {
            get
            {
                return GetAttribute<Dictionary<Guid, WorkflowActivityBlock>>("Blocks");
            }
            set
            {
                SetAttribute<Dictionary<Guid, WorkflowActivityBlock>>("Blocks", value);
            }
        }

    }

    public interface IWorkflowActionIMP
    {
        Task<ValidateResult> Authorize(WorkflowAction action,string userInfo);
        Task Execute(WorkflowAction action,IWorkflowContext context);
    }

    [Injection(InterfaceType = typeof(IWorkflowActionIMP), Scope = InjectionScope.Transient)]
    public class WorkflowActionIMP : IWorkflowActionIMP
    {
        public Task<ValidateResult> Authorize(WorkflowAction action, string userInfo)
        {
            throw new NotImplementedException();
        }

        public Task Execute(WorkflowAction action, IWorkflowContext context)
        {
            throw new NotImplementedException();
        }
    }
}
