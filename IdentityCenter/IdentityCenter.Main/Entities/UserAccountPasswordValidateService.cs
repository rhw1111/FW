using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace IdentityCenter.Main.Entities
{
    [Injection(InterfaceType = typeof(IUserAccountPasswordValidateService), Scope = InjectionScope.Singleton)]
    public class UserAccountPasswordValidateService : IUserAccountPasswordValidateService
    {
        private readonly IUserAccountRepository _userAccountRepository;

        public UserAccountPasswordValidateService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }
        public async Task<UserAccount> Validate(string name, string password, CancellationToken cancellationToken = default)
        {
            var userAccount = await _userAccountRepository.QueryByName(name, cancellationToken);
            if (userAccount == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundUserAccountByName,
                    DefaultFormatting = "找不到名称为{0}的用户账号",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundUserAccountByName, fragment, 1, 0);
            }
            var passwordValidateResult = await userAccount.ValidatePassword(password);

            if (passwordValidateResult)
            {
                return userAccount;
            }
            else
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.UserAccountPasswordInvalid,
                    DefaultFormatting = "名称为{0}的用户账号的密码不正确",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.UserAccountPasswordInvalid, fragment, 1, 0);
            }
        }
    }
}
