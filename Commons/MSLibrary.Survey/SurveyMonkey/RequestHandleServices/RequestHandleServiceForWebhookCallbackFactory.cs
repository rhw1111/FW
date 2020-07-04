using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookCallbackFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookCallbackFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForWebhookCallback _requestHandleServiceForWebhookCallback;

        public RequestHandleServiceForWebhookCallbackFactory(RequestHandleServiceForWebhookCallback requestHandleServiceForWebhookCallback)
        {
            _requestHandleServiceForWebhookCallback = requestHandleServiceForWebhookCallback;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForWebhookCallback;
        }
    }
}
