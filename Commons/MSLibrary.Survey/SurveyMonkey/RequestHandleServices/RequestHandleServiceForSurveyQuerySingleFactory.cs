using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyQuerySingleFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyQuerySingleFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForSurveyQuerySingle _requestHandleServiceForSurveyQuerySingle;

        public RequestHandleServiceForSurveyQuerySingleFactory(RequestHandleServiceForSurveyQuerySingle requestHandleServiceForSurveyQuerySingle)
        {
            _requestHandleServiceForSurveyQuerySingle = requestHandleServiceForSurveyQuerySingle;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForSurveyQuerySingle;
        }
    }
}
