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
using MSLibrary.Xrm.Message.BoundFunction;
using MSLibrary.Xrm.Convert;
using MSLibrary.Serializer;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;


namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForBoundFunction), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForBoundFunction : ICrmMessageHandle
    {
        private ICrmAlternateKeyTypeHandle _crmAlternateKeyTypeHandle;
        private ICrmFunctionParameterConvertService _crmFunctionParameterConvertService;
        public CrmMessageHandleForBoundFunction(ICrmAlternateKeyTypeHandle crmAlternateKeyTypeHandle, ICrmFunctionParameterConvertService crmFunctionParameterConvertService)
        {
            _crmAlternateKeyTypeHandle = crmAlternateKeyTypeHandle;
            _crmFunctionParameterConvertService = crmFunctionParameterConvertService;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmBoundFunctionRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmBoundFunctionRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmBoundFunctionRequestMessage;

            //如果唯一键集合不为空，则使用唯一键作为主键，否则，使用Entity的Id作为主键
            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.EntityName.ToPlural()}";
            if (realRequest.AlternateKeys != null && realRequest.AlternateKeys.Count > 0)
            {
                StringBuilder strAlternateKey = new StringBuilder();
                foreach (var keyItem in realRequest.AlternateKeys)
                {
                    strAlternateKey.Append(keyItem.Key);
                    strAlternateKey.Append("=");
                    strAlternateKey.Append(await _crmAlternateKeyTypeHandle.Convert(keyItem.Value));
                    strAlternateKey.Append(",");
                }

                if (strAlternateKey.Length > 0)
                {
                    strAlternateKey.Remove(strAlternateKey.Length - 1, 1);
                }

                url = $"{url}({strAlternateKey.ToString()})";
            }
            else
            {
                url = $"{url}({realRequest.EntityId.ToString()})";
            }

            url = $"{url}/Microsoft.Dynamics.CRM.{ realRequest.FunctionName}";

            if (realRequest.Parameters==null || realRequest.Parameters.Count==0)
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
                    if (parameterExpression==string.Empty)
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
                        if (strAlise==string.Empty)
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
            CrmBoundFunctionResponseMessage response = new CrmBoundFunctionResponseMessage();
            response.Value = JsonSerializerHelper.Deserialize<JObject>(responseBody);
            return await Task.FromResult(response);
        }
    }
}
