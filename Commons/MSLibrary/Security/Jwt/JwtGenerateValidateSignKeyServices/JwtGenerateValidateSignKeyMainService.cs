using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.Jwt.JwtGenerateValidateSignKeyServices
{
    [Injection(InterfaceType = typeof(IJwtGenerateValidateSignKeyService), Scope = InjectionScope.Singleton)]
    public class JwtGenerateValidateSignKeyMainService : IJwtGenerateValidateSignKeyService
    {
        private static Dictionary<string, IFactory<IJwtGenerateValidateSignKeyService>> _jwtGenerateValidateSignKeyServiceFactories = new Dictionary<string, IFactory<IJwtGenerateValidateSignKeyService>>();

        public static Dictionary<string, IFactory<IJwtGenerateValidateSignKeyService>> JwtGenerateValidateSignKeyServiceFactories
        {
            get 
            {
                return _jwtGenerateValidateSignKeyServiceFactories;
            }
        }
        public async Task<IEnumerable<SecurityKey>> Generate(JwtEnpoint endpoint, string type, string configuration)
        {
            var service = getService(type);
            return await service.Generate(endpoint, type, configuration);
        }


        private IJwtGenerateValidateSignKeyService getService(string type)
        {
            if (!_jwtGenerateValidateSignKeyServiceFactories.TryGetValue(type, out IFactory<IJwtGenerateValidateSignKeyService> factory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundJwtGenerateValidateSignKeyServiceByType,
                    DefaultFormatting = "找不到类型为{0}的生成Jwt验证时使用的签名密钥服务",
                    ReplaceParameters = new List<object>() { type }
                };

                throw new UtilityException((int)Errors.NotFoundJwtGenerateValidateSignKeyServiceByType, fragment);
            }

            return factory.Create();
        }
    }
}
