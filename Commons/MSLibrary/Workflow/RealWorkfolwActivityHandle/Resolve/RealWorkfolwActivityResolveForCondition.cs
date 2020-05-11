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
    /// 条件活动
    /// 格式
    /// <if>
    ///     <inputs>
    ///         <parameter name="condition" datatype="" expressiontype="" configuration=""/>
    ///     </inputs>
    ///     <outputs>
    ///         <parameter name="" datatype="" expressiontype="" configuration=""/>
    ///     </outputs>
    ///     <match></match>
    ///     <elseif>
    ///         <parameters>
    ///             <parameter name="condition" datatype="" expressiontype="" configuration=""/>
    ///         </parameters>
    ///     </elseif>
    ///     <else></else>
    /// </if>
    /// </summary>
    [Injection(InterfaceType = typeof(RealWorkfolwActivityResolveForCondition), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityResolveForCondition : IRealWorkfolwActivityResolve
    {
        private IActivityConfigurationService _activityConfigurationService;
        private IParameterConfigurationService _parameterConfigurationService;
        private IRealWorkfolwActivityResolve _realWorkfolwActivityResolve;

        public RealWorkfolwActivityResolveForCondition(IActivityConfigurationService activityConfigurationService, IParameterConfigurationService parameterConfigurationService, IRealWorkfolwActivityResolve realWorkfolwActivityResolve)
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
            catch(Exception ex)
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

            RealWorkfolwActivityDescriptionDataForCondition data = new RealWorkfolwActivityDescriptionDataForCondition();


            var matchElement=configuration.Element("match");
            if (matchElement==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                    DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                    ReplaceParameters = new List<object>() { activityConfiguration, new TextFragment() {  Code= TextCodes.RealWorkfolwActivityConfigurationMissXMLElement, DefaultFormatting= "工作流活动配置缺少XML节点{0}", ReplaceParameters=new List<object>() { "match" } } }
                };

                var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            var matchActivities = await _activityConfigurationService.SeparateActivities(matchElement.ToString());
            List<RealWorkfolwActivityDescription> matchList = new List<RealWorkfolwActivityDescription>();

            foreach(var itemMatchActivity in matchActivities)
            {
                var match = await _realWorkfolwActivityResolve.Execute(itemMatchActivity.Configuration);
                matchList.Add(match);
            }

            data.Match = matchList;
            data.ElseIfs= await GetElseifs(activityConfiguration, configuration);

            var elseElement = configuration.Element("else");
            if (elseElement == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                    DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                    ReplaceParameters = new List<object>() { activityConfiguration, new TextFragment() { Code = TextCodes.RealWorkfolwActivityConfigurationMissXMLElement, DefaultFormatting = "工作流活动配置缺少XML节点{0}", ReplaceParameters = new List<object>() { "else" } } }
                };

                var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            var elseActivities = await _activityConfigurationService.SeparateActivities(elseElement.ToString());
            List<RealWorkfolwActivityDescription> elseList = new List<RealWorkfolwActivityDescription>();

            foreach (var itemElseActivity in elseActivities)
            {
                var elseActivity = await _realWorkfolwActivityResolve.Execute(itemElseActivity.Configuration);
                elseList.Add(elseActivity);
            }

            data.Else = elseList;


            RealWorkfolwActivityDescription description = new RealWorkfolwActivityDescription()
            {
                Data = data
            };
            return description;
        }

        private async Task<List<RealWorkfolwActivityDescriptionDataForConditionElseIf>> GetElseifs(string activityConfiguration, XElement configuration)
        {
            List<RealWorkfolwActivityDescriptionDataForConditionElseIf> elseifList = new List<RealWorkfolwActivityDescriptionDataForConditionElseIf>();
            var elseifElements = configuration.Elements("elseif");
            if (elseifElements != null && elseifElements.Count() > 0)
            {
                foreach (var elseifElementItem in elseifElements)
                {
                    var parametersElement=elseifElementItem.Element("parameters");
                    if (parametersElement==null)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                            DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                            ReplaceParameters = new List<object>() { activityConfiguration, new TextFragment() { Code = TextCodes.RealWorkfolwActivityConfigurationMissXMLChildElement, DefaultFormatting = "工作流活动配置缺少XML节点{0}，节点所属的父节点内容：{1}", ReplaceParameters = new List<object>() { "parameters", elseifElementItem.ToString() } } }
                        };

                        var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                        exception.Data[UtilityExceptionDataKeys.Catch] = true;
                        throw exception;
                    }
                    var conditionParameterElement = (from parameter in parametersElement.Elements("parameter")
                                              where parameter.Attribute("name").Value == "condition"
                                              select parameter).FirstOrDefault();
                    if (conditionParameterElement==null)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                            DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                            ReplaceParameters = new List<object>() { activityConfiguration, new TextFragment() { Code = TextCodes.RealWorkfolwActivityConfigurationMissXMLChildElement, DefaultFormatting = "工作流活动配置缺少XML节点{0}，节点所属的父节点内容：{1}", ReplaceParameters = new List<object>() { "parameters/parameter[name=condition]", elseifElementItem.ToString() } } }
                        };

                        var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                        exception.Data[UtilityExceptionDataKeys.Catch] = true;
                        throw exception;
                    }

                    var conditionParameter = await _parameterConfigurationService.ResolveParameter(conditionParameterElement.ToString());

                    var matchActivities = await _activityConfigurationService.SeparateActivities(elseifElementItem.ToString());
                    List<RealWorkfolwActivityDescription> matchList = new List<RealWorkfolwActivityDescription>();

                    foreach (var itemMatchActivity in matchActivities)
                    {
                        var match = await _realWorkfolwActivityResolve.Execute(itemMatchActivity.Configuration);
                        matchList.Add(match);
                    }

                    elseifList.Add(new RealWorkfolwActivityDescriptionDataForConditionElseIf()
                    {
                         Condition= conditionParameter,
                         Match =matchList
                    });
                }
            }
            return elseifList;
        }
    }

    public class RealWorkfolwActivityDescriptionDataForCondition
    {
        public List<RealWorkfolwActivityDescription> Match { get; set; }
        public List<RealWorkfolwActivityDescription> Else { get; set; }

        public List<RealWorkfolwActivityDescriptionDataForConditionElseIf> ElseIfs { get; set; }

    }


    public class RealWorkfolwActivityDescriptionDataForConditionElseIf
    {
        public RealWorkfolwActivityParameter Condition { get; set; }
        public List<RealWorkfolwActivityDescription> Match { get; set; }
    }

}
