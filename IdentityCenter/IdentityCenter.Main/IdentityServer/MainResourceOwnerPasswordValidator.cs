using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;


namespace IdentityCenter.Main.IdentityServer
{
    /// <summary>
    /// IdentityServer的用户验证服务
    /// </summary>
    [Injection(InterfaceType = typeof(IResourceOwnerPasswordValidator), Scope = InjectionScope.Transient)]
    public class MainResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserAccountRepository _userAccountRepository;

        public MainResourceOwnerPasswordValidator(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userAccount = await _userAccountRepository.QueryByName(context.UserName);
            if (userAccount==null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundUserAccountByName,
                    DefaultFormatting = "找不到名称为{0}的用户账号",
                    ReplaceParameters = new List<object>() { context.UserName }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundUserAccountByName, fragment, 1, 0);
            }

            var result=await userAccount.ValidatePassword(context.Password);
            if (result)
            {
                context.Result = new GrantValidationResult(userAccount.ID.ToString(), "Password");
            }
            else
            {        
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
            }

        }
    }
}
