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
    /// 设置工作流结果的活动
    /// <result>
    ///     <set>
    ///         <parameter name="" datatype="" expressiontype="" configuration=""/>
    ///     <set>
    /// </result>
    /// </summary>
    [Injection(InterfaceType = typeof(RealWorkfolwActivityResolveForResult), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityResolveForResult : IRealWorkfolwActivityResolve
    {
        private IActivityConfigurationService _activityConfigurationService;
        private IParameterConfigurationService _parameterConfigurationService;
        private IRealWorkfolwActivityResolve _realWorkfolwActivityResolve;

        public RealWorkfolwActivityResolveForResult(IActivityConfigurationService activityConfigurationService, IParameterConfigurationService parameterConfigurationService, IRealWorkfolwActivityResolve realWorkfolwActivityResolve)
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

            RealWorkfolwActivityDescriptionDataForResult data = new RealWorkfolwActivityDescriptionDataForResult();
            data.ResultParameters = new List<RealWorkfolwActivityParameter>();

            var setElement = configuration.Element("set");
            if (setElement == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                    DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                    ReplaceParameters = new List<object>() { activityConfiguration, new TextFragment() { Code= TextCodes.RealWorkfolwActivityConfigurationMissXMLElement, DefaultFormatting= "工作流活动配置缺少XML节点{0}", ReplaceParameters=new List<object>() { "set" } } }
                };

                var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            var parameterElements=setElement.Elements("parameter");

            foreach(var parameterElement in parameterElements)
            {
                var parameter = await _parameterConfigurationService.ResolveParameter(parameterElement.ToString());
                data.ResultParameters.Add(parameter);
            }

            RealWorkfolwActivityDescription description = new RealWorkfolwActivityDescription()
            {
                Data = data
            };
            return description;
        }
    }


    public class RealWorkfolwActivityDescriptionDataForResult
    {
        public List<RealWorkfolwActivityParameter> ResultParameters { get; set; }

    }

}
