using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Workflow.DAL;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 工作流步骤
    /// 记录参与流程的每一步动作
    /// ResourceID+Status+ActionName+UserType+UserKey唯一
    /// </summary>
    public class WorkflowStep : EntityBase<IWorkflowStepIMP>
    {
        private static IFactory<IWorkflowStepIMP> _workflowStepIMPFactory;

        public static IFactory<IWorkflowStepIMP> WorkflowStepIMPFactory
        {
            set
            {
                _workflowStepIMPFactory = value;
            }
        }
        public override IFactory<IWorkflowStepIMP> GetIMPFactory()
        {
            return _workflowStepIMPFactory;
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
        /// 关联的资源ID
        /// </summary>
        public Guid ResourceID
        {
            get
            {
                return GetAttribute<Guid>("ResourceID");
            }
            set
            {
                SetAttribute<Guid>("ResourceID", value);
            }
        }

        /// <summary>
        /// 关联的资源
        /// </summary>
        public WorkflowResource Resource
        {
            get
            {
                return GetAttribute<WorkflowResource>("Resource");
            }
            set
            {
                SetAttribute<WorkflowResource>("Resource", value);
            }
        }

        /// <summary>
        /// 步骤动作名称
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
        /// 动作发生时资源的当前状态
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
        /// 批次号
        /// 用来把相同状态的步骤归为一个批次
        /// 不同的状态不能使用同一个批次号
        /// </summary>
        public string SerialNo
        {
            get
            {
                return GetAttribute<string>("SerialNo");
            }
            set
            {
                SetAttribute<string>("SerialNo", value);
            }
        }

        /// <summary>
        /// 参与步骤的人员类型
        /// </summary>
        public string UserType
        {
            get
            {
                return GetAttribute<string>("UserType");
            }
            set
            {
                SetAttribute<string>("UserType", value);
            }
        }

        /// <summary>
        /// 参与步骤的人员关键字
        /// </summary>
        public string UserKey
        {
            get
            {
                return GetAttribute<string>("UserKey");
            }
            set
            {
                SetAttribute<string>("UserKey", value);
            }
        }

        /// <summary>
        /// 参与该步骤需要的人员数量
        /// 只有达到数量的人员都完成后，才能完成步骤
        /// </summary>
        public int UserCount
        {
            get
            {
                return GetAttribute<int>("UserCount");
            }
            set
            {
                SetAttribute<int>("UserCount", value);
            }
        }

        /// <summary>
        /// 步骤是否已经完成
        /// </summary>
        public bool Complete
        {
            get
            {
                return GetAttribute<bool>("Complete");
            }
            set
            {
                SetAttribute<bool>("Complete", value);
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



        public async Task<ValidateResult> ValidateUser(string userKey)
        {
            return await _imp.ValidateUser(this, userKey);
        }

        /// <summary>
        /// 获取该步骤中UserKey所表示的用户信息
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetUserInfos(Func<string,Task> callback)
        {
            await _imp.GetUserInfos(this,callback);
        }

    }

    public interface IWorkflowStepIMP
    {

        /// <summary>
        /// 验证用户信息是否匹配该步骤
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<ValidateResult> ValidateUser(WorkflowStep step, string userKey);
        /// <summary>
        /// 获取该步骤中UserKey所表示的用户信息
        /// </summary>
        /// <param name="step"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetUserInfos(WorkflowStep step, Func<string, Task> callback);
    }


    [Injection(InterfaceType = typeof(IWorkflowStepIMP), Scope = InjectionScope.Transient)]
    public class WorkflowStepIMP : IWorkflowStepIMP
    {
        private static Dictionary<string, IFactory<IValidateUserKeyService>> _validateUserKeyServiceFactories = new Dictionary<string, IFactory<IValidateUserKeyService>>();


        /// <summary>
        /// 验证用户是否符合指定用户关键字信息的服务工厂键值对
        /// 键为步骤的UserType
        /// </summary>
        public static Dictionary<string, IFactory<IValidateUserKeyService>> ValidateUserKeyServiceFactories
        {
            get
            {
                return _validateUserKeyServiceFactories;
            }
        }

        private IGetUserInfoFromWorkflowStepService _getUserInfoFromWorkflowStepService;
        private IWorkflowStepUserActionStore _workflowStepUserActionStore;

        public WorkflowStepIMP(IGetUserInfoFromWorkflowStepService getUserInfoFromWorkflowStepService,IWorkflowStepUserActionStore workflowStepUserActionStore)
        {
            _getUserInfoFromWorkflowStepService = getUserInfoFromWorkflowStepService;
            _workflowStepUserActionStore = workflowStepUserActionStore;
        }

        public async Task<ValidateResult> ValidateUser(WorkflowStep step, string userKey)
        {
            ValidateResult result = new ValidateResult()
            {
                Result = true
            };

            //如果该步骤已经是完成状态，则直接返回false
            if (step.Complete)
            {
                result.Result = false;
                result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.WorkflowStepHasCompleted, "ID为{0}，资源ID为{1}，动作名称{2},状态为{3},用户类型为{4}，用户关键字为{5}的工作流步骤已经是完成状态"), step.ID, step.ResourceID, step.ActionName, step.Status, step.UserType, step.UserKey);
                return result;
            }

            //需要检查步骤下面是否已经存在相同用户的动作
            var userAction = await _workflowStepUserActionStore.QueryByStepAndUser(step.Resource.Type, step.Resource.Key, step.ID, userKey);
            if (userAction != null)
            {
                //如果存在，表示该用户已经做过动作，返回false
                result.Result = false;
                result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.WorkflowStepUserHasAction, "ID为{0}，资源ID为{1}，动作名称{2},状态为{3},用户类型为{4}，用户关键字为{5}的工作流步骤中用户信息为{6}的用户已经处理过"), step.ID, step.ResourceID, step.ActionName, step.Status, step.UserType, step.UserKey, userKey);
                return result;
            }

            //需要调用服务检查
            if (!_validateUserKeyServiceFactories.TryGetValue(step.UserType, out IFactory<IValidateUserKeyService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundWorkflowValidateUserKeyServiceFactoryByType,
                    DefaultFormatting = "在工作流步骤的实现中，找不到类型为{0}的Factory<IValidateUserKeyService>",
                    ReplaceParameters = new List<object>() { step.UserType }
                };

                throw new UtilityException((int)Errors.NotFoundWorkflowValidateUserKeyServiceFactoryByType, fragment);
            }

            var service = serviceFactory.Create();
            result = await service.Execute(step.UserKey, userKey);

            return result;

        }

        public async Task GetUserInfos(WorkflowStep step, Func<string, Task> callback)
        {     
            await _getUserInfoFromWorkflowStepService.Execute(step.UserType,step.UserKey, callback);
        }
    }

    /// <summary>
    /// 验证用户是否符合指定用户关键字信息的服务
    /// userKey为步骤的UserKey
    /// </summary>
    public interface IValidateUserKeyService
    {
        Task<ValidateResult> Execute(string userKey, string userInfo);
    }



    /// <summary>
    /// 工作流用户信息
    /// </summary>
    public class WorkflowUserInfo
    {
        private string _userType;
        private string _userKey;

        public WorkflowUserInfo(string userType, string userKey)
        {
            _userType = userType;
            _userKey = userKey;
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType
        {
            get
            {
                return _userType;
            }
        }
        /// <summary>
        /// 用户关键字
        /// </summary>
        public string UserKey
        {
            get
            {
                return _userKey;
            }
        }
    }
}
