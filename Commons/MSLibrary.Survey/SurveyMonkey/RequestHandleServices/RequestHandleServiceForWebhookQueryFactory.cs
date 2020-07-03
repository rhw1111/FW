using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookQueryFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookQueryFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForWebhookQuery _requestHandleServiceForWebhookQuery;

        public RequestHandleServiceForWebhookQueryFactory(RequestHandleServiceForWebhookQuery requestHandleServiceForWebhookQuery)
        {
            _requestHandleServiceForWebhookQuery = requestHandleServiceForWebhookQuery;
        }

        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForWebhookQuery;
        }
    }
}
