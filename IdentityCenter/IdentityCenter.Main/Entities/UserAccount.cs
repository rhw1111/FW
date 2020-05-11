using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Security;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using IdentityCenter.Main.Entities.DAL;

namespace IdentityCenter.Main.Entities
{
    /// <summary>
    /// 用户账号
    /// </summary>
    public class UserAccount : EntityBase<IUserAccountIMP>
    {
        private static IFactory<IUserAccountIMP>? _userAccountIMPFactory;

        public static IFactory<IUserAccountIMP> UserAccountIMPFactory
        {
            set
            {
                _userAccountIMPFactory = value;
            }
        }
        public override IFactory<IUserAccountIMP>? GetIMPFactory()
        {
            return _userAccountIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            get
            {

                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {

                return GetAttribute<string>(nameof(Password));
            }
            set
            {
                SetAttribute<string>(nameof(Password), value);
            }
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Active
        {
            get
            {

                return GetAttribute<bool>(nameof(Active));
            }
            set
            {
                SetAttribute<bool>(nameof(Active), value);
            }
        }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ExtensionInfo
        {
            get
            {

                return GetAttribute<string>(nameof(ExtensionInfo));
            }
            set
            {
                SetAttribute<string>(nameof(ExtensionInfo), value);
            }
        }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(CreateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreateTime), value);
            }
        }



        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }


        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }
        public async Task Disable(CancellationToken cancellationToken = default)
        {
            await _imp.Disable(this,cancellationToken);
        }
        public async Task Enable(CancellationToken cancellationToken = default)
        {
            await _imp.Enable(this,cancellationToken);
        }
        public async Task<bool> ValidatePassword(string password, CancellationToken cancellationToken = default)
        {
            return await _imp.ValidatePassword(this, password,cancellationToken);
        }
        public async Task UpdatePassword(string password, CancellationToken cancellationToken = default)
        {
            await _imp.UpdatePassword(this, password,cancellationToken);
        }
        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this,cancellationToken);
        }

        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this,cancellationToken);
        }

        public async Task AddThirdPartyAccount( UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default)
        {
            await _imp.AddThirdPartyAccount(this, partyAccount,cancellationToken);
        }

        public async Task UpdateThirdPartyAccount(UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default)
        {
            await _imp.UpdateThirdPartyAccount(this, partyAccount,cancellationToken);
        }

        public async Task DeleteThirdPartyAccount(Guid partyID, CancellationToken cancellationToken = default)
        {
            await _imp.DeleteThirdPartyAccount(this, partyID,cancellationToken);
        }

        public async Task<UserThirdPartyAccount?> GetThirdPartyAccount(Guid partyID, CancellationToken cancellationToken = default)
        {
            return await _imp.GetThirdPartyAccount(this, partyID,cancellationToken);
        }
        public async Task<UserThirdPartyAccount?> GetThirdPartyAccount(string source, string sourceID, CancellationToken cancellationToken = default)
        {
            return await _imp.GetThirdPartyAccount(this, source, sourceID,cancellationToken);
        }

    }

    public interface IUserAccountIMP
    {
        Task Add(UserAccount account, CancellationToken cancellationToken = default);
        Task Disable(UserAccount account, CancellationToken cancellationToken = default);
        Task Enable(UserAccount account, CancellationToken cancellationToken = default);
        Task<bool> ValidatePassword(UserAccount account,string password, CancellationToken cancellationToken = default);
        Task UpdatePassword(UserAccount account, string password, CancellationToken cancellationToken = default);
        Task Update(UserAccount account, CancellationToken cancellationToken = default);

        Task Delete(UserAccount account, CancellationToken cancellationToken = default);

        Task AddThirdPartyAccount(UserAccount account,UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default);

        Task UpdateThirdPartyAccount(UserAccount account, UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default);

        Task DeleteThirdPartyAccount(UserAccount account, Guid partyID, CancellationToken cancellationToken = default);

        Task<UserThirdPartyAccount?> GetThirdPartyAccount(UserAccount account, Guid partyID, CancellationToken cancellationToken = default);
        Task<UserThirdPartyAccount?> GetThirdPartyAccount(UserAccount account, string source,string sourceID, CancellationToken cancellationToken = default);


    }

    [Injection(InterfaceType = typeof(IUserAccountIMP), Scope = InjectionScope.Transient)]
    public class UserAccountIMP : IUserAccountIMP
    {
        private readonly IUserAccountStore _userAccountStore;
        private readonly IUserThirdPartyAccountStore _userThirdPartyAccountStore;
        private readonly ISecurityService _securityService;

        public UserAccountIMP(IUserAccountStore userAccountStore, IUserThirdPartyAccountStore userThirdPartyAccountStore, ISecurityService securityService)
        {
            _userAccountStore = userAccountStore;
            _userThirdPartyAccountStore = userThirdPartyAccountStore;
            _securityService = securityService;
        }

        public async Task Add(UserAccount account, CancellationToken cancellationToken = default)
        {
            //将密码Hash
            account.Password = _securityService.Hash(account.Password);


            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                var exist = await _userAccountStore.QueryByName(account.Name, cancellationToken);
                if (exist != null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.ExistSameNameUserAccount,
                        DefaultFormatting = "已经存在名称为{0}的用户账号",
                        ReplaceParameters = new List<object>() { account.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.ExistSameNameUserAccount, fragment);
                }

                await _userAccountStore.Add(account, cancellationToken);

                var firstID = await _userAccountStore.QueryFirstIDNolockByName(account.Name, cancellationToken);
                if (firstID != account.ID)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.ExistSameNameUserAccount,
                        DefaultFormatting = "已经存在名称为{0}的用户账号",
                        ReplaceParameters = new List<object>() { account.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.ExistSameNameUserAccount, fragment);
                }

                scope.Complete();
            }
        }

        public async Task AddThirdPartyAccount(UserAccount account, UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default)
        {
            partyAccount.AccountID = account.ID;

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                var exist = await _userThirdPartyAccountStore.QueryFirstIDNolockBySource(partyAccount.Source, partyAccount.ThirdPartyID, cancellationToken);
                if (exist != null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.ExistSameSourceThirdPartyAccount,
                        DefaultFormatting = "已经存在来源为{0}，Id为{1}的第三方账号",
                        ReplaceParameters = new List<object>() { partyAccount.Source,partyAccount.ThirdPartyID }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.ExistSameSourceThirdPartyAccount, fragment);
                }

                await _userThirdPartyAccountStore.Add(partyAccount, cancellationToken);

                var firstID = await _userThirdPartyAccountStore.QueryFirstIDNolockBySource(partyAccount.Source, partyAccount.ThirdPartyID, cancellationToken);
                if (firstID != account.ID)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.ExistSameSourceThirdPartyAccount,
                        DefaultFormatting = "已经存在来源为{0}，Id为{1}的第三方账号",
                        ReplaceParameters = new List<object>() { partyAccount.Source, partyAccount.ThirdPartyID }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.ExistSameSourceThirdPartyAccount, fragment);
                }

                scope.Complete();
            }
        }

        public async Task Delete(UserAccount account, CancellationToken cancellationToken = default)
        {

            await _userAccountStore.Delete(account.ID, cancellationToken);
        }

        public async Task DeleteThirdPartyAccount(UserAccount account, Guid partyID, CancellationToken cancellationToken = default)
        {
            var partyAccount = await GetThirdPartyAccount(account, partyID, cancellationToken);
            if (partyAccount != null)
            {
                /*var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundUserThirdPartyAccountByIDFromAccountName,
                    DefaultFormatting = "名称为{0}的用户账号下，找不到id为{1}的第三方账号",
                    ReplaceParameters = new List<object>() { account.Name, partyID.ToString() }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundUserThirdPartyAccountByIDFromAccountName, fragment);
                */

                await _userThirdPartyAccountStore.Delete(partyAccount.ID, cancellationToken);
            }


        }

        public async Task Disable(UserAccount account, CancellationToken cancellationToken = default)
        {
            await _userAccountStore.UpdateActive(account.ID, false, cancellationToken);
        }

        public async Task Enable(UserAccount account, CancellationToken cancellationToken = default)
        {
            await _userAccountStore.UpdateActive(account.ID, true, cancellationToken);
        }

        public async Task<UserThirdPartyAccount?> GetThirdPartyAccount(UserAccount account, Guid partyID, CancellationToken cancellationToken = default)
        {
            return await _userThirdPartyAccountStore.QueryByID(account.ID, partyID, cancellationToken);
        }

        public async Task<UserThirdPartyAccount?> GetThirdPartyAccount(UserAccount account, string source, string sourceID, CancellationToken cancellationToken = default)
        {
            return await _userThirdPartyAccountStore.QueryBySource(account.ID, source, sourceID, cancellationToken);
        }

        public async Task Update(UserAccount account, CancellationToken cancellationToken = default)
        {
            await _userAccountStore.Update(account, cancellationToken);
        }

        public async Task UpdatePassword(UserAccount account, string password, CancellationToken cancellationToken = default)
        {
            //将密码Hash
            var newPassword = _securityService.Hash(password);
            await _userAccountStore.UpdatePassword(account.ID, newPassword, cancellationToken);
        }

        public async Task UpdateThirdPartyAccount(UserAccount account, UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default)
        {
            partyAccount.AccountID = account.ID;
            partyAccount.Account = account;
            await _userThirdPartyAccountStore.Update(partyAccount, cancellationToken);
        }

        public async Task<bool> ValidatePassword(UserAccount account, string password, CancellationToken cancellationToken = default)
        {
            //将密码Hash
            var hashPassword = _securityService.Hash(password);
            if (account.Password== hashPassword)
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
