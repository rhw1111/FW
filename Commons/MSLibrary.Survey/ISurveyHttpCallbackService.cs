using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Survey
{
    /// <summary>
    /// Survey的Http回调服务
    /// 所有Survey的Http回调服务都通过该服务处理
    /// </summary>
    public interface ISurveyHttpCallbackService
    {
        Task Execute(string surveyType,HttpContext httpContext, CancellationToken cancellationToken = default);
    }
}
