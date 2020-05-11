using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace MSLibrary.AspNet.Filter
{
    /// <summary>
    /// 允许匿名的授权过滤器
    /// </summary>
    public class AllowAnonymousFilter: Attribute, IAllowAnonymousFilter
    {
    }
}
