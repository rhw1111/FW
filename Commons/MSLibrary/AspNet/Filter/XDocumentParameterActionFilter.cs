using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using MSLibrary.DI;

using Microsoft.AspNetCore.Mvc.Filters;

namespace MSLibrary.AspNet.Filter
{
    [Injection(InterfaceType = typeof(XDocumentParameterActionFilter), Scope = InjectionScope.Transient)]
    public class XDocumentParameterActionFilter : ActionFilterBase
    {
        private string _paramName;

        public XDocumentParameterActionFilter(string paramName)
        {
            _paramName = paramName;
        }
        protected override async Task OnRealActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            byte[] bufferBytes = new byte[1024];
            List<byte> requestBytes = new List<byte>();
            while(true)
            {
                var resultCount = await context.HttpContext.Request.Body.ReadAsync(bufferBytes, 0, 1024);
                requestBytes.AddRange(bufferBytes.Take(resultCount));
                if (resultCount<1024)
                {
                    break;
                }
            }

            var str=UTF8Encoding.UTF8.GetString(requestBytes.ToArray());

            context.ActionArguments[_paramName]= XDocument.Parse(str);
            context.ActionArguments["doc"] = context.ActionArguments[_paramName];

            await next();
        }
    }
}
