using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm.Message.Batch;
using MSLibrary.Xrm.Convert;
using MSLibrary.Serializer;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;


namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForBatch), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForBatch : ICrmMessageHandle
    {
        private ICrmMessageHandleSelector _crmMessageHandleSelector;

        public CrmMessageHandleForBatch(ICrmMessageHandleSelector crmMessageHandleSelector)
        {
            _crmMessageHandleSelector = crmMessageHandleSelector;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmBatchRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmBatchRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var batchBoundary = $"batch_{Guid.NewGuid().ToString()}";
            var changeSetBoundary = $"changeset_{Guid.NewGuid().ToString()}";
            var realRequest = request as CrmBatchRequestMessage;

            RequestHandleMap handleMap = new RequestHandleMap();

            string strBody = string.Empty;
            if (realRequest.ChangeSetMessages != null && realRequest.ChangeSetMessages.Count > 0)
            {
                strBody = $"--{batchBoundary}\n";
                strBody = $"{strBody}Content-Type: multipart/mixed;boundary={changeSetBoundary}\n\n";

                var changeSetIndex = 0;
                foreach (var batchItem in realRequest.ChangeSetMessages)
                {
                    batchItem.ApiVersion = realRequest.ApiVersion;
                    batchItem.OrganizationURI = realRequest.OrganizationURI;
                    changeSetIndex++;
                    strBody = $"{strBody}--{changeSetBoundary}\n";
                    strBody = $"{strBody}Content-Type: application/http\n";
                    strBody = $"{strBody}Content-Transfer-Encoding:binary\n";
                    strBody = $"{strBody}Content-ID:{changeSetIndex.ToString()}\n\n";


                    var messageHandle = _crmMessageHandleSelector.Choose(batchItem.GetType().FullName);
                    var handleResult = await messageHandle.ExecuteRequest(batchItem);
                    strBody = $"{strBody}{handleResult.Method.Method} {handleResult.Url} HTTP/1.1\n";
                    foreach (var headerItem in handleResult.Headers)
                    {
                        strBody = $"{strBody}{headerItem.Key}:{string.Join(";", headerItem.Value)}\n";
                    }

                    strBody = $"{strBody}\n";
                    if (handleResult.ReplaceHttpContent != null)
                    {
                        strBody = $"{strBody}{await handleResult.ReplaceHttpContent.ReadAsStringAsync()}\n";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(handleResult.Body))
                        {
                            strBody = $"{strBody}{handleResult.Body}\n";
                        }
                    }



                    handleMap.ChangeSetHandles.Add(new RequestHandleMapItem() { Request = batchItem, HandleResult = handleResult });


                }

                strBody = $"{strBody}\n--{changeSetBoundary}--\n\n";
            }

            if (realRequest.BatchMessages != null && realRequest.BatchMessages.Count > 0)
            {
                foreach (var batchItem in realRequest.BatchMessages)
                {
                    batchItem.ApiVersion = realRequest.ApiVersion;
                    batchItem.OrganizationURI = realRequest.OrganizationURI;

                    strBody = $"{strBody}--{batchBoundary}--\n";
                    strBody = $"{strBody}Content-Type: application/http\n";
                    strBody = $"{strBody}Content-Transfer-Encoding:binary\n\n";

                    var messageHandle = _crmMessageHandleSelector.Choose(batchItem.GetType().FullName);
                    var handleResult = await messageHandle.ExecuteRequest(batchItem);
                    strBody = $"{strBody}{handleResult.Method.Method} {handleResult.Url} HTTP/1.1\n";
                    foreach (var headerItem in handleResult.Headers)
                    {
                        strBody = $"{strBody}{headerItem.Key}:{string.Join(";", headerItem.Value)}\n";
                    }
                    strBody = $"{strBody}\n";

                    handleMap.ChangeSetHandles.Add(new RequestHandleMapItem() { Request = batchItem, HandleResult = handleResult });
                }
            }

            if ((realRequest.BatchMessages != null && realRequest.BatchMessages.Count > 0) || (realRequest.ChangeSetMessages != null && realRequest.ChangeSetMessages.Count > 0))
            {
                strBody = $"{strBody}--{batchBoundary}--";
            }


            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/$batch";
            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["OData-MaxVersion"] = new List<string> { "4.0" };
            headers["OData-Version"] = new List<string> { "4.0" };
            headers["Content-Type"] = new List<string> { "multipart/mixed" };
            headers["Content-Type-boundary"] = new List<string> { batchBoundary };

            headers["Accept"] = new List<string> { "application/json" };

            foreach (var itemHeader in realRequest.Headers)
            {
                headers[itemHeader.Key] = itemHeader.Value;
            }

            handleMap.BatchRequest = realRequest;
            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method = HttpMethod.Post;
            result.Headers = headers;
            result.Body = strBody;
            result.Extension = handleMap;

            return await Task.FromResult(result);
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            CrmBatchResponseMessage response = new CrmBatchResponseMessage()
            {
                BatchResponses = new List<CrmResponseMessage>(),
                ChangeSetResponses = new List<CrmResponseMessage>()
            };
            var handleMap = (RequestHandleMap)extension;

            if (string.IsNullOrEmpty(responseBody))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseIsEmpty,
                    DefaultFormatting = "Crm的Batch操作响应为空",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseIsEmpty, fragment);
            }
            //取第一行
            var arrayBody = responseBody.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            Regex regBatchSplit = new Regex("--batchresponse_([A-Za-z0-9-]+)", RegexOptions.IgnoreCase);
            var batchSplitMath = regBatchSplit.Match(arrayBody[0]);
            if (batchSplitMath == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundBatchCodeInCrmBatchResponse,
                    DefaultFormatting = "在Crm的Batch操作响应中找不到BatchCode，响应内容首行为{0}",
                    ReplaceParameters = new List<object>() { arrayBody[0] }
                };

                throw new UtilityException((int)Errors.NotFoundBatchCodeInCrmBatchResponse, fragment);
            }


            var batchCode = batchSplitMath.Groups[1].Value;

            string strBody = string.Empty;
            int index = 0;
            for (index = 0; index <= arrayBody.Length - 2; index++)
            {
                strBody = $"{ strBody}\r\n{arrayBody[index]}";
            }

            regBatchSplit = new Regex($"--batchresponse_{batchCode} *\r\n", RegexOptions.IgnoreCase);

            arrayBody = regBatchSplit.Split(strBody);

            index = 0;

            foreach (var responseItem in arrayBody)
            {
                if (string.IsNullOrWhiteSpace(responseItem))
                {
                    continue;
                }

                var batchType = GetBatchType(responseItem);

                if (batchType.Type == 0)
                {
                    var batchResponse = await ExecuteBatch(responseItem, index, handleMap.BatchHandles, responseMessage);
                    index++;
                    response.BatchResponses.Add(batchResponse);
                }
                else
                {
                    var changeSetResponses = await ExecuteChangeSet(responseItem, (string)batchType.Exension, handleMap.ChangeSetHandles, responseMessage);
                    response.ChangeSetResponses.AddRange(changeSetResponses);
                }
            }

            return response;
        }


        private BatchTypeInfo GetBatchType(string body)
        {
            BatchTypeInfo result = new BatchTypeInfo();

            var contentArray = body.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            var splitIndex = contentArray[0].IndexOf(":");
            if (splitIndex <= 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "Not correct header format" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }

            if (contentArray[0].Length - splitIndex - 1 < 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "Not correct header format" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }

            var headerName = contentArray[0].Substring(0, splitIndex).Trim();
            var headerValue = contentArray[0].Substring(splitIndex + 1, contentArray[0].Length - splitIndex - 1).Trim();

            if (headerName.ToLower() != "content-type")
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "First line need content-type" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }

            var arrayValue = headerValue.Split(';');
            if (arrayValue[0].Trim().ToLower() != "multipart/mixed")
            {
                result.Type = 0;
            }
            else
            {
                result.Type = 1;
                if (arrayValue.Length < 2)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.CrmBatchResponseItemFormatError,
                        DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                        ReplaceParameters = new List<object>() { body, "First line need content-type" }
                    };

                    throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
                }

                var arrayBoundary = arrayValue[1].Split('=');
                if (arrayBoundary.Length != 2)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.CrmBatchResponseItemFormatError,
                        DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                        ReplaceParameters = new List<object>() { body, "Miss changeset boundary" }
                    };

                    throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
                }

                result.Exension = arrayBoundary[1].Trim();
            }

            return result;
        }



        private MessageItemResponseInfo GetResponseInfo(string body)
        {
            Dictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>();
            MessageItemResponseInfo result = new MessageItemResponseInfo();
            result.Headers = headers;
            result.Body = string.Empty;

            var httpLabelIndex = body.ToLower().IndexOf("http/1.1");
            if (httpLabelIndex == -1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "miss http/1.1 label" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }



            var startIndex = body.IndexOf("\r\n", httpLabelIndex);

            if (startIndex == -1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "miss header item" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }


            if (startIndex - httpLabelIndex <= 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "miss http info" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }

            var strHttpInfo = body.Substring(httpLabelIndex, startIndex - httpLabelIndex);

            Regex regHttpInfo = new Regex("http/1.1 +([0-9\\.]+) +", RegexOptions.IgnoreCase);
            var httpInfoMatch = regHttpInfo.Match(strHttpInfo);
            if (httpInfoMatch == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "miss http status code" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }

            var strStatusCode = httpInfoMatch.Groups[1].Value.Trim();
            if (strStatusCode.Contains("."))
            {
                result.StatusCode = (int)float.Parse(httpInfoMatch.Groups[1].Value.Trim());
            }
            else
            {
                result.StatusCode = int.Parse(httpInfoMatch.Groups[1].Value.Trim());
            }


            var endIndex = body.IndexOf("\r\n\r\n", startIndex + 2);
            if (endIndex == -1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "miss header item" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }

            if (endIndex - startIndex - 2 <= 0)
            {
                return result;
            }


            var strHeaders = body.Substring(startIndex + 2, endIndex - startIndex - 2);

            var arrayHeaders = strHeaders.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var headerItem in arrayHeaders)
            {
                if (string.IsNullOrWhiteSpace(headerItem))
                {
                    continue;
                }

                var splitIndex = headerItem.IndexOf(":");
                if (splitIndex <= 0)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.CrmBatchResponseItemFormatError,
                        DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                        ReplaceParameters = new List<object>() { body, "Not correct header format" }
                    };

                    throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
                }
                if (headerItem.Length - splitIndex - 1 < 0)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.CrmBatchResponseItemFormatError,
                        DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                        ReplaceParameters = new List<object>() { body, "Not correct header format" }
                    };

                    throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
                }

                var headerName = headerItem.Substring(0, splitIndex).Trim();
                var headerValue = headerItem.Substring(splitIndex + 1, headerItem.Length - splitIndex - 1).Trim();

                List<string> headerValueList = new List<string>();
                var arrayHeaderValue = headerValue.Split(';');
                foreach (var valueItem in arrayHeaderValue)
                {
                    headerValueList.Add(valueItem.Trim());
                }

                headers.Add(headerName, headerValueList);
            }

            if (endIndex + 4 >= body.Length - 1)
            {
                return result;
            }

            var strBody = body.Substring(endIndex + 4, body.Length - endIndex - 4);
            result.Body = strBody.Trim();
            return result;
        }

        private async Task<List<CrmResponseMessage>> ExecuteChangeSet(string body, string boundary, List<RequestHandleMapItem> handleResults, HttpResponseMessage responseMessage)
        {
            List<CrmResponseMessage> result = new List<CrmResponseMessage>();

            body = body.Replace($"--{boundary}--", "");

            var regBatchSplit = new Regex($"--{boundary} *\r\n");

            var arrayBody = regBatchSplit.Split(body);

            arrayBody = arrayBody.Skip(1).ToArray();

            int index = 0;

            foreach (var bodyItem in arrayBody)
            {
                if (string.IsNullOrWhiteSpace(bodyItem))
                {
                    continue;
                }

                var responseInfo = GetResponseInfo(bodyItem);

                if (index > handleResults.Count - 1)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.CrmBatchResponseItemFormatError,
                        DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                        ReplaceParameters = new List<object>() { body, "RequestHandleResults index out range" }
                    };

                    throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
                }

                var messageHandle = _crmMessageHandleSelector.Choose(handleResults[index].Request.GetType().FullName);

                var handleResponse = await messageHandle.ExecuteResponse(handleResults[index].HandleResult.Extension, handleResults[index].HandleResult.Url, handleResults[index].HandleResult.Body, responseInfo.StatusCode, responseInfo.Headers, responseInfo.Body, responseMessage);
                result.Add(handleResponse);
                index++;
            }

            return result;
        }

        private async Task<CrmResponseMessage> ExecuteBatch(string body, int index, List<RequestHandleMapItem> handleResults, HttpResponseMessage responseMessage)
        {
            var responseInfo = GetResponseInfo(body);

            if (index > handleResults.Count - 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmBatchResponseItemFormatError,
                    DefaultFormatting = "Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}",
                    ReplaceParameters = new List<object>() { body, "RequestHandleResults index out range" }
                };

                throw new UtilityException((int)Errors.CrmBatchResponseItemFormatError, fragment);
            }

            var messageHandle = _crmMessageHandleSelector.Choose(handleResults[index].Request.GetType().FullName);

            CrmResponseMessage handleResponse = await messageHandle.ExecuteResponse(handleResults[index].HandleResult.Extension, handleResults[index].HandleResult.Url, handleResults[index].HandleResult.Body, responseInfo.StatusCode, responseInfo.Headers, responseInfo.Body, responseMessage);

            return handleResponse;
        }

        class RequestHandleMapItem
        {
            public CrmRequestMessage Request { get; set; }
            public CrmRequestMessageHandleResult HandleResult { get; set; }
        }
        class RequestHandleMap
        {
            public List<RequestHandleMapItem> ChangeSetHandles { get; } = new List<RequestHandleMapItem>();

            public List<RequestHandleMapItem> BatchHandles { get; } = new List<RequestHandleMapItem>();
            public CrmRequestMessage BatchRequest { get; set; }
        }

        class MessageItemResponseInfo
        {
            public int StatusCode { get; set; }
            public Dictionary<string, IEnumerable<string>> Headers { get; set; }
            public string Body { get; set; }
        }

        class BatchTypeInfo
        {
            public int Type { get; set; }
            public object Exension { get; set; }
        }

    }
}
