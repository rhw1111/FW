using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.CommonQueue
{
    [Injection(InterfaceType = typeof(ICommonMessageHandleService), Scope = InjectionScope.Singleton)]
    public class CommonMessageHandleServiceMain : ICommonMessageHandleService
    {
        private static Dictionary<string, IFactory<ICommonMessageHandleService>> _messageHandleServiceFactories = new Dictionary<string, IFactory<ICommonMessageHandleService>>();
       
        public static IDictionary<string, IFactory<ICommonMessageHandleService>> MessageHandleServiceFactories
        {
            get
            {
                return _messageHandleServiceFactories;
            }
        }
        public async Task Handle(CommonMessage message)
        {
            var service = getService(message.Type);
            await service.Handle(message);
        }

        private ICommonMessageHandleService getService(string messageType)
        {
            if (!_messageHandleServiceFactories.TryGetValue(messageType, out IFactory<ICommonMessageHandleService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCommonMessageHandleServiceByMessageType,
                    DefaultFormatting = "找不到消息类型为{0}的消息处理服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { messageType, $"{this.GetType().FullName}.MessageHandleServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCommonMessageHandleServiceByMessageType, fragment);
            }
            return serviceFactory.Create();
        }
    }
}
