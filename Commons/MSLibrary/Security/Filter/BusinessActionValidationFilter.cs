using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using MSLibrary;
using MSLibrary.AspNet.Filter;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;
using MSLibrary.Security.Filter.Application;

namespace MSLibrary.Security.Filter
{
    /// <summary>
    /// 业务动作验证过滤器
    /// 验证指定名称的业务动作是否合法
    /// 动作名称=该Action的控制器完整类名.Action方法名
    /// </summary>
    [Injection(InterfaceType = typeof(BusinessActionValidationFilter), Scope = InjectionScope.Transient)]
    public class BusinessActionValidationFilter : ActionFilterBase
    {
        private IAppBusinessActionValidate _appBusinessActionValidate;

        public BusinessActionValidationFilter(IAppBusinessActionValidate appBusinessActionValidate)
        {
            _appBusinessActionValidate = appBusinessActionValidate;
        }
        protected override async Task OnRealActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            var actionName = $"{actionDescriptor.ControllerTypeInfo.FullName}.{actionDescriptor.ActionName}";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            //获取使用默认值的参数
            foreach(var parameterItem in actionDescriptor.Parameters)
            {
                var pInfo = ((ControllerParameterDescriptor)parameterItem).ParameterInfo;
                if (pInfo.HasDefaultValue && !context.ActionArguments.ContainsKey(pInfo.Name))
                {
                    parameters.Add(pInfo.Name, pInfo.DefaultValue);
                }
            }

            //获取直接赋值的参数
            foreach(var parameterItem in context.ActionArguments)
            {
                parameters.Add(parameterItem.Key, parameterItem.Value);
            }



            var result=await _appBusinessActionValidate.Do(actionName,parameters);
            if (!result.Result)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.BusinessActionValidateFail,
                    DefaultFormatting = "{0}",
                    ReplaceParameters = new List<object>() { result.Description }
                };

                throw new UtilityException((int)Errors.BusinessActionValidateFail, fragment);
            }
            else
            {
                await next();
            }
        }
    }
}
