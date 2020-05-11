using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.Jwt.JwtValidateParameterBuildServices
{
    [Injection(InterfaceType = typeof(IJwtValidateParameterBuildService), Scope = InjectionScope.Singleton)]
    public class JwtValidateParameterBuildMainService : IJwtValidateParameterBuildService
    {
        private static Dictionary<string, IFactory<IJwtValidateParameterBuildService>> _jwtValidateParameterBuildServiceFactories = new Dictionary<string, IFactory<IJwtValidateParameterBuildService>>();

        public static Dictionary<string, IFactory<IJwtValidateParameterBuildService>> JwtValidateParameterBuildServiceFactories
        {
            get
            {
                return _jwtValidateParameterBuildServiceFactories;
            }
        }
        public async Task Build(TokenValidationParameters tokenParameter, JwtValidateParameter parameter)
        {
            var service = getService(parameter.Type);
            await service.Build(tokenParameter,parameter);
        }


        private IJwtValidateParameterBuildService getService(string type)
        {
            if (!_jwtValidateParameterBuildServiceFactories.TryGetValue(type, out IFactory<IJwtValidateParameterBuildService> factory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundjwtValidateParameterBuildServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Jwt验证参数组装服务",
                    ReplaceParameters = new List<object>() { type }
                };

                throw new UtilityException((int)Errors.NotFoundjwtValidateParameterBuildServiceByType, fragment);
            }

            return factory.Create();
        }
    }
}
