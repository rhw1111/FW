using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForWebhookRegisterFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForWebhookRegisterFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForWebhookRegister _requestHandleServiceForWebhookRegister;

        public RequestHandleServiceForWebhookRegisterFactory(RequestHandleServiceForWebhookRegister requestHandleServiceForWebhookRegister)
        {
            _requestHandleServiceForWebhookRegister = requestHandleServiceForWebhookRegister;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForWebhookRegister;
        }
    }
}
