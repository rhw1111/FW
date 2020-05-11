using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using Castle.Components.DictionaryAdapter;

namespace MSLibrary.SRR
{
    [Injection(InterfaceType = typeof(ISRRHostContainer), Scope = InjectionScope.Singleton)]
    public class SRRHostContainerDefault : ISRRHostContainer
    {
        private Dictionary<string, ISRRHostService> _services = new Dictionary<string, ISRRHostService>();
        public async Task<ISRRHostService> Get(string name)
        {
            if (!_services.TryGetValue(name,out ISRRHostService service))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSRRHostServiceInContainerByName,
                    DefaultFormatting = "找不到名称为{0}的消息请求响应主机服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.Get" }
                };

                throw new UtilityException((int)Errors.NotFoundSRRHostServiceInContainerByName, fragment);
            }

            return service;
        }

        public ISRRHostContainer Register(string name, Action<ISRRHostConfiguration> configure)
        {
            var configuration=DIContainerContainer.Get<SRRHostConfigurationDefault>();
            configure(configuration);
            var hostService= DIContainerContainer.Get<ISRRHostService>();
            hostService.Configure(configuration);
            _services[name] = hostService;

            return this;
        }
    }


    [Injection(InterfaceType = typeof(SRRHostConfigurationDefault), Scope = InjectionScope.Transient)]
    public class SRRHostConfigurationDefault : ISRRHostConfiguration
    {
        private Dictionary<string, ISRRRequestHandlerDescription> _handlerDescriptions = new Dictionary<string, ISRRRequestHandlerDescription>();
        public IDictionary<string, IList<ISRRMiddleware>> Middlewares
        {
            get;
        } = new Dictionary<string, IList<ISRRMiddleware>>();

        public IList<ISRRFilter> GlobalFilters
        {
            get;
        } = new List<ISRRFilter>();

        public ISRRRequestHandlerDescription GetHandlerDescription(string requestType)
        {
            if (!_handlerDescriptions.TryGetValue(requestType,out ISRRRequestHandlerDescription description))
            {
                return null;
            }

            return description;
        }

        public void RegisterHandlerDescription(string requestType, IList<ISRRFilter> filters, IFactory<ISRRRequestHandler> handlerFactory)
        {
            var handlerDescription = DIContainerContainer.Get<SRRRequestHandlerDescriptionDefault>();
            foreach(var item in filters)
            {
                handlerDescription.Filters.Add(item);
            }

            handlerDescription.HandlerFactory = handlerFactory;
            _handlerDescriptions[requestType] = handlerDescription;
        }
    }
}
