using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Survey
{
    [Injection(InterfaceType = typeof(ISurveyHttpCallbackService), Scope = InjectionScope.Singleton)]
    public class SurveyHttpCallbackService : ISurveyHttpCallbackService
    {
        public async Task Execute(string surveyType, HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            var resolveServcice=SurveyHttpCallbackExtensionCollection.GetSurveyHttpCallbackResolveService(surveyType);
            var resolveData=await resolveServcice.Resolve(httpContext, cancellationToken);

            var handleService=SurveyHttpCallbackExtensionCollection.GetSurveyHttpCallbackHandleService($"{surveyType}-{ resolveData.Type}");

            await handleService.Execute(resolveData, cancellationToken);
        }
    }
}
