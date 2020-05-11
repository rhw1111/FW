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
    /// 循环活动
    /// <while>
    ///     <inputs>
    ///         <parameter name="condition" datatype="" expressiontype="" configuration=""/>
    ///     <inputs>
    ///     <activities>
    ///     
    ///     </activities>
    /// </while>
    /// </summary>
    [Injection(InterfaceType = typeof(RealWorkfolwActivityResolveForWhile), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityResolveForWhile : IRealWorkfolwActivityResolve
    {
        private IActivityConfigurationService _activityConfigurationService;
        private IParameterConfigurationService _parameterConfigurationService;
        private IRealWorkfolwActivityResolve _realWorkfolwActivityResolve;

        public RealWorkfolwActivityResolveForWhile(IActivityConfigurationService activityConfigurationService, IParameterConfigurationService parameterConfigurationService, IRealWorkfolwActivityResolve realWorkfolwActivityResolve)
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

            RealWorkfolwActivityDescriptionDataForWhile data = new RealWorkfolwActivityDescriptionDataForWhile();


            var activitiesElement = configuration.Element("activities");
            if (activitiesElement == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                    DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                    ReplaceParameters = new List<object>() { activityConfiguration, new TextFragment() { Code= TextCodes.RealWorkfolwActivityConfigurationMissXMLElement, DefaultFormatting= "工作流活动配置缺少XML节点{0}", ReplaceParameters=new List<object>() { "activities" } } }
                };

                var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            var activities = await _activityConfigurationService.SeparateActivities(activitiesElement.ToString());

            List<RealWorkfolwActivityDescription> activityDescriptionList = new List<RealWorkfolwActivityDescription>();

            foreach (var itemActivity in activities)
            {
                var activityDescription = await _realWorkfolwActivityResolve.Execute(itemActivity.Configuration);
                activityDescriptionList.Add(activityDescription);
            }

            RealWorkfolwActivityDescription description = new RealWorkfolwActivityDescription()
            {
                Data = data
            };

            return description;
        }
    }


    public class RealWorkfolwActivityDescriptionDataForWhile
    {
        public List<RealWorkfolwActivityDescription> Activities { get; set; }
    }
}
