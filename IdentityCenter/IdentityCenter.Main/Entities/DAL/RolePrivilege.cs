using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;

namespace IdentityCenter.Main.Entities.DAL
{
    public class RolePrivilege:ModelBase
    {

        /// <summary>
        /// RoleId
        /// </summary>
        public Guid RoleID
        {
            get
            {

                return GetAttribute<Guid>(nameof(RoleID));
            }
            set
            {
                SetAttribute<Guid>(nameof(RoleID), value);
            }
        }

        public Role Role
        {
            get
            {

                return GetAttribute<Role>(nameof(Role));
            }
            set
            {
                SetAttribute<Role>(nameof(Role), value);
            }
        }

        /// <summary>
        /// PrivilegeID
        /// </summary>
        public Guid PrivilegeID
        {
            get
            {

                return GetAttribute<Guid>(nameof(PrivilegeID));
            }
            set
            {
                SetAttribute<Guid>(nameof(PrivilegeID), value);
            }
        }

        public Privilege Privilege
        {
            get
            {

                return GetAttribute<Privilege>(nameof(Privilege));
            }
            set
            {
                SetAttribute<Privilege>(nameof(Privilege), value);
            }
        }

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

    }
}
