using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace IdentityCenter.Main.Providers.AuthenticationHandlers
{
    public class WeChatMiniOptions : OAuthOptions
    {
        /// <summary>
        /// 获取令牌的终结点地址
        /// </summary>
        public string CodeSessionEndpoint { get; set; } = WeCahtMiniOptionsDefault.CodeSessionEndpoint;
       
        /// <summary>
        /// 小程序中用来执行获取Code操作的页面地址
        /// </summary>
        public string LoginPageUrl { get; set; } = null!;

        public WeChatMiniOptions()
        {
            ClaimsIssuer = WeCahtMiniOptionsDefault.Issuer;
            CallbackPath = new PathString(WeCahtMiniOptionsDefault.CallbackPath);
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        }
    }

    public static class WeCahtMiniOptionsDefault
    {
        public const string CodeSessionEndpoint = "https://api.weixin.qq.com/sns/jscode2session";
		
		public const string AuthenticationScheme = "WeChatMini";


		public const string DisplayName = "WeChatMini";


		public const string CallbackPath = "/signin-wechatmini";

		public const string Issuer = "WeChatMini";

	}
}
