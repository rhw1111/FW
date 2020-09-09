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
    public class Role : EntityBase<IRoleIMP>
    {
        private static IFactory<IRoleIMP>? _roleIMPFactory;

        public static IFactory<IRoleIMP> RoleIMPFactory
        {
            set
            {
                _roleIMPFactory = value;
            }
        }

        public override IFactory<IRoleIMP>? GetIMPFactory()
        {
            return _roleIMPFactory;
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
        /// 名称
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

        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }
        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }
        public async Task Delete( CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }

        public async Task AddPrivilege(Guid privilegeID, CancellationToken cancellationToken = default)
        {
            await _imp.AddPrivilege(this, privilegeID, cancellationToken);
        }

        public async Task AddPrivileges( IEnumerable<Guid> privilegeIDs, CancellationToken cancellationToken = default)
        {
            await _imp.AddPrivileges(this, privilegeIDs, cancellationToken);
        }
        public async Task RemovePrivilege(Guid privilegeID, CancellationToken cancellationToken = default)
        {
            await _imp.RemovePrivilege(this, privilegeID, cancellationToken);
        }
        public async Task RemovePrivileges(IEnumerable<Guid> privilegeIDs, CancellationToken cancellationToken = default)
        {
            await _imp.RemovePrivileges(this, privilegeIDs, cancellationToken);
        }

        public async Task<QueryResult<Privilege>> QueryPrivileges(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _imp.QueryPrivileges(this, page, pageSize, cancellationToken);
        }


    }

    public interface IRoleIMP
    {
        Task Add(Role role, CancellationToken cancellationToken = default);
        Task Update(Role role, CancellationToken cancellationToken = default);
        Task Delete(Role role, CancellationToken cancellationToken = default);
        Task AddPrivilege(Role role, Guid privilegeID, CancellationToken cancellationToken = default);
        Task AddPrivileges(Role role, IEnumerable<Guid> privilegeIDs, CancellationToken cancellationToken = default);
        Task RemovePrivilege(Role role, Guid privilegeID, CancellationToken cancellationToken = default);
        Task RemovePrivileges(Role role, IEnumerable<Guid> privilegeIDs, CancellationToken cancellationToken = default);
        Task<QueryResult<Privilege>> QueryPrivileges(Role role,int page,int pageSize, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IRoleIMP), Scope = InjectionScope.Transient)]
    public class RoleIMP : IRoleIMP
    {
        private readonly IRoleStore _roleStore;
        private readonly IPrivilegeStore _privilegeStore;

        public RoleIMP(IRoleStore roleStore, IPrivilegeStore privilegeStore)
        {
            _roleStore = roleStore;
            _privilegeStore = privilegeStore;
        }

        public async Task Add(Role role, CancellationToken cancellationToken = default)
        {
            var existRole=await _roleStore.QueryByName(role.Name, cancellationToken);
            if (existRole!=null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.ExistRoleByName,
                    DefaultFormatting = "已经存在名称为{0}的角色",
                    ReplaceParameters = new List<object>() { role.Name }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.ExistRoleByName, fragment);
            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
               
                await _roleStore.Add(role, cancellationToken);
                //检查是否有名称重复的
                var newId = await _roleStore.QueryByNameNoLock(role.Name, cancellationToken);
                if (newId != null && role.ID != newId)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.ExistRoleByName,
                        DefaultFormatting = "已经存在名称为{0}的角色",
                        ReplaceParameters = new List<object>() { role.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.ExistRoleByName, fragment);
                }
                scope.Complete();
            }

        }

        public async Task AddPrivilege(Role role, Guid privilegeID, CancellationToken cancellationToken = default)
        {
            await _roleStore.AddRolePrivilege(role.ID, privilegeID, cancellationToken);
        }

        public async Task AddPrivileges(Role role, IEnumerable<Guid> privilegeIDs, CancellationToken cancellationToken = default)
        {
            List<(Guid, Guid)> rps = new List<(Guid, Guid)>();
            foreach(var item in privilegeIDs)
            {
                rps.Add((role.ID, item));
            }
            await _roleStore.AddRolePrivilege(rps, cancellationToken);
        }

        public async Task Delete(Role role, CancellationToken cancellationToken = default)
        {
            await _roleStore.Delete(role.ID, cancellationToken);
        }

        public async Task<QueryResult<Privilege>> QueryPrivileges(Role role, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _privilegeStore.QueryByRolePage(role.ID, page, pageSize, cancellationToken);
        }

        public async Task RemovePrivilege(Role role, Guid privilegeID, CancellationToken cancellationToken = default)
        {
            await _roleStore.DeleteRolePrivilege(role.ID, privilegeID, cancellationToken);
        }

        public async Task RemovePrivileges(Role role, IEnumerable<Guid> privilegeIDs, CancellationToken cancellationToken = default)
        {
            List<(Guid, Guid)> rps = new List<(Guid, Guid)>();
            foreach (var item in privilegeIDs)
            {
                rps.Add((role.ID, item));
            }
            await _roleStore.DeleteRolePrivilege(rps, cancellationToken);
        }

        public async Task Update(Role role, CancellationToken cancellationToken = default)
        {
            var existRole = await _roleStore.QueryByName(role.Name, cancellationToken);
            if (existRole != null && existRole.ID!= role.ID)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.ExistRoleByName,
                    DefaultFormatting = "已经存在名称为{0}的角色",
                    ReplaceParameters = new List<object>() { role.Name }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.ExistRoleByName, fragment);
            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {

                await _roleStore.Update(role, cancellationToken);
                //检查是否有名称重复的
                var newId = await _roleStore.QueryByNameNoLock(role.Name, cancellationToken);
                if (newId != null && role.ID != newId)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.ExistRoleByName,
                        DefaultFormatting = "已经存在名称为{0}的角色",
                        ReplaceParameters = new List<object>() { role.Name }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.ExistRoleByName, fragment);
                }
                scope.Complete();
            }

        }
    }

}
