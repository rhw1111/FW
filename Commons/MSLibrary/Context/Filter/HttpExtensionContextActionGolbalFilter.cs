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
    /// 负责Http请求扩展上下文初始化
    /// 需要将该Action过滤器注册到全局Action过滤器中
    /// 且需要放在UserAuthorizeActionGolbalFilter之后，
    /// 表明HttpExtensionContextActionGolbalFilter优先级高，能覆盖之前的相关上下文
    /// </summary>
    [Injection(InterfaceType = typeof(HttpExtensionContextActionGolbalFilter), Scope = InjectionScope.Singleton)]
    public class HttpExtensionContextActionGolbalFilter:ActionFilterAttribute
    {
        private const string _httpContextItemName = "ExtensionContextInits";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //从Http上下文中获取Http请求扩展上下文初始化对象
            if (context.HttpContext.Items.TryGetValue(_httpContextItemName, out object objInit))
            {

                var inits = (Dictionary<string, IHttpExtensionContextInit>)objInit;
                foreach (var item in inits)
                {
                    item.Value.Execute();
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
