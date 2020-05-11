using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Workflow
{
    /// <summary>
    /// 从工作流步骤中的UserKey解析出用户关键字服务的默认实现
    /// </summary>
    [Injection(InterfaceType = typeof(IGetUserInfoFromWorkflowStepService), Scope = InjectionScope.Transient)]
    public class GetUserInfoFromWorkflowStepMainService : IGetUserInfoFromWorkflowStepService
    {
        private static Dictionary<string, IFactory<IGetUserInfoFromWorkflowStepService>> _getUserInfoFromWorkflowStepServiceFactories = new Dictionary<string, IFactory<IGetUserInfoFromWorkflowStepService>>();

        /// <summary>
        /// 获取步骤UserKey解析出用户关键字的服务工厂键值对
        /// 键为步骤的UserType
        /// </summary>
        public static Dictionary<string, IFactory<IGetUserInfoFromWorkflowStepService>> GetUserInfoFromWorkflowStepServiceFactories
        {
            get
            {
                return _getUserInfoFromWorkflowStepServiceFactories;
            }
        }


        public async Task Execute(string userType, string userKey, Func<string, Task> callback)
        {
            //调用服务返回用户信息
            if (!_getUserInfoFromWorkflowStepServiceFactories.TryGetValue(userType, out IFactory<IGetUserInfoFromWorkflowStepService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundWorkflowGetUserInfoServiceFactoryByType,
                    DefaultFormatting = "找不到类型为{0}的Factory<IGetUserInfoFromWorkflowStepService>",
                    ReplaceParameters = new List<object>() { userType }
                };

                throw new UtilityException((int)Errors.NotFoundWorkflowGetUserInfoServiceFactoryByType, fragment);
            }

            var service = serviceFactory.Create();
            await service.Execute(userType, userKey, callback);
        }
    }
}
