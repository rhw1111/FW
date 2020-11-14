using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;

namespace MSLibrary.AspNet.AuthorizationPolicyProviders
{
    public class HttpContextAuthorizePolicyProvider : IAuthorizationPolicyProvider
    {
        public const string _prefix = "#HttpContext_";

        private readonly IHttpContextAccessor _httpContextAccessor;
        public DefaultAuthorizationPolicyProvider _defaultProvider { get; }

        public HttpContextAuthorizePolicyProvider(IHttpContextAccessor httpContextAccessor, IOptions<AuthorizationOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _defaultProvider= new DefaultAuthorizationPolicyProvider(options);
        }
        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await _defaultProvider.GetDefaultPolicyAsync();
        }

        public async Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return await _defaultProvider.GetFallbackPolicyAsync();
        }

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var service=(IHttpContextPolicyResolveService)DIContainerContainer.Get(Type.GetType(policyName.Substring(_prefix.Length)));
                return await service.Execute(httpContext,_defaultProvider);
            }
            else
            {
                return await _defaultProvider.GetPolicyAsync(policyName);
            }
        }
    }
}
