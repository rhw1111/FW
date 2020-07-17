using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyQueryFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyQueryFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private readonly RequestHandleServiceForSurveyQuery _requestHandleServiceForSurveyQuery;

        public RequestHandleServiceForSurveyQueryFactory(RequestHandleServiceForSurveyQuery requestHandleServiceForSurveyQuery)
        {
            _requestHandleServiceForSurveyQuery = requestHandleServiceForSurveyQuery;
        }

        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForSurveyQuery;
        }
    }
}
