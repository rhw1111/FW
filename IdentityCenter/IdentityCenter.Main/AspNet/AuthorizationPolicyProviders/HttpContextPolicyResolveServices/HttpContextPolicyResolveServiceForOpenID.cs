using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MSLibrary.AspNet.AuthorizationPolicyProviders;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace IdentityCenter.Main.AspNet.AuthorizationPolicyProviders.HttpContextPolicyResolveServices
{
    /// <summary>
    /// 针对OpenID的http上下文授权策略服务
    /// 依次从querystring检查binding、formdata检查state、querysting检查state
    /// 这些对应IdentityClientOpenIDBinding的名称，将该名称作为策略中的验证Scheme
    /// </summary>
    public class HttpContextPolicyResolveServiceForOpenID : IHttpContextPolicyResolveService
    {
        private const string  _binding= "binding";
        private const string _state = "state";

        public async Task<AuthorizationPolicy> Execute(HttpContext context)
        {
            bool exist = false;
            string binding=string.Empty;
            if (context.Request.Query.TryGetValue(_binding,out StringValues strBinding))
            {
                exist = true;
                binding = strBinding[0];
            }

            if (!exist)
            {
                if (context.Request.Form.TryGetValue(_state, out strBinding))
                {
                    exist = true;
                    binding = (strBinding[0].Split("+"))[0];
                }
            }

            if (!exist)
            {
                if (context.Request.Query.TryGetValue(_state, out strBinding))
                {
                    exist = true;
                    binding = (strBinding[0].Split("+"))[0];
                }
            }

            if (!exist)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundOpenIDBindingNameInHttpContext,
                    DefaultFormatting = "在Http上下文中找不到OpenID绑定名称",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundOpenIDBindingNameInHttpContext, fragment, 1, 0);
            }

            var policy = new AuthorizationPolicyBuilder(binding);
            return await Task.FromResult(policy.Build());
        }
    }
}
