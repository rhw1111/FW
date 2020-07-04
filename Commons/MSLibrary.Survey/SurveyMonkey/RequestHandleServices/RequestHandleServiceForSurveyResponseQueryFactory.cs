using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyResponseQueryFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyResponseQueryFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForSurveyResponseQuery _requestHandleServiceForSurveyResponseQuery;

        public RequestHandleServiceForSurveyResponseQueryFactory(RequestHandleServiceForSurveyResponseQuery requestHandleServiceForSurveyResponseQuery)
        {
            _requestHandleServiceForSurveyResponseQuery = requestHandleServiceForSurveyResponseQuery;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForSurveyResponseQuery;
        }
    }
}
