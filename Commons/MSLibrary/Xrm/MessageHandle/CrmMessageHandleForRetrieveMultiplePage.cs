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
using MSLibrary.Xrm.Message.RetrieveMultiplePage;
using MSLibrary.Xrm.Convert;
using MSLibrary.Serializer;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;


namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForRetrieveMultiplePage), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForRetrieveMultiplePage : ICrmMessageHandle
    {
        private ICrmRetrieveJTokenConvertService _crmRetrieveJTokenConvertService;
        public CrmMessageHandleForRetrieveMultiplePage(ICrmRetrieveJTokenConvertService crmRetrieveJTokenConvertService)
        {
            _crmRetrieveJTokenConvertService = crmRetrieveJTokenConvertService;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmRetrieveMultiplePageRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmRetrieveMultiplePageRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch,fragment);
            }

            var realRequest = request as CrmRetrieveMultiplePageRequestMessage;


            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.EntityName.ToPlural()}?{realRequest.QueryExpression}";

            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["OData-MaxVersion"] = new List<string> { "4.0" };
            headers["OData-Version"] = new List<string> { "4.0" };
            headers["Content-Type"] = new List<string> { "application/json" };
            headers["Content-Type-ChartSet"] = new List<string> { "utf-8" };
            headers["Accept"] = new List<string> { "application/json" };
            headers["Preference-Applied"] = new List<string> { $"odata.maxpagesize={realRequest.PageSize}" };

            foreach (var itemHeader in realRequest.Headers)
            {
                headers[itemHeader.Key] = itemHeader.Value;
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
            var request = (CrmRetrieveMultiplePageRequestMessage)extension;

            CrmRetrieveMultiplePageResponseMessage response = new CrmRetrieveMultiplePageResponseMessage();

            var jObject = JsonSerializerHelper.Deserialize<JObject>(responseBody);

            var entityList = await _crmRetrieveJTokenConvertService.Convert<CrmEntityCollection>(jObject, new Dictionary<string, object>() { { CrmRetrieveJTokenHandleExtensionParameterNames.EntityName, request.EntityName } });

            response.Value = entityList;

            return await Task.FromResult(response);
        }
    }
}
