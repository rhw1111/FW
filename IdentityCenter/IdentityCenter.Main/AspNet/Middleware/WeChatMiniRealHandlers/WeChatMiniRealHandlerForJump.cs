using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace IdentityCenter.Main.AspNet.Middleware.WeChatMiniRealHandlers
{
    [Injection(InterfaceType = typeof(WeChatMiniRealHandlerForJump), Scope = InjectionScope.Singleton)]
    public class WeChatMiniRealHandlerForJump : IWeChatMiniRealHandler
    {
        private static string WeixinJSUrl { get; set; } = "https://res.wx.qq.com/open/js/jweixin-1.3.2.js";
        public async Task Execute(HttpContext context)
        {
            if (!context.Request.Query.TryGetValue("to", out StringValues uriVaue))
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.WeChatMiniHandleRequestMissPara,
                    DefaultFormatting = "微信小程序请求处理缺少参数，缺少参数{0}",
                    ReplaceParameters = new List<object>() { "to" }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.WeChatMiniHandleRequestMissPara, fragment);
            }

            string uri = uriVaue[0];

            Dictionary<string, string> dictPara = new Dictionary<string, string>();

            foreach (var item in context.Request.Query)
            {
                if (item.Key!="to")
                {
                    dictPara[item.Key]=item.Value[0];
                }
            }

            foreach (var item in context.Request.Form)
            {
                dictPara[item.Key] = item.Value[0];
            }

            

            var content = @$"<html>
                                <script type=""text/javascript"" src=""{WeixinJSUrl}""></script>
                                <script type=""text/javascript"">
                                    wx.miniProgram.navigateTo({{url: '{QueryHelpers.AddQueryString(uri, dictPara)}'}})
                                </script>
                            </html>";

            await context.Response.WriteAsync(content);
            await context.Response.CompleteAsync();
        }
    }
}
