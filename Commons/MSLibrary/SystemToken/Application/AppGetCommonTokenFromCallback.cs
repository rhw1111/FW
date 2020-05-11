using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;

namespace MSLibrary.SystemToken.Application
{
    [Injection(InterfaceType = typeof(IAppGetCommonTokenFromCallback), Scope = InjectionScope.Singleton)]
    public class AppGetCommonTokenFromCallback : IAppGetCommonTokenFromCallback
    {
        private ISystemLoginEndpointRepository _systemLoginEndpointRepository;

        public AppGetCommonTokenFromCallback(ISystemLoginEndpointRepository systemLoginEndpointRepository)
        {
            _systemLoginEndpointRepository = systemLoginEndpointRepository;
        }
        public async Task<string> Do(HttpRequest request)
        {
            //从request的querystring中获取sysname参数
            if (!request.Query.TryGetValue("sysname", out StringValues strSysName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSysNameQuerystringInCallbackRequest,
                    DefaultFormatting = "在第三方认证系统回调请求处理中，回调请求的Url中不包含sysname参数，回调请求的Url为{0}",
                    ReplaceParameters = new List<object>() { request.Path.Value }
                };

                throw new UtilityException((int)Errors.NotFoundSysNameQuerystringInCallbackRequest, fragment);
            }

            //获取系统登录终结点
            var systemLoginEndpoint = await _systemLoginEndpointRepository.QueryByName(strSysName[0]);

            if (systemLoginEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSystemLoginEndpointByName,
                    DefaultFormatting = "找不到名称为{0}的系统登录终结点",
                    ReplaceParameters = new List<object>() { strSysName[0] }
                };

                throw new UtilityException((int)Errors.NotFoundSystemLoginEndpointByName, fragment);
            }


            var redirectUrl=await systemLoginEndpoint.GetCommonToken(request);

            return redirectUrl;
        }
    }
}
