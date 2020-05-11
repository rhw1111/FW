using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MSLibrary.SystemToken.Application
{
    /// <summary>
    /// 应用层获取通用令牌对应的保持登录Url和登出Url
    /// 传入通用令牌的JWT字符串，获取保持登录Url和登出Url
    /// 如果不需要，则对应的Url为null
    /// </summary>
    public interface IAppGetKeepLoginUrlAndLogoutUrl
    {
        Task<AppGetKeepLoginUrlAndLogoutUrlResult> Do(string strToken);
    }

    /// <summary>
    /// 应用层获取通用令牌对应的保持登录Url和登出Url的动作结果
    /// </summary>
    [DataContract]
    public class AppGetKeepLoginUrlAndLogoutUrlResult
    {
        [DataMember]
        public string KeepLoginUrl { get; set; }
        [DataMember]
        public string LogoutUrl { get; set; }
    }
}
