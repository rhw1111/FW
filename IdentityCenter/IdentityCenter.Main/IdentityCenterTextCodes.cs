using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCenter.Main
{
    /// <summary>
    /// 认证中心文本片段Code
    /// </summary>
    public static class IdentityCenterTextCodes
    {
        /// <summary>
        /// 在指定用户账号下找不到指定Id的第三方账号
        /// 格式为“名称为{0}的用户账号下，找不到id为{1}的第三方账号”
        /// {0}：用户账号名称
        /// {1}：第三方账号ID
        /// </summary>
        public const string NotFoundUserThirdPartyAccountByIDFromAccountName = "NotFoundUserThirdPartyAccountByIDFromAccountName";
        /// <summary>
        /// 认证服务中的用户ID格式不正确
        /// 格式为“认证服务中的用户ID格式不正确，期待的格式为{0}，用户ID为{1}”
        /// {0}：正确的格式
        /// {1}：用户ID
        /// </summary>
        public const string IdentityServerUserIDFormatError = "IdentityServerUserIDFormatError";
        /// <summary>
        /// 找不到指定ID的用户账号
        /// 格式为“找不到ID为{0}的用户账号”
        /// {0}：用户ID
        /// </summary>
        public const string NotFoundUserAccountByID = "NotFoundUserAccountByID";

        /// <summary>
        /// 找不到指定名称的用户账号
        /// 格式为“找不到名称为{0}的用户账号”
        /// {0}：用户名称
        /// </summary>
        public const string NotFoundUserAccountByName = "NotFoundUserAccountByName";
        /// <summary>
        ///  已经存在相同用户名的用户账号
        ///  格式为“已经存在名称为{0}的用户账号”
        ///  {0}：账号名称
        /// </summary>
        public const string ExistSameNameUserAccount = "ExistSameNameUserAccount";
        /// <summary>
        /// 已经存在相同的第三方账号
        /// 格式为“已经存在来源为{0}，Id为{1}的第三方账号”
        /// </summary>
        public const string ExistSameSourceThirdPartyAccount = "ExistSameSourceThirdPartyAccount";

        /// <summary>
        /// 找不到指定ID的认证服务客户端
        /// 格式为“找不到id为{0}的认证服务客户端”
        /// {0}：认证服务客户端ID
        /// </summary>
        public const string NotFoundIdentityServerClientByID = "NotFoundIdentityServerClientByID";
        /// <summary>
        /// 用户账号密码不正确
        /// 格式为“名称为{0}的用户账号的密码不正确”
        /// {0}：账号名称
        /// </summary>
        public const string UserAccountPasswordInvalid = "UserAccountPasswordInvalid";
        /// <summary>
        /// 认证服务的重定向地址验证不通过
        /// 格式为“认证服务的重定向地址验证不通过，当前地址为{0}”
        /// {0}：当前的重定向地址
        /// </summary>
        public const string IdentityServerReturnUrlInvalid = "IdentityServerReturnUrlInvalid";
        /// <summary>
        /// 找不到指定SchemeName的认证方
        /// 格式为“找不到SchemeName为{0}的认证方”
        /// {0}：认证方的SchemeName
        /// </summary>
        public const string NotFoundIdentityProviderBySchemeName = "NotFoundIdentityProviderBySchemeName";
        /// <summary>
        /// 指定SchemeName的认证方未激活
        /// 格式为“SchemeName为{0}的认证方未激活”
        /// {0}：认证方的SchemeName
        /// </summary>
        public const string IdentityProviderNotActiveBySchemeName = "IdentityProviderNotActiveBySchemeName";
        /// <summary>
        /// 第三方认证方认证失败
        /// 格式为“第三方认证方认证失败”
        /// </summary>
        public const string ExternalIdentityAuthenticationError = "ExternalIdentityAuthenticationError";
        /// <summary>
        ///  找不到指定类型的认证方服务
        ///  格式为“找不到类型为{0}的认证方服务，发生位置为{1}”
        ///  {0}：认证方类型
        ///  {1}：发生的位置
        /// </summary>
        public const string NotFoundIdentityProviderServiceByType = "NotFoundIdentityProviderServiceByType";
        /// <summary>
        /// 找不到指定名称的认证主机
        /// 格式为“找不到名称为{0}的认证主机”
        /// {0}：主机名称
        /// </summary>
        public const string NotFoundIdentityHostByName = "NotFoundIdentityHostByName";
        /// <summary>
        /// 认证主机的签名密钥类型不正确
        /// 格式为“认证主机{0}的签名密钥类型不正确，当前类型为{1}”
        /// {0}：主机名称
        /// {1}：签名密钥类型
        /// </summary>
        public const string IdentityHostSigningCredentialTypeError = "IdentityHostSigningCredentialTypeError";
        /// <summary>
        /// 找不到指定Code的IdentityAuthorizationCode
        /// 格式为“找不到Code为{0}的IdentityAuthorizationCode”
        /// {0}：code
        /// </summary>
        public const string NotFoundIdentityAuthorizationCodeByCode = "NotFoundIdentityAuthorizationCodeByCode";
        /// <summary>
        /// 找不到指定Handle的认证刷新令牌
        /// 格式为“找不到Handle为{0}的认证刷新令牌”
        /// {0}：handle
        /// </summary>
        public const string NotFoundIdentityRefreshTokenByHandle = "NotFoundIdentityRefreshTokenByHandle";
        /// <summary>
        /// 找不到指定SubjectId、ClientId的认证确认
        /// 格式为“找不到SubjectId为{0}、ClientId为{1}的认证确认”
        /// {0}：SubjectId
        /// {1}:ClientId
        /// </summary>
        public const string NotFoundIdentityConsentBySubjectAndClient = "NotFoundIdentityConsentBySubjectAndClient";

        /// <summary>
        /// 找不到指定名称的Api资源
        /// 格式为“找不到名称为{0}的Api资源”
        /// {0}：资源名称
        /// </summary>
        public const string NotFoundApiResourceByName = "NotFoundApiResourceByName";
        /// <summary>
        /// 授权准许认证失败
        /// 格式为“授权准许认证失败”
        /// </summary>
        public const string ConsentAuthenticationError = "ConsentAuthenticationError";
        /// <summary>
        /// 授权准许请求解析失败
        /// 格式为“授权准许请求解析失败”
        /// </summary>
        public const string ConsentRequestResolveError = "ConsentRequestResolveError";
        /// <summary>
        /// 在OpenID认证请求中找不到ReturnUrl参数
        /// 格式为“在OpenID认证请求中找不到ReturnUrl参数,绑定名称为{0}”
        /// {0}：绑定名称
        /// </summary>
        public const string NotFoundReturnUrlInOpenIDRequest = "NotFoundReturnUrlInOpenIDRequest";
        /// <summary>
        ///  OpenID认证请求中的ReturnUrl验证错误
        ///  格式为“OpenID认证请求中的ReturnUrl验证错误，当前ReturnUrl为{0}，合法的基地址为{1}，绑定名称为{2}”
        ///  {0}：ReturnUrl
        ///  {1}：合法的基地址
        ///  {2}：绑定名称
        /// </summary>
        public const string OpenIDRequestReturnUrlInvalid = "OpenIDRequestReturnUrlInvalid";
        /// <summary>
        ///  在OpenID的Cookies中找不到StateReturnUrl
        ///  格式为“在OpenID的Cookies中找不到名称为{0}的StateReturnUrl，绑定名称为{1}”
        ///  {0}：cookies的名称
        ///  {1}：绑定名称
        /// </summary>
        public const string NotFoundReturnUrlStateInOpenIDCookies = "NotFoundReturnUrlStateInOpenIDCookies";
        /// <summary>
        /// OpenID认证中的State验证错误
        /// 格式为“OpenID认证中的State验证错误，接收的state为{0}，存储的state为{1}，绑定名称为{2}”
        /// {0}：接收的state
        /// {1}：存储的state
        /// {2}：绑定名称
        /// </summary>
        public const string OpenIDStateInvalid = "OpenIDStateInvalid";
        /// <summary>
        /// OpenID刷新令牌请求出错
        /// 格式为“OpenID刷新令牌请求出错，错误内容为{0}，绑定名称为{1}”
        /// {0}：错误内容
        /// {1}：绑定名称
        /// </summary>
        public const string OpenIDRefreshTokenRequestError = "OpenIDRefreshTokenRequestError";
        /// <summary>
        /// OpenID刷新令牌响应出错
        /// 格式为“OpenID刷新令牌响应出错，错误内容为{0}，绑定名称为{1}”
        /// {0}：错误内容
        /// {1}：绑定名称
        /// </summary>
        public const string OpenIDRefreshTokenResponseError = "OpenIDRefreshTokenResponseError";
        /// <summary>
        /// 找不到指定名称客户端认证OpenID绑定
        /// 格式为“找不到名称为{0}的客户端认证OpenID绑定”
        /// {0}：绑定名称
        /// </summary>
        public const string NotFoundIdentityClientOpenIDBindingByName = "NotFoundIdentityClientOpenIDBindingByName";
        /// <summary>
        ///  在Http上下文中找不到OpenID绑定名称
        ///  格式为“在Http上下文中找不到OpenID绑定名称”
        /// </summary>
        public const string NotFoundOpenIDBindingNameInHttpContext = "NotFoundOpenIDBindingNameInHttpContext";

        /// <summary>
        /// 在Cookies中找不到指定名称的OpenID刷新令牌
        /// 格式为“在Cookies中找不到名称为{0}的OpenID刷新令牌”
        /// {0}：cookies名称
        /// </summary>
        public const string NotFoundOpenIDRefreshTokenInCookies = "NotFoundOpenIDRefreshTokenInCookies";


        /// <summary>
        /// 找不到指定名称的认证客户端主机
        /// 格式为“找不到名称为{0}的认证客户端主机”
        /// {0}：客户端主机名称
        /// </summary>
        public const string NotFoundIdentityClientHostByName = "NotFoundIdentityClientHostByName";
        /// <summary>
        /// 微信小程序请求处理缺少参数
        /// 格式为“微信小程序请求处理缺少参数，缺少参数{0}”
        /// {0}：缺少的参数名称
        /// </summary>
        public const string WeChatMiniHandleRequestMissPara = "WeChatMiniHandleRequestMissPara";
        /// <summary>
        /// 找不到指定类型的微信小程序请求处理
        /// 格式为“找不到类型为{0}的微信小程序请求处理，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundWeChatMiniRealHandlerByType = "NotFoundWeChatMiniRealHandlerByType";
        /// <summary>
        /// 微信小程序登录错误
        /// 格式为“微信小程序登录错误，错误码：{0}，错误内容：{1}”
        /// {0}：错误码
        /// {1}：错误内容
        /// </summary>
        public const string WeChatMiniLoginError = "WeChatMiniLoginError";
        /// <summary>
        /// 已经存在指定名称的角色
        /// 格式为“已经存在名称为{0}的角色”
        /// {0}：名称
        /// </summary>
        public const string ExistRoleByName = "ExistRoleByName";
        /// <summary>
        /// 已经存在指定编码的权限
        /// 格式为“已经存在编码为{0}的权限”
        /// {0}：编码
        /// </summary>
        public const string ExistPrivilegeByCode = "ExistPrivilegeByCode";

    }

}
