using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.Loader;


namespace MSLibrary.FactoryStorage
{
    /// <summary>
    /// 工厂存储初始化接口默认实现
    /// </summary>
    public class FactoryStorageInitDefault : IFactoryStorageInit
    {
        public void Execute(params string[] assemblyNames)
        {

            var addMethod = typeof(FactoryStorageContainer).GetMethod("Add");

            //循环加载程序集
            foreach (var itemName in assemblyNames)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(itemName));
                //检查每个程序集中的类是否需要注入
                var types = assembly.GetTypes();
                foreach (var itemType in types)
                {
                    //检查类型是否有特性FactoryStorage，并且实现了接口IFactory

                    var typeInfo = itemType.GetTypeInfo();

                    var storgageAttribute = typeInfo.GetCustomAttribute<FactoryStorageAttribute>();

                    if (storgageAttribute != null)
                    {
                        var interfaces = typeInfo.ImplementedInterfaces;
                        foreach (var interfaceItem in interfaces)
                        {
                            if (interfaceItem.GetTypeInfo().GetGenericTypeDefinition().FullName == typeof(IFactory<>).FullName)
                            {
                                addMethod.MakeGenericMethod(interfaceItem.GenericTypeArguments[0]).Invoke(null, new object[] { storgageAttribute.Name, Activator.CreateInstance(itemType) });
                            }
                        }
                    }


                }
            }
        }
    }
}
