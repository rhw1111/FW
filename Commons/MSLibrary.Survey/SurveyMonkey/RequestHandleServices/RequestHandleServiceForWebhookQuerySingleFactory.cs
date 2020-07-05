using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookQuerySingleFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookQuerySingleFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForWebhookQuerySingle _requestHandleServiceForWebhookQuerySingle;

        public RequestHandleServiceForWebhookQuerySingleFactory(RequestHandleServiceForWebhookQuerySingle requestHandleServiceForWebhookQuerySingle)
        {
            _requestHandleServiceForWebhookQuerySingle = requestHandleServiceForWebhookQuerySingle;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForWebhookQuerySingle;
        }
    }
}
