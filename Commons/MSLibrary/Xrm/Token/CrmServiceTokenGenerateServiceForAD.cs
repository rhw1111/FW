using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Collections.Hash.HashDataMigrateServices;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm.Token
{
    /// <summary>
    /// 针对AD模式的令牌生成
    /// 需要传入的参数：
    /// UserName:string,用户名
    /// Password:string,密码
    /// Domain:string,AD域名
    /// </summary>
    [Injection(InterfaceType = typeof(CrmServiceTokenGenerateServiceForAD), Scope = InjectionScope.Singleton)]
    public class CrmServiceTokenGenerateServiceForAD : ICrmServiceTokenGenerateService
    {
        public async Task<string> Genereate(Dictionary<string, object> parameters)
        {
            TextFragment fragment;
            //检查参数
            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.UserName) || parameters[CrmServiceTokenGenerateServiceParameterNames.UserName] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.UserName }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }

            string userName = parameters[CrmServiceTokenGenerateServiceParameterNames.UserName].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.Password) || parameters[CrmServiceTokenGenerateServiceParameterNames.Password] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.Password }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }

            string password = parameters[CrmServiceTokenGenerateServiceParameterNames.Password].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.Domain) || parameters[CrmServiceTokenGenerateServiceParameterNames.Domain] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.Domain }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }

            string domain = parameters[CrmServiceTokenGenerateServiceParameterNames.Domain].ToString();

            string basicToken = $"{userName}@{domain}:{password}";
            return await Task.FromResult($"Basic {basicToken.Base64Encode()}");
        }
    }
}
