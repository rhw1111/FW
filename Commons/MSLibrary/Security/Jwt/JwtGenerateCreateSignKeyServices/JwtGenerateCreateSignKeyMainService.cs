using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.Jwt.JwtGenerateCreateSignKeyServices
{
    [Injection(InterfaceType = typeof(IJwtGenerateCreateSignKeyService), Scope = InjectionScope.Singleton)]
    public class JwtGenerateCreateSignKeyMainService : IJwtGenerateCreateSignKeyService
    {
        private static Dictionary<string, IFactory<IJwtGenerateCreateSignKeyService>> _jwtGenerateCreateSignKeyServiceFactories = new Dictionary<string, IFactory<IJwtGenerateCreateSignKeyService>>();

        /// <summary>
        /// jwt生成生成时使用的签名服务工厂键值对
        /// </summary>
        public static Dictionary<string, IFactory<IJwtGenerateCreateSignKeyService>> JwtGenerateCreateSignKeyServiceFactories
        {
            get
            {
                return _jwtGenerateCreateSignKeyServiceFactories;
            }
        }
        public async Task<SigningCredentials> Generate(JwtEnpoint endpoint, string type, string configuration)
        {
            var service= getService(type);
            return await service.Generate(endpoint, type, configuration);
        }

        private IJwtGenerateCreateSignKeyService getService(string type)
        {
            if (!_jwtGenerateCreateSignKeyServiceFactories.TryGetValue(type,out IFactory<IJwtGenerateCreateSignKeyService> factory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundJwtGenerateCreateSignKeyServiceByType,
                    DefaultFormatting = "找不到类型为{0}的生成Jwt生成时使用的签名密钥服务",
                    ReplaceParameters = new List<object>() { type }
                };

                throw new UtilityException((int)Errors.NotFoundJwtGenerateCreateSignKeyServiceByType, fragment);
            }

            return factory.Create();
        }
    }
}
