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
    /// 事务活动
    /// 格式
    /// <transaction scope="" level="" timeout="">
    ///     <activities>
    ///     </activities>
    /// </transaction>
    /// </summary>
    [Injection(InterfaceType = typeof(RealWorkfolwActivityResolveForTransaction), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityResolveForTransaction : IRealWorkfolwActivityResolve
    {
        private IActivityConfigurationService _activityConfigurationService;
        private IParameterConfigurationService _parameterConfigurationService;
        private IRealWorkfolwActivityResolve _realWorkfolwActivityResolve;

        public RealWorkfolwActivityResolveForTransaction(IActivityConfigurationService activityConfigurationService, IParameterConfigurationService parameterConfigurationService, IRealWorkfolwActivityResolve realWorkfolwActivityResolve)
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

            RealWorkfolwActivityDescriptionDataForTransaction data = new RealWorkfolwActivityDescriptionDataForTransaction();
            data.Activities = new List<RealWorkfolwActivityDescription>();

            var scopeAttribute = configuration.Attribute("scope");
            if (scopeAttribute==null)
            {
                data.Scope = "1";
            }
            else
            {
                data.Scope = scopeAttribute.Value;
            }

            var levelAttribute = configuration.Attribute("level");
            if (levelAttribute == null)
            {
                data.Level = "1";
            }
            else
            {
                data.Level = levelAttribute.Value;
            }

            var timeoutAttribute = configuration.Attribute("timeout");
            if (timeoutAttribute != null)
            {
                try
                {
                    data.Timeout = TimeSpan.Parse(timeoutAttribute.Value);
                }
                catch
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.RealWorkfolwActivityConfigurationAttributeParseTypeError,
                        DefaultFormatting = "工作流活动配置中的属性{0}的值无法转换为类型{1}，属性值为{2}，所属节点的值为{3}",
                        ReplaceParameters = new List<object>() { "timeout", typeof(TimeSpan).FullName, timeoutAttribute.Value, configuration.ToString() }
                    };

                    throw new UtilityException((int)Errors.RealWorkfolwActivityConfigurationAttributeParseTypeError, fragment);
                }
            }



            var activitiesElement = configuration.Element("activities");
            if (activitiesElement != null)
            {
                var activitise = await _activityConfigurationService.SeparateActivities(activitiesElement.ToString());

                foreach (var activityItem in activitise)
                {
                    var activityDescription = await _realWorkfolwActivityResolve.Execute(activityItem.Configuration);

                    data.Activities.Add(activityDescription);
                }
            }

            RealWorkfolwActivityDescription description = new RealWorkfolwActivityDescription()
            {
                Data = data
            };

            return description;

        }
    }

    public class RealWorkfolwActivityDescriptionDataForTransaction
    {
        public string Scope { get; set; }

        public string Level { get; set; }

        public TimeSpan? Timeout { get; set; }
        public List<RealWorkfolwActivityDescription> Activities { get; set; }

    }
}
