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
using MSLibrary.Xrm.Message.Create;
using MSLibrary.Xrm.Convert;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForCreate), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForCreate : ICrmMessageHandle
    {
        private ICrmExecuteEntityConvertJObjectService _crmExecuteEntityConvertJObjectService;
        public CrmMessageHandleForCreate(ICrmExecuteEntityConvertJObjectService crmExecuteEntityConvertJObjectService)
        {
            _crmExecuteEntityConvertJObjectService = crmExecuteEntityConvertJObjectService;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmCreateRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmCreateRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmCreateRequestMessage;

            //获取实体记录的json对象
            var entityJObject = await _crmExecuteEntityConvertJObjectService.Convert(realRequest.Entity);
            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.Entity.EntityName.ToPlural()}";
            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["OData-MaxVersion"] = new List<string> { "4.0" };
            headers["OData-Version"] = new List<string> { "4.0" };
            headers["Content-Type"] = new List<string> { "application/json" };
            headers["Content-Type-ChartSet"] = new List<string> { "utf-8" };
            headers["Accept"] = new List<string> { "application/json" };

            foreach(var itemHeader in realRequest.Headers)
            {
                headers[itemHeader.Key] = itemHeader.Value;
            }

            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method = HttpMethod.Post;
            result.Headers = headers;
            result.Body = entityJObject.ToString();



            return result;
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension,string requestUrl,string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            if (!responseHeaders.TryGetValue("OData-EntityId",out IEnumerable<string> entityIdList))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmWebApiHttpResponseNotFoundHeaderByName,
                    DefaultFormatting = "在Crm的Webapi响应中，找不到名称为{0}的头，Uri:{1},Body：{2}",
                    ReplaceParameters = new List<object>() { "OData-EntityId", requestUrl, requestBody }
                };

                throw new UtilityException((int)Errors.CrmWebApiHttpResponseNotFoundHeaderByName, fragment);
            }

            var entityIdValue=entityIdList.FirstOrDefault();
            if (string.IsNullOrEmpty(entityIdValue))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmWebApiHttpResponseNotFoundHeaderByName,
                    DefaultFormatting = "在Crm的Webapi响应中，找不到名称为{0}的头，Uri:{1},Body：{2}",
                    ReplaceParameters = new List<object>() { "OData-EntityId", requestUrl, requestBody }
                };

                throw new UtilityException((int)Errors.CrmWebApiHttpResponseNotFoundHeaderByName, fragment);
            }

            string regPattern = @"\(\{?([A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12})}?\)";
            Regex reg = new Regex(regPattern);
            var match= reg.Match(entityIdValue);
            if (match==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmWebApiHttpResponseRegexMatchFail,
                    DefaultFormatting = "在Crm的Webapi响应中，正则匹配失败，要匹配的字符串为{0}，表达式为{1}，Uri:{2},Body：{3}",
                    ReplaceParameters = new List<object>() { entityIdValue, regPattern, requestUrl, requestBody }
                };

                throw new UtilityException((int)Errors.CrmWebApiHttpResponseRegexMatchFail, fragment);
            }
            var strEntityId= match.Groups[1].Value;

            CrmCreateResponseMessage response = new CrmCreateResponseMessage()
            {
                 Id=Guid.Parse(strEntityId)
            };

            return await Task.FromResult(response);
        }

    }
}
