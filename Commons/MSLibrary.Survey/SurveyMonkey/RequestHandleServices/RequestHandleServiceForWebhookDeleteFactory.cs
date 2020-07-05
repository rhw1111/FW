using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookDeleteFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookDeleteFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForWebhookDelete _requestHandleServiceForWebhookDelete;

        public RequestHandleServiceForWebhookDeleteFactory(RequestHandleServiceForWebhookDelete requestHandleServiceForWebhookDelete)
        {
            _requestHandleServiceForWebhookDelete = requestHandleServiceForWebhookDelete;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForWebhookDelete;
        }
    }
}
