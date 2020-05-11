using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Security;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Oauth
{
    /// <summary>
    /// 令牌服务默认实现
    /// </summary>
    [Injection(InterfaceType =typeof(ITokenService),Scope =InjectionScope.Singleton)]
    public class TokenService : ITokenService
    {
        private ISecurityService _securityService;
        public TokenService(ISecurityService securityService)
        {
            _securityService = securityService;
        }
        public async Task<Token> ConvertFromAccessToken(string accessToken)
        {
            //accessToken由TokenWrapper的json序列化后加密组成
            //需要先解密，再反序列化
            var dAccessToken=_securityService.Decrypt(accessToken);
            var tokenWrapper=JsonSerializerHelper.Deserialize<TokenWrapper>(dAccessToken);
            if (tokenWrapper.Type!=1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotAccessToken,
                    DefaultFormatting = "传入的字符串不是AccessToken类型",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)Errors.NotAccessToken, fragment);
            }
            return await Task.FromResult(tokenWrapper.Token);
        }

        public async Task<Token> ConvertFromRefreashToken(string refreashToken)
        {
            //refreashToken由TokenWrapper的json序列化后加密组成
            //需要先解密，再反序列化
            var dAccessToken = _securityService.Decrypt(refreashToken);
            var tokenWrapper = JsonSerializerHelper.Deserialize<TokenWrapper>(dAccessToken);
            if (tokenWrapper.Type != 2)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotRefreashAccessToken,
                    DefaultFormatting = "传入的字符串不是RefreashAccessToken类型",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)Errors.NotRefreashAccessToken,fragment);
            }
            return await Task.FromResult(tokenWrapper.Token);
        }

        public async Task<string> ConvertToAccessToken(Token token)
        {
            //将token转成TokenWrapper后序列化，然后加密
            TokenWrapper wrapper = new TokenWrapper()
            {
                Token = token,
                Type = 1
            };

            var strJson=JsonSerializerHelper.Serializer<TokenWrapper>(wrapper);
            var eJson = _securityService.Encrypt(strJson);
            return await Task.FromResult(eJson);

        }

        public async Task<string> ConvertToRefreashToken(Token token)
        {
            //将token转成TokenWrapper后序列化，然后加密
            TokenWrapper wrapper = new TokenWrapper()
            {
                Token = token,
                Type = 2
            };

            var strJson = JsonSerializerHelper.Serializer<TokenWrapper>(wrapper);
            var eJson = _securityService.Encrypt(strJson);
            return await Task.FromResult(eJson);
        }
    }


    [DataContract]
    public class TokenWrapper
    {
        [DataMember]
        public Token Token { get; set; }
        [DataMember]
        public int Type { get; set; }
    }
}
