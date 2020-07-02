using System;

namespace MSLibrary.Survey
{
    public enum SurveyErrorCodes
    {
        /// <summary>
        /// 找不到指定类型的SurveyMonkey的Http鉴权处理服务
        /// </summary>
        NotFoundSurveyMonkeyHttpAuthHandleServiceByType = 315000301,
        /// <summary>
        /// 找不到指定类型的SurveyMonkey的请求处理服务
        /// </summary>
        NotFoundSurveyMonkeyRequestHandleServiceByType = 315000302,
        /// <summary>
        /// SurveyMonkey鉴权错误
        /// </summary>
        SurveyMonkeyAuthError = 315000310,
    }
}
