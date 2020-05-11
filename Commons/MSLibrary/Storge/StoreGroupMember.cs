using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;

namespace MSLibrary.Storge
{
    /// <summary>
    /// 存储组成员
    /// 管理存储的相关信息
    /// </summary>
    public class StoreGroupMember : EntityBase<IStoreGroupMemberIMP>
    {
        private static IFactory<IStoreGroupMemberIMP> _storeGroupMemberIMPFactory;

        public static IFactory<IStoreGroupMemberIMP> StoreGroupMemberIMPFactory
        {
            set
            {
                _storeGroupMemberIMPFactory = value;
            }
        }
        public override IFactory<IStoreGroupMemberIMP> GetIMPFactory()
        {
            return _storeGroupMemberIMPFactory;
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
        /// 名称
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
        /// 所属组
        /// </summary>
        public StoreGroup Group
        {
            get
            {
                return GetAttribute<StoreGroup>("Group");
            }
            set
            {
                SetAttribute<StoreGroup>("Group", value);
            }
        }

        /// <summary>
        /// 所属组ID
        /// </summary>
        public Guid GroupID
        {
            get
            {
                return GetAttribute<Guid>("GroupID");
            }
            set
            {
                SetAttribute<Guid>("GroupID", value);
            }
        }



        /// <summary>
        /// 存储信息
        /// </summary>
        public string StoreInfo
        {
            get
            {
                return GetAttribute<string>("StoreInfo");
            }
            set
            {
                SetAttribute<string>("StoreInfo", value);
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

        public async Task<T> GetStoreInfo<T>()
        {
            return await _imp.GetStoreInfo<T>(this);
        }
    }

    public interface IStoreGroupMemberIMP
    {
        Task<T> GetStoreInfo<T>(StoreGroupMember member);
    }

    [Injection(InterfaceType = typeof(IStoreGroupMemberIMP), Scope = InjectionScope.Transient)]
    public class StoreGroupMemberIMP : IStoreGroupMemberIMP
    {
        public async Task<T> GetStoreInfo<T>(StoreGroupMember member)
        {
            return await Task.FromResult(JsonSerializerHelper.Deserialize<T>(member.StoreInfo));
        }
    }
}
