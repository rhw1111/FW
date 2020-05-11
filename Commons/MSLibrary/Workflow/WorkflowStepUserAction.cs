using MSLibrary.DI;
using MSLibrary.Workflow.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 工作流步骤用户动作
    /// 记录参与每一步工作流步骤的用户动作
    /// </summary>
    public class WorkflowStepUserAction : EntityBase<IWorkflowStepUserActionIMP>
    {
        private static IFactory<IWorkflowStepUserActionIMP> _workflowStepUserActionIMPFactory;

        public static IFactory<IWorkflowStepUserActionIMP> WorkflowStepUserActionIMPFactory
        {
            set
            {
                _workflowStepUserActionIMPFactory = value;
            }
        }

        public override IFactory<IWorkflowStepUserActionIMP> GetIMPFactory()
        {
            return _workflowStepUserActionIMPFactory;
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
        /// 关联的步骤ID
        /// </summary>
        public Guid StepID
        {
            get
            {
                return GetAttribute<Guid>("StepID");
            }
            set
            {
                SetAttribute<Guid>("StepID", value);
            }
        }

        /// <summary>
        /// 关联的步骤
        /// </summary>
        public WorkflowStep Step
        {
            get
            {
                return GetAttribute<WorkflowStep>("Step");
            }
            set
            {
                SetAttribute<WorkflowStep>("Step", value);
            }
        }

        /// <summary>
        /// 用户关键字
        /// </summary>
        public string UserKey
        {
            get
            {
                return GetAttribute<string>("UserKey");
            }
            set
            {
                SetAttribute<string>("UserKey", value);
            }
        }

        /// <summary>
        /// 用户对该步骤做出的响应结果
        /// </summary>
        public int Result
        {
            get
            {
                return GetAttribute<int>("Result");
            }
            set
            {
                SetAttribute<int>("Result", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

    }

    public interface IWorkflowStepUserActionIMP
    {

    }

    [Injection(InterfaceType = typeof(IWorkflowStepUserActionIMP), Scope = InjectionScope.Transient)]
    public class WorkflowStepUserActionIMP : IWorkflowStepUserActionIMP
    {

    }
}
