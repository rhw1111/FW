using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Workflow.Description
{
    /// <summary>
    /// 工作流活动块
    /// 包含多个工作流活动，块内活动按顺序执行
    /// </summary>
    public class WorkflowActivityBlock : EntityBase<IWorkflowActivityBlockIMP>
    {
        private static IFactory<IWorkflowActivityBlockIMP> _workflowActivityBlockFactory;

        public static IFactory<IWorkflowActivityBlockIMP> WorkflowActivityBlockFactory
        {
            set
            {
                _workflowActivityBlockFactory = value;
            }
        }
        public override IFactory<IWorkflowActivityBlockIMP> GetIMPFactory()
        {
            return _workflowActivityBlockFactory;
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
        /// 是否是起始块
        /// </summary>
        public bool Root
        {
            get
            {
                return GetAttribute<bool>("Root");
            }
            set
            {
                SetAttribute<bool>("Root", value);
            }
        }

        /// <summary>
        /// 活动列表
        /// </summary>
        public List<WorkfalowActivity> Activities
        {
            get
            {
                return GetAttribute<List<WorkfalowActivity>>("Activities");
            }
            set
            {
                SetAttribute<List<WorkfalowActivity>>("Activities", value);
            }
        }


        public async Task<WorkflowGoToResult> Execute(IWorkflowContext context)
        {
            return await _imp.Execute(this, context);
        }

    }

    public interface IWorkflowActivityBlockIMP
    {
        Task<WorkflowGoToResult> Execute(WorkflowActivityBlock block,IWorkflowContext context);
    }

    [Injection(InterfaceType = typeof(IWorkflowActivityBlockIMP), Scope = InjectionScope.Transient)]
    public class WorkflowActivityBlockIMP : IWorkflowActivityBlockIMP
    {
        public async Task<WorkflowGoToResult> Execute(WorkflowActivityBlock block, IWorkflowContext context)
        {
            //跟踪活动
            context.CurrentBlock = block.ID;
            context.CurrentActivity = Guid.Empty;
            await context.TraceService.Trace(context.ResourceType, context.ResourceInfo, context.UserInfo, context.CurrentDescription, context.CurrentNode, context.CurrentAction, context.CurrentBlock, context.CurrentActivity, string.Empty);

            WorkflowGoToResult result = null;
            //顺序执行所有的活动
            foreach (var item in block.Activities)
            {
                var activityResult=await item.Execute(context);
                //将活动的结果值计入上下文
                //参数名称为{活动ID}-{参数名称}
                foreach(var pItem in activityResult.OutParameters)
                {
                    context.Paremeters[$"{item.ID.ToString()}-{pItem.Key}"] = pItem.Value;
                }
                //如果活动返回值的GoTo不为空，则直接返回
                if (activityResult.GoTo!=null)
                {
                    result = new WorkflowGoToResult()
                    {
                        Type = activityResult.GoTo.Type,
                        ActionName = activityResult.GoTo.ActionName,
                        BlockID = activityResult.GoTo.BlockID,
                        Status = activityResult.GoTo.Status
                    };
                    break;
                }
            }

            return result;
        }
    }


    public class WorkflowGoToResult
    {
        public int Type { get; set; }
        public Guid BlockID { get; set; }
        public int Status { get; set; }
        public string ActionName { get; set; }
    }
}
