using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;
using MSLibrary.AspNet.Filter;
using MSLibrary.AspNet;
using MSLibrary.LanguageTranslate;
using MSLibrary.Context.Application;

namespace MSLibrary.Context.Filter
{
    /// <summary>
    /// 用户授权过滤器
    /// 负责授权验证、上下文的初始化，按指定的userGeneratorName执行上下文生成
    /// 如果允许匿名，并且认证未通过，按指定的anonymousGeneratorName执行上下文生成
    /// 如果directGeneratorName不为空，则忽略其他条件，直接使用该生成器执行上下文生成
    /// </summary>
    [Injection(InterfaceType = typeof(UserAuthorizeFilter), Scope = InjectionScope.Transient)]
    public class UserAuthorizeFilter : AuthorizationFilterBase
    {
        private bool _allowAnonymous;
        private string _userGeneratorName;
        private string _anonymousGeneratorName;
        private string _directGeneratorName;
        private IAppUserAuthorize _appUserAuthorize;

        /// <summary>
        /// 错误日志的目录
        /// </summary>
        public static string ErrorCatalogName
        {
            get;set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="allowAnonymous">是否允许匿名</param>
        /// <param name="matchPath">匹配的路径</param>
        /// <param name="userGeneratorName">认证通过时，使用的用户声明上下文生成器的名称</param>
        /// <param name="anonymousGeneratorName">认证不通过时，如果允许匿名，使用的用户声明上下文生成器名称</param>
        /// <param name="directGeneratorName">忽略其他条件，直接使用该用户声明上下文生成器名称的生成器来生成上下文</param>
        /// <param name="appUserAuthorize">应用层用户验证应用</param>

        public UserAuthorizeFilter(bool allowAnonymous,string matchPath, string userGeneratorName,string anonymousGeneratorName,string directGeneratorName, IAppUserAuthorize appUserAuthorize):base(matchPath)
        {
            _allowAnonymous = allowAnonymous;
            _userGeneratorName = userGeneratorName;
            _anonymousGeneratorName = anonymousGeneratorName;
            _directGeneratorName = directGeneratorName;
            _appUserAuthorize = appUserAuthorize;
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
                  //如果_directGeneratorName不为空，则直接使用该生成器名称
                  if (!string.IsNullOrEmpty(_directGeneratorName))
                  {
                      var authorizeResult = await _appUserAuthorize.Do(null, _directGeneratorName);
                      //存储到http上下文中
                      context.HttpContext.Items["AuthorizeResult"] = authorizeResult;
                      return;
                  }

                  //判断是否已经通过验证
                  if (context.HttpContext.User != null && context.HttpContext.User.Identity!=null && context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.User.Claims!=null)
                  {
                      var authorizeResult = await _appUserAuthorize.Do(context.HttpContext.User.Claims, _userGeneratorName);

                      //存储到Http上下文中
                      context.HttpContext.Items["AuthorizeResult"] = authorizeResult;
                      //authorizeResult.Execute();
                  }
                  else
                  {
                      if (_allowAnonymous)
                      {
                          //未通过验证，但允许匿名，则调用匿名上下文生成
                          var authorizeResult = await _appUserAuthorize.Do(null, _anonymousGeneratorName);
                          //存储到http上下文中
                          context.HttpContext.Items["AuthorizeResult"] = authorizeResult;
                          //authorizeResult.Execute();
                      }
                      else
                      {
                          //返回错误响应

                          ErrorMessage errorMessage = new ErrorMessage()
                          {
                              Type = 1,
                              Level = 1,
                              Code = (int)Errors.AuthorizeFail,
                              Message = string.Format(StringLanguageTranslate.Translate(TextCodes.AuthorizeFail, "用户授权失败，没有找到对应的身份信息"))
                          };
                          context.Result = new JsonContentResult<ErrorMessage>(StatusCodes.Status401Unauthorized, errorMessage);
                      }
                  }
              });


        }
    }
}
