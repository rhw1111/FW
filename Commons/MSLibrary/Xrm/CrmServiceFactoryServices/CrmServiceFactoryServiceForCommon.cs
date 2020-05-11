using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Template;

namespace MSLibrary.Xrm.CrmServiceFactoryServices
{
    /// <summary>
    /// 针对通用ADFS的CRM服务工厂服务
    /// configuration格式为
    /// {
    ///     "TokenServiceType":"令牌服务类型,AD,ADFS,ADFSPassword,S2S",
    ///     "CrmUrl":"Crm地址",
    ///     "CrmApiVersion":"CrmApi版本",
    ///     "CrmApiMaxRetry":服务最大重试次数，整数,
    ///     "TokenServiceParameters":
    ///     不同的TokenServiceType有不同的配置
    ///     AD
    ///     {
    ///         "UserName":"string,用户名（不带域名）",
    ///         "Password":"string,密码",
    ///         "Domain":"string,域名"
    ///     }
    ///     ADFS
    ///     {
    ///         "AdfsUrl":"string,ADFS的地址",
    ///         "CrmUrl":"string,Crm的地址",
    ///         "ClientId":"string,在ADFS中注册的ClientId",
    ///         "RedirectUri":"string,在ADFS中注册的RedirectUri",
    ///         "UserName":"string,用户名",
    ///         "Password":"string,密码"
    ///     }
    ///     ADFSPassword
    ///     {
    ///         "AdfsUrl":"string,ADFS的地址",
    ///         "CrmUrl":"string,Crm的地址",
    ///         "ClientId":"string,在ADFS中注册的ClientId",
    ///         "ClientSecret":"string,在ADFS中注册的服务器应用程序的密码",
    ///         "UserName":"string,用户名",
    ///         "Password":"string,密码"    
    ///     }
    ///     S2S
    ///     {
    ///         "BaseUri":"string,Azure云的login基地址"
    ///         "ApplicationId":"string,应用的Id",
    ///         "ApplicationKey":"string,应用的密钥",
    ///         "CrmUrl":"string,Crm的地址",
    ///         "AADId":"string,Application所在的AAD的id"   
    ///     }  
    /// 
    /// 
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(CrmServiceFactoryServiceForCommon), Scope = InjectionScope.Singleton)]
    public class CrmServiceFactoryServiceForCommon : ICrmServiceFactoryService
    {
        /// <summary>
        /// 文本替换服务
        /// 如果该属性赋值，则configuration中的内容将首先使用该服务来替换占位符
        /// </summary>
        public static ITextReplaceService TextReplaceService { set; get; }
        public async Task<ICrmService> Create(string configuration)
        {
            if (TextReplaceService!=null)
            {
                configuration = await TextReplaceService.Replace(configuration);
            }
            var crmService=DIContainerContainer.Get<CrmService>();

            var serviceConfiguration=JsonSerializerHelper.Deserialize<CrmServiceConfiguration>(configuration);

            foreach(var item in serviceConfiguration.TokenServiceParameters)
            {
                crmService.TokenServiceParameters.Add(item.Key, item.Value);
            }

            crmService.CrmApiVersion = serviceConfiguration.CrmApiVersion;
            crmService.CrmApiMaxRetry = serviceConfiguration.CrmApiMaxRetry;
            crmService.CrmUrl = serviceConfiguration.CrmUrl;
            crmService.TokenServiceType = serviceConfiguration.TokenServiceType;

            return await Task.FromResult(crmService);
        }

    }
}
