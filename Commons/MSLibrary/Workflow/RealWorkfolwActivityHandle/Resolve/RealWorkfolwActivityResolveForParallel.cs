using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwConfiguration;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve
{
    /// <summary>
    /// 并行处理动作
    /// 格式
    /// <parallel max="">
    ///     <items>
    ///         <item>
    ///             <activities>
    ///             </activities>
    ///             <errorhandle>
    ///             </errorhandle>
    ///         </item>
    ///     </items>
    /// </parallel>
    /// </summary>
    public class RealWorkfolwActivityResolveForParallel : IRealWorkfolwActivityResolve
    {
        private IActivityConfigurationService _activityConfigurationService;
        private IParameterConfigurationService _parameterConfigurationService;
        private IRealWorkfolwActivityResolve _realWorkfolwActivityResolve;

        public RealWorkfolwActivityResolveForParallel(IActivityConfigurationService activityConfigurationService, IParameterConfigurationService parameterConfigurationService, IRealWorkfolwActivityResolve realWorkfolwActivityResolve)
        {
            _activityConfigurationService = activityConfigurationService;
            _parameterConfigurationService = parameterConfigurationService;
            _realWorkfolwActivityResolve = realWorkfolwActivityResolve;
        }

        public async Task<RealWorkfolwActivityDescription> Execute(string activityConfiguration)
        {
            XElement configuration = null;
            try
            {
                configuration = XElement.Parse(activityConfiguration);
            }
            catch (Exception ex)
            {

                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                    DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                    ReplaceParameters = new List<object>() { activityConfiguration, ex.Message }
                };
                var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            RealWorkfolwActivityDescriptionDataForParallel data = new RealWorkfolwActivityDescriptionDataForParallel();
            data.Items = new List<RealWorkfolwActivityDescriptionDataForParallelItem>();
            
            var maxAttribute = configuration.Attribute("max");
            if (maxAttribute == null)
            {
                data.Max = 4;
            }
            else
            {
                
                if (!int.TryParse(maxAttribute.Value,out int max))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.RealWorkfolwActivityConfigurationAttributeParseTypeError,
                        DefaultFormatting = "工作流活动配置中的属性{0}的值无法转换为类型{1}，属性值为{2}，所属节点的值为{3}",
                        ReplaceParameters = new List<object>() { "max", typeof(int).FullName, maxAttribute.Value, configuration.ToString() }
                    };

                    throw new UtilityException((int)Errors.RealWorkfolwActivityConfigurationAttributeParseTypeError, fragment);
                }
                data.Max = max;
            }

            var itemsElement = configuration.Element("items");

            if (itemsElement!=null)
            {
                var itemElementList = itemsElement.Elements("item");
                foreach(var itemElement in itemElementList)
                {
                    var activitiesElement = itemsElement.Element("activities");
                    var errorHandleElement= itemsElement.Element("errorhandle");

                    RealWorkfolwActivityDescriptionDataForParallelItem item = new RealWorkfolwActivityDescriptionDataForParallelItem()
                    {
                        Activities = new List<RealWorkfolwActivityDescription>(),
                        ErrorHandle = null
                    };

                    data.Items.Add(item);

                    if (activitiesElement!=null)
                    {
                        var activitise = await _activityConfigurationService.SeparateActivities(activitiesElement.ToString());

                        foreach (var activityItem in activitise)
                        {
                            var activityDescription = await _realWorkfolwActivityResolve.Execute(activityItem.Configuration);

                            item.Activities.Add(activityDescription);
                        }
                    }

                    if (errorHandleElement != null && errorHandleElement.FirstNode!=null)
                    {
                        var errorHandleActivity = await _activityConfigurationService.ResolveActivity(errorHandleElement.FirstNode.ToString());
                        var errorHandleActivityDescription= await _realWorkfolwActivityResolve.Execute(errorHandleActivity.Configuration);
                        item.ErrorHandle = errorHandleActivityDescription;
                    }
                }
            }

            RealWorkfolwActivityDescription description = new RealWorkfolwActivityDescription()
            {
                Data = data
            };

            return description;
        }
    }

    public class RealWorkfolwActivityDescriptionDataForParallel
    {
        public int Max { get; set; }

        public List<RealWorkfolwActivityDescriptionDataForParallelItem> Items { get; set; }

    }

    public class RealWorkfolwActivityDescriptionDataForParallelItem
    {
        public List<RealWorkfolwActivityDescription> Activities { get; set; }
        public RealWorkfolwActivityDescription ErrorHandle { get; set; }
    }
}
