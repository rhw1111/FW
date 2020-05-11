using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using MSLibrary.DI;
using MSLibrary.Thread;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.DAL;
using MSLibrary.Transaction;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 通用审批配置
    /// WorkflowResourceType唯一
    /// </summary>
    public class CommonSignConfiguration : EntityBase<ICommonSignConfigurationIMP>
    {
        private static IFactory<ICommonSignConfigurationIMP> _commonSignConfigurationIMPFactory;

        public static IFactory<ICommonSignConfigurationIMP> CommonSignConfigurationIMPFactory
        {
            set
            {
                _commonSignConfigurationIMPFactory = value;
            }
        }
        public override IFactory<ICommonSignConfigurationIMP> GetIMPFactory()
        {
            return _commonSignConfigurationIMPFactory;
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
        /// 资源类型
        /// </summary>
        public string WorkflowResourceType
        {
            get
            {
                return GetAttribute<string>("WorkflowResourceType");
            }
            set
            {
                SetAttribute<string>("WorkflowResourceType", value);
            }
        }
        /// <summary>
        /// 对应的实体类型
        /// </summary>
        public string EntityType
        {
            get
            {
                return GetAttribute<string>("EntityType");
            }
            set
            {
                SetAttribute<string>("EntityType", value);
            }
        }



        /// <summary>
        /// 工作流资源默认完成状态
        /// </summary>
        public int WorkflowResourceDefaultCompleteStatus
        {
            get
            {
                return GetAttribute<int>("WorkflowResourceDefaultCompleteStatus");
            }
            set
            {
                SetAttribute<int>("WorkflowResourceDefaultCompleteStatus", value);
            }
        }

        /// <summary>
        /// 完成后要处理的服务的配置信息
        /// 针对完成后要处理的服务有不同的配置信息类型
        /// </summary>
        public string CompleteServiceConfiguration
        {
            get
            {
                return GetAttribute<string>("CompleteServiceConfiguration");
            }
            set
            {
                SetAttribute<string>("CompleteServiceConfiguration", value);
            }
        }

        /// <summary>
        /// 完成后要处理的服务名称
        /// </summary>
        public string CompleteServiceName
        {
            get
            {
                return GetAttribute<string>("CompleteServiceName");
            }
            set
            {
                SetAttribute<string>("CompleteServiceName", value);
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
        /// 获取指定名称的节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public async Task<CommonSignConfigurationNode> GetNode(string nodeName)
        {
            return await _imp.GetNode(this, nodeName);
        }

        /// <summary>
        /// 获取当前配置下指定名称的初始动作
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public async Task<CommonSignConfigurationRootAction> GetRootAction(string actionName)
        {
            return await _imp.GetRootAction(this, actionName);
        }

        /// <summary>
        /// 获取当前配置下的所有初始动作
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetRootAction(Func<CommonSignConfigurationRootAction, Task> callback)
        {
            await _imp.GetRootAction(this, callback);
        }


        /// <summary>
        /// 执行入口动作
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="executeUserKey"></param>
        /// <param name="userInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task ExecuteEntry(string entityKey, string executeUserKey, string executeUserExtensionInfo, string actionName, List<WorkflowUserInfo> userInfo, string data)
        {
            await _imp.ExecuteEntry(this, entityKey, actionName, executeUserKey, executeUserExtensionInfo, userInfo, data);
        }
        /// <summary>
        /// 执行完成动作
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="workflowResourceCompleteStatus">工作流资源完成状态</param>
        /// <returns>当前状态</returns>
        public async Task<int> ExecuteComplete(string entityKey, string executeUserKey, string executeUserExtensionInfo, int? workflowResourceCompleteStatus)
        {
            return await _imp.ExecuteComplete(this, entityKey, executeUserKey, executeUserExtensionInfo, workflowResourceCompleteStatus);
        }

    }


    public interface ICommonSignConfigurationIMP
    {
        /// <summary>
        /// 获取指定名称的节点
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        Task<CommonSignConfigurationNode> GetNode(CommonSignConfiguration configuration, string nodeName);
        /// <summary>
        /// 获取指定动作名称的初始动作配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        Task<CommonSignConfigurationRootAction> GetRootAction(CommonSignConfiguration configuration, string actionName);
        /// <summary>
        /// 获取当前配置下的所有初始动作
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetRootAction(CommonSignConfiguration configuration, Func<CommonSignConfigurationRootAction, Task> callback);

        /// <summary>
        /// 执行入口动作
        /// </summary>
        /// <returns></returns>
        Task ExecuteEntry(CommonSignConfiguration configuration, string entityKey, string executeUserKey, string executeUserExtensionInfo, string actionName, List<WorkflowUserInfo> userInfos, string data);
        /// <summary>
        /// 执行完成动作
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="entityKey"></param>
        /// <param name="executeUserKey"></param>
        /// <param name="executeUserExtensionInfo"></param>
        /// <param name="workflowResourceCompleteStatus">工作流资源完成状态</param>
        /// <returns></returns>
        Task<int> ExecuteComplete(CommonSignConfiguration configuration, string entityKey, string executeUserKey, string executeUserExtensionInfo, int? workflowResourceCompleteStatus);

    }

    [Injection(InterfaceType = typeof(ICommonSignConfigurationIMP), Scope = InjectionScope.Transient)]
    public class CommonSignConfigurationIMP : ICommonSignConfigurationIMP
    {
        private IWorkflowResourceRepository _workflowResourceRepository;
        private ICommonSignConfigurationCompleteServiceSelector _commonSignConfigurationCompleteServiceSelector;
        private ICommonSignConfigurationNodeStore _commonSignConfigurationNodeStore;
        private ICommonSignConfigurationRootActionStore _commonSignConfigurationRootActionStore;
        private ICommonSignConfigurationStore _commonSignConfigurationStore;

        public CommonSignConfigurationIMP(IWorkflowResourceRepository workflowResourceRepository, ICommonSignConfigurationCompleteServiceSelector commonSignConfigurationCompleteServiceSelector, ICommonSignConfigurationNodeStore commonSignConfigurationNodeStore, ICommonSignConfigurationRootActionStore commonSignConfigurationRootActionStore, ICommonSignConfigurationStore commonSignConfigurationStore)
        {
            _workflowResourceRepository = workflowResourceRepository;
            _commonSignConfigurationCompleteServiceSelector = commonSignConfigurationCompleteServiceSelector;
            _commonSignConfigurationNodeStore = commonSignConfigurationNodeStore;
            _commonSignConfigurationRootActionStore = commonSignConfigurationRootActionStore;
            _commonSignConfigurationStore = commonSignConfigurationStore;
        }



        public async Task ExecuteEntry(CommonSignConfiguration configuration, string entityKey, string actionName, string executeUserKey, string executeUserExtensionInfo, List<WorkflowUserInfo> userInfos, string data)
        {
            //获取指定动作名称的初始化动作
            var rootAction = await _commonSignConfigurationRootActionStore.QueryByActionName(configuration.ID, actionName);
            if (rootAction == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationRootActionByActionName,
                    DefaultFormatting = "工作流资源类型为{0}的通用审批配置下，找不到动作名称为{1}的初始化动作",
                    ReplaceParameters = new List<object>() { configuration.WorkflowResourceType, actionName }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationRootActionByActionName,fragment);
            }



            await _commonSignConfigurationStore.Lock(string.Format(ApplicationLockBaseNames.CommonSignConfiguration, configuration.EntityType, entityKey),
                async () =>
                {
                    await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 5, 0) }))
                    {
                        await rootAction.ExecuteEntry(entityKey, executeUserKey, executeUserExtensionInfo, userInfos, data);

                        scope.Complete();
                    };
                },
                50000
            );

        }

        public async Task<CommonSignConfigurationNode> GetNode(CommonSignConfiguration configuration, string nodeName)
        {
            return await _commonSignConfigurationNodeStore.QueryByConfigurationName(configuration.ID, nodeName);
        }


        public async Task<int> ExecuteComplete(CommonSignConfiguration configuration, string entityKey, string executeUserKey, string executeUserExtensionInfo, int? workflowResourceCompleteStatus)
        {
            var service = _commonSignConfigurationCompleteServiceSelector.Choose(configuration.CompleteServiceName);
            int status = configuration.WorkflowResourceDefaultCompleteStatus;

            await _commonSignConfigurationStore.Lock(string.Format(ApplicationLockBaseNames.CommonSignConfiguration, configuration.EntityType, entityKey),
                async () =>
                {
                    using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 5, 0) }))
                    {
                        if (workflowResourceCompleteStatus.HasValue)
                        {
                            status = workflowResourceCompleteStatus.Value;
                        }



                        await service.Execute(configuration, entityKey, executeUserKey, executeUserExtensionInfo, status);

                        //删除工作流资源

                        var workflowResource = await _workflowResourceRepository.QueryByKey(configuration.WorkflowResourceType, entityKey);
                        if (workflowResource != null)
                        {
                            await workflowResource.Delete();
                        }
                        scope.Complete();
                    };
                },
                50000
            );

            return status;
        }

        public async Task<CommonSignConfigurationRootAction> GetRootAction(CommonSignConfiguration configuration, string actionName)
        {
            return await _commonSignConfigurationRootActionStore.QueryByActionName(configuration.ID, actionName);
        }

        public async Task GetRootAction(CommonSignConfiguration configuration, Func<CommonSignConfigurationRootAction, Task> callback)
        {
            await _commonSignConfigurationRootActionStore.QueryAll(configuration.ID, callback);
        }
    }

    /// <summary>
    /// 通用审批配置起始动作
    /// 通用审批配置与起始动作的关系是1：N
    /// ConfigurationId+ActionName唯一
    /// </summary>
    public class CommonSignConfigurationRootAction : EntityBase<ICommonSignConfigurationRootActionIMP>
    {
        private static IFactory<ICommonSignConfigurationRootActionIMP> _commonSignConfigurationRootActionIMPFactory;

        public static IFactory<ICommonSignConfigurationRootActionIMP> CommonSignConfigurationRootActionIMPFactory
        {
            set
            {
                _commonSignConfigurationRootActionIMPFactory = value;
            }
        }

        public override IFactory<ICommonSignConfigurationRootActionIMP> GetIMPFactory()
        {
            return _commonSignConfigurationRootActionIMPFactory;
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
        /// 所属通用审批配置Id
        /// </summary>
        public Guid ConfigurationId
        {
            get
            {
                return GetAttribute<Guid>("ConfigurationId");
            }
            set
            {
                SetAttribute<Guid>("ConfigurationId", value);
            }
        }

        /// <summary>
        /// 所属通用审批配置
        /// </summary>
        public CommonSignConfiguration Configuration
        {
            get
            {
                return GetAttribute<CommonSignConfiguration>("Configuration");
            }
            set
            {
                SetAttribute<CommonSignConfiguration>("Configuration", value);
            }
        }


        /// <summary>
        /// 初始动作名称
        /// </summary>
        public string ActionName
        {
            get
            {
                return GetAttribute<string>("ActionName");
            }
            set
            {
                SetAttribute<string>("ActionName", value);
            }
        }
        /// <summary>
        /// 工作流资源的初始状态
        /// </summary>
        public int WorkflowResourceInitStatus
        {
            get
            {
                return GetAttribute<int>("WorkflowResourceInitStatus");
            }
            set
            {
                SetAttribute<int>("WorkflowResourceInitStatus", value);
            }
        }
        /// <summary>
        /// 工作流资源默认完成状态
        /// </summary>
        public int WorkflowResourceDefaultCompleteStatus
        {
            get
            {
                return GetAttribute<int>("WorkflowResourceDefaultCompleteStatus");
            }
            set
            {
                SetAttribute<int>("WorkflowResourceDefaultCompleteStatus", value);
            }
        }

        /// <summary>
        /// 入口节点Id
        /// 可以为空
        /// </summary>
        public Guid? EntryNodeId
        {
            get
            {
                return GetAttribute<Guid?>("EntryNodeId");
            }
            set
            {
                SetAttribute<Guid?>("EntryNodeId", value);
            }
        }

        /// <summary>
        /// 入口节点
        /// </summary>
        public CommonSignConfigurationNode EntryNode
        {
            get
            {
                return GetAttribute<CommonSignConfigurationNode>("EntryNode");
            }
            set
            {
                SetAttribute<CommonSignConfigurationNode>("EntryNode", value);
            }
        }

        /// <summary>
        /// 入口节点查找服务使用的配置信息
        /// 不同的入口节点查找服务有不同的配置信息
        /// 可以为空
        /// </summary>
        public string EntryNodeFindServiceConfiguration
        {
            get
            {
                return GetAttribute<string>("EntryNodeFindServiceConfiguration");
            }
            set
            {
                SetAttribute<string>("EntryNodeFindServiceConfiguration", value);
            }
        }

        /// <summary>
        /// 入口节点查找服务名称
        /// 可以为空
        /// </summary>
        public string EntryNodeFindServiceName
        {
            get
            {
                return GetAttribute<string>("EntryNodeFindServiceName");
            }
            set
            {
                SetAttribute<string>("EntryNodeFindServiceName", value);
            }
        }


        /// <summary>
        /// 入口服务使用的配置信息
        /// 不同的入口服务有不同类型的配置信息
        /// </summary>
        public string EntryServiceConfiguration
        {
            get
            {
                return GetAttribute<string>("EntryServiceConfiguration");
            }
            set
            {
                SetAttribute<string>("EntryServiceConfiguration", value);
            }
        }
        /// <summary>
        /// 使用的入口服务名称
        /// </summary>
        public string EntryServiceName
        {
            get
            {
                return GetAttribute<string>("EntryServiceName");
            }
            set
            {
                SetAttribute<string>("EntryServiceName", value);
            }
        }

        /// <summary>
        /// 获取可以进行入口操作的用户列表服务配置
        /// 不同的服务使用不同的配置
        /// </summary>
        public string EntryGetExecuteUsersServiceConfiguration
        {
            get
            {
                return GetAttribute<string>("EntryGetExecuteUsersServiceConfiguration");
            }
            set
            {
                SetAttribute<string>("EntryGetExecuteUsersServiceConfiguration", value);
            }
        }

        /// <summary>
        /// 获取可以进行入口操作的用户列表服务名称
        /// </summary>
        public string EntryGetExecuteUsersServiceName
        {
            get
            {
                return GetAttribute<string>("EntryGetExecuteUsersServiceName");
            }
            set
            {
                SetAttribute<string>("EntryGetExecuteUsersServiceName", value);
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
        /// 获取执行入口操作的用户
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetExecuteEntryUsers(string entityKey, Func<string, Task> callback)
        {
            await _imp.GetExecuteEntryUsers(this, entityKey, callback);
        }
        /// <summary>
        /// 执行入口动作
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="executeUserKey"></param>
        /// <param name="executeUserExtensionInfo"></param>
        /// <param name="userInfos"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task ExecuteEntry(string entityKey, string executeUserKey, string executeUserExtensionInfo, List<WorkflowUserInfo> userInfos, string data)
        {
            await _imp.ExecuteEntry(this, entityKey, executeUserKey, executeUserExtensionInfo, userInfos, data);
        }

    }


    public interface ICommonSignConfigurationRootActionIMP
    {
        /// <summary>
        /// 执行入口动作
        /// </summary>
        /// <param name="rootAction"></param>
        /// <param name="entityKey"></param>
        /// <param name="executeUserKey"></param>
        /// <param name="userInfos"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task ExecuteEntry(CommonSignConfigurationRootAction rootAction, string entityKey, string executeUserKey, string executeUserExtensionInfo, List<WorkflowUserInfo> userInfos, string data);
        /// <summary>
        /// 获取可以执行入口操作的用户
        /// </summary>
        /// <param name="rootAction"></param>
        /// <param name="entityKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetExecuteEntryUsers(CommonSignConfigurationRootAction rootAction, string entityKey, Func<string, Task> callback);
    }

    [Injection(InterfaceType = typeof(ICommonSignConfigurationRootActionIMP), Scope = InjectionScope.Transient)]
    public class CommonSignConfigurationRootActionIMP : ICommonSignConfigurationRootActionIMP
    {
        private IWorkflowResourceRepository _workflowResourceRepository;
        private ICommonSignConfigurationEntryServiceSelector _commonSignConfigurationEntryServiceSelector;
        private ICommonSignConfigurationEntryNodeFindServiceSelector _commonSignConfigurationEntryNodeFindServiceSelector;
        private ICommonSignConfigurationEntryGetExecuteUsersServiceSelector _commonSignConfigurationEntryGetExecuteUsersServiceSelector;
        public CommonSignConfigurationRootActionIMP(IWorkflowResourceRepository workflowResourceRepository, ICommonSignConfigurationEntryServiceSelector commonSignConfigurationEntryServiceSelector, ICommonSignConfigurationEntryNodeFindServiceSelector commonSignConfigurationEntryNodeFindServiceSelector, ICommonSignConfigurationEntryGetExecuteUsersServiceSelector commonSignConfigurationEntryGetExecuteUsersServiceSelector)
        {
            _workflowResourceRepository = workflowResourceRepository;
            _commonSignConfigurationEntryServiceSelector = commonSignConfigurationEntryServiceSelector;
            _commonSignConfigurationEntryNodeFindServiceSelector = commonSignConfigurationEntryNodeFindServiceSelector;
            _commonSignConfigurationEntryGetExecuteUsersServiceSelector = commonSignConfigurationEntryGetExecuteUsersServiceSelector;
        }
        public async Task ExecuteEntry(CommonSignConfigurationRootAction rootAction, string entityKey, string executeUserKey, string executeUserExtensionInfo, List<WorkflowUserInfo> userInfos, string data)
        {
            //判断执行操作的用户是否可以操作
            bool canExecute = false;
            var entryGetExecuteUsersService = _commonSignConfigurationEntryGetExecuteUsersServiceSelector.Choose(rootAction.EntryGetExecuteUsersServiceName);
            List<string> userKeys = new List<string>();
            await entryGetExecuteUsersService.Execute(rootAction, entityKey, async (userKey) =>
              {
                  userKeys.Add(userKey);
                  if (executeUserKey.ToLower() == userKey.ToLower())
                  {

                      canExecute = true;
                  }
                  await Task.FromResult(0);
              });
            if (!canExecute)
            {
                string strUserKey = string.Empty;
                foreach (var item in userKeys)
                {
                    strUserKey += $"{item},";
                }
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CommonSignConfigurationRootActionUserCanNotEntry,
                    DefaultFormatting = "工作流资源类型为{0}的通用审批配置下动作名称为{1}的初始化动作，用户{2}不能执行,userkeys:{3}",
                    ReplaceParameters = new List<object>() { rootAction.Configuration.WorkflowResourceType, rootAction.ActionName, executeUserKey, strUserKey }
                };

                throw new UtilityException((int)Errors.CommonSignConfigurationRootActionUserCanNotEntry, fragment);
            }
          


            //创建工作流资源
            var workflowResource = await WorkflowResourceHelper.GetWorkflowResource(_workflowResourceRepository, rootAction.Configuration.WorkflowResourceType, entityKey, rootAction.WorkflowResourceInitStatus);


            //执行入口服务
            var entryService = _commonSignConfigurationEntryServiceSelector.Choose(rootAction.EntryServiceName);
            var canEntryNode = await entryService.Execute(workflowResource, rootAction.Configuration.EntityType, entityKey, executeUserKey, executeUserExtensionInfo, rootAction.ActionName, userInfos, data, rootAction.EntryServiceConfiguration);



            if (canEntryNode)
            {
                //获取入口节点查找
                CommonSignConfigurationNode node = null;
                bool runComplete = true;
                if (!string.IsNullOrEmpty(rootAction.EntryNodeFindServiceName))
                {
                    var nodeFindService = _commonSignConfigurationEntryNodeFindServiceSelector.Choose(rootAction.EntryNodeFindServiceName);
                    node = await nodeFindService.Execute(rootAction, entityKey, userInfos, data, rootAction.EntryNodeFindServiceConfiguration);
                    if (node != null)
                    {
                        runComplete = false;
                    }
                }


                if (runComplete)
                {
                    //判断入口节点是否有值
                    if (rootAction.EntryNode != null)
                    {
                        node = rootAction.EntryNode;
                        runComplete = false;
                    }

                }

                if (runComplete || node == null)
                {
                    //表示不需要再往下面的节点执行，直接执行完成服务
                    await rootAction.Configuration.ExecuteComplete(entityKey, executeUserKey, executeUserExtensionInfo, rootAction.WorkflowResourceDefaultCompleteStatus);
                    return;
                }
                else
                {
                    //验证入口节点
                    await ValidateEntryNode(rootAction.Configuration, node);
                    //需要执行入口节点的CreateFlow方法创建工作流相应的步骤
                    await node.CreateFlowExecute(entityKey, rootAction.ActionName, rootAction.WorkflowResourceInitStatus, executeUserKey, executeUserExtensionInfo, userInfos);
                }
            }


        }

        public async Task GetExecuteEntryUsers(CommonSignConfigurationRootAction rootAction, string entityKey, Func<string, Task> callback)
        {
            var entryGetExecuteUsersService = _commonSignConfigurationEntryGetExecuteUsersServiceSelector.Choose(rootAction.EntryGetExecuteUsersServiceName);
            await entryGetExecuteUsersService.Execute(rootAction, entityKey, async (userKey) =>
            {
                await callback(userKey);
            });
        }


        /// <summary>
        /// 验证入口节点
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private async Task ValidateEntryNode(CommonSignConfiguration configuration, CommonSignConfigurationNode node)
        {
            //检查节点的状态，如果状态为不可用，则抛出异常
            if (node.Status != CommonSignConfigurationNodeStatus.Enabled)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CommonSignConfigurationEntryNodeStatusError,
                    DefaultFormatting = "工作流资源类型为{0}的通用审批配置入口节点{1}的状态为{2}，但要求的状态为{3}",
                    ReplaceParameters = new List<object>() { configuration.WorkflowResourceType, node.Name, node.Status, CommonSignConfigurationNodeStatus.Enabled }
                };

                throw new UtilityException((int)Errors.CommonSignConfigurationEntryNodeStatusError, fragment);
            }
            //检查入口节点的所属配置是否与当前配置相同
            if (node.Configuration.ID != configuration.ID)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CommonSignConfigurationEntryNodeNotSameConfiguration,
                    DefaultFormatting = "工作流资源类型为{0}的通用审批配置的入口节点{1}的所属配置为工作流资源类型为{2}的通用审批配置，两者不一致",
                    ReplaceParameters = new List<object>() { configuration.WorkflowResourceType, node.Name, node.Configuration.WorkflowResourceType }
                };

                throw new UtilityException((int)Errors.CommonSignConfigurationEntryNodeNotSameConfiguration, fragment);
            }

            await Task.FromResult(0);
        }
    }


    /// <summary>
    /// 通用审批配置入口服务
    /// 一个工作流初始化时需要执行的业务
    /// </summary>
    public interface ICommonSignConfigurationEntryService
    {
        /// <summary>
        /// 执行初始化业务
        /// 返回结果为是否可以进入节点
        /// </summary>
        /// <param name="workflowResource">工作流资源</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityKey">实体关键字</param>
        /// <param name="executeUserKey">执行处理的用户的关键字</param>
        /// <param name="executeUserExtensionInfo">执行处理的用户的扩展信息</param>
        /// <param name="actionName">动作名称</param>
        /// <param name="userInfo">从外部传来的下一步审批用户信息</param>
        /// <param name="data">业务数据</param>
        /// <param name="configurationData">配置数据</param>
        /// <returns></returns>
        Task<bool> Execute(WorkflowResource workflowResource, string entityType, string entityKey, string executeUserKey, string executeUserExtensionInfo, string actionName, List<WorkflowUserInfo> userInfo, string data, string configurationData);
    }

    /// <summary>
    /// 通用审批配置入口节点寻找服务
    /// </summary>
    public interface ICommonSignConfigurationEntryNodeFindService
    {
        /// <summary>
        /// 执行查找逻辑
        /// </summary>
        /// <param name="rootAction">初始动作</param>
        /// <param name="entityKey">实体关键字</param>
        /// <param name="actionName">动作名称</param>
        /// <param name="userInfo">从外部传来的下一步审批用户信息</param>
        /// <param name="data">业务数据</param>
        /// <param name="configurationData">配置数据</param>
        /// <returns></returns>
        Task<CommonSignConfigurationNode> Execute(CommonSignConfigurationRootAction rootAction, string entityKey, List<WorkflowUserInfo> userInfo, string data, string configurationData);
    }


    /// <summary>
    /// 获取可以进行入口操作的用户列表服务
    /// </summary>
    public interface ICommonSignConfigurationEntryGetExecuteUsersService
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="rootAction"></param>
        /// <param name="entityKey"></param>
        /// <param name="callback">用户关键字</param>
        /// <returns></returns>
        Task Execute(CommonSignConfigurationRootAction rootAction, string entityKey, Func<string, Task> callback);
    }
    /// <summary>
    /// 获取可以进行入口操作的用户列表服务选择器接口
    /// </summary>
    public interface ICommonSignConfigurationEntryGetExecuteUsersServiceSelector : ISelector<ICommonSignConfigurationEntryGetExecuteUsersService>
    {

    }

    /// <summary>
    /// 获取可以进行入口操作的用户列表服务选择器默认实现
    /// </summary>
    [Injection(InterfaceType = typeof(ICommonSignConfigurationEntryGetExecuteUsersServiceSelector), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationEntryGetExecuteUsersServiceSelector : ICommonSignConfigurationEntryGetExecuteUsersServiceSelector
    {
        private static Dictionary<string, IFactory<ICommonSignConfigurationEntryGetExecuteUsersService>> _serviceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationEntryGetExecuteUsersService>>();

        public static Dictionary<string, IFactory<ICommonSignConfigurationEntryGetExecuteUsersService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }

        public ICommonSignConfigurationEntryGetExecuteUsersService Choose(string name)
        {
            if (!_serviceFactories.TryGetValue(name, out IFactory<ICommonSignConfigurationEntryGetExecuteUsersService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationEntryGetExecuteUsersServiceByName,
                    DefaultFormatting ="找不到名称为{0}的获取可以进行入口操作的用户列表服务，位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationEntryGetExecuteUsersServiceByName, fragment);
            }

            return serviceFactory.Create();
        }
    }


    /// <summary>
    /// 通用审批配置入口服务选择器接口
    /// </summary>
    public interface ICommonSignConfigurationEntryServiceSelector : ISelector<ICommonSignConfigurationEntryService>
    {

    }

    /// <summary>
    /// 通用审批配置入口服务选择器默认实现
    /// </summary>
    [Injection(InterfaceType = typeof(ICommonSignConfigurationEntryServiceSelector), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationEntryServiceSelector : ICommonSignConfigurationEntryServiceSelector
    {
        private static Dictionary<string, IFactory<ICommonSignConfigurationEntryService>> _serviceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationEntryService>>();

        public static Dictionary<string, IFactory<ICommonSignConfigurationEntryService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }
        public ICommonSignConfigurationEntryService Choose(string name)
        {
            if (!_serviceFactories.TryGetValue(name, out IFactory<ICommonSignConfigurationEntryService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationNodeGetExecuteUserServiceByName,
                    DefaultFormatting = "找不到名称为{0}的通用审批配置入口服务，位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationNodeGetExecuteUserServiceByName, fragment);
            }

            return serviceFactory.Create();
        }
    }



    /// <summary>
    /// 通用审批配置入口节点寻找服务选择器接口
    /// </summary>
    public interface ICommonSignConfigurationEntryNodeFindServiceSelector : ISelector<ICommonSignConfigurationEntryNodeFindService>
    {

    }

    /// <summary>
    /// 通用审批配置入口节点寻找服务选择器默认实现
    /// </summary>
    [Injection(InterfaceType = typeof(ICommonSignConfigurationEntryNodeFindServiceSelector), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationEntryNodeFindServiceSelector : ICommonSignConfigurationEntryNodeFindServiceSelector
    {
        private static Dictionary<string, IFactory<ICommonSignConfigurationEntryNodeFindService>> _serviceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationEntryNodeFindService>>();

        public static Dictionary<string, IFactory<ICommonSignConfigurationEntryNodeFindService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }
        public ICommonSignConfigurationEntryNodeFindService Choose(string name)
        {
            if (!_serviceFactories.TryGetValue(name, out IFactory<ICommonSignConfigurationEntryNodeFindService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationEntryNodeFindServiceByName,
                    DefaultFormatting = "找不到名称为{0}的通用审批配置入口节点寻找服务，位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationEntryNodeFindServiceByName, fragment);
            }

            return serviceFactory.Create();
        }
    }

    /// <summary>
    /// 通用工作流完成后执行的服务
    /// </summary>
    public interface ICommonSignConfigurationCompleteService
    {
        Task Execute(CommonSignConfiguration configuration, string entityKey, string executeUserKey, string executeUserExtensionInfo, int workflowResourceCompleteStatus);
    }


    /// <summary>
    /// 通用工作流完成后执行的服务选择器接口
    /// </summary>
    public interface ICommonSignConfigurationCompleteServiceSelector : ISelector<ICommonSignConfigurationCompleteService>
    {

    }

    /// <summary>
    /// 通用工作流完成后执行的服务选择器默认实现
    /// </summary>
    [Injection(InterfaceType = typeof(ICommonSignConfigurationCompleteServiceSelector), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationCompleteServiceSelector : ICommonSignConfigurationCompleteServiceSelector
    {
        private static Dictionary<string, IFactory<ICommonSignConfigurationCompleteService>> _serviceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationCompleteService>>();

        public static Dictionary<string, IFactory<ICommonSignConfigurationCompleteService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }

        public ICommonSignConfigurationCompleteService Choose(string name)
        {
            if (!_serviceFactories.TryGetValue(name, out IFactory<ICommonSignConfigurationCompleteService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationCompleteServiceByName,
                    DefaultFormatting = "找不到名称为{0}的通用审批完成后处理服务，位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationCompleteServiceByName, fragment);
            }

            return serviceFactory.Create();
        }
    }

}
