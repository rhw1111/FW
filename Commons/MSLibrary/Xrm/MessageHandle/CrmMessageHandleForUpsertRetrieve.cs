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
using MSLibrary.Serializer;
using MSLibrary.Xrm.Message.UpsertRetrieve;
using MSLibrary.Xrm.Convert;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUpsertRetrieve), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUpsertRetrieve : ICrmMessageHandle
    {
        private ICrmExecuteEntityConvertJObjectService _crmExecuteEntityConvertJObjectService;
        private ICrmAlternateKeyTypeHandle _crmAlternateKeyTypeHandle;
        public CrmMessageHandleForUpsertRetrieve(ICrmExecuteEntityConvertJObjectService crmExecuteEntityConvertJObjectService, ICrmAlternateKeyTypeHandle crmAlternateKeyTypeHandle)
        {
            _crmExecuteEntityConvertJObjectService = crmExecuteEntityConvertJObjectService;
            _crmAlternateKeyTypeHandle = crmAlternateKeyTypeHandle;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmUpsertRetrieveRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmUpsertRetrieveRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmUpsertRetrieveRequestMessage;
            //获取实体记录的json对象
            var entityJObject = await _crmExecuteEntityConvertJObjectService.Convert(realRequest.Entity);
            //如果唯一键集合不为空，则使用唯一键作为主键，否则，使用Entity的Id作为主键
            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.Entity.EntityName.ToPlural()}";
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
                url = $"{url}({realRequest.Entity.Id.ToString()})";
            }

            url = $"{url}?$select={string.Join(",", realRequest.Attributes)}";
            //var url = $"{realRequest.OrganizationURI}/api/v{realRequest.ApiVersion}/{realRequest.MessageName.ToPlural()}({realRequest.Entity.Id.ToString()})";
            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["OData-MaxVersion"] = new List<string> { "4.0" };
            headers["OData-Version"] = new List<string> { "4.0" };
            headers["Content-Type"] = new List<string> { "application/json" };
            headers["Content-Type-ChartSet"] = new List<string> { "utf-8" };
            headers["Prefer"] = new List<string> { "return=representation" };

            foreach (var itemHeader in realRequest.Headers)
            {
                headers[itemHeader.Key] = itemHeader.Value;
            }


            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method = HttpMethod.Patch;
            result.Headers = headers;
            result.Body = entityJObject.ToString();
            result.Extension = realRequest;

            return result;
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            var request = (CrmUpsertRetrieveRequestMessage)extension;
            var jObject = JsonSerializerHelper.Deserialize<JObject>(responseBody);
            var entityId = Guid.Parse(jObject[$"{request.Entity.EntityName}id"].Value<string>());
            var version = jObject["@odata.etag"].Value<string>();

            CrmUpsertRetrieveResponseMessage response = new CrmUpsertRetrieveResponseMessage()
            {
                Entity = new CrmEntity(request.Entity.EntityName, entityId)
                {
                    Version = version,
                    Attributes = jObject
                }
            };

            return await Task.FromResult(response);
        }
    }
}
