using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;
using MSLibrary.Security.Jwt.DAL;

namespace MSLibrary.Security.Jwt
{
    /// <summary>
    /// 处理Jwt的终结点
    /// </summary>
    public class JwtEnpoint : EntityBase<IJwtEndpointIMP>
    {
        public const string Attribute_ID = "ID";
        public const string Attribute_Name = "Name";
        public const string Attribute_CreateSignKeyType = "CreateSignKeyType";
        public const string Attribute_CreateSignKeyTypeConfiguration = "CreateSignKeyTypeConfiguration";
        public const string Attribute_ValidateSignKeyType = "ValidateSignKeyType";
        public const string Attribute_ValidateSignKeyTypeConfiguration = "ValidateSignKeyTypeConfiguration";
        public const string Attribute_CreateTime = "CreateTime";
        public const string Attribute_ModifyTime = "ModifyTime";

        private static IFactory<IJwtEndpointIMP> _jwtEndpointIMPFactory;
        public static IFactory<IJwtEndpointIMP> JwtEndpointIMPFactory
        {
            set
            {
                _jwtEndpointIMPFactory = value;
            }
        }
        public override IFactory<IJwtEndpointIMP> GetIMPFactory()
        {
            return _jwtEndpointIMPFactory;
        }



        /// <summary>
        /// ID
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(Attribute_ID);
            }
            set
            {
                SetAttribute<Guid>(Attribute_ID, value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {

                return GetAttribute<string>(Attribute_Name);
            }
            set
            {
                SetAttribute<string>(Attribute_Name, value);
            }
        }


        /// <summary>
        /// 生成时使用的签名类型
        /// </summary>
        public string CreateSignKeyType
        {
            get
            {

                return GetAttribute<string>(Attribute_CreateSignKeyType);
            }
            set
            {
                SetAttribute<string>(Attribute_CreateSignKeyType, value);
            }
        }

        /// <summary>
        /// 生成时使用的签名类型的对应配置
        /// </summary>
        public string CreateSignKeyConfiguration
        {
            get
            {

                return GetAttribute<string>(Attribute_CreateSignKeyTypeConfiguration);
            }
            set
            {
                SetAttribute<string>(Attribute_CreateSignKeyTypeConfiguration, value);
            }
        }

        /// <summary>
        /// 验证时使用的签名类型
        /// </summary>
        public string ValidateSignKeyType
        {
            get
            {

                return GetAttribute<string>(Attribute_ValidateSignKeyType);
            }
            set
            {
                SetAttribute<string>(Attribute_ValidateSignKeyType, value);
            }
        }

        /// <summary>
        /// 验证时使用的签名类型的对应配置
        /// </summary>
        public string ValidateSignKeyConfiguration
        {
            get
            {

                return GetAttribute<string>(Attribute_ValidateSignKeyTypeConfiguration);
            }
            set
            {
                SetAttribute<string>(Attribute_ValidateSignKeyTypeConfiguration, value);
            }
        }



        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(Attribute_CreateTime);
            }
            set
            {
                SetAttribute<DateTime>(Attribute_CreateTime, value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(Attribute_ModifyTime);
            }
            set
            {
                SetAttribute<DateTime>(Attribute_ModifyTime, value);
            }
        }

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Update()
        {
            await _imp.Update(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        public async Task<ClaimsPrincipal> ValidateJwt(string token, IList<JwtValidateParameter> validateParameters)
        {
            return await _imp.ValidateJwt(this, token, validateParameters);
        }


        public async Task<string> CreateJwt(string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt)
        {
            return await _imp.CreateJwt(this, issuer, audience, subject, notBefore, expires, issuedAt);
        }


    }

    public interface IJwtEndpointIMP
    {
        Task Add(JwtEnpoint endpoint);
        Task Update(JwtEnpoint endpoint);
        Task Delete(JwtEnpoint endpoint);

        Task<ClaimsPrincipal> ValidateJwt(JwtEnpoint endpoint, string token, IList<JwtValidateParameter> validateParameters);

        Task<string> CreateJwt(JwtEnpoint endpoint, string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt);

    }


    [Injection(InterfaceType = typeof(IJwtEndpointIMP), Scope = InjectionScope.Transient)]
    public class JwtEndpointIMP : IJwtEndpointIMP
    {
        private IJwtGenerateCreateSignKeyService _jwtGenerateCreateSignKeyService;
        private IJwtGenerateValidateSignKeyService _jwtGenerateValidateSignKeyService;
        private IJwtValidateParameterBuildService _jwtValidateParameterBuildService;
        private IJwtEnpointStore _jwtEnpointStore;

        public JwtEndpointIMP(IJwtGenerateCreateSignKeyService jwtGenerateCreateSignKeyService, IJwtGenerateValidateSignKeyService jwtGenerateValidateSignKeyService, IJwtValidateParameterBuildService jwtValidateParameterBuildService, IJwtEnpointStore jwtEnpointStore)
        {
            _jwtGenerateCreateSignKeyService = jwtGenerateCreateSignKeyService;
            _jwtGenerateValidateSignKeyService = jwtGenerateValidateSignKeyService;
            _jwtValidateParameterBuildService = jwtValidateParameterBuildService;
            _jwtEnpointStore = jwtEnpointStore;
        }

        public async Task Add(JwtEnpoint endpoint)
        {
            await _jwtEnpointStore.Add(endpoint);
        }

        public async Task<string> CreateJwt(JwtEnpoint endpoint, string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt)
        {
            //生成生成时用到的签名密钥
            var signingCredentials = await _jwtGenerateCreateSignKeyService.Generate(endpoint, endpoint.CreateSignKeyType, endpoint.CreateSignKeyConfiguration);
            //生成Jwt字符串
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.CreateEncodedJwt(issuer, audience, subject, notBefore, expires, issuedAt, signingCredentials);
        }

        public async Task Delete(JwtEnpoint endpoint)
        {
            await _jwtEnpointStore.Delete(endpoint.ID);
        }

        public async Task Update(JwtEnpoint endpoint)
        {
            await _jwtEnpointStore.Updtae(endpoint);
        }

        public async Task<ClaimsPrincipal> ValidateJwt(JwtEnpoint endpoint, string token, IList<JwtValidateParameter> validateParameters)
        {
            //生成验证时使用的签名密钥
            var keys=await _jwtGenerateValidateSignKeyService.Generate(endpoint, endpoint.ValidateSignKeyType, endpoint.ValidateSignKeyConfiguration);

            //为验证参数列表组装令牌验证参数
            TokenValidationParameters tokenParameter = new TokenValidationParameters();
            //初始化令牌验证参数
            tokenParameter.ValidateActor = false;
            tokenParameter.ValidateAudience = false;
            tokenParameter.ValidateIssuer = false;
            tokenParameter.ValidateLifetime = false;
            tokenParameter.ValidateTokenReplay = false;
        
            //为签名密钥属性赋值并启用
            tokenParameter.IssuerSigningKeys = keys;

            if (validateParameters != null)
            {
                foreach (var parameterItem in validateParameters)
                {
                    await _jwtValidateParameterBuildService.Build(tokenParameter, parameterItem);
                }
            }

            //执行验证
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var claims = handler.ValidateToken(token, tokenParameter, out SecurityToken validatedToken);
            
            return claims;
        }
    }

    /// <summary>
    /// Jwt验证参数
    /// </summary>
    public class JwtValidateParameter
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 生成Jwt验证时使用的签名服务
    /// </summary>
    public interface IJwtGenerateValidateSignKeyService
    {
        Task<IEnumerable<SecurityKey>> Generate(JwtEnpoint endpoint, string type, string configuration);
    }

    /// <summary>
    /// 生成Jwt生成时使用的签名服务
    /// </summary>
    public interface IJwtGenerateCreateSignKeyService
    {
        Task<SigningCredentials> Generate(JwtEnpoint endpoint, string type, string configuration);
    }

    /// <summary>
    /// Jwt验证参数组装服务
    /// 为tokenParameter组装
    /// </summary>
    public interface IJwtValidateParameterBuildService
    {
        Task Build(TokenValidationParameters tokenParameter, JwtValidateParameter parameter);
    }
}
