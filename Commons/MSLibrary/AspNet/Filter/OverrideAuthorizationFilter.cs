using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MSLibrary.AspNet.Filter
{
    public class OverrideAuthorizationFilter : Attribute,IAsyncAuthorizationFilter, IOverrideFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
             await Task.FromResult(0);
        }
    }
}
