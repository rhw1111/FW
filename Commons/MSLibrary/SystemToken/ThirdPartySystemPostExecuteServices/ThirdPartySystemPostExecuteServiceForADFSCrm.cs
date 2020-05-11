using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.Security;
using MSLibrary.Xrm;

namespace MSLibrary.SystemToken.ThirdPartySystemPostExecuteServices
{
    /// <summary>
    /// 基于Adfs、Crm的第三方系统后续处理服务
    /// configuration格式为
    /// {
    ///     "CrmServiceFactoryName":"crm服务工厂名称"
    /// }
    /// attributes里面需要有upn
    /// 获取的键值对为
    /// userId
    /// </summary>
    public class ThirdPartySystemPostExecuteServiceForAdfsCrm : IThirdPartySystemPostExecuteService
    {
        private ICrmServiceFactoryRepositoryCacheProxy _crmServiceFactoryRepositoryCacheProxy;

        public ThirdPartySystemPostExecuteServiceForAdfsCrm(ICrmServiceFactoryRepositoryCacheProxy crmServiceFactoryRepositoryCacheProxy)
        {
            _crmServiceFactoryRepositoryCacheProxy=crmServiceFactoryRepositoryCacheProxy;
        }
        public async Task<ThirdPartySystemPostExecuteResult> Execute(Dictionary<string, string> attributes, string configurationInfo)
        {
            TextFragment fragment;
            //获取指定的Crm服务
            PostExecuteServiceConfiguration configuration = JsonSerializerHelper.Deserialize<PostExecuteServiceConfiguration>(configurationInfo);
            var serviceFactory= await _crmServiceFactoryRepositoryCacheProxy.QueryByName(configuration.CrmServiceFactoryName);
            if (serviceFactory==null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFountCrmServiceFactorybyName,
                    DefaultFormatting = "找不到名称为{0}的Crm服务工厂",
                    ReplaceParameters = new List<object>() { configuration.CrmServiceFactoryName }
                };

                throw new UtilityException((int)Errors.NotFountCrmServiceFactorybyName, fragment);
            }

            
            //从attribute中获取upn
            if (!attributes.TryGetValue("upn",out string upn))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundNeedAttributeInThirdPartySystemPostExecuteService,
                    DefaultFormatting = "在第三方系统后续操作服务{0}中，在上游第三方系统服务传入的键值对中，找不到键为{1}的值，请检查这两个服务设置是否匹配",
                    ReplaceParameters = new List<object>() { typeof(ThirdPartySystemPostExecuteServiceForAdfsCrm).FullName, "upn" }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.NotFoundNeedAttributeInThirdPartySystemPostExecuteService, fragment);
            }


            var service=await serviceFactory.Create();

            string strQuery = string.Format($"$select=systemuserid&$filter=domainname eq '{upn}'");
            var collection=await service.RetrieveMultiple("systemuser", strQuery);

            if (collection.Results.Count==0)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmUserInfoInThirdPartySystemPostExecuteService,
                    DefaultFormatting = "在第三方系统后续操作服务{0}中找不到Crm用户信息,CrmServiceFactory名称：{1}，DomainName:{2}",
                    ReplaceParameters = new List<object>() { typeof(ThirdPartySystemPostExecuteServiceForAdfsCrm).FullName, configuration.CrmServiceFactoryName,upn }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.NotFoundCrmUserInfoInThirdPartySystemPostExecuteService, fragment);
            }

            var userid=collection.Results[0].Attributes["systemuserid"].ToString();
            ThirdPartySystemPostExecuteResult result = new ThirdPartySystemPostExecuteResult()
            {
                UserInfoAttributes = new Dictionary<string, string>()
                {
                    { "userid",userid}
                },
                AdditionalRedirectUrlQueryAttributes = new Dictionary<string, string>() 
            };

            return result;
        }


        [DataContract]
        private class PostExecuteServiceConfiguration
        {
            [DataMember]
            public string CrmServiceFactoryName { get; set; }
        }
    }
}
