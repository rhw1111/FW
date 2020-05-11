using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace MSLibrary.DI
{
    /// <summary>
    /// 依赖注入容器接口
    /// </summary>
    public interface IDIContainer : IDisposable
    {
        /// <summary>
        /// 以类型方式注入
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <typeparam name="V">实际类型</typeparam>
        /// <param name="scope">注入范围</param>
        void Inject<T, V>(InjectionScope scope)
            where T : class
            where V : class,T;


        /// <summary>
        /// 以类型方式注入
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="implementType">实际类型</param>
        /// <param name="scope">注入范围</param>
        void Inject(Type serviceType,Type implementType, InjectionScope scope);


        /// <summary>
        /// 以工厂方式注入
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="scope">注入范围</param>
        /// <param name="factory">接口工厂</param>
        void Inject<T>(InjectionScope scope, IFactory<T> factory)
             where T : class;

        /// <summary>
        /// 直接注入对象
        /// 使用单例模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        void Inject<T>(object obj)
            where T:class;
        /// <summary>
        /// 直接注入对象
        /// 使用单例模式
        /// </summary>
        /// <param name="serviceType">要注入的类型</param>
        /// <param name="obj">要注入的对象</param>
        void Inject(Type serviceType, object obj);

        /// <summary>
        /// 注入类型
        /// 使用单例模式
        /// 要注入的对象初始化构造函数中的部分参数来自arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="arguments"></param>
        void Inject<T, V>(params object[] arguments)
            where T : class
            where V : class, T;


        /// <summary>
        /// 注入类型
        /// 使用单例模式
        /// 要注入的对象初始化构造函数中的部分参数来自arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="arguments"></param>
        /// <param name="argumentTypes">对应参数的类型数组</param>
        void Inject<T, V>(object[] arguments, Type[] argumentTypes)
            where T : class
            where V : class, T;




        /// <summary>
        /// 注入类型
        /// 使用单例模式
        /// 要注入的对象初始化构造函数中的部分参数来自arguments
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementType"></param>
        /// <param name="arguments"></param>
        void Inject(Type serviceType, Type implementType, params object[] arguments);


        /// <summary>
        /// 注入类型
        /// 使用单例模式
        /// 要注入的对象初始化构造函数中的部分参数来自arguments
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementType"></param>
        /// <param name="arguments"></param>
        /// <param name="argumentTypes">对应参数的类型数组</param>
        void Inject(Type serviceType, Type implementType,  object[] arguments, Type[] argumentTypes);




        /// <summary>
        /// 获取指定服务类型的注入对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <returns></returns>
        T Get<T>();

        /// <summary>
        /// 获取指定服务类型的注入对象
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        object Get(Type serviceType);


        /// <summary>
        /// 获取指定服务类型的注入对象
        /// 如果存在arguments，优先使用arguments参数作为注入参数
        /// </summary>
        /// <param name="serviceType">服务类型，必须为具体实现类</param>
        /// <param name="arguments">优先注入构造函数的参数</param>
        /// <returns></returns>
        object Get(Type serviceType,params object[] arguments);

        /// <summary>
        /// 获取指定服务类型的注入对象
        /// 如果存在arguments，优先使用arguments参数作为注入参数
        /// </summary>
        /// <param name="serviceType">服务类型，必须为具体实现类</param>
        /// <param name="arguments">优先注入构造函数的参数</param>
        /// <param name="argumentTypes">对应参数的类型数组</param>
        /// <returns></returns>
        object Get(Type serviceType, object[] arguments, Type[] argumentTypes);

        /// <summary>
        /// 获取指定服务类型的注入对象
        /// 如果存在arguments，优先使用arguments参数作为注入参数
        /// </summary>
        /// <typeparam name="T">必须为具体实现类</typeparam>
        /// <param name="arguments">优先注入构造函数的参数</param>
        /// <returns></returns>
        T Get<T>(params object[] arguments);
        /// <summary>
        /// 获取指定服务类型的注入对象
        /// 如果存在arguments，优先使用arguments参数作为注入参数
        /// </summary>
        /// <typeparam name="T">必须为具体实现类</typeparam>
        /// <param name="arguments">优先注入构造函数的参数</param>
        /// <param name="argumentTypes">对应参数的类型数组</param>
        /// <returns></returns>
        T Get<T>(object[] arguments, Type[] argumentTypes);

        /// <summary>
        /// 获取指定服务类型的所有注入对象
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>();




        /// <summary>
        /// 获取指定服务类型的所有注入对象
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        IEnumerable<object> GetAll(Type serviceType);

        /// <summary>
        /// 注册全局Aop拦截器
        /// </summary>
        /// <param name="interceptorTypes">注册的拦截器列表</param>
        void RegisterInterceptor(params Type[] interceptorTypes);

        /// <summary>
        /// 为类型T注册Aop拦截器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isOverride">是否覆盖之前的拦截器</param>
        /// <param name="interceptorTypes">注册的拦截器列表</param>
        void RegisterInterceptor<T>(bool isOverride,params Type[] interceptorTypes);

        /// <summary>
        /// 为类型T的指定方法注册Aop拦截器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isOverride">是否覆盖之前的拦截器</param>
        /// <param name="method">方法元数据</param>
        /// <param name="interceptorTypes">注册的拦截器列表</param>
        void RegisterInterceptor<T>(bool isOverride,MethodInfo method, params Type[] interceptorTypes);


        /// <summary>
        /// 获取指定类型指定方法的拦截器类型列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">方法元数据</param>
        /// <returns>拦截器类型列表</returns>
        IEnumerable<Type> GetInterceptorType<T>(MethodInfo method);


        /// <summary>
        /// 获取指定类型指定方法的拦截器类型列表
        /// </summary>
        /// <param name="targetType">目标类型</param>
        /// <param name="method">方法元数据</param>
        /// <returns>拦截器类型列表</returns>
        IEnumerable<Type> GetInterceptorType(Type targetType,  MethodInfo method);

        /// <summary>
        /// 基于自身创建一个新的容器
        /// </summary>
        /// <returns></returns>
        IDIContainer CreateContainer();
    }


    /// <summary>
    /// 注入范围
    /// </summary>
    public enum InjectionScope
    {
        Singleton = 0,
        Scoped = 1,
        Transient = 2
    }
}
