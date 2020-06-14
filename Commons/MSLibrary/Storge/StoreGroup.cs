using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Collections.Hash;
using MSLibrary.LanguageTranslate;
using MSLibrary.Storge.DAL;

namespace MSLibrary.Storge
{
    /// <summary>
    /// 存储组
    /// 将存储信息归类
    /// </summary>
    public class StoreGroup : EntityBase<IStoreGroupIMP>
    {
        private static IFactory<IStoreGroupIMP> _storeGroupIMPFactory;

        public static IFactory<IStoreGroupIMP> StoreGroupIMPFactory
        {
            set
            {
                _storeGroupIMPFactory = value;
            }
        }
        public override IFactory<IStoreGroupIMP> GetIMPFactory()
        {
            return _storeGroupIMPFactory;
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

        public async Task<StoreGroupMember> ChooseMember( params string[] keys)
        {
            return await _imp.ChooseMember(this, keys);
        }
        public async Task<StoreGroupMember> GetMember(string memberName)
        {
            return await _imp.GetMember(this, memberName);
        }



    }

    public interface IStoreGroupIMP
    {
        Task<StoreGroupMember> ChooseMember(StoreGroup group,params string[] keys);
        Task<StoreGroupMember> GetMember(StoreGroup group,string memberName);

    }


    [Injection(InterfaceType = typeof(IStoreGroupIMP), Scope = InjectionScope.Transient)]
    public class StoreGroupIMP : IStoreGroupIMP
    {
        private IHashGroupRepositoryCacheProxy _hashGroupRepositoryCacheProxy;
        private IStoreGroupMemberStore _storeGroupMemberStore;

        private ConcurrentDictionary<string, StoreGroupMember> _members = new ConcurrentDictionary<string, StoreGroupMember>();


        public StoreGroupIMP(IHashGroupRepositoryCacheProxy hashGroupRepositoryCacheProxy, IStoreGroupMemberStore storeGroupMemberStore)
        {
            _hashGroupRepositoryCacheProxy = hashGroupRepositoryCacheProxy;
            _storeGroupMemberStore = storeGroupMemberStore;
        }
        public async Task<StoreGroupMember> ChooseMember(StoreGroup group, params string[] keys)
        {
            //需要在HashGroup中存在名称为StoreFroup-{group.Name}的哈希组
            string hashGroupName = $"StoreFroup-{group.Name}";
            var hashGroup = await _hashGroupRepositoryCacheProxy.QueryByName(hashGroupName);
            if (hashGroup == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHashGroupByName,
                    DefaultFormatting = "没有找到名称为{0}的一致性哈希组",
                    ReplaceParameters = new List<object>() { hashGroupName }
                };

                throw new UtilityException((int)Errors.NotFoundHashGroupByName, fragment);
            }
            //从哈希组获取实际节点信息，节点信息为MemberName
            var memberName =await hashGroup.GetHashNodeKey(string.Join(".", keys), 1, 2, 3);

            var member=await getMember(group.ID, memberName);
            if (member == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFounStoreGroupMemberByName,
                    DefaultFormatting = "在id为{0}的存储组中找不到名称为{1}的组成员",
                    ReplaceParameters = new List<object>() { group.ID.ToString(), memberName }
                };

                throw new UtilityException((int)Errors.NotFounStoreGroupMemberByName, fragment);
            }
            return member;
        }

        public async Task<StoreGroupMember> GetMember(StoreGroup group, string memberName)
        {
            var member = await getMember(group.ID, memberName);
            return member;
        }

        private async Task<StoreGroupMember> getMember(Guid groupID,string memberName)
        {
            if (!_members.TryGetValue(memberName, out StoreGroupMember member))
            {
                member = await _storeGroupMemberStore.QueryByName(groupID, memberName);
                if (member != null)
                {
                    _members[memberName] = member;
                }

            }
            return member;
        }
    }
}
