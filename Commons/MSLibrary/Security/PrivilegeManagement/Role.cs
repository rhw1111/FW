using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.PrivilegeManagement.DAL;

namespace MSLibrary.Security.PrivilegeManagement
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : EntityBase<IRoleIMP>
    {
        private static IFactory<IRoleIMP> _roleIMPFactory;

        public static IFactory<IRoleIMP> RoleIMPFactory
        {
            set {
                _roleIMPFactory = value;
            }
        }

        public override IFactory<IRoleIMP> GetIMPFactory()
        {
            return _roleIMPFactory;
        }


        /// <summary>
        /// id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }


        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }

        /// <summary>
        /// 所属系统Id
        /// </summary>
        public Guid SystemId
        {
            get
            {
                return GetAttribute<Guid>("SystemId");
            }
            set
            {
                SetAttribute<Guid>("SystemId", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public async Task Update()
        {
            await _imp.Update(this);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        /// <summary>
        /// 增加用户关联关系
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task AddUserRelation(UserRoleRelation relation)
        {
            await _imp.AddUserRelation(this,relation);
        }

        /// <summary>
        /// 删除用户关联关系
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task DeleteUserRelation(Guid relationId)
        {
            await _imp.DeleteUserRelation(this,relationId);
        }

        public async Task UpdateUserRelation(UserRoleRelation relation)
        {
            await _imp.UpdateUserRelation(this,relation);
        }

        /// <summary>
        /// 获取关联的用户关联关系
        /// </summary>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public async Task<UserRoleRelation> GetUserRelation(Guid relationId)
        {
            return await _imp.GetUserRelation(this,relationId); 
        }

        /// <summary>
        /// 根据用户关键字匹配，分页获取关联的用户关联关系
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<UserRoleRelation>> GetUserRelation(string userKey,int page,int pageSize)
        {
            return await _imp.GetUserRelation(this,userKey,page,pageSize);
        }


        /// <summary>
        /// 新增权限关联关系
        /// </summary>
        /// <param name="privilegeId"></param>
        /// <returns></returns>
        public async Task AddPrivilegeRelation(Guid privilegeId)
        {
            await _imp.AddPrivilegeRelation(this,privilegeId);
        }

        /// <summary>
        /// 移除权限关联关系
        /// </summary>
        /// <param name="privilegeId"></param>
        /// <returns></returns>
        public async Task RemovePrivilegeRelation(Guid privilegeId)
        {
            await _imp.RemovePrivilegeRelation(this,privilegeId);
        }

        /// <summary>
        /// 获取关联的所有权限
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetAllPrivilege(Func<Privilege,Task> callback)
        {
            await _imp.GetAllPrivilege(this,callback);
        }

        /// <summary>
        /// 分页获取关联的权限
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<Privilege>> GetPrivilege(int page, int pageSize)
        {
            return await _imp.GetPrivilege(this,page,pageSize);
        }



    }

    public interface IRoleIMP
    {
        Task Add(Role role);
        Task Update(Role role);
        Task Delete(Role role);

        Task AddUserRelation(Role role, UserRoleRelation relation);
        Task DeleteUserRelation(Role role, Guid relationId);
        Task UpdateUserRelation(Role role, UserRoleRelation relation);
        Task<UserRoleRelation> GetUserRelation(Role role,Guid relationId);

        Task<QueryResult<UserRoleRelation>> GetUserRelation(Role role, string userKey, int page, int pageSize);
        Task AddPrivilegeRelation(Role role, Guid privilegeId);
        Task RemovePrivilegeRelation(Role role, Guid privilegeId);
        Task GetAllPrivilege(Role role, Func<Privilege, Task> callback);
        Task<QueryResult<Privilege>> GetPrivilege(Role role, int page, int pageSize);
    }

    [Injection(InterfaceType = typeof(IRoleIMP), Scope = InjectionScope.Transient)]
    public class RoleIMP : IRoleIMP
    {
        private IRoleStore _roleStore;
        private IPrivilegeStore _privilegeStore;
        private IUserRoleRelationStore _userRoleRelationStore;

        public RoleIMP(IRoleStore roleStore, IPrivilegeStore privilegeStore, IUserRoleRelationStore userRoleRelationStore)
        {
            _roleStore = roleStore;
            _privilegeStore = privilegeStore;
            _userRoleRelationStore = userRoleRelationStore;
        }
        public async Task Add(Role role)
        {
            await _roleStore.Add(role);
        }

        public async Task AddPrivilegeRelation(Role role, Guid privilegeId)
        {
            await _privilegeStore.AddPrivilegeRelation(role.ID, privilegeId);
        }

        public async Task AddUserRelation(Role role, UserRoleRelation relation)
        {
            relation.Role = role;
            await _userRoleRelationStore.Add(relation);
        }

        public async Task Delete(Role role)
        {
            await _roleStore.Delete(role.ID);
        }

        public async Task DeleteUserRelation(Role role, Guid relationId)
        {
            await _userRoleRelationStore.Delete(role.ID,relationId);
        }

        public async Task GetAllPrivilege(Role role, Func<Privilege, Task> callback)
        {
            await _privilegeStore.QueryByRole(role.ID, callback);
        }

        public async Task<QueryResult<Privilege>> GetPrivilege(Role role, int page, int pageSize)
        {
            return await _privilegeStore.QueryByRole(role.ID, page, pageSize);
        }

        public async Task<UserRoleRelation> GetUserRelation(Role role, Guid relationId)
        {
            return await _userRoleRelationStore.QueryByRole(role.ID,relationId);
        }

        public async Task<QueryResult<UserRoleRelation>> GetUserRelation(Role role, string userKey, int page, int pageSize)
        {
            return await _userRoleRelationStore.QueryByRoleAndUserKey(role.ID,userKey, page, pageSize);
        }

        public async Task RemovePrivilegeRelation(Role role, Guid privilegeId)
        {
             await _privilegeStore.DeletePrivilegeRelation(role.ID, privilegeId);
        }

        public async Task Update(Role role)
        {
            await _roleStore.Update(role);
        }

        public async Task UpdateUserRelation(Role role, UserRoleRelation relation)
        {
            relation.Role = role;
            await _userRoleRelationStore.Update(relation);
        }
    }
}
