using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Oauth
{
    /// <summary>
    /// Oauth的令牌服务
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 从令牌字符串转换成令牌
        /// </summary>
        /// <param name="accessToken">令牌字符串</param>
        /// <returns></returns>
        Task<Token> ConvertFromAccessToken(string accessToken);
        /// <summary>
        /// 从令牌转成令牌字符串
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>令牌字符串</returns>
        Task<string> ConvertToAccessToken(Token token);
        /// <summary>
        /// 从刷新令牌转成令牌
        /// </summary>
        /// <param name="refreashToken">刷新令牌</param>
        /// <returns>令牌</returns>
        Task<Token> ConvertFromRefreashToken(string refreashToken);

        /// <summary>
        /// 从令牌转成刷新字符串
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>刷新字符串</returns>
        Task<string> ConvertToRefreashToken(Token token);
    }
}
