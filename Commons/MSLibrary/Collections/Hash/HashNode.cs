using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Collections.Hash
{
    /// <summary>
    /// 一致性哈希节点
    /// </summary>
    public class HashNode : EntityBase<IHashNodeIMP>
    {
        private static IFactory<IHashNodeIMP> _hashNodeIMPFactory;
        public static IFactory<IHashNodeIMP> HashNodeIMPFactory
        {
            set
            {
                _hashNodeIMPFactory = value;
            }
        }
        public override IFactory<IHashNodeIMP> GetIMPFactory()
        {
            return _hashNodeIMPFactory;
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
        /// 用来标识唯一节点
        /// </summary>
        public long Code
        {
            get
            {
                return GetAttribute<long>("Code");
            }
            set
            {
                SetAttribute<long>("Code", value);
            }
        }


        public Guid RealNodeId
        {
            get
            {
                return GetAttribute<Guid>("RealNodeId");
            }
            set
            {
                SetAttribute<Guid>("RealNodeId", value);
            }
        }


        public HashRealNode RealNode
        {
            get
            {
                return GetAttribute<HashRealNode>("HashRealNode");
            }
            set
            {
                SetAttribute<HashRealNode>("HashRealNode", value);
            }
        }


        /// <summary>
        /// 状态
        /// 按照实际业务状态可以设置不同的值
        /// 如0：新节点，1：原有节点，2：原有节点，需要重新计算,3：待删除，4：删除，5：已经删除
        /// </summary>
        public int Status
        {
            get
            {
                return GetAttribute<int>("Status");
            }
            set
            {
                SetAttribute<int>("Status", value);
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

    public interface IHashNodeIMP
    {

    }

    [Injection(InterfaceType = typeof(IHashNodeIMP), Scope = InjectionScope.Transient)]
    public class HashNodeIMP : IHashNodeIMP
    {

    }
}
