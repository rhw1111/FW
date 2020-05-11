using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Security.BusinessSecurityRule.OriginalParametersFilterServices
{
    /// <summary>
    /// 业务动作原始参数信息过滤处理服务选择器
    /// 通过静态键值属性SeviceFactories存储所有过滤处理服务工厂
    /// </summary>
    [Injection(InterfaceType = typeof(ISelector<IOriginalParametersFilterService>), Scope = InjectionScope.Singleton)]
    public class OriginalParametersFilterServiceSelector : ISelector<IOriginalParametersFilterService>
    {
        private static Dictionary<string, IFactory<IOriginalParametersFilterService>> _serviceFactories = new Dictionary<string, IFactory<IOriginalParametersFilterService>>();

        /// <summary>
        /// 所有过滤处理服务
        /// </summary>
        public static Dictionary<string, IFactory<IOriginalParametersFilterService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }

        public IOriginalParametersFilterService Choose(string name)
        {
            if (_serviceFactories.TryGetValue(name.ToLower(), out IFactory<IOriginalParametersFilterService> factory))
            {
                return factory.Create();
            }
            return null;
        }
    }
}
