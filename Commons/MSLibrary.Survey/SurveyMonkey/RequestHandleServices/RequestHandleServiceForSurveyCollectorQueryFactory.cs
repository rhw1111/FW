using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;


namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyCollectorQueryFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyCollectorQueryFactory : IFactory<ISurveyMonkeyRequestHandleService>
    {
        private RequestHandleServiceForSurveyCollectorQuery _requestHandleServiceForSurveyCollectorQuery;

        public RequestHandleServiceForSurveyCollectorQueryFactory(RequestHandleServiceForSurveyCollectorQuery requestHandleServiceForSurveyCollectorQuery)
        {
            _requestHandleServiceForSurveyCollectorQuery = requestHandleServiceForSurveyCollectorQuery;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForSurveyCollectorQuery;
        }
    }
}
