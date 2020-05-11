using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwActivityParameterHandle;
using MSLibrary.Workflow.RealWorkfolwActivityHandle;
using MSLibrary.Workflow.RealWorkfolwConfiguration;
using MSLibrary.Workflow.RealWorkfolwActivityParameterDataHandle;

namespace MSLibrary.Workflow
{
    public class RealWorkfolw : EntityBase<IRealWorkfolwIMP>
    {
        private static IFactory<IRealWorkfolwIMP> _realWorkfolwIMPFactory;

        public static IFactory<IRealWorkfolwIMP> RealWorkfolwIMPFactory
        {
            set
            {
                _realWorkfolwIMPFactory = value;
            }
        }
        public override IFactory<IRealWorkfolwIMP> GetIMPFactory()
        {
            return _realWorkfolwIMPFactory;
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
        /// 配置
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

    public interface IRealWorkfolwIMP
    {
        Task<Dictionary<string, object>> Execute(RealWorkfolw workflow, RealWorkfolwContext context);
    }

    [Injection(InterfaceType = typeof(IRealWorkfolwIMP), Scope = InjectionScope.Transient)]
    public class RealWorkfolwIMP : IRealWorkfolwIMP
    {
        private IActivityConfigurationService _activityConfigurationService;
        private IRealWorkfolwActivityParameterDataHandleSelector _realWorkfolwActivityParameterDataHandleSelector;

        public RealWorkfolwIMP(IActivityConfigurationService activityConfigurationService, IRealWorkfolwActivityParameterDataHandleSelector realWorkfolwActivityParameterDataHandleSelector)
        {
            _activityConfigurationService = activityConfigurationService;
            _realWorkfolwActivityParameterDataHandleSelector = realWorkfolwActivityParameterDataHandleSelector;
        }
        public async Task<Dictionary<string, object>> Execute(RealWorkfolw workflow, RealWorkfolwContext context)
        {
            List<RealWorkfolwActivity> activities;
            if (!workflow.Extensions.TryGetValue("activities", out object objActivities))
            {
                activities = await _activityConfigurationService.SeparateActivities(workflow.Configuration);
                objActivities = activities;
                lock (workflow)
                {
                    workflow.Extensions["activities"] = objActivities;
                }
            }
            activities = (List<RealWorkfolwActivity>)objActivities;


            int index = 0, count = activities.Count;
            for(index=0;index<=count-1;index++)
            {
                var activityResult= await activities[index].Calculate(context);
                var activityOutputs = await activities[index].GetOutputParameters();
                foreach (var activityResultItem in activityResult)
                {
                    if (!activityOutputs.TryGetValue(activityResultItem.Key,out RealWorkfolwActivityParameter outputParameter))
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.NotFoundRealWorkfolwActivityOutputParameterByActivityResult,
                            DefaultFormatting = "工作流活动{0}的结果中的输出参数名称{1}在工作流活动的输出参数中未定义，工作流id为{2}",
                            ReplaceParameters = new List<object>() { activities[index].ID.ToString(), activityResultItem.Key, workflow.ID.ToString() }
                        };

                        throw new UtilityException((int)Errors.NotFoundRealWorkfolwActivityOutputParameterByActivityResult, fragment);
                    }

                    var outputDataHandle = _realWorkfolwActivityParameterDataHandleSelector.Choose(outputParameter.DataType);
                    var outputDataValidateResult = await outputDataHandle.ValidateType(activityResultItem.Value);
                    if (!outputDataValidateResult)
                    {
                        string strResult;
                        if (activityResultItem.Value == null)
                        {
                            strResult = "null";
                        }
                        else
                        {
                            strResult = activityResultItem.Value.GetType().FullName;
                        }

                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.RealWorkfolwActivityOutputParameterDataHandleValidateError,
                            DefaultFormatting = "工作流活动参数数据处理验证失败，验证器数据类型为{0}，要验证的实际数据类型为{1}，参数名称为{2},工作流活动Id为{3}，工作流Id为{4}",
                            ReplaceParameters = new List<object>() { outputParameter.DataType, strResult, outputParameter.Name, activities[index].ID.ToString(), workflow.ID.ToString() }
                        };

                        throw new UtilityException((int)Errors.RealWorkfolwActivityOutputParameterDataHandleValidateError, fragment);
                    }

                }
                context.ActivityOutputParameters[activities[index].ID.ToString()] = activityResult;
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach(var resultItem in context.Result)
            {
                result.Add(resultItem.Key, resultItem.Value);
            }

            return result;
        }
    }


    public class RealWorkfolwContext
    {
        public Dictionary<string, object> InitInputParameters { get; } = new Dictionary<string, object>();

        public ConcurrentDictionary<string, Dictionary<string, object>> ActivityOutputParameters { get; } = new ConcurrentDictionary<string, Dictionary<string, object>>();

        public ConcurrentDictionary<string, List<object>> ActivityInnerOutputParameters { get; } = new ConcurrentDictionary<string, List<object>>();

        public ConcurrentDictionary<string, object> ParameterExpressionResults { get; } = new ConcurrentDictionary<string, object>();

        public ConcurrentDictionary<string, object> Result { get; } = new ConcurrentDictionary<string, object>();

    }


}
