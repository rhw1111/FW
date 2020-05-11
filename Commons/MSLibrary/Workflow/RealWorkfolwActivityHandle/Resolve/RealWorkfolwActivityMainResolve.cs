using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwConfiguration;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve
{

    [Injection(InterfaceType = typeof(IRealWorkfolwActivityResolve), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityMainResolve : IRealWorkfolwActivityResolve
    {
        
        private static Dictionary<string, IFactory<IRealWorkfolwActivityResolve>> _resolveFactories = new Dictionary<string, IFactory<IRealWorkfolwActivityResolve>>();

        public static Dictionary<string, IFactory<IRealWorkfolwActivityResolve>> ResolveFactories
        {
            get
            {
                return _resolveFactories;
            }
        }

        private IActivityConfigurationService _activityConfigurationService;
    
        public RealWorkfolwActivityMainResolve(IActivityConfigurationService activityConfigurationService)
        {
            _activityConfigurationService = activityConfigurationService;
        }
        public async Task<RealWorkfolwActivityDescription> Execute(string activityConfiguration)
        {
            XElement element = null;
            try
            {
                element = XElement.Parse(activityConfiguration);
            }
            catch (Exception ex)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                    DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                    ReplaceParameters = new List<object>() { activityConfiguration, ex.Message }
                };

                var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError,fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            var idAttribute = element.Attribute("id");
            if (idAttribute == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                    DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                    ReplaceParameters = new List<object>() { activityConfiguration, new TextFragment() { Code=TextCodes.RealWorkfolwActivityConfigurationMissXMLAttribute, DefaultFormatting= "工作流活动配置缺少XML属性{0}", ReplaceParameters=new List<object>() { "id"} } }
                };

                var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            if (!Guid.TryParse(idAttribute.Value,out Guid id))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityConfigurationParseXMLError,
                    DefaultFormatting = "工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}",
                    ReplaceParameters = new List<object>() { activityConfiguration, new TextFragment() { Code = TextCodes.RealWorkfolwActivityConfigurationIdAttributeParseError, DefaultFormatting = "工作流活动配置的Id属性不能转换成Guid，当前的Id属性为{0}", ReplaceParameters = new List<object>() { idAttribute.Value } } }
                };

                var exception = new UtilityException((int)Errors.RealWorkfolwActivityConfigurationParseXMLError, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            if (!_resolveFactories.TryGetValue(element.Name.ToString().ToLower(),out IFactory<IRealWorkfolwActivityResolve> resolveFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRealWorkfolwActivityResolveByName,
                    DefaultFormatting = "找不到名称为{0}的工作流活动解析",
                    ReplaceParameters = new List<object>() { element.Name.ToString().ToLower() }
                };

                throw new UtilityException((int)Errors.NotFoundRealWorkfolwActivityResolveByName, fragment);
            }

            var description= await resolveFactory.Create().Execute(activityConfiguration);

            description.InputParameters=await _activityConfigurationService.ResolveActivityInputParameters(activityConfiguration);
            description.OutputParameters = await _activityConfigurationService.ResolveActivityOutputParameters(activityConfiguration);
            description.Name = element.Name.ToString().ToLower();
            description.Id = id;
            return description;
        }
    }
}
