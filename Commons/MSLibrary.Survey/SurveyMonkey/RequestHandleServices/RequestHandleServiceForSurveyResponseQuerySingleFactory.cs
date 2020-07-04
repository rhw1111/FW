using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyResponseQuerySingleFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyResponseQuerySingleFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForSurveyResponseQuerySingle _requestHandleServiceForSurveyResponseQuery;

        public RequestHandleServiceForSurveyResponseQuerySingleFactory(RequestHandleServiceForSurveyResponseQuerySingle requestHandleServiceForSurveyResponseQuery)
        {
            _requestHandleServiceForSurveyResponseQuery = requestHandleServiceForSurveyResponseQuery;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForSurveyResponseQuery;
        }
    }
}
