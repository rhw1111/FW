using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace MSLibrary.AspNet.AuthenticationHandlers
{
   /// <summary>
   /// 令牌认证参数
   /// </summary>
    public class TokenAuthenticationOptions: AuthenticationSchemeOptions
    {
        /// <summary>
        /// 在header中存储令牌的头名称
        /// </summary>
        public string HeaderName { get; set; }
        /// <summary>
        /// 要使用的令牌控制器名称
        /// </summary>
        public string TokenControllerName { get; set; }
        /// <summary>
        /// 参数解析，如果赋值，优先处理
        /// </summary>
        public ITokenAuthenticationOptionsResolve Resolve { get; set; }
    }


    
    
}
