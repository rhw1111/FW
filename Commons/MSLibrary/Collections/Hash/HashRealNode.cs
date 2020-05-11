using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Collections.Hash
{
    /// <summary>
    /// 一致性哈希节点(真实节点)
    /// </summary>
    public class HashRealNode : EntityBase<IHashRealNodeIMP>
    {
        private static IFactory<IHashRealNodeIMP> _hashRealNodeIMPFactory;

        public static IFactory<IHashRealNodeIMP> HashRealNodeIMPFactory
        {
            set
            {
                _hashRealNodeIMPFactory = value;
            }
        }
        public override IFactory<IHashRealNodeIMP> GetIMPFactory()
        {
            return _hashRealNodeIMPFactory;
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
        /// 所属一致性哈希组
        /// </summary>
        public HashGroup Group
        {
            get
            {
                return GetAttribute<HashGroup>("Group");
            }
            set
            {
                SetAttribute<HashGroup>("Group", value);
            }
        }

        /// <summary>
        /// 所属一致性哈希组ID
        /// </summary>
        public Guid GroupId
        {
            get
            {
                return GetAttribute<Guid>("GroupId");
            }
            set
            {
                SetAttribute<Guid>("GroupId", value);
            }
        }


        /// <summary>
        /// 节点关键字
        /// 按实际业务可以表示不同的含义，例如可以表示数据库中表的名称
        /// </summary>
        public string NodeKey
        {
            get
            {
                return GetAttribute<string>("NodeKey");
            }
            set
            {
                SetAttribute<string>("NodeKey", value);
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


    public interface IHashRealNodeIMP
    {

    }

    [Injection(InterfaceType = typeof(IHashRealNodeIMP), Scope = InjectionScope.Transient)]
    public class HashRealNodeIMP:IHashRealNodeIMP
    {

    }
}
