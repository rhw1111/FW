using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Security;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.SystemToken.Application
{
    [Injection(InterfaceType = typeof(IAppRefreashCommonToken), Scope = InjectionScope.Singleton)]
    public class AppRefreashCommonToken : IAppRefreashCommonToken
    {
        private ISecurityService _securityService;
        private ISystemLoginEndpointRepository _systemLoginEndpointRepository;

        public AppRefreashCommonToken(ISecurityService securityService, ISystemLoginEndpointRepository systemLoginEndpointRepository)
        {
            _securityService = securityService;
            _systemLoginEndpointRepository = systemLoginEndpointRepository;
        }
        public async Task<string> Do(string strToken)
        {
            //从strToken中分解出JWT的键值对，从键值对中获取SystemName
            //_securityService.ValidateJWT()

            var jwtResult = _securityService.GetPlayloadFromJWT(strToken);

            if (!jwtResult.ValidateResult.Result)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ExecuteAppJWTError,
                    DefaultFormatting = "在执行应用{0}时，处理的JWT{1}错误，错误原因{2}",
                    ReplaceParameters = new List<object>() { "MSLibrary.SystemToken.Application.AppRefreashCommonToken", strToken, jwtResult.ValidateResult.Description }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.ExecuteAppJWTError, fragment);
            }




            Dictionary<string, string> jwtDict = jwtResult.Playload;
            if (!jwtDict.TryGetValue("SystemName",out string systemName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInCommonTokenJWT,
                    DefaultFormatting = "在通用令牌的JWT字符串{0}中，找不到名称为{1}的键",
                    ReplaceParameters = new List<object>() { strToken, "SystemName" }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInCommonTokenJWT, fragment);
            }

            //获取系统登录终结点
            var systemLoginEndpoint = await _systemLoginEndpointRepository.QueryByName(systemName);

            if (systemLoginEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSystemLoginEndpointByName,
                    DefaultFormatting = "找不到名称为{0}的系统登录终结点",
                    ReplaceParameters = new List<object>() { systemName }
                };

                throw new UtilityException((int)Errors.NotFoundSystemLoginEndpointByName, fragment);
            }
            //刷新令牌的JWT字符串
            return await systemLoginEndpoint.RefreshToken(strToken);
        }
    }
}
