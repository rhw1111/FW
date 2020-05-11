using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.Description
{
    /// <summary>
    /// 工作流描述
    /// 用来定义一个工作流
    /// </summary>
    public class WorkflowDescription : EntityBase<IWorkflowDescriptionIMP>
    {
        private static IFactory<IWorkflowDescriptionIMP> _workflowDescriptionIMPFactory;

        public static IFactory<IWorkflowDescriptionIMP> WorkflowDescriptionIMPFactory
        {
            set
            {
                _workflowDescriptionIMPFactory = value;
            }
        }
        public override IFactory<IWorkflowDescriptionIMP> GetIMPFactory()
        {
            return _workflowDescriptionIMPFactory;
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
        /// 名称
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
        /// 配置信息
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
            }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version
        {
            get
            {
                return GetAttribute<int>("Version");
            }
            set
            {
                SetAttribute<int>("Version", value);
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

    public interface IWorkflowDescriptionIMP
    {
        Task Add(WorkflowDescription description);
        Task Delete(WorkflowDescription description);
        Task Update(WorkflowDescription description);

        Task Execute(WorkflowDescription description,int status,string actionName,string resourceType,string resourceInfo,string userInfo);
    }

}
