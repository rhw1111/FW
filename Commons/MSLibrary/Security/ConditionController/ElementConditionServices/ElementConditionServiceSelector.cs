using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ConditionServices
{
    /// <summary>
    /// 逻辑条件节点服务选择器
    /// 通过静态键值属性SeviceFactories存储所有节点处理服务工厂
    /// 键为XML节点的名称
    /// </summary>
    [Injection(InterfaceType = typeof(ISelector<IElementConditionService>), Scope = InjectionScope.Singleton)]
    public class ElementConditionServiceSelector : ISelector<IElementConditionService>
    {
        private static Dictionary<string, IFactory<IElementConditionService>> _serviceFactories = new Dictionary<string, IFactory<IElementConditionService>>();

        public static Dictionary<string, IFactory<IElementConditionService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }

        public IElementConditionService Choose(string name)
        {
            if (_serviceFactories.TryGetValue(name.ToLower(), out IFactory<IElementConditionService> factory))
            {
                return factory.Create();
            }
            return null;
        }
    }
}
