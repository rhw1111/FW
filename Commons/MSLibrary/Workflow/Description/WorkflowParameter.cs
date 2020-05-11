using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.Description
{
    /// <summary>
    /// 工作流参数
    /// 用来定义在流程中使用到的参数
    /// </summary>
    public class WorkflowParameter : EntityBase<IWorkflowParameterIMP>
    {
        private static IFactory<IWorkflowParameterIMP> _workflowParameterIMPFactory;

        public static IFactory<IWorkflowParameterIMP> WorkflowParameterIMPFactory
        {
            set
            {
                _workflowParameterIMPFactory = value;
            }
        }
        public override IFactory<IWorkflowParameterIMP> GetIMPFactory()
        {
            return _workflowParameterIMPFactory;
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
        /// 参数名称
        /// 
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
        /// 表达式
        /// </summary>
        public string Expression
        {
            get
            {
                return GetAttribute<string>("Expression");
            }
            set
            {
                SetAttribute<string>("Expression", value);
            }
        }

        public async Task<object> Calculate(IWorkflowContext context)
        {
            return await _imp.Calculate(this, context);
        }
    }

    public interface IWorkflowParameterIMP
    {
        Task<object> Calculate(WorkflowParameter parameter,IWorkflowContext context);
    }
}
