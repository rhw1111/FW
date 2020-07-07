using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Survey.SurveyMonkey.Message;
using MSLibrary.Security;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookCallback), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookCallback : ISurveyMonkeyRequestHandleService
    {
        public static IDictionary<string, IFactory<IWebhookCallbackValidationService>> WebhookCallbackValidationServiceFactories { get; } = new Dictionary<string, IFactory<IWebhookCallbackValidationService>>()
        {
            { SurveyMonkeyTypes.OAuth,DIContainerContainer.Get<WebhookCallbackValidationServiceDefalutFactory>()}
        };
        
        public RequestHandleServiceForWebhookCallback()
        {

        }

        public async Task<SurveyMonkeyResponse> Execute(Func<HttpClient, Task> authHandler, Func<SurveyMonkeyRequest, Task<SurveyMonkeyResponse>> requestHandler, string type, string configuration, SurveyMonkeyRequest request, CancellationToken cancellationToken = default)
        {
            if (!WebhookCallbackValidationServiceFactories.TryGetValue(type,out IFactory<IWebhookCallbackValidationService> validateServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyMonkeyWebhookCallbackValidationServiceByType,
                    DefaultFormatting = "找不到类型为{0}的SurveyMonkey的Webhook回调验证服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type,$"{typeof(RequestHandleServiceForWebhookCallback).FullName}.WebhookCallbackValidationServiceFactories" }
                };

                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyMonkeyWebhookCallbackValidationServiceByType, fragment, 1, 0);
            }

            var realRequest = (WebhookCallbackRequest)request;
            await validateServiceFactory.Create().Validate(realRequest, configuration, cancellationToken);

            var body = JsonSerializerHelper.Deserialize<Response>(realRequest.Body);

            return new WebhookCallbackResponse()
            {
                EventID = body.EventID,
                EventDatetime = body.EventDatetime,
                EventType = body.EventType,
                FilterID = body.FilterID,
                FilterType = body.FilterType,
                Name = body.Name,
                ObjectID = body.ObjectID,
                ObjectType = body.ObjectType,
                Resources = body.Resources
            };
        }

        private async Task<string> getResponseError(HttpResponseMessage response)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = response.ReasonPhrase;
            }
            return errorMessage;
        }


        [DataContract]
        private class Response
        {
            [DataMember(Name= "name")]
            public string Name { get; set; } = null!;
            [DataMember(Name = "filter_type")]
            public string FilterType { get; set; } = null!;
            [DataMember(Name = "filter_id")]
            public string FilterID { get; set; } = null!;
            [DataMember(Name = "event_type")]
            public string EventType { get; set; } = null!;
            [DataMember(Name = "event_id")]
            public string EventID { get; set; } = null!;
            [DataMember(Name = "object_type")]
            public string ObjectType { get; set; } = null!;
            [DataMember(Name = "object_id")]
            public string ObjectID { get; set; } = null!;
            [DataMember(Name = "event_datetime")]
            public DateTime EventDatetime { get; set; }
            [DataMember(Name = "resources")]
            public JObject Resources { get; set; } = null!;
        }
    }


    public interface IWebhookCallbackValidationService
    {
        Task Validate(WebhookCallbackRequest request,string configuration, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(WebhookCallbackValidationServiceDefalut), Scope = InjectionScope.Singleton)]
    public class WebhookCallbackValidationServiceDefalut : IWebhookCallbackValidationService
    {
        private readonly ISecurityService _securityService;

        public WebhookCallbackValidationServiceDefalut(ISecurityService securityService)
        {
            _securityService = securityService;
        }
        public async Task Validate(WebhookCallbackRequest request, string configuration, CancellationToken cancellationToken = default)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);
            //验证SmApikey
            if (!_securityService.VerifySignByKey(request.Body, request.SmSignature,$"{configurationObj.ClientID}&{configurationObj.ClientSecret}"))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.SurveyMonkeyWebhookCallbackValidateError,
                    DefaultFormatting = "SurveyMonkey的Webhook回调验证失败，错误信息为{0}",
                    ReplaceParameters = new List<object>() { "signature valiate fail" }
                };

                throw new UtilityException((int)SurveyErrorCodes.SurveyMonkeyWebhookCallbackValidateError, fragment, 1, 0);
            }

            await Task.CompletedTask;
        }

        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string ClientID { get; set; } = null!;
            [DataMember]
            public string ClientSecret { get; set; } = null!;
        }
    }

    [Injection(InterfaceType = typeof(WebhookCallbackValidationServiceDefalutFactory), Scope = InjectionScope.Singleton)]
    public class WebhookCallbackValidationServiceDefalutFactory : IFactory<IWebhookCallbackValidationService>
    {
        private readonly WebhookCallbackValidationServiceDefalut _webhookCallbackValidationServiceDefalut;

        public WebhookCallbackValidationServiceDefalutFactory(WebhookCallbackValidationServiceDefalut webhookCallbackValidationServiceDefalut)
        {
            _webhookCallbackValidationServiceDefalut = webhookCallbackValidationServiceDefalut;
        }
        public IWebhookCallbackValidationService Create()
        {
            return _webhookCallbackValidationServiceDefalut;
        }
    }
}
