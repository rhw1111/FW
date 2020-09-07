using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Context.Application;
using MSLibrary.ExceptionHandle;
using MSLibrary;


namespace IdentityCenter.Main.AspNet.Middleware
{
    /// <summary>
    /// 负责所有微信小程序交互请求的处理
    /// </summary>
    public class WeChatMiniHandler
    {
        private RequestDelegate _nextMiddleware;
        private string _categoryName;
        private bool _isDebug = false;

        public static IDictionary<string, IFactory<IWeChatMiniRealHandler>> HandlerFactories = new Dictionary<string, IFactory<IWeChatMiniRealHandler>>();

        public WeChatMiniHandler(RequestDelegate nextMiddleware, string categoryName, bool isDebug)
        {
            _nextMiddleware = nextMiddleware;
            _categoryName = categoryName;
            _isDebug = isDebug;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Query.TryGetValue("type", out StringValues typeVaue))
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.WeChatMiniHandleRequestMissPara,
                    DefaultFormatting = "微信小程序请求处理缺少参数，缺少参数{0}",
                    ReplaceParameters = new List<object>() { "type" }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.WeChatMiniHandleRequestMissPara, fragment);
            }

            string type = typeVaue[0];

            if (!HandlerFactories.TryGetValue(type,out IFactory<IWeChatMiniRealHandler>? handlerFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundWeChatMiniRealHandlerByType,
                    DefaultFormatting = "找不到类型为{0}的微信小程序请求处理，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type,$"{this.GetType().FullName}.HandlerFactories" }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundWeChatMiniRealHandlerByType, fragment);               
            }

            await handlerFactory.Create().Execute(context);

        }

    }

    public interface IWeChatMiniRealHandler
    {
        Task Execute(HttpContext context);
    }
}
