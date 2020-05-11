using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MSLibrary.AspNet.Filter;
using MSLibrary.AspNet;
using MSLibrary.Security.RequestController.Application;
using MSLibrary.LanguageTranslate;


namespace MSLibrary.Security.RequestController.Filter
{
    /// <summary>
    /// 请求跟踪的Action过滤器
    /// 使用该过滤器表明该Action需要进行针对输入请求的跟踪控制
    /// 根据Action的DisplayName来匹配配置项
    /// </summary>
    public class RequestTrackerActionFilter : ActionFilterBase
    {
        private static string _errorCatalogName;

        private ILoggerFactory _loggerFactory;
        private IAppRequestControl _appRequestControl;

        public static string ErrorCatalogName
        {
            set
            {
                _errorCatalogName = value;
            }
        }


        public RequestTrackerActionFilter(ILoggerFactory loggerFactory, IAppRequestControl appRequestControl)
        {
            _loggerFactory = loggerFactory;
            _appRequestControl = appRequestControl;
        }
        protected override async Task OnRealActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var logger = _loggerFactory.CreateLogger(_errorCatalogName);
            await HttpErrorHelper.ExecuteByActionExecutingFilterContextAsync(context, logger, async () =>
            {

                var actionName = context.ActionDescriptor.DisplayName;

                using (var resultDispoint = await _appRequestControl.Do(actionName))
                {
                    var result = await resultDispoint.Execute();

                    if (!result.Result)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = string.Empty,
                            DefaultFormatting = result.Description,
                            ReplaceParameters = new List<object>() { }
                        };

                        throw new UtilityException((int)Errors.RequestOverflow, fragment);
                    }
                    else
                    {
                        await next();
                    }
                }

            });



        }
    }
}
