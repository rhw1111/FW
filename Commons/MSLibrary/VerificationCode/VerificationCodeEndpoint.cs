using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm.Message.RetrieveCollectionAttributeSavedQuery;

namespace MSLibrary.VerificationCode
{
    /// <summary>
    /// 验证码终结点
    /// </summary>
    public class VerificationCodeEndpoint : EntityBase<IVerificationCodeEndpointIMP>
    {
        private static IFactory<IVerificationCodeEndpointIMP>? _verificationCodeEndpointIMPFactory;

        public static IFactory<IVerificationCodeEndpointIMP> VerificationCodeEndpointIMPFactory
        {
            set
            {
                _verificationCodeEndpointIMPFactory = value;
            }
        }

        public override IFactory<IVerificationCodeEndpointIMP>? GetIMPFactory()
        {
            return _verificationCodeEndpointIMPFactory;
        }

        /// <summary>
        /// id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 类型
        /// 用来决定实际使用的服务
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>(nameof(Type));
            }
            set
            {
                SetAttribute<string>(nameof(Type), value);
            }
        }

        /// <summary>
        /// 配置信息
        /// 供实际使用的服务使用
        /// 不同的Type有不同的配置信息
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>(nameof(Configuration));
            }
            set
            {
                SetAttribute<string>(nameof(Configuration), value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(DateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(DateTime), value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(DateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(DateTime), value);
            }
        }

    }

    public interface IVerificationCodeEndpointIMP
    {
        /// <summary>
        /// 生成指定标识的验证码
        /// 如果为null,表示验证码生成频率过快
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        Task<string?> GenerateCode(VerificationCodeEndpoint endpoint,string identity);
        /// <summary>
        /// 验证指定标识的验证码是否正确
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="identity"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> ValidateCode(VerificationCodeEndpoint endpoint, string identity, string code);
        /// <summary>
        /// 获取指定标识最后生成Code的时间
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        Task<DateTime?> GetLatestCodeTime(VerificationCodeEndpoint endpoint, string identity);
    }

    [Injection(InterfaceType = typeof(IVerificationCodeEndpointIMP), Scope = InjectionScope.Transient)]
    public class VerificationCodeEndpointIMP : IVerificationCodeEndpointIMP
    {
        public static IDictionary<string, IFactory<IVerificationCodeService>> VerificationCodeServiceFactories = new Dictionary<string, IFactory<IVerificationCodeService>>();

        public async Task<string?> GenerateCode(VerificationCodeEndpoint endpoint, string identity)
        {

            var service = getService(endpoint.Type);
            await service.DeleteCode(endpoint.Configuration, identity);
            var newCode=await service.GenerateCode(endpoint.Configuration);
            var saveResult=await service.SaveCode(endpoint.Configuration, identity, newCode);
            if (!saveResult)
            {
                return null;
            }
            return newCode;
        }

        public async Task<DateTime?> GetLatestCodeTime(VerificationCodeEndpoint endpoint, string identity)
        {
            var service = getService(endpoint.Type);
            return await service.GetLatestCodeTime(endpoint.Configuration, identity);
        }

        public async Task<bool> ValidateCode(VerificationCodeEndpoint endpoint, string identity, string code)
        {
            var service = getService(endpoint.Type);
            var latestCode = await service.GetLatestCode(endpoint.Configuration, identity);
            await service.DeleteCode(endpoint.Configuration, identity);
            if (latestCode==null || code != latestCode)
            {
                return false;
            }
            return true;
        }

        private IVerificationCodeService getService(string type)
        {
            if (!VerificationCodeServiceFactories.TryGetValue(type,out IFactory<IVerificationCodeService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundVerificationCodeServiceByType,
                    DefaultFormatting = "找不到类型为{0}的验证码服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, $"{ this.GetType().FullName }.VerificationCodeServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundVerificationCodeServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }

    public interface IVerificationCodeService
    {
        /// <summary>
        /// 存储Code
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="identity"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> SaveCode(string configuration,string identity,string code);
        /// <summary>
        /// 删除指定标识的Code
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        Task DeleteCode(string configuration,string identity);
        /// <summary>
        /// 生成Code
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<string> GenerateCode(string configuration);
        /// <summary>
        /// 获取指定标识最后生成Code的时间
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        Task<DateTime?> GetLatestCodeTime(string configuration, string identity);
        /// <summary>
        /// 获取指定标识最后生成的Code
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        Task<string?> GetLatestCode(string configuration, string identity);
    }
}
