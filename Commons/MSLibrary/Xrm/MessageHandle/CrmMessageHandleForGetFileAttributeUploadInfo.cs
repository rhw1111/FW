using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm.Message.GetFileAttributeUploadInfo;

namespace MSLibrary.Xrm.MessageHandle
{

    [Injection(InterfaceType = typeof(CrmMessageHandleForGetFileAttributeUploadInfo), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForGetFileAttributeUploadInfo : ICrmMessageHandle
    {
        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmGetFileAttributeUploadInfoRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmGetFileAttributeUploadInfoRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }


            var realRequest = request as CrmGetFileAttributeUploadInfoRequestMessage;

 
            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.EntityName.ToPlural()}({realRequest.EntityId.ToString()})/{realRequest.AttributeName}";
            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["x-ms-transfer-mode"] = new List<string> { "chunked" };
            headers["x-ms-file-name"] = new List<string> { realRequest.FileName };


            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method = HttpMethod.Patch;
            result.Headers = headers;
            result.Body = string.Empty;
            result.Extension = realRequest;

            return await Task.FromResult(result);
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {

            CrmGetFileAttributeUploadInfoResponseMessage response = new CrmGetFileAttributeUploadInfoResponseMessage();
            if (!responseHeaders.TryGetValue("Location", out IEnumerable<string> headerValues))
            {
                var headerKeys = await responseHeaders.Keys.ToDisplayString(
                    async (key) =>
                    {
                        return await Task.FromResult(key);
                    },
                    async () =>
                    {
                        return await Task.FromResult(",");
                    }
                    );
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHeaderFromResponse,
                    DefaultFormatting = "在消息{0}的响应中，找不到名称为{1}的header，返回的header名称集合为{2}",
                    ReplaceParameters = new List<object>() { "GetFileAttributeUploadInfo", "Location", headerKeys }
                };

                throw new UtilityException((int)Errors.NotFoundHeaderFromResponse, fragment);
            }

            var strLocation = headerValues.First();

            if (!responseHeaders.TryGetValue("x-ms-chunk-size", out headerValues))
            {
                var headerKeys = await responseHeaders.Keys.ToDisplayString(
                    async (key) =>
                    {
                        return await Task.FromResult(key);
                    },
                    async () =>
                    {
                        return await Task.FromResult(",");
                    }
                    );
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHeaderFromResponse,
                    DefaultFormatting = "在消息{0}的响应中，找不到名称为{1}的header，返回的header名称集合为{2}",
                    ReplaceParameters = new List<object>() { "GetFileAttributeUploadInfo", "x-ms-chunk-size", headerKeys }
                };

                throw new UtilityException((int)Errors.NotFoundHeaderFromResponse, fragment);
            }

            var strPreSize = headerValues.First();

            response.UploadUrl = strLocation;
            response.PerSize = System.Convert.ToInt32(strPreSize);

            return response;
        }
    }
}
