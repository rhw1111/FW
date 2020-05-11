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
using MSLibrary.Xrm.Message.Update;
using MSLibrary.Xrm.Convert;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForUpdate), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForUpdate : ICrmMessageHandle
    {

        private ICrmExecuteEntityConvertJObjectService _crmExecuteEntityConvertJObjectService;
        private ICrmAlternateKeyTypeHandle _crmAlternateKeyTypeHandle;
        public CrmMessageHandleForUpdate(ICrmExecuteEntityConvertJObjectService crmExecuteEntityConvertJObjectService, ICrmAlternateKeyTypeHandle crmAlternateKeyTypeHandle)
        {
            _crmExecuteEntityConvertJObjectService = crmExecuteEntityConvertJObjectService;
            _crmAlternateKeyTypeHandle = crmAlternateKeyTypeHandle;
        }

        public async Task<CrmRequestMessageHandleResult> ExecuteRequest(CrmRequestMessage request)
        {
            if (!(request is CrmUpdateRequestMessage))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CrmRequestMessageTypeNotMatch,
                    DefaultFormatting = "消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}",
                    ReplaceParameters = new List<object>() { typeof(CrmUpdateRequestMessage).FullName, request.GetType().FullName, $"{ this.GetType().FullName }.ExecuteRequest" }
                };

                throw new UtilityException((int)Errors.CrmRequestMessageTypeNotMatch, fragment);
            }

            var realRequest = request as CrmUpdateRequestMessage;
            //获取实体记录的json对象
            var entityJObject = await _crmExecuteEntityConvertJObjectService.Convert(realRequest.Entity);
            //如果唯一键集合不为空，则使用唯一键作为主键，否则，使用Entity的Id作为主键
            var url = $"{realRequest.OrganizationURI}/api/data/v{realRequest.ApiVersion}/{realRequest.Entity.EntityName.ToPlural()}";
            if (realRequest.AlternateKeys!=null && realRequest.AlternateKeys.Count>0)
            {
                StringBuilder strAlternateKey = new StringBuilder();
                foreach (var keyItem in realRequest.AlternateKeys)
                {
                    strAlternateKey.Append(keyItem.Key);
                    strAlternateKey.Append("=");
                    strAlternateKey.Append(await _crmAlternateKeyTypeHandle.Convert(keyItem.Value));
                    strAlternateKey.Append(",");
                }

                if (strAlternateKey.Length>0)
                {
                    strAlternateKey.Remove(strAlternateKey.Length - 1, 1);
                }

                url = $"{url}({strAlternateKey.ToString()})";
            }
            else
            {
                url = $"{url}({realRequest.Entity.Id.ToString()})";
            }
            //var url = $"{realRequest.OrganizationURI}/api/v{realRequest.ApiVersion}/{realRequest.MessageName.ToPlural()}({realRequest.Entity.Id.ToString()})";
            var headers = new Dictionary<string, IEnumerable<string>>();
            headers["OData-MaxVersion"] = new List<string> { "4.0" };
            headers["OData-Version"] = new List<string> { "4.0" };
            headers["Content-Type"] = new List<string> { "application/json" };
            headers["Content-Type-ChartSet"] = new List<string> { "utf-8" };

            foreach (var itemHeader in realRequest.Headers)
            {
                headers[itemHeader.Key] = itemHeader.Value;
            }

            //判断是否需要进行版本检查
            if (!string.IsNullOrEmpty(realRequest.Version))
            {
                headers["If-Match"] = new List<string>() { realRequest.Version };
            }
            else
            {
                headers["If-Match"] = new List<string>() { "*" };
            }

            CrmRequestMessageHandleResult result = new CrmRequestMessageHandleResult();
            result.Url = url;
            result.Method =HttpMethod.Patch;
            result.Headers = headers;
            result.Body = entityJObject.ToString();

            return result;
        }

        public async Task<CrmResponseMessage> ExecuteResponse(object extension, string requestUrl, string requestBody, int responseCode, Dictionary<string, IEnumerable<string>> responseHeaders, string responseBody, HttpResponseMessage responseMessage)
        {
            CrmUpdateResponseMessage response = new CrmUpdateResponseMessage();
            return await Task.FromResult(response);
        }
    }
}
