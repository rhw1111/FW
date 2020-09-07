using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using System.Web;

namespace IdentityCenter.Main.Providers.AuthenticationHandlers
{
    public class WeChatMiniAuthenticationHandler:OAuthHandler<WeChatMiniOptions>
    {
        /// <summary>
        /// 中间处理基地址
        /// </summary>
        public static string MiddleHandleBaseUrl { get; set; } = "/wechatmini";

		public WeChatMiniAuthenticationHandler(IOptionsMonitor<WeChatMiniOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
		{
		}

		protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
		{

			var userOpenId = await ObtainUserOpenIdAsync(tokens);
			var sessionkey = tokens.Response.RootElement.GetProperty("session_key").GetString();
			var unionid = tokens.Response.RootElement.GetProperty("unionid").GetString();
			var openid = tokens.Response.RootElement.GetProperty("openid").GetString();

			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userOpenId, Options.ClaimsIssuer));
			identity.AddClaim(new Claim("openid", openid, Options.ClaimsIssuer));
			identity.AddClaim(new Claim("sessionkey", sessionkey, Options.ClaimsIssuer));

			var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, tokens.Response.RootElement);

			context.RunClaimActions();

			await Events.CreatingTicket(context);

			return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);

		}

		protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
		{
			var address = QueryHelpers.AddQueryString(Options.CodeSessionEndpoint, new Dictionary<string, string>()
			{
				["appid"] = Options.ClientId,
				["secret"] = Options.ClientSecret,
				["js_code"] = "JSCODE",
				["grant_type"] = "authorization_code"
			});

			var response = await Backchannel.GetAsync(address);
			var responseString = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{

				var fragment = new TextFragment()
				{
					Code = IdentityCenterTextCodes.WeChatMiniLoginError,
					DefaultFormatting = "微信小程序登录错误，错误码：{0}，错误内容：{1}",
					ReplaceParameters = new List<object>() { "-100", responseString }
				};

				var ex= new UtilityException((int)IdentityCenterErrorCodes.WeChatMiniLoginError, fragment);
				return OAuthTokenResponse.Failed(ex);
			}


			var responseObj=JsonSerializerHelper.Deserialize<code2Session>(responseString);

			if (responseObj.ErrCode!=0)
            {
				var fragment = new TextFragment()
				{
					Code = IdentityCenterTextCodes.WeChatMiniLoginError,
					DefaultFormatting = "微信小程序登录错误，错误码：{0}，错误内容：{1}",
					ReplaceParameters = new List<object>() { responseObj.ErrCode.ToString(), responseObj.ErrMsg }
				};

				var ex = new UtilityException((int)IdentityCenterErrorCodes.WeChatMiniLoginError, fragment);
				return OAuthTokenResponse.Failed(ex);
			}


			JsonDocument payload = JsonDocument.Parse(responseString);
		
			return OAuthTokenResponse.Success(payload);
		}


		protected async Task<string> ObtainUserOpenIdAsync(OAuthTokenResponse tokens)
		{
			var unionID=tokens.Response.RootElement.GetProperty("unionid").GetString();

			return await Task.FromResult(unionID);
		}

		protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var url = QueryHelpers.AddQueryString(MiddleHandleBaseUrl, new Dictionary<string, string>()
            {
                { "type","jump"},
                { "to",$"{Options.LoginPageUrl }?redirecturi={HttpUtility.UrlEncode(redirectUri)}" }
            });
            return url;
        }


		protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			if (!string.IsNullOrEmpty(properties.RedirectUri))
			{
				var url = QueryHelpers.AddQueryString(MiddleHandleBaseUrl, new Dictionary<string, string>()
				{
					{ "type","jump"},
					{ "to",properties.RedirectUri}
				});
				properties.RedirectUri = url;
			}
			await base.HandleChallengeAsync(properties);
		}
		[DataContract]
		private class code2Session
        {
			[DataMember(Name = "openid")]
			public string OpenID { get; set; } = null!;
			[DataMember(Name = "session_key")]
			public string SessionKey { get; set; } = null!;
			[DataMember(Name = "unionid")]
			public string UnionID { get; set; } = null!;
			[DataMember(Name = "errcode")]
			public int ErrCode { get; set; }
			[DataMember(Name = "errmsg")]
			public string ErrMsg { get; set; } = null!;
		}



	}
}
