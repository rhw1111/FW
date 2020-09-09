using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using IdentityCenter.Main.Entities.DAL;

namespace IdentityCenter.Main.Entities
{
    public class Privilege : EntityBase<IPrivilegeIMP>
    {
        private static IFactory<IPrivilegeIMP>? _privilegeIMPFactory;

        public static IFactory<IPrivilegeIMP> PrivilegeIMPFactory
        {
            set
            {
                _privilegeIMPFactory = value;
            }
        }

        public override IFactory<IPrivilegeIMP>? GetIMPFactory()
        {
            return _privilegeIMPFactory;
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
        /// 编码
        /// </summary>
        public string Code
        {
            get
            {

                return GetAttribute<string>(nameof(Code));
            }
            set
            {
                SetAttribute<string>(nameof(Code), value);
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {

                return GetAttribute<string>(nameof(Description));
            }
            set
            {
                SetAttribute<string>(nameof(Description), value);
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
    }

    public interface IPrivilegeIMP
    {
        Task Add(Privilege privilege, CancellationToken cancellationToken = default);
        Task Update(Privilege privilege, CancellationToken cancellationToken = default);
        Task Delete(Privilege privilege, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IPrivilegeIMP), Scope = InjectionScope.Transient)]
    public class PrivilegeIMP : IPrivilegeIMP
    {
        private readonly IPrivilegeStore _privilegeStore;

        public PrivilegeIMP(IPrivilegeStore privilegeStore)
        {
            _privilegeStore = privilegeStore;
        }
        public async Task Add(Privilege privilege, CancellationToken cancellationToken = default)
        {
            var existPrivilege = await _privilegeStore.QueryByCode(privilege.Code, cancellationToken);
            if (existPrivilege != null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.ExistPrivilegeByCode,
                    DefaultFormatting = "已经存在编码为{0}的权限",
                    ReplaceParameters = new List<object>() { privilege.Code }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.ExistPrivilegeByCode, fragment);
            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {

                await _privilegeStore.Add(privilege, cancellationToken);
                //检查是否有编码重复的
                var newId = await _privilegeStore.QueryByCodeNoLock(privilege.Code, cancellationToken);
                if (newId != null && privilege.ID != newId)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.ExistPrivilegeByCode,
                        DefaultFormatting = "已经存在编码为{0}的权限",
                        ReplaceParameters = new List<object>() { privilege.Code }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.ExistPrivilegeByCode, fragment);
                }
                scope.Complete();
            }

        }

        public async Task Delete(Privilege privilege, CancellationToken cancellationToken = default)
        {
            await _privilegeStore.Delete(privilege.ID, cancellationToken);
        }

        public async Task Update(Privilege privilege, CancellationToken cancellationToken = default)
        {
            var existPrivilege = await _privilegeStore.QueryByCode(privilege.Code, cancellationToken);
            if (existPrivilege != null && existPrivilege.ID!= privilege.ID)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.ExistPrivilegeByCode,
                    DefaultFormatting = "已经存在编码为{0}的权限",
                    ReplaceParameters = new List<object>() { privilege.Code }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.ExistPrivilegeByCode, fragment);
            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {

                await _privilegeStore.Update(privilege, cancellationToken);
                //检查是否有编码重复的
                var newId = await _privilegeStore.QueryByCodeNoLock(privilege.Code, cancellationToken);
                if (newId != null && privilege.ID != newId)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.ExistPrivilegeByCode,
                        DefaultFormatting = "已经存在编码为{0}的权限",
                        ReplaceParameters = new List<object>() { privilege.Code }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.ExistPrivilegeByCode, fragment);
                }
                scope.Complete();
            }
        }
    }
}
