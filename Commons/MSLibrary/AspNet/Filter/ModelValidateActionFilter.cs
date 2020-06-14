using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.DI;
using Microsoft.AspNetCore.Mvc.Filters;
using MSLibrary.AspNet.Filter.Application;

namespace MSLibrary.AspNet.Filter
{
    /// <summary>
    /// 用于检查参数模型是否正确
    /// 如果参数模型不正确，则抛出异常
    /// 该过滤器在系统初始化配置中使用
    /// </summary>
    [Injection(InterfaceType = typeof(ModelValidateActionFilter), Scope = InjectionScope.Transient)]
    public class ModelValidateActionFilter : ActionFilterBase
    {
        private string _categoryName;
        private IAppExceptionMessageLogConvert _appExceptionMessageLogConvert;

        public ModelValidateActionFilter(string categoryName, IAppExceptionMessageLogConvert appExceptionMessageLogConvert) : base()
        {
            _categoryName = categoryName;
            _appExceptionMessageLogConvert = appExceptionMessageLogConvert;
        }

        protected override async Task OnRealActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (!context.ModelState.IsValid)
            {
                StringBuilder strErrorMessage = new StringBuilder();
                StringBuilder strDetailErrorMessage = new StringBuilder();

                foreach (var parameterItem in context.ModelState)
                {
                    strErrorMessage.Append($"{parameterItem.Key}");
                    strErrorMessage.Append("\r\n");
                    foreach (var errorItem in parameterItem.Value.Errors)
                    {
                        Exception ex = errorItem.Exception;

                        if (ex != null)
                        {
                            strErrorMessage.Append(ex.ToStackTraceString());
                        }
              
                    }
                }

                var logObj = await _appExceptionMessageLogConvert.Do(context, strDetailErrorMessage.ToString());


                //LoggerHelper.LogError(_categoryName, logObj);
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ApiModelValidateError,
                    DefaultFormatting = "Api{0}中参数验证错误，详细信息为{1}",
                    ReplaceParameters = new List<object>() { context.ActionDescriptor.DisplayName, strErrorMessage.ToString() }
                };


                throw new UtilityException((int)Errors.ApiModelValidateError,fragment,-1,0);
            }
            else
            {
                await next();
            }
        }
    }
}
