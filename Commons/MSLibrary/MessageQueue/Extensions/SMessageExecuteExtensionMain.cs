using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue.Extensions
{
    /// <summary>
    /// 消息执行扩展点默认实现
    /// 按照不同的消息类型和监听者名称转发执行
    /// key的格式为消息类型-监听者名称
    /// </summary>
    [Injection(InterfaceType = typeof(SMessageExecuteExtensionMain), Scope = InjectionScope.Singleton)]
    public class SMessageExecuteExtensionMain : ISMessageExecuteExtension
    {
        private static Dictionary<string, IFactory<ISMessageExecuteExtension>> _sMessageExecuteExtensionFactories = new Dictionary<string, IFactory<ISMessageExecuteExtension>>();

        public static Dictionary<string, IFactory<ISMessageExecuteExtension>> SMessageExecuteExtensionFactories
        {
            get
            {
                return _sMessageExecuteExtensionFactories;
            }
        }

        public async Task OnSMessageTypeListenerExecuted(ISMessageTypeListenerPostContext context)
        {
            if (_sMessageExecuteExtensionFactories.TryGetValue($"{context.MessageType}-{context.ListenerName}", out IFactory<ISMessageExecuteExtension> factory))
            {
                var smessageExecuteExtension = factory.Create();
                await smessageExecuteExtension.OnSMessageTypeListenerExecuted(context);
            }
        }
    }

    /// <summary>
    /// 消息执行扩展点默认实现的工厂
    /// </summary>
    [Injection(InterfaceType = typeof(SMessageExecuteExtensionMainFactory), Scope = InjectionScope.Singleton)]
    public class SMessageExecuteExtensionMainFactory : IFactory<ISMessageExecuteExtension>
    {
        private SMessageExecuteExtensionMain _sMessageExecuteExtensionMain;
        public SMessageExecuteExtensionMainFactory(SMessageExecuteExtensionMain sMessageExecuteExtensionMain)
        {
            _sMessageExecuteExtensionMain = sMessageExecuteExtensionMain;
        }

        public ISMessageExecuteExtension Create()
        {
            return _sMessageExecuteExtensionMain;
        }
    }
}
