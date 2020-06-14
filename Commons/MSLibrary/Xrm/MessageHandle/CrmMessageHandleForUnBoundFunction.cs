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
using MSLibrary.Xrm.Message.UnBoundFunction;
using MSLibrary.Xrm.Convert;
using MSLibrary.Serializer;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;


namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUnBoundFunction), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUnBoundFunction : ICrmMessageHandle
    {
        private ICrmFunctionParameterConvertService _crmFunctionParameterConvertService;
        public CrmMessageHandleForUnBoundFunction(ICrmFunctionParameterConvertService crmFunctionParameterConvertService)
        {
            _crmFunctionParameterConvertService = crmFunctionParameterConvertService;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmUnBoundFunctionRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmUnBoundFunctionRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmUnBoundFunctionRequestMessage;

            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.FunctionName}";
 
            if (realRequest.Parameters == null || realRequest.Parameters.Count == 0)
            {
                url = $"{url}()";
            }
            else
            {
                bool useAlise = false;
                string parameterExpression = string.Empty;
                string strAlise = string.Empty;
                foreach (var parameterItem in realRequest.Parameters)
                {
                    if (parameterExpression == string.Empty)
                    {
                        parameterExpression = $"{parameterItem.Name}=";
                    }
                    else
                    {
                        parameterExpression = $"{parameterExpression},{parameterItem.Name}=";
                    }

                    if (!string.IsNullOrEmpty(parameterItem.Alias))
                    {
                        parameterExpression = $"{parameterExpression},{parameterItem.Name}={parameterItem.Alias}";
                        useAlise = true;
                        if (strAlise == string.Empty)
                        {
                            strAlise = $"{parameterItem.Alias}={await _crmFunctionParameterConvertService.Convert(parameterItem.Value)}";
                        }
                        else
                        {
                            strAlise = $"{strAlise}&{parameterItem.Alias}={await _crmFunctionParameterConvertService.Convert(parameterItem.Value)}";
                        }
                    }
                    else
                    {
                        parameterExpression = $"{parameterExpression},{parameterItem.Name}={await _crmFunctionParameterConvertService.Convert(parameterItem.Value)}";

                    }
                }

                url = $"{url}({parameterExpression})";
                if (useAlise)
                {
                    url = $"{url}?{strAlise}";
                }

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
            result.Method = HttpMethod.Get;
            result.Headers = headers;

            return result;
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            CrmUnBoundFunctionResponseMessage response = new CrmUnBoundFunctionResponseMessage();
            response.Value = JsonSerializerHelper.Deserialize<JObject>(responseBody);
            return await Task.FromResult(response);
        }
    }
}
