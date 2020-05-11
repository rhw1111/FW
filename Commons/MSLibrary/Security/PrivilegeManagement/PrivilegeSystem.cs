using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.PrivilegeManagement.DAL;

namespace MSLibrary.Security.PrivilegeManagement
{
    /// <summary>
    /// 权限系统
    /// 用来区分权限所属的系统
    /// </summary>
    public class PrivilegeSystem : EntityBase<IPrivilegeSystemIMP>
    {
        private static IFactory<IPrivilegeSystemIMP> _privilegeSystemIMPFactory;

        public static IFactory<IPrivilegeSystemIMP> PrivilegeSystemIMPFactory
        {
           set
            {
                _privilegeSystemIMPFactory = value;
            }
        }

        public override IFactory<IPrivilegeSystemIMP> GetIMPFactory()
        {
            return _privilegeSystemIMPFactory;
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
        /// 系统名称
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

    public interface IPrivilegeSystemIMP
    {
        Task Add(PrivilegeSystem system);
        Task Update(PrivilegeSystem system);
        Task Delete(PrivilegeSystem system);
    }


    [Injection(InterfaceType = typeof(IPrivilegeSystemIMP), Scope = InjectionScope.Transient)]
    public class PrivilegeSystemIMP : IPrivilegeSystemIMP
    {
        private IPrivilegeSystemStore _privilegeSystemStore;

        public PrivilegeSystemIMP(IPrivilegeSystemStore privilegeSystemStore)
        {
            _privilegeSystemStore = privilegeSystemStore;
        }

        public async Task Add(PrivilegeSystem system)
        {
            await _privilegeSystemStore.Add(system);
        }

        public async Task Delete(PrivilegeSystem system)
        {
            await _privilegeSystemStore.Delete(system.ID);
        }

        public async Task Update(PrivilegeSystem system)
        {
            await _privilegeSystemStore.Update(system);
        }
    }
}
