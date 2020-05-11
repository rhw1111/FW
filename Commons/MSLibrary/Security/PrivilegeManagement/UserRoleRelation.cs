using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Security.PrivilegeManagement
{
    public class UserRoleRelation : EntityBase<IUserRoleRelationIMP>
    {
        private static IFactory<IUserRoleRelationIMP> _userRoleRelationIMPFactory;
        public static IFactory<IUserRoleRelationIMP> UserRoleRelationIMPFactory
        {
            set
            {
                _userRoleRelationIMPFactory = value;
            }
        }
        public override IFactory<IUserRoleRelationIMP> GetIMPFactory()
        {
            return _userRoleRelationIMPFactory;
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
        /// 用户关联的角色
        /// </summary>
        public Role Role
        {
            get
            {
                return GetAttribute<Role>("Role");
            }
            set
            {
                SetAttribute<Role>("Role", value);
            }
        }

        /// <summary>
        /// 用户关键字
        /// </summary>
        public string UserKey
        {
            get
            {
                return GetAttribute<string>("UserKey");
            }
            set
            {
                SetAttribute<string>("UserKey", value);
            }
        }

        /// <summary>
        /// 用户所属系统
        /// </summary>
        public PrivilegeSystem System
        {
            get
            {
                return GetAttribute<PrivilegeSystem>("System");
            }
            set
            {
                SetAttribute<PrivilegeSystem>("System", value);
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

    }


    public interface IUserRoleRelationIMP
    {

    }

    [Injection(InterfaceType = typeof(IUserRoleRelationIMP), Scope = InjectionScope.Transient)]
    public class UserRoleRelationIMP : IUserRoleRelationIMP
    {
        public UserRoleRelationIMP()
        {
        }
    }
}
