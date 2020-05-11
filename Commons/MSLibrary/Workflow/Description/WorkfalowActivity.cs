using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Workflow.Description
{
    /// <summary>
    /// 工作流活动
    /// 工作流处理最小单元
    /// </summary>
    public class WorkfalowActivity : EntityBase<IWorkfalowActivityIMP>
    {
        private static IFactory<IWorkfalowActivityIMP> _workfalowActivityIMPFactory;

        public static IFactory<IWorkfalowActivityIMP> WorkfalowActivityIMPFactory
        {
            set
            {
                _workfalowActivityIMPFactory = value;
            }
        }
        public override IFactory<IWorkfalowActivityIMP> GetIMPFactory()
        {
            return _workfalowActivityIMPFactory;
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

        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }

        /// <summary>
        /// 描述信息
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
        /// 输入参数键值对
        /// </summary>
        public Dictionary<string,WorkflowParameter> InputParameters
        {
            get
            {
                return GetAttribute<Dictionary<string, WorkflowParameter>>("InputParameters");
            }
            set
            {
                SetAttribute<Dictionary<string, WorkflowParameter>>("InputParameters", value);
            }
        }

        public async Task<IWorkfalowActivityResult> Execute(IWorkflowContext context)
        {
            return await _imp.Execute(this, context);
        }
    }

    public interface IWorkfalowActivityIMP
    {
        Task<IWorkfalowActivityResult> Execute(WorkfalowActivity activity,IWorkflowContext context);
    }

    [Injection(InterfaceType = typeof(IWorkfalowActivityIMP), Scope = InjectionScope.Transient)]
    public class WorkfalowActivityIMP : IWorkfalowActivityIMP
    {
        private static Dictionary<string, IFactory<IWorkflowActivityService>> _activityServiceFactories = new Dictionary<string, IFactory<IWorkflowActivityService>>();

        public static Dictionary<string, IFactory<IWorkflowActivityService>> ActivityServiceFactories
        {
            get
            {
                return _activityServiceFactories;
            }
        }
        public async Task<IWorkfalowActivityResult> Execute(WorkfalowActivity activity, IWorkflowContext context)
        {
            //跟踪活动
            context.CurrentActivity = activity.ID;
            context.TraceService.Trace(context.ResourceType, context.ResourceInfo, context.UserInfo, context.CurrentDescription, context.CurrentNode, context.CurrentAction, context.CurrentBlock, context.CurrentActivity, string.Empty);

            List<object> inputParameters = new List<object>();          
            //获取输入参数
            foreach(var pItem in activity.InputParameters.Values)
            {
                inputParameters.Add(await pItem.Calculate(context));
            }

            var service=getService(activity.Type);

            //获取从配置文件获取额外输入参数
            var extraInputParameters = await service.GenerateInputParameters(activity.Configuration, context);

            inputParameters.AddRange(extraInputParameters);

            return await service.Execute(inputParameters, context);
        }

        private IWorkflowActivityService getService(string type)
        {
            if (!_activityServiceFactories.TryGetValue(type,out IFactory<IWorkflowActivityService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundWorkflowActivityServiceByType,
                    DefaultFormatting = "找不到类型为{0}的工作流活动服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type,$"{this.GetType().FullName}.ActivityServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundWorkflowActivityServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }



    /// <summary>
    /// 活动结果
    /// </summary>
    public interface IWorkfalowActivityResult
    {
        WorkflowGoToResult GoTo { get; }
        /// <summary>
        /// 输出参数键值对
        /// </summary>
        IDictionary<string, object> OutParameters { get;}
    }

    public class WorkflowActivityResultDefault : IWorkfalowActivityResult
    {
        public WorkflowGoToResult GoTo
        {
            get; set;
        }

        public IDictionary<string, object> OutParameters
        {
            get;
        } = new Dictionary<string, object>();
    }
}
