using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MSLibrary.DI
{
    /// <summary>
    /// 依赖注入容器的静态统一入口点
    /// </summary>
    public static class DIContainerContainer
    {
        private static IDIContainer _diContainer;
        /// <summary>
        /// 设置提供服务的容器对象
        /// </summary>
        public static IDIContainer DIContainer
        {
            set
            {
                _diContainer = value;
            }
            get
            {
                return _diContainer;
            }
        }
        /// <summary>
        /// 以类型方式注入
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <typeparam name="V">实际类型</typeparam>
        /// <param name="scope">注入范围</param>
        public static void Inject<T, V>(InjectionScope scope)
            where T : class
            where V : class, T
        {
            _diContainer.Inject<T, V>(scope);
        }

        /// <summary>
        /// 以类型方式注入
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="implementType">实现类型</param>
        /// <param name="scope">注入范围</param>
        public static void Inject(Type serviceType, Type implementType,InjectionScope scope)
        {
            _diContainer.Inject(serviceType,implementType, scope);
        }


        /// <summary>
        /// 以工厂方式注入
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="scope">注入范围</param>
        /// <param name="factory">接口工厂</param>
        public static void Inject<T>(InjectionScope scope, IFactory<T> factory)
             where T : class
        {
            _diContainer.Inject<T>(scope, factory);
        }

        /// <summary>
        /// 直接注入对象
        /// 使用单例模式
        /// </summary>
        /// <param name="serviceType">要注入的类型</param>
        /// <param name="obj">要注入的对象</param>
        public static void Inject(Type serviceType, object obj)
        {
            _diContainer.Inject(serviceType,obj);
        }


        /// <summary>
        /// 直接注入对象
        /// 使用单例模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void Inject<T>(object obj)
            where T : class
        {
            _diContainer.Inject<T>(obj);
        }

        /// <summary>
        /// 注入类型
        /// 使用单例模式
        /// 要注入的对象初始化构造函数中的部分参数来自arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="arguments"></param>
        public static void Inject<T, V>(params object[] arguments)
            where T : class
            where V : class, T
        {
            _diContainer.Inject<T, V>(arguments);
        }

        /// <summary>
        /// 注入类型
        /// 使用单例模式
        /// 要注入的对象初始化构造函数中的部分参数来自arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="arguments"></param>
        /// <param name="argumentTypes"></param>
        public static void Inject<T, V>( object[] arguments, Type[] argumentTypes)
            where T : class
            where V : class, T
        {
            _diContainer.Inject<T, V>(arguments,argumentTypes);
        }

        /// <summary>
        /// 注入类型
        /// 使用单例模式
        /// 要注入的对象初始化构造函数中的部分参数来自arguments
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementType"></param>
        /// <param name="arguments"></param>
        public static void Inject(Type serviceType, Type implementType, params object[] arguments)
        {
            _diContainer.Inject(serviceType, implementType, arguments);
        }

        /// <summary>
        /// 注入类型
        /// 使用单例模式
        /// 要注入的对象初始化构造函数中的部分参数来自arguments
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementType"></param>
        /// <param name="arguments"></param>
        /// <param name="argumentTypes"></param>
        public static void Inject(Type serviceType, Type implementType,  object[] arguments,Type[] argumentTypes)
        {
            _diContainer.Inject(serviceType, implementType, arguments,argumentTypes);
        }


        /// <summary>
        /// 获取指定服务类型的注入对象
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            
            return _diContainer.Get<T>();
        }

        /// <summary>
        /// 获取指定服务类型的注入对象
        /// 如果存在arguments，优先使用arguments参数作为注入参数
        /// </summary>
        /// <typeparam name="T">必须为具体实现类</typeparam>
        /// <param name="arguments">优先注入构造函数的参数</param>
        /// <returns></returns>
        public static T Get<T>(params object[] arguments)
        {
            return _diContainer.Get<T>(arguments);
        }

        /// <summary>
        /// 获取指定服务类型的注入对象
        /// 如果存在arguments，优先使用arguments参数作为注入参数
        /// </summary>
        /// <typeparam name="T">必须为具体实现类</typeparam>
        /// <param name="arguments">优先注入构造函数的参数</param>
        /// <param name="argumentTypes">对应参数的类型数组</param>
        /// <returns></returns>
        public static T Get<T>(object[] arguments, Type[] argumentTypes)
        {
            return _diContainer.Get<T>(arguments,argumentTypes);
        }

        /// <summary>
        /// 获取指定服务类型的注入对象
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public static object Get(Type serviceType)
        {
            return _diContainer.Get(serviceType);
        }

        /// <summary>
        /// 获取指定服务类型的注入对象
        /// 如果存在arguments，优先使用arguments参数作为注入参数
        /// </summary>
        /// <param name="serviceType">服务类型，必须为具体实现类</param>
        /// <param name="arguments">优先注入构造函数的参数</param>
        /// <returns></returns>
        public static object Get(Type serviceType, params object[] arguments)
        {
            return _diContainer.Get(serviceType, arguments);
        }


        /// <summary>
        /// 获取指定服务类型的注入对象
        /// 如果存在arguments，优先使用arguments参数作为注入参数
        /// </summary>
        /// <param name="serviceType">服务类型，必须为具体实现类</param>
        /// <param name="arguments">优先注入构造函数的参数</param>
        /// <param name="argumentTypes">对应参数的类型数组</param>
        /// <returns></returns>
        public static object Get(Type serviceType, object[] arguments, Type[] argumentTypes)
        {
            return _diContainer.Get(serviceType, arguments,argumentTypes);
        }


        /// <summary>
        /// 获取指定服务类型的所有注入对象
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>()
        {
            return _diContainer.GetAll<T>();
        }

        /// <summary>
        /// 获取指定服务类型的所有注入对象
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public static IEnumerable<object> GetAll(Type serviceType)
        {
            return _diContainer.GetAll(serviceType);
        }

        /// <summary>
        /// 基于自身创建一个新的容器
        /// </summary>
        /// <returns></returns>
        public static IDIContainer CreateContainer()
        {
            return _diContainer.CreateContainer();
        }
    }
}
