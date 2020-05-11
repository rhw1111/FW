using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Collections.Hash.DAL;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Collections.Hash
{
    /// <summary>
    /// 一致性哈希组
    /// </summary>
    public class HashGroup : EntityBase<IHashGroupIMP>
    {
        private static IFactory<IHashGroupIMP> _hashGroupIMPFactory;

        public static IFactory<IHashGroupIMP> HashGroupIMPFactory
        {
            set
            {
                _hashGroupIMPFactory = value;
            }
        }
        public override IFactory<IHashGroupIMP> GetIMPFactory()
        {
            return _hashGroupIMPFactory;
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
        /// 类型
        /// 用于哈希组归类
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }

        /// <summary>
        /// 哈希总数
        /// 关键字将以该数作为基数计算
        /// </summary>
        public long Count
        {
            get
            {
                return GetAttribute<long>("Count");
            }
            set
            {
                SetAttribute<long>("Count", value);
            }
        }


        /// <summary>
        /// 所使用的策略id
        /// </summary>
        public Guid StrategyID
        {
            get
            {
                return GetAttribute<Guid>("StrategyID");
            }
            set
            {
                SetAttribute<Guid>("StrategyID", value);
            }
        }

        /// <summary>
        /// 所使用的策略
        /// </summary>
        public HashGroupStrategy Strategy
        {
            get
            {
                return GetAttribute<HashGroupStrategy>("Strategy");
            }
            set
            {
                SetAttribute<HashGroupStrategy>("Strategy", value);
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

        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        /// <summary>
        /// 为该哈希组新增哈希节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task AddNode(HashNode node)
        {
            await _imp.AddNode(this, node);
        }

        /// <summary>
        /// 为该哈希组修改哈希节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task UpdateNode(HashNode node)
        {
            await _imp.UpdateNode(this, node);
        }

        /// <summary>
        /// 为该哈希组更新哈希节点状态
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task UpdateNodeStatus(HashNode node)
        {
            await _imp.UpdateNodeStatus(node);
        }
        /// <summary>
        /// 为该哈希组删除哈希节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public async Task DeleteNode(Guid nodeId)
        {
            await _imp.DeleteNode(this, nodeId);
        }



        /// <summary>
        /// 为该哈希组新增真实哈希节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task AddRealNode(HashRealNode node)
        {
            await _imp.AddRealNode(this, node);
        }

        /// <summary>
        /// 为该哈希组修改真实哈希节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task UpdateRealNode(HashRealNode node)
        {
            await _imp.UpdateRealNode(this, node);
        }
        /// <summary>
        /// 为该哈希组删除真实哈希节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public async Task DeleteRealNode(Guid nodeId)
        {
            await _imp.DeleteRealNode(this, nodeId);
        }

        /// <summary>
        /// 根据传入的key和指定状态找到按对应策略运算得出的节点关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<string> GetHashNodeKey(string key, params int[] status)
        {
            return await _imp.GetHashNodeKey(this, key, status);
        }


        /// <summary>
        /// 根据传入的key和指定状态找到按对应策略运算得出的节点关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <returns></returns>

        /// <summary>
        /// 根据传入的key和指定状态找到按对应策略运算得出的节点关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetHashNodeKeySync(string key, params int[] status)
        {
            return _imp.GetHashNodeKeySync(this, key, status);
        }



        /// <summary>
        /// 获取指定状态的所有节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetHashNode(int status, Func<HashNode, Task> callback)
        {
            await _imp.GetHashNode(this, status, callback);
        }

        /// <summary>
        /// 分页获取所有节点
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<HashNode>> GetHashNode(int page, int pageSize)
        {
            return await _imp.GetHashNode(this, page, pageSize);
        }

        /// <summary>
        /// 获取指定节点的第一个小于指定节点Code值的节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task<HashNode> GetFirstLessNode(HashNode node)
        {
            return await _imp.GetFirstLessNode(this, node);
        }

        /// <summary>
        /// 获取指定节点的第一个大于指定节点Code值的节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task<HashNode> GetFirstGreaterNode(HashNode node)
        {
            return await _imp.GetFirstGreaterNode(this, node);
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public async Task<HashNode> GetHashNode(Guid nodeId)
        {
            return await _imp.GetHashNode(this, nodeId);
        }


        /// <summary>
        /// 获取指定所有真实节点
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetHashRealNode(Func<HashRealNode, Task> callback)
        {
            await _imp.GetHashRealNode(this, callback);
        }
        /// <summary>
        /// 分页获取所有真实节点
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<HashRealNode>> GetHashRealNode(int page, int pageSize)
        {
            return await _imp.GetHashRealNode(this, page, pageSize);
        }

        /// <summary>
        /// 跳过指定的数量后获取指定数量的真实节点
        /// </summary>
        /// <param name="skipNum"></param>
        /// <param name="takeNum"></param>
        /// <returns></returns>
        public async Task<List<HashRealNode>> GetHashRealNodeByCreateTime(int skipNum, int takeNum)
        {
            return await _imp.GetHashRealNodeByCreateTime(this, skipNum, takeNum);
        }

        /// <summary>
        /// 根据Id获取真实节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public async Task<HashRealNode> GetHashRealNode(Guid nodeId)
        {
            return await _imp.GetHashRealNode(this, nodeId);
        }


        public async Task<List<HashNode>> GetHashNodeOrderByCode(int skipNum, int takeNum)
        {
            return await _imp.GetHashNodeOrderByCode(this, skipNum, takeNum);
        }

        /// <summary>
        /// 获取第一个code小于指定code的指定状态的节点
        /// 如果找不到，返回null
        /// </summary>
        /// <param name="group"></param>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<HashNode> GetFirstByLessCode(long code, params int[] status)
        {
            return await _imp.GetFirstByLessCode(this, code, status);
        }


        /// <summary>
        /// 获取第一个code大于指定code的指定状态的节点
        /// 如果找不到，返回null
        /// </summary>
        /// <param name="group"></param>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<HashNode> GetFirstByGreaterCode(long code, params int[] status)
        {
            return await _imp.GetFirstByGreaterCode(this, code, status);
        }

        /// <summary>
        /// 获取指定状态的最小code的节点
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<HashNode> GetMinCode(params int[] status)
        {
            return await _imp.GetMinCode(this, status);
        }


        /// <summary>
        /// 获取指定状态的最大code的节点
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<HashNode> GetMaxCode(params int[] status)
        {
            return await _imp.GetMaxCode(this, status);
        }

        /// <summary>
        /// 分页获取指定状态的节点
        /// </summary>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<HashNode>> GetHashNode(int status, int page, int pageSize)
        {
            return await _imp.GetHashNode(this, status, page, pageSize);
        }
    }


    public interface IHashGroupIMP
    {
        Task Add(HashGroup group);
        Task Update(HashGroup group);
        Task Delete(HashGroup group);
        Task AddNode(HashGroup group, HashNode node);
        Task UpdateNode(HashGroup group, HashNode node);
        Task UpdateNodeStatus(HashNode node);
        Task DeleteNode(HashGroup group, Guid nodeId);

        Task AddRealNode(HashGroup group, HashRealNode node);
        Task UpdateRealNode(HashGroup group, HashRealNode node);
        Task DeleteRealNode(HashGroup group, Guid nodeId);


        /// <summary>
        /// 根据传入的key和指定状态找到按对应策略运算得出的节点关键字
        /// </summary>
        /// <param name="group"></param>
        /// <param name="status"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetHashNodeKey(HashGroup group, string key, params int[] status);



        /// <summary>
        /// 根据传入的key和指定状态找到按对应策略运算得出的节点关键字（同步）
        /// </summary>
        /// <param name="group"></param>
        /// <param name="status"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetHashNodeKeySync(HashGroup group, string key, params int[] status);



        /// <summary>
        /// 获取指定状态的所有节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetHashNode(HashGroup group, int status, Func<HashNode, Task> callback);
        /// <summary>
        /// 分页获取所有节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<HashNode>> GetHashNode(HashGroup group, int page, int pageSize);


        /// <summary>
        /// 分页获取指定状态所有节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<HashNode>> GetHashNode(HashGroup group, int status, int page, int pageSize);




        /// <summary>
        /// 根据Id获取节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<HashNode> GetHashNode(HashGroup group, Guid nodeId);

        /// <summary>
        /// 跳过指定数量后获取指定数量的节点（按code值顺序排序）
        /// </summary>
        /// <param name="group"></param>
        /// <param name="skipNum"></param>
        /// <param name="takeNum"></param>
        /// <returns></returns>
        Task<List<HashNode>> GetHashNodeOrderByCode(HashGroup group, int skipNum, int takeNum);


        /// <summary>
        /// 获取指定所有真实节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetHashRealNode(HashGroup group, Func<HashRealNode, Task> callback);
        /// <summary>
        /// 分页获取所有真实节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<HashRealNode>> GetHashRealNode(HashGroup group, int page, int pageSize);

        /// <summary>
        /// 根据Id获取真实节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<HashRealNode> GetHashRealNode(HashGroup group, Guid nodeId);




        /// <summary>
        /// 获取指定节点的第一个小于指定节点Code值的节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        Task<HashNode> GetFirstLessNode(HashGroup group, HashNode node);

        /// <summary>
        /// 获取指定节点的第一个大于指定节点Code值的节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        Task<HashNode> GetFirstGreaterNode(HashGroup group, HashNode node);

        /// <summary>
        /// 获取第一个code小于指定code的指定状态的节点
        /// 如果找不到，返回null
        /// </summary>
        /// <param name="group"></param>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<HashNode> GetFirstByLessCode(HashGroup group, long code, params int[] status);

        /// <summary>
        /// 获取第一个code大于指定code的指定状态的节点
        /// 如果找不到，返回null
        /// </summary>
        /// <param name="group"></param>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<HashNode> GetFirstByGreaterCode(HashGroup group, long code, params int[] status);

        /// <summary>
        /// 跳过指定数量后获取指定数量的真实节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="skipNum"></param>
        /// <param name="takeNum"></param>
        /// <returns></returns>
        Task<List<HashRealNode>> GetHashRealNodeByCreateTime(HashGroup group, int skipNum, int takeNum);
        /// <summary>
        /// 获取指定状态的最小节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<HashNode> GetMinCode(HashGroup group, params int[] status);
        /// <summary>
        /// 获取指定状态的最大节点
        /// </summary>
        /// <param name="group"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<HashNode> GetMaxCode(HashGroup group, params int[] status);
    }

    [Injection(InterfaceType = typeof(IHashGroupIMP), Scope = InjectionScope.Transient)]
    public class HashGroupIMP : IHashGroupIMP
    {
        private  string _strategyServiceFactoryTypeName;
        public  IFactory<IHashGroupStrategyService> _strategyServiceFactory;
        private  object _lockObj = new object();


        private IHashGroupStore _hashGroupStore;
        private IHashNodeStore _hashNodeStore;
        private IHashRealNodeStore _hashRealNodeStore;

        public HashGroupIMP(IHashGroupStore hashGroupStore, IHashNodeStore hashNodeStore, IHashRealNodeStore hashRealNodeStore)
        {
            _hashGroupStore = hashGroupStore;
            _hashNodeStore = hashNodeStore;
            _hashRealNodeStore = hashRealNodeStore;
        }
        public async Task Add(HashGroup group)
        {
            await _hashGroupStore.Add(group);
        }

        public async Task AddNode(HashGroup group, HashNode node)
        {
            node.Group = group;
            node.GroupId = group.ID;
            await _hashNodeStore.Add(node);
        }

        public async Task Delete(HashGroup group)
        {
            await _hashGroupStore.Delete(group.ID);
        }

        public async Task DeleteNode(HashGroup group, Guid nodeId)
        {
            await _hashNodeStore.DeleteByRelation(group.ID, nodeId);
        }


        public async Task<string> GetHashNodeKey(HashGroup group, string key, params int[] status)
        {
            var service = GetStrategyService(group);
            return await service.GetHashNodeKey(group, key, status);

        }

        private IHashGroupStrategyService GetStrategyService(HashGroup group)
        {
            if (_strategyServiceFactory == null || group.Strategy.StrategyServiceFactoryType != _strategyServiceFactoryTypeName)
            {
                lock (_lockObj)
                {
                    if (_strategyServiceFactory == null || group.Strategy.StrategyServiceFactoryType != _strategyServiceFactoryTypeName)
                    {
                        _strategyServiceFactoryTypeName = group.Strategy.StrategyServiceFactoryType;


                        Type strategyServiceFactoryType = Type.GetType(group.Strategy.StrategyServiceFactoryType);


                        object strategyServiceFactory;
                        if (group.Strategy.StrategyServiceFactoryTypeUseDI == true)
                        {
                            //通过DI容器创建
                            strategyServiceFactory = DIContainerContainer.Get(strategyServiceFactoryType);
                        }
                        else
                        {
                            //通过反射创建
                            strategyServiceFactory = strategyServiceFactoryType.Assembly.CreateInstance(strategyServiceFactoryType.FullName);
                        }

                        if (!(strategyServiceFactory is IFactory<IHashGroupStrategyService>))
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.HashGroupStrategyServiceFactoryTypeError,
                                DefaultFormatting = "在一致性哈希策略{0}中，策略服务类型{1}未实现接口IFactory<IHashGroupStrategyService>",
                                ReplaceParameters = new List<object>() { group.Strategy.Name, group.Strategy.StrategyServiceFactoryType }
                            };

                            throw new UtilityException((int)Errors.HashGroupStrategyServiceFactoryTypeError,fragment);
                        }

                        _strategyServiceFactory = (IFactory<IHashGroupStrategyService>)strategyServiceFactory;

                    }
                }
            }

            return _strategyServiceFactory.Create();

        }

        public async Task Update(HashGroup group)
        {
            await _hashGroupStore.Update(group);
        }

        public async Task UpdateNodeStatus(HashNode node)
        {
            await _hashNodeStore.UpdateStatus(node.ID, node.Status);
        }

        public async Task UpdateNode(HashGroup group, HashNode node)
        {
            node.Group = group;
            node.GroupId = group.ID;
            await _hashNodeStore.Update(group.ID, node);
        }

        public async Task GetHashNode(HashGroup group, int status, Func<HashNode, Task> callback)
        {
            await _hashNodeStore.QueryByStatus(group.ID, status, callback);
        }

        public async Task<HashNode> GetFirstLessNode(HashGroup group, HashNode node)
        {
            return await _hashNodeStore.QueryFirstLessNode(group.ID, node.ID);
        }

        public async Task<HashNode> GetFirstGreaterNode(HashGroup group, HashNode node)
        {
            return await _hashNodeStore.QueryFirstGreaterNode(group.ID, node.ID);
        }

        public async Task<QueryResult<HashNode>> GetHashNode(HashGroup group, int page, int pageSize)
        {
            return await _hashNodeStore.QueryByGroup(group.ID, page, pageSize);
        }


        public string GetHashNodeKeySync(HashGroup group, string key, params int[] status)
        {
            var service = GetStrategyService(group);
            return service.GetHashNodeKeySync(group, key, status);
        }

        public async Task AddRealNode(HashGroup group, HashRealNode node)
        {
            node.Group = group;
            node.GroupId = group.ID;
            await _hashRealNodeStore.Add(node);
        }

        public async Task UpdateRealNode(HashGroup group, HashRealNode node)
        {
            node.Group = group;
            node.GroupId = group.ID;
            await _hashRealNodeStore.Update(group.ID, node);
        }

        public async Task DeleteRealNode(HashGroup group, Guid nodeId)
        {
            await _hashRealNodeStore.DeleteByRelation(group.ID, nodeId);
        }

        public async Task<HashNode> GetHashNode(HashGroup group, Guid nodeId)
        {
            return await _hashNodeStore.QueryByGroup(group.ID, nodeId);
        }

        public async Task GetHashRealNode(HashGroup group, Func<HashRealNode, Task> callback)
        {
            await _hashRealNodeStore.QueryByAll(group.ID, callback);
        }

        public async Task<QueryResult<HashRealNode>> GetHashRealNode(HashGroup group, int page, int pageSize)
        {
            return await _hashRealNodeStore.QueryByGroup(group.ID, page, pageSize);
        }

        public async Task<HashRealNode> GetHashRealNode(HashGroup group, Guid nodeId)
        {
            return await _hashRealNodeStore.QueryByGroup(group.ID, nodeId);
        }

        public async Task<List<HashNode>> GetHashNodeOrderByCode(HashGroup group, int skipNum, int takeNum)
        {
            return await _hashNodeStore.QueryOrderByCode(group.ID, skipNum, takeNum);
        }

        public async Task<HashNode> GetFirstByLessCode(HashGroup group, long code, params int[] status)
        {
            return await _hashNodeStore.QueryFirstByLessCode(group.ID, code, status);
        }

        public async Task<List<HashRealNode>> GetHashRealNodeByCreateTime(HashGroup group, int skipNum, int takeNum)
        {
            return await _hashRealNodeStore.QueryHashRealNodeByCreateTime(group.ID, skipNum, takeNum);
        }

        public async Task<HashNode> GetFirstByGreaterCode(HashGroup group, long code, params int[] status)
        {
            return await _hashNodeStore.QueryFirstByGreaterCode(group.ID, code, status);
        }

        public async Task<HashNode> GetMinCode(HashGroup group, params int[] status)
        {
            return await _hashNodeStore.QueryByMinCode(group.ID, status);
        }

        public async Task<HashNode> GetMaxCode(HashGroup group, params int[] status)
        {
            return await _hashNodeStore.QueryByMaxCode(group.ID, status);
        }

        public async Task<QueryResult<HashNode>> GetHashNode(HashGroup group, int status, int page, int pageSize)
        {
            return await _hashNodeStore.QueryByStatus(group.ID, status, page, pageSize);
        }

    }




}
