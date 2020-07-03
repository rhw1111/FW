using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Survey.SurveyMonkey.HttpAuthHandleServices
{
    [Injection(InterfaceType = typeof(SurveyMonkeyHttpAuthHandleServiceForOAuthFactory), Scope = InjectionScope.Singleton)]
    public class SurveyMonkeyHttpAuthHandleServiceForOAuthFactory : IFactory<ISurveyMonkeyHttpAuthHandleService>
    {
        private readonly SurveyMonkeyHttpAuthHandleServiceForOAuth _surveyMonkeyHttpAuthHandleServiceForOAuth;

        public SurveyMonkeyHttpAuthHandleServiceForOAuthFactory(SurveyMonkeyHttpAuthHandleServiceForOAuth surveyMonkeyHttpAuthHandleServiceForOAuth)
        {
            _surveyMonkeyHttpAuthHandleServiceForOAuth = surveyMonkeyHttpAuthHandleServiceForOAuth;
        }
        public ISurveyMonkeyHttpAuthHandleService Create()
        {
            return _surveyMonkeyHttpAuthHandleServiceForOAuth;
        }
    }
}
