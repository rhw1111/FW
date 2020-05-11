using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using MSLibrary.Context.Application;
using MSLibrary.DI;

namespace MSLibrary.Context.Filter
{
    /// <summary>
    /// 负责国际化上下文初始化
    /// 需要将该Action过滤器注册到全局Action过滤器中
    /// 且需要放在UserAuthorizeActionGolbalFilter之后，
    /// 表明InternationalizationActionGlobalFilter优先级高，能覆盖之前的相关国际化的上下文
    /// </summary>
    [Injection(InterfaceType = typeof(InternationalizationActionGlobalFilter), Scope = InjectionScope.Singleton)]
    public class InternationalizationActionGlobalFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //从Http上下文中获取国际化上下文初始化对象
            if (context.HttpContext.Items.TryGetValue("InternationalizationContextInit", out object objInit))
            {
                ((IInternationalizationContextInit)objInit).Execute();
            }
            base.OnActionExecuting(context);
        }
    }
}
