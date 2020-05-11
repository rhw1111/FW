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
    /// 协助用户验证
    /// 需要将该Action过滤器注册到全局Action过滤器中
    /// </summary>
    [Injection(InterfaceType = typeof(UserAuthorizeActionGolbalFilter), Scope = InjectionScope.Singleton)]
    public class UserAuthorizeActionGolbalFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //从Http上下文中获取上下文生成结果
            if (context.HttpContext.Items.TryGetValue("AuthorizeResult",out object objResult))
            {
                ((IAppUserAuthorizeResult)objResult).Execute();
            }
            base.OnActionExecuting(context);
        }
    }
}
