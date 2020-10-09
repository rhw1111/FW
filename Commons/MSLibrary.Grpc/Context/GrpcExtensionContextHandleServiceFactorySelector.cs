using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Grpc.Context
{
    [Injection(InterfaceType = typeof(IGrpcExtensionContextHandleServiceFactorySelector), Scope = InjectionScope.Singleton)]
    public class GrpcExtensionContextHandleServiceFactorySelector : IGrpcExtensionContextHandleServiceFactorySelector
    {
        public static IDictionary<string, IFactory<IGrpcExtensionContextHandleService>> GrpcExtensionContextHandleServiceFactories { get; } = new Dictionary<string, IFactory<IGrpcExtensionContextHandleService>>();


        public IFactory<IGrpcExtensionContextHandleService> Choose(string name)
        {
            if (!GrpcExtensionContextHandleServiceFactories.TryGetValue(name, out IFactory<IGrpcExtensionContextHandleService>? factory))
            {
                var fragment = new TextFragment()
                {
                    Code = GrpcTextCodes.NotFountGrpcExtensionContextHandleServiceFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Grpc请求扩展上下文处理服务工厂，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.GrpcExtensionContextHandleServiceFactories" }
                };

                throw new UtilityException((int)GrpcErrorCodes.NotFountGrpcExtensionContextHandleServiceFactoryByName, fragment);
            }

            return factory;
        }
    }
}
