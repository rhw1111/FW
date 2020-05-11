using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Loader;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MSLibrary.FactoryStorage;

namespace MSLibrary.DI
{
    /// <summary>
    /// 依赖注入容器初始化处理的默认实现
    /// </summary>
    public class DIContainerInitDefault : IDIContainerInit
    {
        /// <summary>
        /// 加载指定程序集中的类
        /// </summary>
        /// <param name="assemblyNames">程序集名称集合</param>
        public void Execute(params string[] assemblyNames)
        {

            //循环加载程序集
            foreach (var itemName in assemblyNames)
            {


                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(itemName));

                //检查每个程序集中的类是否需要注入
                var types = assembly.GetTypes();
                foreach (var itemType in types)
                {
                    //优先处理注入工厂属性
                    var injectionFactoryAttributes = itemType.GetTypeInfo().GetCustomAttributes<InjectionFactoryAttribute>();
                    if (injectionFactoryAttributes.Count() != 0)
                    {
                        foreach (var itemAttribute in injectionFactoryAttributes)
                        {
                            //注入依赖容器
                            ExecuteInjectionFactory(itemAttribute.InterfaceType, itemAttribute.Name, itemAttribute.Scope);
                        }
                    }
                    else
                    {
                        //处理注入类型属性
                        var injectionAttributes = itemType.GetTypeInfo().GetCustomAttributes<InjectionAttribute>();

                        foreach (var itemAttribute in injectionAttributes)
                        {

                            //注入依赖容器
                            ExecuteInjection(itemAttribute.InterfaceType, itemType, itemAttribute.Scope);
                        }
                    }
                }
            }


        }


        /// <summary>
        /// 执行注入工厂
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="factoryName"></param>
        /// <param name="scope"></param>
        private void ExecuteInjectionFactory(Type interfaceType, string factoryName, InjectionScope scope)
        {
            //通过构建表达式树来动态执行
            var factoryGetMethod = typeof(FactoryStorageContainer).GetMethod("Get").MakeGenericMethod(interfaceType);
            ParameterExpression factoryNameExp = Expression.Parameter(typeof(string));
            MethodCallExpression factoryGetCallExp = Expression.Call(factoryGetMethod, factoryNameExp);

            //这里总是得到null，所以没办法只能通过下面的方式获得methodinfo
            //var method = typeof(DIContainerContainer).GetMethod("Inject",new Type[] {typeof(InjectionScope),typeof(IFactory<>) });

            var injectMethod = typeof(DIContainerContainer).GetMethods().First(m => m.Name.Equals("Inject") && m.IsGenericMethod && m.GetParameters().Length == 2).MakeGenericMethod(interfaceType);
            ParameterExpression scopeExp = Expression.Parameter(typeof(InjectionScope));
            MethodCallExpression injectCallExp = Expression.Call(injectMethod, scopeExp, factoryGetCallExp);


            LambdaExpression lambda = Expression.Lambda(typeof(Action<string, InjectionScope>), injectCallExp, factoryNameExp, scopeExp);

            var fun = lambda.Compile();
            fun.DynamicInvoke(factoryName, scope);

        }

        /// <summary>
        /// 执行注入类型
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="type"></param>
        /// <param name="scope"></param>
        private void ExecuteInjection(Type interfaceType, Type type, InjectionScope scope)
        {
            var injectMethod = typeof(DIContainerContainer).GetMethods().First(m => m.Name.Equals("Inject") && m.IsGenericMethod && m.GetParameters().Length == 1).MakeGenericMethod(interfaceType, type);
            ParameterExpression scopeExp = Expression.Parameter(typeof(InjectionScope));
            MethodCallExpression injectCallExp = Expression.Call(injectMethod, scopeExp);

            LambdaExpression lambda = Expression.Lambda(typeof(Action<InjectionScope>), injectCallExp, scopeExp);

            var fun = lambda.Compile();
            fun.DynamicInvoke(scope);
        }
    }
}
