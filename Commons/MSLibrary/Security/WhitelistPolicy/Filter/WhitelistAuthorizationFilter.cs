using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using MSLibrary.DI;
using MSLibrary.AspNet.Filter;
using MSLibrary.AspNet;
using MSLibrary.LanguageTranslate;
using MSLibrary.Security.WhitelistPolicy.Application;

namespace MSLibrary.Security.WhitelistPolicy.Filter
{
    /// <summary>
    /// 白名单的身份验证过滤器
    /// 需要在Http头中加入两个参数
    /// SystemName:调用方系统名称
    /// WhitelistAuthorization:调用方身份令牌,可以直接为SystemSecret，
    /// 或者使用SystemSecret作为签名密钥的JWT格式
    /// 其中playload的格式为
    /// {
    ///     "iat":颁发时间,
    ///     "exp":过期时间,
    ///     "systemname":系统名称
    /// }
    /// 系统操作名称为调用的WebApi接口全名称
    /// </summary>
    [Injection(InterfaceType = typeof(WhitelistAuthorizationFilter), Scope = InjectionScope.Transient)]
    public class WhitelistAuthorizationFilter : AuthorizationFilterBase
    {
        private ILoggerFactory _loggerFactory;
        private IAppValidateRequestForWhitelist _appValidateRequestForWhitelist;
        

        public WhitelistAuthorizationFilter(string matchPath, ILoggerFactory loggerFactory, IAppValidateRequestForWhitelist appValidateRequestForWhitelist):base(matchPath)
        {
            _loggerFactory = loggerFactory;
            _appValidateRequestForWhitelist = appValidateRequestForWhitelist;
        }

        /// <summary>
        /// 错误日志的目录
        /// </summary>
        public static string ErrorCatalogName
        {
            get; set;
        }


        protected override async Task OnRealAuthorizationAsync(AuthorizationFilterContext context)
        {

            ILogger logger = null;
            using (var diContainer = DIContainerContainer.CreateContainer())
            {
                var loggerFactory = diContainer.Get<ILoggerFactory>();
                logger = loggerFactory.CreateLogger(ErrorCatalogName);
            }

            await HttpErrorHelper.ExecuteByAuthorizationFilterContextAsync(context, logger, async () =>
              {
                  //检查在http头中是否已经存在Authorization
                  if (!context.HttpContext.Request.Headers.TryGetValue("WhitelistAuthorization", out StringValues authorizationValue))
                  {
                      ErrorMessage errorMessage = new ErrorMessage()
                      {
                          Code = (int)Errors.NotFoundAuthorizationInHttpHeader,
                          Message = StringLanguageTranslate.Translate(TextCodes.NotFoundWhitelistAuthorizationInHttpHeader, "在http头中没有找到WhitelistAuthorization")
                      };
                      context.Result = new JsonContentResult<ErrorMessage>(StatusCodes.Status401Unauthorized, errorMessage);
                  }
                  //检查在http头中是否已经存在SystemName
                  if (!context.HttpContext.Request.Headers.TryGetValue("SystemName", out StringValues systemNameValue))
                  {
                      ErrorMessage errorMessage = new ErrorMessage()
                      {
                          Code = (int)Errors.NotFoundSystemNameInHttpHeader,
                          Message = StringLanguageTranslate.Translate(TextCodes.NotFoundSystemNameInHttpHeader, "在http头中没有找到SystemName")
                      };
                      context.Result = new JsonContentResult<ErrorMessage>(StatusCodes.Status401Unauthorized, errorMessage);
                  }

                  //获取ActionName的全路径名称
                  var actinName = context.ActionDescriptor.DisplayName.Split(' ')[0];
                  //获取访问的IP
                  var ip = context.HttpContext.Connection.RemoteIpAddress.ToString();

                  //执行验证
                  var validateResult = await _appValidateRequestForWhitelist.Do(actinName, systemNameValue[0], authorizationValue[0], ip);

                  if (!validateResult.Result)
                  {
                      ErrorMessage errorMessage = new ErrorMessage()
                      {
                          Code = (int)Errors.WhitelistValidateFail,
                          Message = string.Format(StringLanguageTranslate.Translate(TextCodes.WhitelistValidateFail, "系统操作{0}针对调用方系统{1}的白名单验证未通过，原因：{2}"), actinName, systemNameValue[0], validateResult.Description)
                      };
                      context.Result = new JsonContentResult<ErrorMessage>(StatusCodes.Status401Unauthorized, errorMessage);
                  }
              });

        }
    }
}
