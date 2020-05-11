using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Transactions;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.DAL;
using MSLibrary.Thread;
using MSLibrary.Transaction;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 通用审批配置节点
    /// ConfigurationId+WorkflowStatus唯一
    /// ConfigurationId+Name唯一
    /// </summary>
    public class CommonSignConfigurationNode : EntityBase<ICommonSignConfigurationNodeIMP>
    {
        private static IFactory<ICommonSignConfigurationNodeIMP> _commonSignConfigurationNodeIMPFactory;

        public static IFactory<ICommonSignConfigurationNodeIMP> CommonSignConfigurationNodeIMPFactory
        {
            set
            {
                _commonSignConfigurationNodeIMPFactory = value;
            }
        }
        public override IFactory<ICommonSignConfigurationNodeIMP> GetIMPFactory()
        {
            return _commonSignConfigurationNodeIMPFactory;
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
        /// 节点名称
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
        /// 节点工作流状态
        /// </summary>
        public int WorkflowStatus
        {
            get
            {
                return GetAttribute<int>("WorkflowStatus");
            }
            set
            {
                SetAttribute<int>("WorkflowStatus", value);
            }
        }



        /// <summary>
        /// 直接跳转处理服务配置
        /// 供直接跳转处理服务使用
        /// 可以为空
        /// </summary>
        public string DirectGoExecuteServiceConfiguration
        {
            get
            {
                return GetAttribute<string>("DirectGoExecuteServiceConfiguration");
            }
            set
            {
                SetAttribute<string>("DirectGoExecuteServiceConfiguration", value);
            }
        }

        /// <summary>
        /// 直接跳转处理服务名称
        /// 可以为空
        /// </summary>
        public string DirectGoExecuteServiceName
        {
            get
            {
                return GetAttribute<string>("DirectGoExecuteServiceName");
            }
            set
            {
                SetAttribute<string>("DirectGoExecuteServiceName", value);
            }
        }


        /// <summary>
        /// 状态
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

        /// <summary>
        /// 创建流程处理
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="userInfos"></param>
        /// <returns></returns>
        public async Task CreateFlowExecute(string entityKey, string sourceActionName, int sourceStatus, string executeUseKey, string executeUserEntensionInfo, List<WorkflowUserInfo> userInfos)
        {
            await _imp.CreateFlowExecute(this, entityKey, sourceActionName, sourceStatus, executeUseKey, executeUserEntensionInfo, userInfos);
        }

        /// <summary>
        /// 用户审批
        /// </summary>
        /// <param name="actionName">要审批的动作名称</param>
        /// <param name="entityKey">实体关键字</param>
        /// <param name="signUserKey">进行审批的用户的关键字</param>
        /// <param name="signUserExtensionInfo">进行审批的用户的扩展信息</param>
        /// <param name="signResult">审批结果</param>
        /// <returns></returns>
        public async Task Sign(string actionName, string entityKey, string signUserKey, string signUserExtensionInfo, SignResult signResult)
        {
            await _imp.Sign(this, actionName, entityKey, signUserKey, signUserExtensionInfo, signResult);
        }
        /// <summary>
        /// 获取指定动作名称的节点动作
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public async Task<CommonSignConfigurationNodeAction> GetAction(string actionName)
        {
            return await _imp.GetAction(this, actionName);
        }

        /// <summary>
        /// 直接跳转到该节点
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="executeUseKey"></param>
        /// <param name="executeUserEntensionInfo"></param>
        /// <param name="userInfos"></param>
        /// <returns></returns>
        public async Task DirectGo(string entityKey, string executeUseKey, string executeUserEntensionInfo, List<WorkflowUserInfo> userInfos)
        {
            await _imp.DirectGo(this, entityKey, executeUseKey, executeUserEntensionInfo, userInfos);
        }
    }

    public interface ICommonSignConfigurationNodeIMP
    {
        /// <summary>
        /// 创建流程处理
        /// </summary>
        /// <param name="node"></param>
        /// <param name="entityKey">实体关键字</param>
        /// <param name="sourceActionName">发起该动作的发起方动作名称</param>
        /// <param name="sourceStatus">发起该动作的发起方工作流资源状态</param>
        /// <param name="executeUseKey">处理人关键字</param>
        /// <param name="executeUserEntensionInfo">处理人扩展信息</param>
        /// <param name="userInfos">待处理用户信息</param>
        /// <returns></returns>
        Task CreateFlowExecute(CommonSignConfigurationNode node, string entityKey, string sourceActionName, int sourceStatus, string executeUseKey, string executeUserEntensionInfo, List<WorkflowUserInfo> userInfos);
        /// <summary>
        /// 直接跳转到该节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="entityKey"></param>
        /// <param name="executeUseKey"></param>
        /// <param name="executeUserEntensionInfo"></param>
        /// <param name="userInfos"></param>
        /// <returns></returns>
        Task DirectGo(CommonSignConfigurationNode node, string entityKey, string executeUseKey, string executeUserEntensionInfo, List<WorkflowUserInfo> userInfos);

        /// <summary>
        /// 用户审批
        /// </summary>
        /// <param name="node"></param>
        /// <param name="actionName">要进行审批的动作</param>
        /// <param name="entityKey">实体关键字</param>
        /// <param name="signUserKey">进行审批的用户的关键字</param>
        /// <param name="executeUserEntensionInfo">行审批的用户的扩展信息</param>
        /// <param name="signResult">审批结果</param>
        /// <returns></returns>
        Task Sign(CommonSignConfigurationNode node, string actionName, string entityKey, string signUserKey, string executeUserEntensionInfo, SignResult signResult);
        /// <summary>
        /// 获取节点下所有的动作
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetAllAction(CommonSignConfigurationNode node, Func<CommonSignConfigurationNodeAction, Task> callback);
        /// <summary>
        /// 获取指定动作名称的节点动作
        /// </summary>
        /// <param name="node"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        Task<CommonSignConfigurationNodeAction> GetAction(CommonSignConfigurationNode node, string actionName);

    }


    [Injection(InterfaceType = typeof(ICommonSignConfigurationNodeIMP), Scope = InjectionScope.Transient)]
    public class CommonSignConfigurationNodeIMP : ICommonSignConfigurationNodeIMP
    {


        public static Dictionary<string, IFactory<ICommonSignConfigurationNodeSignExtensionService>> _commonSignConfigurationNodeSignExtensionServiceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationNodeSignExtensionService>>();

        /// <summary>
        /// 通用审批配置节点审批后处理服务工厂键值对
        /// 键为审批类型
        /// </summary>
        public static Dictionary<string, IFactory<ICommonSignConfigurationNodeSignExtensionService>> CommonSignConfigurationNodeSignExtensionServiceFactories
        {
            get
            {
                return _commonSignConfigurationNodeSignExtensionServiceFactories;
            }
        }

        private ICommonSignConfigurationNodeActionStore _commonSignConfigurationNodeActionStore;

        private IWorkflowResourceRepository _workflowResourceRepository;
        private ICommonSignConfigurationNodeGetExecuteUserServiceSelector _commonSignConfigurationNodeGetExecuteUserServiceSelector;
        private ICommonSignConfigurationNodeCreateFlowExecuteServiceSelector _commonSignConfigurationNodeCreateFlowExecuteServiceSelector;
        private ICommonSignConfigurationNodeDirectGoExecuteServiceSelector _commonSignConfigurationNodeDirectGoExecuteServiceSelector;
        private ICommonSignConfigurationNodeStore _commonSignConfigurationNodeStore;
        private IApplicationLockService _applicationLockService;

        public CommonSignConfigurationNodeIMP(ICommonSignConfigurationNodeActionStore commonSignConfigurationNodeActionStore, IWorkflowResourceRepository workflowResourceRepository, ICommonSignConfigurationNodeGetExecuteUserServiceSelector commonSignConfigurationNodeGetExecuteUserServiceSelector, ICommonSignConfigurationNodeCreateFlowExecuteServiceSelector commonSignConfigurationNodeCreateFlowExecuteServiceSelector, ICommonSignConfigurationNodeDirectGoExecuteServiceSelector commonSignConfigurationNodeDirectGoExecuteServiceSelector, ICommonSignConfigurationNodeStore commonSignConfigurationNodeStore, IApplicationLockService applicationLockService)
        {
            _commonSignConfigurationNodeActionStore = commonSignConfigurationNodeActionStore;
            _workflowResourceRepository = workflowResourceRepository;
            _commonSignConfigurationNodeGetExecuteUserServiceSelector = commonSignConfigurationNodeGetExecuteUserServiceSelector;
            _commonSignConfigurationNodeCreateFlowExecuteServiceSelector = commonSignConfigurationNodeCreateFlowExecuteServiceSelector;
            _commonSignConfigurationNodeDirectGoExecuteServiceSelector = commonSignConfigurationNodeDirectGoExecuteServiceSelector;
            _commonSignConfigurationNodeStore = commonSignConfigurationNodeStore;
            _applicationLockService = applicationLockService;
        }

        public async Task CreateFlowExecute(CommonSignConfigurationNode node, string entityKey, string sourceActionName, int sourceStatus, string executeUseKey, string executeUserEntensionInfo, List<WorkflowUserInfo> userInfos)
        {
            var configuration = node.Configuration;

            //获取工作流资源
            var workflowResource = await _workflowResourceRepository.QueryByKey(configuration.WorkflowResourceType, entityKey);

            if (workflowResource == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundWorkflowResourceByKey,
                    DefaultFormatting = "找不到Type为{0}，Key为{1}的工作流资源",
                    ReplaceParameters = new List<object>() { configuration.WorkflowResourceType, entityKey }
                };

                throw new UtilityException((int)Errors.NotFoundWorkflowResourceByKey, fragment);
            }
            //更新工作流资源状态为当前节点状态
            await workflowResource.UpdateStatus(node.WorkflowStatus);
            //针对该节点下的所有动作做处理
            await _commonSignConfigurationNodeActionStore.QueryByNode(node.ID, async (action) =>
             {
                 List<WorkflowUserInfo> stepUserInfos = new List<WorkflowUserInfo>();
                 //判断处理用户是否由外部手动输入
                 if (action.ExecuteUserIsManual)
                 {
                     if ((userInfos == null || userInfos.Count == 0) && string.IsNullOrEmpty(action.ExecuteUserGetServiceName))
                     {
                         var fragment = new TextFragment()
                         {
                             Code = TextCodes.CommonSignConfigurationNodeActionManualUserEmpty,
                             DefaultFormatting = "工作流资源类型为{0}的通用审批配置下名称为{1}的节点下的动作名称为{2}的节点动作需要外部传入用户，但没有传入",
                             ReplaceParameters = new List<object>() { configuration.WorkflowResourceType, node.Name, action.ActionName }
                         };

                         throw new UtilityException((int)Errors.CommonSignConfigurationNodeActionManualUserEmpty, fragment);
                     }
                     if (userInfos != null)
                     {
                         foreach (var userInfoItem in userInfos)
                         {
                             //手动输入直接使用参数中的用户类型和用户关键字
                             WorkflowStep workflowStep = new WorkflowStep()
                             {
                                 ResourceID = workflowResource.ID,
                                 ActionName = action.ActionName,
                                 UserType = userInfoItem.UserType,
                                 UserKey = userInfoItem.UserKey,
                                 UserCount = 1,
                                 Complete = false
                             };
                             //创建工作流步骤
                             await workflowResource.AddStep(workflowStep);
                             stepUserInfos.Add(userInfoItem);
                         }
                     }
                 }

                 if (!string.IsNullOrEmpty(action.ExecuteUserGetServiceName))
                 {
                     //需要通过获取待处理用户接口获取所有用户信息
                     var getExecuteUserService = _commonSignConfigurationNodeGetExecuteUserServiceSelector.Choose(action.ExecuteUserGetServiceName);

                     await getExecuteUserService.Execute(action, configuration.EntityType, entityKey, action.ExecuteUserGetServiceConfiguration, async (result) =>
                     {
                         var existsItem = (from item in stepUserInfos
                                           where item.UserType == result.UserType && item.UserKey == result.UserKey
                                           select item).FirstOrDefault();
                         if (existsItem == null)
                         {
                             //新建步骤
                             WorkflowStep workflowStep = new WorkflowStep()
                             {
                                 ResourceID = workflowResource.ID,
                                 ActionName = action.ActionName,
                                 UserType = result.UserType,
                                 UserKey = result.UserKey,
                                 UserCount = 1,
                                 Complete = false
                             };
                             //创建工作流步骤
                             await workflowResource.AddStep(workflowStep);

                             stepUserInfos.Add(new WorkflowUserInfo(result.UserType, result.UserKey));
                         }
                     });
                 }

                 //执行扩展动作
                 if (!string.IsNullOrEmpty(action.CreateFlowExecuteServiceName))
                 {
                     var executeService = _commonSignConfigurationNodeCreateFlowExecuteServiceSelector.Choose(action.CreateFlowExecuteServiceName);
                     await executeService.Execute(action, action.CreateFlowExecuteServiceConfiguration, sourceActionName, sourceStatus, entityKey, executeUseKey, executeUserEntensionInfo, stepUserInfos);
                 }
             });





        }

        public async Task DirectGo(CommonSignConfigurationNode node, string entityKey, string executeUseKey, string executeUserEntensionInfo, List<WorkflowUserInfo> userInfos)
        {
            var configuration = node.Configuration;
            //获取工作流资源
            var workflowResource = await _workflowResourceRepository.QueryByKey(configuration.WorkflowResourceType, entityKey);
            if (workflowResource == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundWorkflowResourceByKey,
                    DefaultFormatting = "找不到Type为{0}，Key为{1}的工作流资源",
                    ReplaceParameters = new List<object>() { configuration.WorkflowResourceType, entityKey }
                };

                throw new UtilityException((int)Errors.NotFoundWorkflowResourceByKey, fragment);
            }



            await _commonSignConfigurationNodeStore.Lock(string.Format(ApplicationLockBaseNames.CommonSignConfiguration, configuration.EntityType, entityKey),
                async () =>
                {
                    await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 5, 0) }))
                    {
                        if (!string.IsNullOrEmpty(node.DirectGoExecuteServiceName))
                        {
                            var directGoExecuteService = _commonSignConfigurationNodeDirectGoExecuteServiceSelector.Choose(node.DirectGoExecuteServiceName);
                            await directGoExecuteService.Execute(node, node.DirectGoExecuteServiceConfiguration, null, workflowResource.Status, entityKey, executeUseKey, executeUserEntensionInfo, userInfos);
                        }

                        //获取当前状态下所有步骤
                        await workflowResource.GetStepByCurrentStatus(async (step) =>
                        {
                            //为每个状态执行关闭动作
                            await workflowResource.UpdateStepCompleteStatus(step.ActionName, step.Status, true, async (userAction) =>
                            {
                                await Task.FromResult(0);
                            });
                        });
                        //创建流程处理
                        await CreateFlowExecute(node, entityKey, null, workflowResource.Status, executeUseKey, executeUserEntensionInfo, userInfos);

                        scope.Complete();
                    };
                },
                50000
            );




        }

        public async Task Sign(CommonSignConfigurationNode node, string actionName, string entityKey, string signUserKey, string signUserExtensionInfo, SignResult signResult)
        {
            var configuration = node.Configuration;

            //获取工作流资源
            var workflowResource = await _workflowResourceRepository.QueryByKey(configuration.WorkflowResourceType, entityKey);
            if (workflowResource == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundWorkflowResourceByKey,
                    DefaultFormatting = "找不到Type为{0}，Key为{1}的工作流资源",
                    ReplaceParameters = new List<object>() { configuration.WorkflowResourceType, entityKey }
                };

                throw new UtilityException((int)Errors.NotFoundWorkflowResourceByKey, fragment);
            }


            //获取对应动作名称的节点动作
            var nodeAction = await _commonSignConfigurationNodeActionStore.QueryByNodeAndActionName(node.ID, actionName);
            if (nodeAction == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationNodeActionByActionName,
                    DefaultFormatting = "工作流资源类型为{0}的通用审批配置下的节点{1}中，找不到动作名称为{2}的节点动作",
                    ReplaceParameters = new List<object>() { configuration.WorkflowResourceType, node.Name, actionName }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationNodeActionByActionName, fragment);
            }

            await _commonSignConfigurationNodeStore.Lock(string.Format(ApplicationLockBaseNames.CommonSignConfiguration, configuration.EntityType, entityKey),
                async () =>
                {
                    using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 5, 0) }))
                    {
                        //创建工作流用户动作
                        await workflowResource.AddUserAction(nodeAction.ActionName, node.WorkflowStatus, signUserKey, signResult.Result);
                        //根据审批类型获取通用审批配置节点审批后处理服务工厂
                        var signExtensionService = GetICommonSignConfigurationNodeSignExtensionService(nodeAction.SignType);
                        await signExtensionService.Sign(nodeAction, entityKey, signUserKey, signUserExtensionInfo, signResult);
                        
                        scope.Complete();
                    };
                },
                50000
            );


        }



        private ICommonSignConfigurationNodeSignExtensionService GetICommonSignConfigurationNodeSignExtensionService(string signType)
        {
            if (!_commonSignConfigurationNodeSignExtensionServiceFactories.TryGetValue(signType, out IFactory<ICommonSignConfigurationNodeSignExtensionService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationNodeSignExtensionServiceBySignType,
                    DefaultFormatting = "找不到审批类型为{0}的审批类型获取通用审批配置节点审批后处理服务，位置为{1}",
                    ReplaceParameters = new List<object>() { signType, "MSLibrary.Workflow.CommonSignConfigurationNodeIMP.CommonSignConfigurationNodeSignExtensionServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationNodeSignExtensionServiceBySignType, fragment);
            }

            return serviceFactory.Create();
        }

        public async Task GetAllAction(CommonSignConfigurationNode node, Func<CommonSignConfigurationNodeAction, Task> callback)
        {
            await _commonSignConfigurationNodeActionStore.QueryByNode(node.ID, callback);
        }

        public async Task<CommonSignConfigurationNodeAction> GetAction(CommonSignConfigurationNode node, string actionName)
        {
            return await _commonSignConfigurationNodeActionStore.QueryByNodeAndActionName(node.ID, actionName);
        }

    }


    /// <summary>
    /// 通用审批配置节点动作
    /// 通用审批配置节点与通用审批配置节点动作关系是1：N
    /// NodeId+ActionName唯一
    /// </summary>
    public class CommonSignConfigurationNodeAction : EntityBase<ICommonSignConfigurationNodeActionIMP>
    {
        private static IFactory<ICommonSignConfigurationNodeActionIMP> _commonSignConfigurationNodeActionIMPFactory;

        public static IFactory<ICommonSignConfigurationNodeActionIMP> CommonSignConfigurationNodeActionIMPFactory
        {
            set
            {
                _commonSignConfigurationNodeActionIMPFactory = value;
            }
        }
        public override IFactory<ICommonSignConfigurationNodeActionIMP> GetIMPFactory()
        {
            return _commonSignConfigurationNodeActionIMPFactory;
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
        /// 所属节点Id
        /// </summary>

        public Guid NodeId
        {
            get
            {
                return GetAttribute<Guid>("NodeId");
            }
            set
            {
                SetAttribute<Guid>("NodeId", value);
            }
        }
        /// <summary>
        /// 所属节点
        /// </summary>
        public CommonSignConfigurationNode Node
        {
            get
            {
                return GetAttribute<CommonSignConfigurationNode>("Node");
            }
            set
            {
                SetAttribute<CommonSignConfigurationNode>("Node", value);
            }
        }

        /// <summary>
        /// 动作名称
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
        /// 处理用户是否由外部手动输入
        /// </summary>
        public bool ExecuteUserIsManual
        {
            get
            {
                return GetAttribute<bool>("ExecuteUserIsManual");
            }
            set
            {
                SetAttribute<bool>("ExecuteUserIsManual", value);
            }
        }


        /// <summary>
        ///  获取处理用户的服务的配置信息
        ///  不同的获取处理用户的服务有不同类型的配置信息
        ///  可以为空
        /// </summary>
        public string ExecuteUserGetServiceConfiguration
        {
            get
            {
                return GetAttribute<string>("ExecuteUserGetServiceConfiguration");
            }
            set
            {
                SetAttribute<string>("ExecuteUserGetServiceConfiguration", value);
            }
        }

        /// <summary>
        /// 获取处理用户的服务名称
        /// 对应接口为ICommonSignConfigurationNodeGetExecuteUserService
        /// 可以为空
        /// </summary>
        public string ExecuteUserGetServiceName
        {
            get
            {
                return GetAttribute<string>("ExecuteUserGetServiceName");
            }
            set
            {
                SetAttribute<string>("ExecuteUserGetServiceName", value);
            }
        }

        /// <summary>
        /// 创建流程处理服务配置
        /// 供创建流程处理服务使用
        /// 可以为空
        /// </summary>
        public string CreateFlowExecuteServiceConfiguration
        {
            get
            {
                return GetAttribute<string>("CreateFlowExecuteServiceConfiguration");
            }
            set
            {
                SetAttribute<string>("CreateFlowExecuteServiceConfiguration", value);
            }
        }

        /// <summary>
        /// 创建流程处理服务名称
        /// 可以为空
        /// </summary>
        public string CreateFlowExecuteServiceName
        {
            get
            {
                return GetAttribute<string>("CreateFlowExecuteServiceName");
            }
            set
            {
                SetAttribute<string>("CreateFlowExecuteServiceName", value);
            }
        }


        /// <summary>
        /// 审批类型
        /// 来源自BOC.GSP2.Main.CommonSignConfigurationNodeSignTypes
        /// </summary>
        public string SignType
        {
            get
            {
                return GetAttribute<string>("SignType");
            }
            set
            {
                SetAttribute<string>("SignType", value);
            }
        }

        /// <summary>
        /// 审批类型的配置信息
        /// 不同的SignType要求不同的配置类型
        /// </summary>
        public string SignTypeConfiguration
        {
            get
            {
                return GetAttribute<string>("SignTypeConfiguration");
            }
            set
            {
                SetAttribute<string>("SignTypeConfiguration", value);
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
        /// 获取该节点动作的审批类型的配置信息转换成的配置对象
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetSignTypeConfiguration()
        {
            return await _imp.GetSignTypeConfiguration(this);
        }

    }


    public interface ICommonSignConfigurationNodeActionIMP
    {
        /// <summary>
        /// 获取审批类型的配置信息转换成的配置对象
        /// </summary>
        /// <param name="nodeAction"></param>
        /// <returns></returns>
        Task<object> GetSignTypeConfiguration(CommonSignConfigurationNodeAction nodeAction);
    }
    [Injection(InterfaceType = typeof(ICommonSignConfigurationNodeActionIMP), Scope = InjectionScope.Transient)]
    public class CommonSignConfigurationNodeActionIMP : ICommonSignConfigurationNodeActionIMP
    {
        private ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService _commonSignConfigurationNodeActionSignTypeConfigurationConvertService;


        public CommonSignConfigurationNodeActionIMP(ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService commonSignConfigurationNodeActionSignTypeConfigurationConvertService)
        {
            _commonSignConfigurationNodeActionSignTypeConfigurationConvertService = commonSignConfigurationNodeActionSignTypeConfigurationConvertService;
        }
        /// <summary>
        /// 获取审批类型的配置信息转换成的配置对象
        /// </summary>
        /// <param name="nodeAction"></param>
        /// <returns></returns>
        public async Task<object> GetSignTypeConfiguration(CommonSignConfigurationNodeAction nodeAction)
        {
            return await _commonSignConfigurationNodeActionSignTypeConfigurationConvertService.Convert(nodeAction.SignType, nodeAction.SignTypeConfiguration);
        }
    }

    /// <summary>
    /// 通用审批配置节点动作转换审批类型配置对象服务
    /// </summary>
    public interface ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="signType">审批类型</param>
        /// <param name="configurationInfo">审批类型配置信息</param>
        /// <returns></returns>
        Task<object> Convert(string signType, string configurationInfo);
    }

    /// <summary>
    /// 通用审批配置节点动作转换审批类型配置对象服务主实现
    /// </summary>
    [Injection(InterfaceType = typeof(ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationNodeActionSignTypeConfigurationConvertMainService : ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService
    {
        private static Dictionary<string, IFactory<ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService>> _serviceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService>>();

        public static Dictionary<string, IFactory<ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }
        public async Task<object> Convert(string signType, string configurationInfo)
        {
            if (!_serviceFactories.TryGetValue(signType, out IFactory<ICommonSignConfigurationNodeActionSignTypeConfigurationConvertService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationNodeActionSignTypeConfigurationConvertServiceBySignType,
                    DefaultFormatting = "找不到审批类型为{0}的通用审批配置节点动作转换审批类型配置对象服务",
                    ReplaceParameters = new List<object>() { signType, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationNodeActionSignTypeConfigurationConvertServiceBySignType, fragment);
            }

            return await Task.FromResult(await serviceFactory.Create().Convert(signType, configurationInfo));
        }
    }

    /// <summary>
    /// 通用审批配置节点创建流程处理服务接口
    /// 在CreateFlow方法执行后执行
    /// </summary>
    public interface ICommonSignConfigurationNodeCreateFlowExecuteService
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="nodeAction">节点动作</param>
        /// <param name="configurationData">配置数据</param>
        /// <param name="sourceActionName">执行该操作时所处的动作名称</param>
        /// <param name="sourceStatus">执行该操作时所处的工作流资源状态</param>
        /// <param name="entityKey">实体关键字</param>
        /// <param name="executeUserKey">处理用户的关键字</param>
        /// <param name="executeUserEntensionInfo">处理用户的扩展信息</param>
        /// <param name="userInfos">待处理用户信息列表</param>
        /// <returns></returns>
        Task Execute(CommonSignConfigurationNodeAction nodeAction, string configurationData, string sourceActionName, int sourceStatus, string entityKey, string executeUserKey, string executeUserEntensionInfo, List<WorkflowUserInfo> userInfos);
    }

    /// <summary>
    /// 通用审批配置节点创建流程处理服务选择器接口
    /// </summary>
    public interface ICommonSignConfigurationNodeCreateFlowExecuteServiceSelector : ISelector<ICommonSignConfigurationNodeCreateFlowExecuteService>
    {

    }

    /// <summary>
    /// 通用审批配置节点创建流程处理服务选择器
    /// </summary>
    [Injection(InterfaceType = typeof(ICommonSignConfigurationNodeCreateFlowExecuteServiceSelector), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationNodeCreateFlowExecuteServiceSelector : ICommonSignConfigurationNodeCreateFlowExecuteServiceSelector
    {
        private static Dictionary<string, IFactory<ICommonSignConfigurationNodeCreateFlowExecuteService>> _serviceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationNodeCreateFlowExecuteService>>();

        /// <summary>
        /// 通用审批配置节点创建流程处理服务工厂键值对
        /// 键为服务名称
        /// </summary>
        public static Dictionary<string, IFactory<ICommonSignConfigurationNodeCreateFlowExecuteService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }

        public ICommonSignConfigurationNodeCreateFlowExecuteService Choose(string name)
        {
            if (!_serviceFactories.TryGetValue(name, out IFactory<ICommonSignConfigurationNodeCreateFlowExecuteService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationNodeCreateFlowExecuteServiceByName,
                    DefaultFormatting = "找不到名称为{0}的通用审批配置节点创建流程处理服务，位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationNodeCreateFlowExecuteServiceByName, fragment);
            }

            return serviceFactory.Create();
        }
    }


    /// <summary>
    /// 通用审批配置节点直接跳转处理服务接口
    /// 在DirectGo方法执行前执行
    /// </summary>
    public interface ICommonSignConfigurationNodeDirectGoExecuteService
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="action">节点</param>
        /// <param name="configurationData">配置数据</param>
        /// <param name="sourceActionName">执行该操作时所处的动作名称</param>
        /// <param name="sourceStatus">执行该操作时所处的工作流资源状态</param>
        /// <param name="entityKey">实体关键字</param>
        /// <param name="executeUserKey">处理用户的关键字</param>
        /// <param name="executeUserEntensionInfo">处理用户的扩展信息</param>
        /// <param name="userInfos">待处理用户信息列表</param>
        /// <returns></returns>
        Task Execute(CommonSignConfigurationNode node, string configurationData, string sourceActionName, int sourceStatus, string entityKey, string executeUserKey, string executeUserEntensionInfo, List<WorkflowUserInfo> userInfos);
    }

    /// <summary>
    /// 通用审批配置节点直接跳转处理服务选择器接口
    /// </summary>
    public interface ICommonSignConfigurationNodeDirectGoExecuteServiceSelector : ISelector<ICommonSignConfigurationNodeDirectGoExecuteService>
    {

    }

    [Injection(InterfaceType = typeof(ICommonSignConfigurationNodeDirectGoExecuteServiceSelector), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationNodeDirectGoExecuteServiceSelector : ICommonSignConfigurationNodeDirectGoExecuteServiceSelector
    {
        private static Dictionary<string, IFactory<ICommonSignConfigurationNodeDirectGoExecuteService>> _serviceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationNodeDirectGoExecuteService>>();

        /// <summary>
        /// 通用审批配置节点创建流程处理服务工厂键值对
        /// 键为服务名称
        /// </summary>
        public static Dictionary<string, IFactory<ICommonSignConfigurationNodeDirectGoExecuteService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }

        public ICommonSignConfigurationNodeDirectGoExecuteService Choose(string name)
        {
            if (!_serviceFactories.TryGetValue(name, out IFactory<ICommonSignConfigurationNodeDirectGoExecuteService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationNodeDirectGoExecuteServiceByName,
                    DefaultFormatting = "找不到名称为{0}的通用审批配置节点动作直接跳转处理服务，位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationNodeDirectGoExecuteServiceByName, fragment);
            }

            return serviceFactory.Create();
        }
    }

    /// <summary>
    /// 通用审批配置节点审批后处理服务接口
    /// </summary>
    public interface ICommonSignConfigurationNodeSignExtensionService
    {
        Task Sign(CommonSignConfigurationNodeAction nodeAction, string entityKey, string signUserKey, string signUserExtensionInfo, SignResult signResult);
    }


    /// <summary>
    /// 通用审批配置节点获取待处理用户服务
    /// </summary>
    public interface ICommonSignConfigurationNodeGetExecuteUserService
    {
        Task Execute(CommonSignConfigurationNodeAction nodeAction, string entityType, string entityKey, string configurationData, Func<WorkflowUserInfo, Task> callback);
    }




    /// <summary>
    /// 审批结果
    /// </summary>
    public class SignResult
    {
        private int _result;
        private string _data;
        public SignResult(int result, string data)
        {
            _result = result;
            _data = data;
        }

        /// <summary>
        /// 审批的结果
        /// 由应用程序自己定义，如Approve、Reject、Return等等
        /// </summary>
        public int Result
        {
            get
            {
                return _result;
            }
        }

        /// <summary>
        /// 审批数据
        /// </summary>
        public string Data
        {
            get
            {
                return _data;
            }
        }
    }


    /// <summary>
    /// 通用审批配置节点获取待处理用户服务选择器接口
    /// </summary>
    public interface ICommonSignConfigurationNodeGetExecuteUserServiceSelector : ISelector<ICommonSignConfigurationNodeGetExecuteUserService>
    {

    }

    /// <summary>
    /// 通用审批配置节点获取待处理用户服务选择器默认实现
    /// </summary>
    [Injection(InterfaceType = typeof(ICommonSignConfigurationNodeGetExecuteUserServiceSelector), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationNodeGetExecuteUserServiceSelector : ICommonSignConfigurationNodeGetExecuteUserServiceSelector
    {
        private static Dictionary<string, IFactory<ICommonSignConfigurationNodeGetExecuteUserService>> _serviceFactories = new Dictionary<string, IFactory<ICommonSignConfigurationNodeGetExecuteUserService>>();

        /// <summary>
        /// 通用审批配置节点获取待处理用户服务工厂键值对
        /// 键为服务名称
        /// </summary>
        public static Dictionary<string, IFactory<ICommonSignConfigurationNodeGetExecuteUserService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }
        public ICommonSignConfigurationNodeGetExecuteUserService Choose(string name)
        {
            if (!_serviceFactories.TryGetValue(name, out IFactory<ICommonSignConfigurationNodeGetExecuteUserService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonSignConfigurationNodeGetExecuteUserServiceByName,
                    DefaultFormatting = "找不到名称为{0}的通用审批配置节点获取待处理用户服务，位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonSignConfigurationNodeGetExecuteUserServiceByName, fragment);
            }

            return serviceFactory.Create();
        }
    }

}
