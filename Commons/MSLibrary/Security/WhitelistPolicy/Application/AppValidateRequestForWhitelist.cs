using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.EntityMetadata;

namespace MSLibrary.Security.WhitelistPolicy.Application
{
    [Injection(InterfaceType = typeof(IAppValidateRequestForWhitelist), Scope = InjectionScope.Singleton)]
    public class AppValidateRequestForWhitelist : IAppValidateRequestForWhitelist
    {
        private ISystemOperationRepository _systemOperationRepository;
        private IOptionSetValueMetadataRepository _optionSetValueMetadataRepository;
        public AppValidateRequestForWhitelist(ISystemOperationRepository systemOperationRepository, IOptionSetValueMetadataRepository optionSetValueMetadataRepository)
        {
            _systemOperationRepository = systemOperationRepository;
            _optionSetValueMetadataRepository = optionSetValueMetadataRepository;
        }
        public async Task<ValidateResult> Do(string systemOperationName, string systemName, string strToken, string ip)
        {
            ValidateResult result;
            var systemOperation = await _systemOperationRepository.QueryByName(systemOperationName,1);
            //如果找不到对应名称的系统操作，返回错误
            if (systemOperation == null)
            {
                result = new ValidateResult()
                {
                    Result = false,
                    Description = string.Format(StringLanguageTranslate.Translate(TextCodes.NotFoundWhitelistSystemOperationWithNameStatus, "找不到名称为{0}、状态为{2}的白名单系统操作"), systemOperationName,OptionSetMetadataValueHelper.GetLable(_optionSetValueMetadataRepository,$"{ typeof(SystemOperation).FullName }.Status",1))
                };

                return await Task.FromResult(result);
            }

            result =await systemOperation.Validate(systemName, strToken, ip);

            return result;
        }
    }
}
