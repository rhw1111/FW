using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using MSLibrary.LanguageTranslate;
using MSLibrary.AspNet;
using MSLibrary.Security.WhitelistPolicy.Application;
using MSLibrary.DI;

namespace MSLibrary.Security.WhitelistPolicy.Middleware
{
    /// <summary>
    /// 白名单的身份验证中间件
    /// 需要在Http头中加入两个参数
    /// SystemName:调用方系统名称
    /// Authorization:调用方身份令牌,可以直接为SystemSecret，
    /// 或者使用SystemSecret作为签名密钥的JWT格式
    /// 其中playload的格式为
    /// {
    ///     "iat":颁发时间,
    ///     "exp":过期时间,
    ///     "systemname":系统名称
    /// }
    /// 系统操作名称为请求的相对路径（不包含根路径）
    /// </summary>
    public class WhitelistAuthorization
    {
        private RequestDelegate _nextMiddleware;
        private string _errorCatalogName;
        private ILoggerFactory _loggerFactory;
        private IAppValidateRequestForWhitelist _appValidateRequestForWhitelist;

        public WhitelistAuthorization(RequestDelegate next,string errorCatalogName, ILoggerFactory loggerFactory, IAppValidateRequestForWhitelist appValidateRequestForWhitelist)
        {
            _nextMiddleware = next;
            _errorCatalogName = errorCatalogName;
            _loggerFactory = loggerFactory;
            _appValidateRequestForWhitelist = appValidateRequestForWhitelist;
        }

        public async Task Invoke(HttpContext context)
        {

            ILogger logger = null;

            using (var diContainer = DIContainerContainer.CreateContainer())
            {
                var loggerFactory = diContainer.Get<ILoggerFactory>();
                logger = loggerFactory.CreateLogger(_errorCatalogName);
            }

            await HttpErrorHelper.ExecuteByHttpContextAsync(context,logger, async () =>
             {
                 bool complete = false;
                 //检查在http头中是否已经存在WhitelistAuthorization
                 if (!context.Request.Headers.TryGetValue("WhitelistAuthorization", out StringValues authorizationValue))
                 {
                     ErrorMessage errorMessage = new ErrorMessage()
                     {
                         Code = (int)Errors.NotFoundAuthorizationInHttpHeader,
                         Message = StringLanguageTranslate.Translate(TextCodes.NotFoundWhitelistAuthorizationInHttpHeader, "在http头中没有找到WhitelistAuthorization")
                     };

                     await context.Response.WriteJson(StatusCodes.Status401Unauthorized, errorMessage);
                     complete = true;
                 }
                 //检查在http头中是否已经存在SystemName
                 if (!context.Request.Headers.TryGetValue("SystemName", out StringValues systemNameValue))
                 {
                     ErrorMessage errorMessage = new ErrorMessage()
                     {
                         Code = (int)Errors.NotFoundSystemNameInHttpHeader,
                         Message = StringLanguageTranslate.Translate(TextCodes.NotFoundSystemNameInHttpHeader, "在http头中没有找到SystemName")
                     };
                     await context.Response.WriteJson(StatusCodes.Status401Unauthorized, errorMessage);
                     complete = true;
                 }

                 //获取请求路径名称
                 var actinName = context.Request.Path;
                 //获取访问的IP
                 var ip = context.Connection.RemoteIpAddress.ToString();

                 //执行验证
                 var validateResult = await _appValidateRequestForWhitelist.Do(actinName, systemNameValue[0], authorizationValue[0], ip);

                 if (!validateResult.Result)
                 {
                     ErrorMessage errorMessage = new ErrorMessage()
                     {
                         Code = (int)Errors.WhitelistValidateFail,
                         Message = string.Format(StringLanguageTranslate.Translate(TextCodes.WhitelistValidateFail, "系统操作{0}针对调用方系统{1}的白名单验证未通过，原因：{2}"), actinName, systemNameValue[0], validateResult.Description)
                     };
                     await context.Response.WriteJson(StatusCodes.Status401Unauthorized, errorMessage);
                     complete = true;
                 }

                 //如果检测通过，则继续执行下面的管道
                 if (!complete)
                 {
                     await _nextMiddleware(context);
                 }
             });


        }
    }
}
