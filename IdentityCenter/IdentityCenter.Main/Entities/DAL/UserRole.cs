using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;

namespace IdentityCenter.Main.Entities.DAL
{
    public class UserRole: ModelBase
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
        /// UserID
        /// </summary>
        public Guid UserID
        {
            get
            {

                return GetAttribute<Guid>(nameof(UserID));
            }
            set
            {
                SetAttribute<Guid>(nameof(UserID), value);
            }
        }

        public UserAccount User
        {
            get
            {

                return GetAttribute<UserAccount>(nameof(User));
            }
            set
            {
                SetAttribute<UserAccount>(nameof(User), value);
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
