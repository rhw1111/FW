using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm.Message.FileAttributeDownloadChunking;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForFileAttributeDownloadChunking), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForFileAttributeDownloadChunking : ICrmMessageHandle
    {
        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmFileAttributeDownloadChunkingRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmFileAttributeDownloadChunkingRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmFileAttributeDownloadChunkingRequestMessage;

            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.EntityName.ToPlural()}({realRequest.EntityId.ToString()})/{realRequest.AttributeName}/$value";
            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["range"] = new List<string> { $"bytes={realRequest.Start.ToString()}-{realRequest.End.ToString()}" };


            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method = HttpMethod.Get;
            result.Headers = headers;
            result.Body = string.Empty;
            result.Extension = realRequest;

            return await Task.FromResult(result);


        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            CrmFileAttributeDownloadChunkingResponseMessage response = new CrmFileAttributeDownloadChunkingResponseMessage();
            if (!responseHeaders.TryGetValue("x-ms-file-name", out IEnumerable<string> headerValues))
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
                    ReplaceParameters = new List<object>() { "FileAttributeDownloadChunking", "x-ms-file-name", headerKeys }
                };

                throw new UtilityException((int)Errors.NotFoundHeaderFromResponse, fragment);
            }

            var strFileName = headerValues.First();

            if (!responseHeaders.TryGetValue("x-ms-file-size", out headerValues))
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
                    ReplaceParameters = new List<object>() { "FileAttributeDownloadChunking", "x-ms-file-size", headerKeys }
                };

                throw new UtilityException((int)Errors.NotFoundHeaderFromResponse, fragment);
            }

            var strFileSize = headerValues.First();
            response.Data =await responseMessage.Content.ReadAsByteArrayAsync();
            response.FileName = strFileName;
            response.Total = System.Convert.ToInt32(strFileSize);

            return response;
        }
    }
}
