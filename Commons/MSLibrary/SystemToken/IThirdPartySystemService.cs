using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 第三方系统服务
    /// </summary>
    public interface IThirdPartySystemService
    {
        /// <summary>
        /// 根据配置信息和客户端重定向地址获取对应第三方系统的登录地址
        /// </summary>
        /// <param name="configurationInfo">配置信息</param>
        /// <param name="systemLoginRedirectUrl">验证中心的重定向地址</param>
        /// <param name="clientRedirectUrl">客户端重定向地址</param>
        /// <returns>登陆地址</returns>
        Task<string> GetLoginUrl(string configurationInfo, string systemLoginRedirectUrl, string clientRedirectUrl);

        /// <summary>
        /// 验证第三方令牌是否合法
        /// </summary>
        /// <param name="configurationInfo">配置信息</param>
        /// <param name="systemToken">要验证的系统令牌</param>
        /// <returns>是否合法</returns>
        Task<bool> VerifyToken(string configurationInfo, string systemToken);
        /// <summary>
        /// 获取第三方系统的注销地址
        /// </summary>
        /// <param name="configurationInfo">配置信息</param>
        /// <param name="systemToken">第三方令牌</param>
        /// <returns>注销地址</returns>
        Task<string> GetLogoutUrl(string configurationInfo,string systemToken);


        /// <summary>
        /// 获取第三方系统令牌
        /// returnUrl为经过系统登录终结点将接入方系统的重定向地址转换后的，属于验证中心的返回地址
        /// </summary>
        /// <param name="configurationInfo"></param>
        /// <param name="systemLoginRedirectUrl">验证中心的重定向地址</param>
        /// <param name="clientRedirectUrl">客户端重定向地址</param>
        /// <returns>系统令牌结果</returns>
        Task<GetThirdPartySystemTokenResult> GetSystemToken(string configurationInfo, string systemLoginRedirectUrl, string clientRedirectUrl);

        /// <summary>
        /// 获取从第三方登陆系统回调的令牌和解析后的键值对
        /// </summary>
        /// <param name="configurationInfo"></param>
        /// <param name="request">回调请求</param>
        /// <returns></returns>
        Task<ThirdPartySystemToken> GetSystemToken(string configurationInfo, HttpRequest request);


        /// <summary>
        /// 从第三方令牌中获取实际通信需要的令牌
        /// </summary>
        /// <param name="configurationInfo">配置信息</param>
        /// <param name="systemToken">系统令牌</param>
        /// <returns></returns>
        Task<string> GetCommunicationToken(string configurationInfo, string systemToken);

        /// <summary>
        /// 刷新第三方令牌
        /// </summary>
        /// <param name="configurationInfo">配置信息</param>
        /// <param name="token">要刷新的系统令牌</param>
        /// <returns></returns>
        Task<string> RefreshToken(string configurationInfo, string systemToken);
        /// <summary>
        /// 使用第三方令牌对第三方验证系统执行登出操作
        /// 只有拥有后台登出操作的验证服务才需要实现该方法
        /// 没有后台登出操作的验证服务空实现即可
        /// </summary>
        /// <param name="configurationInfo">配置信息</param>
        /// <param name="systemToken">系统令牌</param>
        /// <returns></returns>
        Task Logout(string configurationInfo, string systemToken);

        /// <summary>
        /// 从回调请求中获取实际重定向地址
        /// </summary>
        /// <param name="configurationInfo"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> GetRealRedirectUrl(string configurationInfo,HttpRequest request);

        /// <summary>
        /// 根据用户名密码获取令牌键值对
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ThirdPartySystemToken> GetSystemTokenByPassword(string configurationInfo, string userName, string password);
        /// <summary>
        /// 获取超时时间
        /// </summary>
        /// <param name="configurationInfo"></param>
        /// <returns></returns>
        Task<int> GetTimeout(string configurationInfo);
        /// <summary>
        /// 获取保持第三方登录状态的Url
        /// </summary>
        /// <param name="configurationInfo"></param>
        /// <param name="systemToken"></param>
        /// <returns></returns>
        Task<string> GetKeepLoginUrl(string configurationInfo, string systemToken);

    }

    [DataContract]
    public class GetThirdPartySystemTokenResult
    {
        /// <summary>
        /// 是否是直接返回系统令牌
        /// </summary>
        [DataMember]
        public bool Direct { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        [DataMember]
        public ThirdPartySystemToken Token { get; set; }
        /// <summary>
        /// 如果Direct=false，则RedirectUrl为第三方验证系统的登陆地址
        /// </summary>
        [DataMember]
        public string RedirectUrl { get; set; }


    }

    [DataContract]
    public class ThirdPartySystemToken
    {
        /// <summary>
        /// 令牌
        /// </summary>
        [DataMember]
        public string Token { get; set; }
        /// <summary>
        /// 令牌转换后的数据键值对
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Attributes { get; set; }
    }
}
