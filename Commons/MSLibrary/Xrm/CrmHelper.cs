using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net;
using Newtonsoft.Json.Linq;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// Crm帮助
    /// </summary>
    public static class CrmHelper
    {
        /// <summary>
        /// weiapi的错误处理
        /// 默认使用CrmWebApiErrorDefaultHandler
        /// </summary>
        public static IHttpErrorHandler WebApiErrorHandler
        {
            get; set;
        } = new CrmWebApiErrorDefaultHandler();


        public static async Task RetryPost(int maxRetry,JObject request, string url, Dictionary<string,string> headers,Func<JObject,HttpResponseMessage, Task> action)
        {
            int retryNumber = 0;
            while (true)
            {
                HttpResult<JObject> resultObj = null;
                try
                {
                    resultObj = await HttpClinetHelper.PostWithResponseAsync<JObject, JObject>(request, url, headers, CrmHelper.WebApiErrorHandler);
                }
                catch(UtilityException ex) when(ex.Code==(int)Errors.CrmWebApiLimitError)
                {
                    if (retryNumber>=maxRetry)
                    {
                        throw;
                    }
                    await Task.Delay((TimeSpan)ex.Data[CrmWebApiResponseKeyNames.RetryAfter]);
                    //System.Threading.Thread.Sleep(((TimeSpan)ex.Data[CrmWebApiResponseKeyNames.RetryAfter]));
                    retryNumber++;
                    continue;
                }
                await action(resultObj.Value,resultObj.Response);
                break;
            }
            

        }

    }


    /// <summary>
    /// Crm调用WebApi的默认错误处理
    /// </summary>
    public class CrmWebApiErrorDefaultHandler : IHttpErrorHandler
    {
        public async Task<Exception> Do(HttpResponseMessage response)
        {
            var requestBody=await response.RequestMessage.Content.ReadAsStringAsync();
            var strContent = await response.Content.ReadAsStringAsync();

            var error = JsonSerializerHelper.Deserialize<CrmWebApiError>(strContent);
            UtilityException ex = null;
            TextFragment fragment;
            switch ((int)response.StatusCode)
            {
                case 412:
                    fragment = new TextFragment()
                    {
                        Code = TextCodes.CrmWebApiConcurrencyError,
                        DefaultFormatting = "调用Crm的webapi出现并发性错误，Uri:{0},Body：{1}，错误信息：{2}",
                        ReplaceParameters = new List<object>() { response.RequestMessage.RequestUri.ToString(), requestBody, strContent }
                    };


                    ex = new UtilityException((int)Errors.CrmWebApiConcurrencyError, fragment);
                    break;
                case 429:
                    fragment = new TextFragment()
                    {
                        Code = TextCodes.CrmWebApiLimitError,
                        DefaultFormatting = "调用Crm的webapi出现限制性错误，Uri:{0},Body：{1}，错误信息：{2}",
                        ReplaceParameters = new List<object>() { response.RequestMessage.RequestUri.ToString(), requestBody, strContent }
                    };

                    ex = new UtilityException((int)Errors.CrmWebApiLimitError,fragment);
                    ex.Data[CrmWebApiResponseKeyNames.RetryAfter] = response.Headers.RetryAfter.Delta.Value;
                    break;
                default:
                    fragment = new TextFragment()
                    {
                        Code = TextCodes.CrmWebApiCommonError,
                        DefaultFormatting = "调用Crm的webapi出现错误，Uri:{0},Body：{1}，错误信息：{2}",
                        ReplaceParameters = new List<object>() { response.RequestMessage.RequestUri.ToString(), requestBody, strContent }
                    };
                    ex = new UtilityException((int)Errors.CrmWebApiCommonError, fragment);
                    break;
            }

            return ex;
        }
    }

    [DataContract]
    public class CrmWebApiError
    {
        [DataMember(Name = "error")]
        public CrmWebApiErrorData Error { get; set; }
    }

    [DataContract]
    public class CrmWebApiErrorData
    {
        [DataMember(Name ="code")]
        public string Code { get; set; }
        [DataMember(Name ="message")]
        public string Message { get; set; }
        [DataMember(Name = "innererror")]
        public CrmWebApiInnerErrorData InnerError { get; set; }
    }

    [DataContract]
    public class CrmWebApiInnerErrorData
    {
        [DataMember(Name ="message")]
        public string Message { get; set; }
        [DataMember(Name ="type")]
        public string Type { get; set; }
        [DataMember(Name = "stacktrace")]
        public string StackTrace { get; set; }
    }
}
