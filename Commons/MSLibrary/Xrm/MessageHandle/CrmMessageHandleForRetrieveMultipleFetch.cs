using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Xrm.Message.RetrieveMultipleFetch;
using MSLibrary.Xrm.Convert;
using MSLibrary.Serializer;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;


namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveMultipleFetch), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveMultipleFetch : ICrmMessageHandle
    {
        private ICrmRetrieveJTokenConvertService _crmRetrieveJTokenConvertService;
        public CrmMessageHandleForRetrieveMultipleFetch(ICrmRetrieveJTokenConvertService crmRetrieveJTokenConvertService)
        {
            _crmRetrieveJTokenConvertService = crmRetrieveJTokenConvertService;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmRetrieveMultipleFetchRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmRetrieveMultipleFetchRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmRetrieveMultipleFetchRequestMessage;


            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.EntityName.ToPlural()}?fetchXml={WebUtility.UrlEncode(realRequest.FetchXml.ToString())}";
            if (!string.IsNullOrEmpty(realRequest.AdditionalQueryExpression))
            {
                url = $"{url}&{realRequest.AdditionalQueryExpression}";
            }
            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["OData-MaxVersion"] = new List<string> { "4.0" };
            headers["OData-Version"] = new List<string> { "4.0" };
            headers["Content-Type"] = new List<string> { "application/json" };
            headers["Content-Type-ChartSet"] = new List<string> { "utf-8" };
            headers["Accept"] = new List<string> { "application/json" };



            foreach (var itemHeader in realRequest.Headers)
            {
                headers[itemHeader.Key] = itemHeader.Value;
            }

            if (realRequest.NeedPagingCookie)
            {
                if (headers.ContainsKey("odata.include-annotations"))
                {
                    headers["odata.include-annotations"] = new List<string> { "Microsoft.Dynamics.CRM.fetchxmlpagingcookie" };
                }
                else
                {
                    var annotations=headers["odata.include-annotations"].ToList();
                    annotations.Add("Microsoft.Dynamics.CRM.fetchxmlpagingcookie");
                    headers["odata.include-annotations"] = annotations;
                }
            }


            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method = HttpMethod.Get;
            result.Headers = headers;
            result.Extension = realRequest;

            return await Task.FromResult(result);
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            var request = (CrmRetrieveMultipleFetchRequestMessage)extension;

            CrmRetrieveMultipleFetchResponseMessage response = new CrmRetrieveMultipleFetchResponseMessage();

            var jObject = JsonSerializerHelper.Deserialize<JObject>(responseBody);

            var entityList = await _crmRetrieveJTokenConvertService.Convert<CrmEntityCollection>(jObject, new Dictionary<string, object>() { { CrmRetrieveJTokenHandleExtensionParameterNames.EntityName, request.EntityName } });

            response.Value = entityList;

            return await Task.FromResult(response);
        }
    }
}
