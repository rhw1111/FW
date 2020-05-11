using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCenter.Main
{
    /// <summary>
    /// 认证中心错误码
    /// </summary>
    public enum IdentityCenterErrorCodes
    {
        /// <summary>
        /// 在指定用户账号下找不到指定Id的第三方账号
        /// </summary>
        NotFoundUserThirdPartyAccountByIDFromAccountName = 324710000,
        /// <summary>
        /// 找不到指定ID的用户账号
        /// </summary>
        NotFoundUserAccountByID= 324710001,
        /// <summary>
        /// 找不到指定名称的用户账号
        /// </summary>
        NotFoundUserAccountByName= 324710002,
        /// <summary>
        /// 用户账号密码不正确
        /// </summary>
        UserAccountPasswordInvalid= 324710003,
        /// <summary>
        /// 已经存在相同用户名的用户账号
        /// </summary>
        ExistSameNameUserAccount= 324710004,
        /// <summary>
        /// 已经存在相同的第三方账号
        /// </summary>
        ExistSameSourceThirdPartyAccount = 324710005,

        /// <summary>
        /// 认证服务中的用户ID格式不正确
        /// </summary>
        IdentityServerUserIDFormatError = 324710010,
        /// <summary>
        /// 找不到指定ID的认证服务客户端
        /// </summary>
        NotFoundIdentityServerClientByID= 324710011,
        /// <summary>
        /// 认证服务的重定向地址验证不通过
        /// </summary>
        IdentityServerReturnUrlInvalid= 324710012,
        /// <summary>
        /// 第三方认证方认证失败
        /// </summary>
        ExternalIdentityAuthenticationError= 324710013,
        /// <summary>
        /// 找不到指定SchemeName的认证方
        /// </summary>
        NotFoundIdentityProviderBySchemeName = 324710014,
        /// <summary>
        /// 指定SchemeName的认证方未激活
        /// </summary>
        IdentityProviderNotActiveBySchemeName = 324710015,
        /// <summary>
        /// 找不到指定类型的认证方服务
        /// </summary>
        NotFoundIdentityProviderServiceByType= 324710016,
        /// <summary>
        /// 找不到指定名称的认证主机
        /// </summary>
        NotFoundIdentityHostByName= 324710017,
        /// <summary>
        /// 认证主机的签名密钥类型不正确
        /// </summary>
        IdentityHostSigningCredentialTypeError= 324710018,
        /// <summary>
        /// 找不到指定Code的IdentityAuthorizationCode
        /// </summary>
        NotFoundIdentityAuthorizationCodeByCode = 324710020,
        /// <summary>
        /// 找不到指定Handle的认证刷新令牌
        /// </summary>
        NotFoundIdentityRefreshTokenByHandle= 324710021,
        /// <summary>
        /// 找不到指定SubjectId、ClientId的认证确认
        /// </summary>
        NotFoundIdentityConsentBySubjectAndClient= 324710022,
        /// <summary>
        /// 找不到指定名称的Api资源
        /// </summary>
        NotFoundApiResourceByName= 324710030,
        /// <summary>
        /// 授权准许认证失败
        /// </summary>
        ConsentAuthenticationError= 324710040,
        /// <summary>
        /// 授权准许请求解析失败
        /// </summary>
        ConsentRequestResolveError= 324710041,
        /// <summary>
        /// 在OpenID认证请求中找不到ReturnUrl参数
        /// </summary>
        NotFoundReturnUrlInOpenIDRequest = 324710050,
        /// <summary>
        /// OpenID认证请求中的ReturnUrl验证错误
        /// </summary>
        OpenIDRequestReturnUrlInvalid = 324710051,
        /// <summary>
        /// 在OpenID的Cookies中找不到StateReturnUrl
        /// </summary>
        NotFoundReturnUrlStateInOpenIDCookies= 324710052,
        /// <summary>
        /// OpenID认证中的State验证错误
        /// </summary>
        OpenIDStateInvalid= 324710053,
        /// <summary>
        /// OpenID刷新令牌请求出错
        /// </summary>
        OpenIDRefreshTokenRequestError = 324710054,
        /// <summary>
        /// OpenID刷新令牌响应出错
        /// </summary>
        OpenIDRefreshTokenResponseError = 324710055,
        /// <summary>
        /// 找不到指定名称的客户端认证OpenID绑定
        /// </summary>
        NotFoundIdentityClientOpenIDBindingByName= 324710056,
        /// <summary>
        /// 在Http上下文中找不到OpenID绑定名称
        /// </summary>
        NotFoundOpenIDBindingNameInHttpContext = 324710060,
        /// <summary>
        /// 在Cookies中找不到指定名称的OpenID刷新令牌
        /// </summary>
        NotFoundOpenIDRefreshTokenInCookies = 324710061,
        /// <summary>
        /// 找不到指定名称的认证客户端主机
        /// </summary>
        NotFoundIdentityClientHostByName = 324710070,
    }
}
