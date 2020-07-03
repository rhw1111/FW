using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Template;
using Microsoft.Azure.Amqp.Framing;

namespace MSLibrary.Survey.SurveyMonkey
{
    public class SurveyMonkeyEndpoint : EntityBase<ISurveyMonkeyEndpointIMP>
    {
        private static IFactory<ISurveyMonkeyEndpointIMP>? _surveyMonkeyEndpointIMPFactory;

        public static IFactory<ISurveyMonkeyEndpointIMP>? SurveyMonkeyEndpointIMPFactory
        {
            set
            {
                _surveyMonkeyEndpointIMPFactory = value;
            }
        }

        public override IFactory<ISurveyMonkeyEndpointIMP>? GetIMPFactory()
        {
            return _surveyMonkeyEndpointIMPFactory;
        }

        /// <summary>
        /// Id
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
        /// 服务地址
        /// </summary>
        public string Address
        {
            get
            {

                return GetAttribute<string>(nameof(Address));
            }
            set
            {
                SetAttribute<string>(nameof(Address), value);
            }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Vesion
        {
            get
            {

                return GetAttribute<string>(nameof(Vesion));
            }
            set
            {
                SetAttribute<string>(nameof(Vesion), value);
            }
        }

        /// <summary>
        /// 类型
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
        /// 配置
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
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<SurveyMonkeyResponse> Execute(SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            return await _imp.Execute(this, request, cancellationToken);
        }
    }

    public interface ISurveyMonkeyEndpointIMP
    {
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SurveyMonkeyResponse> Execute(SurveyMonkeyEndpoint endpoint, SurveyMonkeyRequest request, CancellationToken cancellationToken = default);
    }

   
    /// <summary>
    /// Http鉴权处理服务
    /// 为HttpClient补全鉴权信息
    /// </summary>
    public interface ISurveyMonkeyHttpAuthHandleService
    {
        Task Handle(HttpClient httpClient, string address, string configuration, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 请求处理服务
    /// </summary>
    public interface ISurveyMonkeyRequestHandleService
    {
        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="authHandler"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SurveyMonkeyResponse> Execute(Func<HttpClient,Task> authHandler,string type,string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(ISurveyMonkeyEndpointIMP), Scope = InjectionScope.Transient)]
    public class SurveyMonkeyEndpointIMP : ISurveyMonkeyEndpointIMP
    {
        /// <summary>
        /// 文本替换服务
        /// 如果该属性赋值，则configuration中的内容将首先使用该服务来替换占位符
        /// </summary>
        public static ITextReplaceService? TextReplaceService { set; get; }

        /// <summary>
        /// Http鉴权处理服务工厂键值对
        /// 键为AuthType
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyMonkeyHttpAuthHandleService>> SurveyMonkeyHttpAuthHandleServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyMonkeyHttpAuthHandleService>>();
        /// <summary>
        /// 请求处理服务工厂键值对
        /// 键为SurveyMonkeyRequest.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyMonkeyRequestHandleService>> SurveyMonkeyRequestHandleServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyMonkeyRequestHandleService>>();
       
        public async Task<SurveyMonkeyResponse> Execute(SurveyMonkeyEndpoint endpoint, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            if (SurveyMonkeyHttpAuthHandleServiceFactories.TryGetValue(endpoint.Type,out IFactory<ISurveyMonkeyHttpAuthHandleService>? authServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyMonkeyHttpAuthHandleServiceByType,
                    DefaultFormatting = "找不到类型为{0}的SurveyMonkey的Http鉴权处理服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type,$"{this.GetType().FullName}.SurveyMonkeyHttpAuthHandleServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyMonkeyHttpAuthHandleServiceByType, fragment, 1, 0);
            }

            if (SurveyMonkeyRequestHandleServiceFactories.TryGetValue(request.Type, out IFactory<ISurveyMonkeyRequestHandleService>? requestServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyMonkeyRequestHandleServiceByType,
                    DefaultFormatting = "找不到类型为{0}的SurveyMonkey的请求处理服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { request.Type, $"{this.GetType().FullName}.SurveyMonkeyRequestHandleServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyMonkeyRequestHandleServiceByType, fragment, 1, 0);
            }

            request.Address = endpoint.Address;
            request.Version = endpoint.Vesion;
            return await requestServiceFactory.Create().Execute(
                async(httpClinet)=>
                {
                    await authServiceFactory.Create().Handle(httpClinet, endpoint.Address, await getContent(endpoint.Configuration), cancellationToken);
                }
                ,
                endpoint.Type
                ,
                endpoint.Configuration
                ,
                request
                ,
                cancellationToken
                );
        }

        private async Task<string> getContent(string content)
        {
            if (TextReplaceService != null)
            {
                return await TextReplaceService.Replace(content);
            }
            else
            {
                return content;
            }
        }
    }
}
