using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Collections.Hash.DAL;

namespace MSLibrary.Collections.Hash
{
    /// <summary>
    /// 哈希组策略
    /// 为哈希节点分布的策略
    /// </summary>
    public class HashGroupStrategy : EntityBase<IHashGroupStrategyIMP>
    {
        private static IFactory<IHashGroupStrategyIMP> _hashGroupStrategyIMPFactory;

        public static IFactory<IHashGroupStrategyIMP> HashGroupStrategyIMPFactory
        {
            set
            {
                _hashGroupStrategyIMPFactory = value;
            }
        }

        public override IFactory<IHashGroupStrategyIMP> GetIMPFactory()
        {
            return _hashGroupStrategyIMPFactory;
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
        /// 策略名称
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
        /// 具体执行的策略服务的完整类型名称（包括程序集名称）
        /// 该类型必须实现IFactory<IHashGroupStrategyService>接口
        /// </summary>
        public string StrategyServiceFactoryType
        {
            get
            {
                return GetAttribute<string>("StrategyServiceFactoryType");
            }
            set
            {
                SetAttribute<string>("StrategyServiceFactoryType", value);
            }
        }

        /// <summary>
        /// 执行的策略服务类型是否使用DI容器
        /// </summary>
        public bool StrategyServiceFactoryTypeUseDI
        {
            get
            {
                return GetAttribute<bool>("StrategyServiceFactoryTypeUseDI");
            }
            set
            {
                SetAttribute<bool>("StrategyServiceFactoryTypeUseDI", value);
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

    public interface IHashGroupStrategyIMP
    {
        Task Add(HashGroupStrategy strategy);
        Task Update(HashGroupStrategy strategy);
        Task Delete(HashGroupStrategy strategy);
    }

    [Injection(InterfaceType = typeof(IHashGroupStrategyIMP), Scope = InjectionScope.Transient)]
    public class HashGroupStrategyIMP : IHashGroupStrategyIMP
    {

        private IHashGroupStrategyStore _hashGroupStrategyStore;

        public HashGroupStrategyIMP(IHashGroupStrategyStore hashGroupStrategyStore)
        {
            _hashGroupStrategyStore = hashGroupStrategyStore;
        }

        public async Task Add(HashGroupStrategy strategy)
        {
            await _hashGroupStrategyStore.Add(strategy);
        }

        public async Task Delete(HashGroupStrategy strategy)
        {
            await _hashGroupStrategyStore.Delete(strategy.ID);
        }

        public async Task Update(HashGroupStrategy strategy)
        {
            await _hashGroupStrategyStore.Update(strategy);
        }
    }

    /// <summary>
    /// 哈希组策略服务
    /// 用于根据传入的关键字获取节点关键字
    /// </summary>
    public interface IHashGroupStrategyService
    {
       
        Task<string> GetHashNodeKey(HashGroup group, string key, params int[] status);

   
        string GetHashNodeKeySync(HashGroup group, string key, params int[] status);
    }
}
