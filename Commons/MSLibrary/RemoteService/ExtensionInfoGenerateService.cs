using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.RemoteService
{
    [Injection(InterfaceType = typeof(IExtensionInfoGenerateService), Scope = InjectionScope.Singleton)]
    public class ExtensionInfoGenerateService : IExtensionInfoGenerateService
    {
        public static IDictionary<string, IList<IFactory<IExtensionInfoGenerateService>>> NameServiceFactories { get; } = new Dictionary<string, IList<IFactory<IExtensionInfoGenerateService>>>();
        
        public async Task<IDictionary<string, string>> Generate(string name, object state)
        {
            Dictionary<string, string> infos = new Dictionary<string, string>();
            var services = getServices(name);
            foreach(var item in services)
            {
                infos.Merge(await item.Create().Generate(name,state));
            }
            return infos;
        }

        public IDictionary<string, string> GenerateSync(string name, object state)
        {
            Dictionary<string, string> infos = new Dictionary<string, string>();
            var services = getServices(name);
            foreach (var item in services)
            {
                infos.Merge(item.Create().GenerateSync(name, state));
            }
            return infos;
        }

        private IList<IFactory<IExtensionInfoGenerateService>> getServices(string name)
        {
            if (!NameServiceFactories.TryGetValue(name,out IList<IFactory<IExtensionInfoGenerateService>>? serviceFactories))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundNameExtensionInfoGenerateServiceByName,
                    DefaultFormatting = "找不到名称为{0}的命名远程调用附加信息生成服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() {name,  $"{ this.GetType().FullName }.NameServices" }
                };

                throw new UtilityException((int)Errors.NotFoundNameExtensionInfoGenerateServiceByName, fragment);
            }

            return serviceFactories;
        }
    }

}
