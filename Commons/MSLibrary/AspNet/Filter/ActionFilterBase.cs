using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MSLibrary.AspNet.Filter
{
    /// <summary>
    /// ActionFilter的基类
    /// 检查是否包含order大于当前过滤器order的IoverriderFilter接口，如果存在则不执行当前业务
    /// 可以设置请求路径检测，通过正则表达式检查请求路径是否匹配，只有匹配的请求才执行过滤器
    /// </summary>
    public abstract class ActionFilterBase:ActionFilterAttribute
    {
        private string _matchPath=null;

        public ActionFilterBase()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchPath">要匹配的路径的正则表达式</param>
        public ActionFilterBase(string matchPath)
        {
            _matchPath = matchPath;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //在Http上下文中存储ActionName
            context.HttpContext.Items["ActionName"] = context.ActionDescriptor.DisplayName.Split("(")[0].Trim();
            
            bool needDo = true;

            var overrideFilter = (from item in context.Filters
                                  where item is OverrideActionFilter
                                  select item).FirstOrDefault();

            var overrideIndex = -1;
            if (overrideFilter != null)
            {
                overrideIndex=context.Filters.IndexOf(overrideFilter);
            }

            var index = context.Filters.IndexOf(this);

            if (overrideIndex >= index)
            {
                needDo = false;
            }

            if (needDo && _matchPath!=null && context.HttpContext.Request.Path.HasValue)
            {
                //检查当前请求路径是否匹配
                Regex regex = new Regex(_matchPath, RegexOptions.IgnoreCase);

                if (!regex.IsMatch(context.HttpContext.Request.Path.Value))
                {
                    needDo = false;
                }
            }


            if (needDo)
            {
                await OnRealActionExecutionAsync(context,next);
            }
            else
            {
                
                await next();
            }
        }

        protected abstract Task OnRealActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next);
    }
}
