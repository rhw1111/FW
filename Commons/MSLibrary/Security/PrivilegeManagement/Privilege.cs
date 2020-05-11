using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.PrivilegeManagement.DAL;

namespace MSLibrary.Security.PrivilegeManagement
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Privilege : EntityBase<IPrivilegeIMP>
    {
        private static IFactory<IPrivilegeIMP> _privilegeIMPFactory;

        public static IFactory<IPrivilegeIMP> PrivilegeIMPFactory
        {
            set
            {
                _privilegeIMPFactory = value;
            }
        }
        public override IFactory<IPrivilegeIMP> GetIMPFactory()
        {
            return _privilegeIMPFactory;
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
        /// 权限名称
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
        /// 权限描述
        /// </summary>
        public string Description
        {
            get
            {
                return GetAttribute<string>("Description");
            }
            set
            {
                SetAttribute<string>("Description", value);
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
    }

    public interface IPrivilegeIMP
    {
        Task Add(Privilege privilege);
        Task Update(Privilege privilege);
        Task Delete(Privilege privilege);
    }

    [Injection(InterfaceType = typeof(IPrivilegeIMP), Scope = InjectionScope.Transient)]
    public class PrivilegeIMP : IPrivilegeIMP
    {
        private IPrivilegeStore _privilegeStore;

        public PrivilegeIMP(IPrivilegeStore privilegeStore)
        {
            _privilegeStore = privilegeStore;
        }
        public async Task Add(Privilege privilege)
        {
            await _privilegeStore.Add(privilege);
        }

        public async Task Delete(Privilege privilege)
        {
            await _privilegeStore.Delete(privilege.ID);
        }

        public async Task Update(Privilege privilege)
        {
            await _privilegeStore.Update(privilege);
        }
    }
}
