using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.RequestHandleServices
{
    [Injection(InterfaceType = typeof(RequestHandleServiceForSurveyMessageQueryFactory), Scope = InjectionScope.Singleton)]
    public class RequestHandleServiceForSurveyMessageQueryFactory: IFactory<ISurveyMonkeyRequestHandleService>
    {
        private RequestHandleServiceForSurveyMessageQuery _requestHandleServiceForSurveyMessageQuery;

        public RequestHandleServiceForSurveyMessageQueryFactory(RequestHandleServiceForSurveyMessageQuery requestHandleServiceForSurveyMessageQuery)
        {
            _requestHandleServiceForSurveyMessageQuery = requestHandleServiceForSurveyMessageQuery;
        }
        public ISurveyMonkeyRequestHandleService Create()
        {
            return _requestHandleServiceForSurveyMessageQuery;
        }
    }
}
