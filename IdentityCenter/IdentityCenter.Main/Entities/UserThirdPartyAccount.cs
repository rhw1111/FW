using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace IdentityCenter.Main.Entities
{
    public class UserThirdPartyAccount : EntityBase<IUserThirdPartyAccountIMP>
    {
        private static IFactory<IUserThirdPartyAccountIMP>? _userThirdPartyAccountIMPFactory;

        public static IFactory<IUserThirdPartyAccountIMP> UserThirdPartyAccountIMPFactory
        {
            set
            {
                _userThirdPartyAccountIMPFactory = value;
            }
        }
        public override IFactory<IUserThirdPartyAccountIMP>? GetIMPFactory()
        {
            return _userThirdPartyAccountIMPFactory;
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
        /// 来源
        /// </summary>
        public string Source
        {
            get
            {

                return GetAttribute<string>(nameof(Source));
            }
            set
            {
                SetAttribute<string>(nameof(Source), value);
            }
        }
        /// <summary>
        /// 第三方ID
        /// </summary>
        public string ThirdPartyID
        {
            get
            {

                return GetAttribute<string>(nameof(ThirdPartyID));
            }
            set
            {
                SetAttribute<string>(nameof(ThirdPartyID), value);
            }
        }

        /// <summary>
        /// 所属用户账号ID
        /// </summary>
        public Guid AccountID
        {
            get
            {

                return GetAttribute<Guid>(nameof(AccountID));
            }
            set
            {
                SetAttribute<Guid>(nameof(AccountID), value);
            }
        }

        /// <summary>
        /// 所属用户账号
        /// </summary>
        public UserAccount Account
        {
            get
            {

                return GetAttribute<UserAccount>(nameof(Account));
            }
            set
            {
                SetAttribute<UserAccount>(nameof(Account), value);
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

    }

    public interface IUserThirdPartyAccountIMP
    {

    }


    [Injection(InterfaceType = typeof(IUserThirdPartyAccountIMP), Scope = InjectionScope.Transient)]
    public class UserThirdPartyAccountIMP : IUserThirdPartyAccountIMP
    {

    }
}
