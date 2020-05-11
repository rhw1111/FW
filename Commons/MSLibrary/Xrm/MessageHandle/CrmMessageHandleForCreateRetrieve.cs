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
using MSLibrary.Xrm.Message.CreateRetrieve;
using MSLibrary.Xrm.Convert;
using MSLibrary.Serializer;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForCreateRetrieve), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForCreateRetrieve : ICrmMessageHandle
    {
        private ICrmExecuteEntityConvertJObjectService _crmExecuteEntityConvertJObjectService;
        public CrmMessageHandleForCreateRetrieve(ICrmExecuteEntityConvertJObjectService crmExecuteEntityConvertJObjectService)
        {
            _crmExecuteEntityConvertJObjectService = crmExecuteEntityConvertJObjectService;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmCreateRetrieveRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmCreateRetrieveRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmCreateRetrieveRequestMessage;

            //获取实体记录的json对象
            var entityJObject = await _crmExecuteEntityConvertJObjectService.Convert(realRequest.Entity);
            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.Entity.EntityName.ToPlural()}?$select={string.Join(",",realRequest.Attributes)}";
            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["OData-MaxVersion"] = new List<string> { "4.0" };
            headers["OData-Version"] = new List<string> { "4.0" };
            headers["Content-Type"] = new List<string> { "application/json" };
            headers["Content-Type-ChartSet"] = new List<string> { "utf-8" };
            headers["Accept"] = new List<string> { "application/json" };
            headers["Prefer"]= new List<string> { "return=representation" };

            foreach (var itemHeader in realRequest.Headers)
            {
                headers[itemHeader.Key] = itemHeader.Value;
            }

            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method = HttpMethod.Post;
            result.Headers = headers;
            result.Body = entityJObject.ToString();
            result.Extension = realRequest;


            return result;
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            var request = (CrmCreateRetrieveRequestMessage)extension;
            var jObject=JsonSerializerHelper.Deserialize<JObject>(responseBody);

            Guid entityId;
            if (jObject["activityid"] != null)
            {
                entityId = Guid.Parse(jObject["activityid"].Value<string>());
            }
            else
            {
                entityId = Guid.Parse(jObject[$"{request.Entity.EntityName}id"].Value<string>());
            }
            var version= jObject["@odata.etag"].Value<string>();

            CrmCreateRetrieveResponseMessage response = new CrmCreateRetrieveResponseMessage()
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
