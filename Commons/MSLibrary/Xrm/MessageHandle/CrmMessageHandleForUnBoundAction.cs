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
using MSLibrary.Xrm.Message.UnBoundAction;
using MSLibrary.Xrm.Convert;
using MSLibrary.Serializer;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;


namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUnBoundAction), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUnBoundAction : ICrmMessageHandle
    {
        private ICrmActionParameterConvertService _crmActionParameterConvertService;
        public CrmMessageHandleForUnBoundAction(ICrmActionParameterConvertService crmActionParameterConvertService)
        {
            _crmActionParameterConvertService = crmActionParameterConvertService;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmUnBoundActionRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmUnBoundActionRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmUnBoundActionRequestMessage;

            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.ActionName}";


            JObject parameterJObject = new JObject();

            foreach (var parameterItem in realRequest.Parameters)
            {
                var itemResult = await _crmActionParameterConvertService.Convert(parameterItem);
                parameterJObject[parameterItem.Name] = itemResult;
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

            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method = HttpMethod.Post;
            result.Headers = headers;
            result.Body = JsonSerializerHelper.Serializer(parameterJObject);

            return result;
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            CrmUnBoundActionResponseMessage response = new CrmUnBoundActionResponseMessage();
            response.Value = JsonSerializerHelper.Deserialize<JObject>(responseBody);
            return await Task.FromResult(response);
        }
    }
}
