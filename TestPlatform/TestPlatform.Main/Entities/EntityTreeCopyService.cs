using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using PacketDotNet.Ieee80211;

namespace FW.TestPlatform.Main.Entities
{
    [Injection(InterfaceType = typeof(IEntityTreeCopyService), Scope = InjectionScope.Singleton)]
    public class EntityTreeCopyService : IEntityTreeCopyService
    {
        public static IDictionary<string, IFactory<IEntityTreeCopyService>> EntityTreeCopyServiceFactories { get; } = new Dictionary<string, IFactory<IEntityTreeCopyService>>();
        public async Task<bool> Execute(string type, Guid entityID, Guid? parentTreeID)
        {
            var service = getService(type);
            return await service.Execute(type, entityID, parentTreeID);
        }

        private IEntityTreeCopyService getService(string type)
        {
            if (!EntityTreeCopyServiceFactories.TryGetValue(type,out IFactory<IEntityTreeCopyService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundEntityTreeCopyServiceByType,
                    DefaultFormatting = "找不到类型为{0}的实体树复制服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.EntityTreeCopyServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundEntityTreeCopyServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }
}
