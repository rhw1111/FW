using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace MSLibrary.DI
{
    /// <summary>
    /// 依赖注入容器接口的默认实现
    /// 通过包装外部ServiceCollection的方式，
    /// 将依赖注入功能转发到外部ServiceCollection
    /// 从而实现功能
    /// </summary>
    public class DIContainerDefault : IDIContainer
    {
        private IServiceCollection _serviceCollection;
        //private AsyncLocal<IServiceProvider> _serviceProvider = new AsyncLocal<IServiceProvider>();
        private IServiceProvider _serviceProvider;
        private bool _disposeProvider=true;

        public DIContainerDefault(IServiceCollection serviceCollection, IServiceProvider serviceProvider)
        {
            
            _serviceCollection = serviceCollection;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 私有构造函数
        /// 创建一个拥有新的ServiceScope的容器
        /// 该容器可以被独立销毁
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="serviceScope"></param>
        /// <param name="disposeProvider"></param>
        private DIContainerDefault(IServiceCollection serviceCollection, IServiceScope serviceScope,bool disposeProvider)
        {
            _serviceCollection = serviceCollection;
            
            _serviceProvider = serviceScope.ServiceProvider;
            _disposeProvider = disposeProvider;

        }

        public DIContainerDefault(IServiceCollection serviceCollection, IServiceScope serviceScope)
        {
            _serviceCollection = serviceCollection;
            _serviceProvider = serviceScope.ServiceProvider;
            _disposeProvider = false;

        }

        public IDIContainer CreateContainer()
        {
            var disposeProvider = false;
            var scope = _serviceProvider.CreateScope();

            var newContainer = new DIContainerDefault(_serviceCollection, scope, disposeProvider);
            return newContainer;
        }

        public void Dispose()
        {
            var serviceProvider = _serviceProvider;
       
            if (serviceProvider != null && _disposeProvider)
            {
                lock (serviceProvider)
                {

                    ((IDisposable)serviceProvider).Dispose();
                }
            }
        }

        public T Get<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public object Get(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public T Get<T>(params object[] arguments)
        {
            var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();

            var factory = ActivatorUtilities.CreateFactory(typeof(T), argumentTypes ?? Type.EmptyTypes);

            return (T)factory(_serviceProvider, arguments);
        }

        public T Get<T>(object[] arguments,Type[] argumentTypes)
        {
            //var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();

            var factory = ActivatorUtilities.CreateFactory(typeof(T), argumentTypes);

            return (T)factory(_serviceProvider, arguments);
        }


        public object Get(Type serviceType, params object[] arguments)
        {
            var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();

            var factory = ActivatorUtilities.CreateFactory(serviceType, argumentTypes ?? Type.EmptyTypes);

            return factory(_serviceProvider, arguments);
        }

        public object Get(Type serviceType, object[] arguments, Type[] argumentTypes)
        {
            var factory = ActivatorUtilities.CreateFactory(serviceType, argumentTypes);

            return factory(_serviceProvider, arguments);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _serviceProvider.GetServices<T>();
        }

        public IEnumerable<object> GetAll(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }


        public IEnumerable<Type> GetInterceptorType<T>(MethodInfo method)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Type> GetInterceptorType(Type targetType, MethodInfo method)
        {
            throw new NotImplementedException();
        }

        public void Inject<T, V>(InjectionScope scope)
            where T : class
            where V : class,T
        {
            Dispose();

            try
            {
                switch (scope)
                {
                    case InjectionScope.Singleton:
                        _serviceCollection.AddSingleton<T, V>();
                        break;
                    case InjectionScope.Scoped:
                        _serviceCollection.AddScoped<T, V>();
                        break;
                    case InjectionScope.Transient:
                        _serviceCollection.AddTransient<T, V>();
                        break;
                }
            }
            finally
            {
                NewServiceProvider();
            }
        }

        public void Inject<T>(InjectionScope scope, IFactory<T> factory)
            where T : class
        {
            Dispose();

            try
            {
                switch (scope)
                {
                    case InjectionScope.Singleton:
                        _serviceCollection.AddSingleton<T>((provider) =>
                        {
                            return factory.Create();
                        });
                        break;
                    case InjectionScope.Scoped:
                        _serviceCollection.AddScoped<T>((provider) =>
                        {
                            return factory.Create();
                        });
                        break;
                    case InjectionScope.Transient:
                        _serviceCollection.AddTransient<T>((provider) =>
                        {
                            return factory.Create();
                        });
                        break;
                }
            }
            finally
            {
                NewServiceProvider();
            }
        }

        public void Inject(Type serviceType, Type implementType, InjectionScope scope)
        {
            Dispose();

            try
            {
                switch (scope)
                {
                    case InjectionScope.Singleton:
                        _serviceCollection.AddSingleton(serviceType,implementType);
                        break;
                    case InjectionScope.Scoped:
                        _serviceCollection.AddScoped(serviceType, implementType);
                        break;
                    case InjectionScope.Transient:
                        _serviceCollection.AddTransient(serviceType, implementType);
                        break;
                }
            }
            finally
            {
                NewServiceProvider();
            }
        }

        public void Inject<T>(object obj)
             where T : class
        {
            Dispose();
            try
            {
                _serviceCollection.AddSingleton(typeof(T), obj);
            }
            finally
            {
                NewServiceProvider();
            }
        }

        public void Inject(Type serviceType,object obj)
        {
            Dispose();
            try
            {
                _serviceCollection.AddSingleton(serviceType,obj);
            }
            finally
            {
                NewServiceProvider();
            }
        }

        public void Inject<T, V>(params object[] arguments)
            where T : class
            where V : class, T
        {
            Dispose();

            var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();

            var factory = ActivatorUtilities.CreateFactory(typeof(V), argumentTypes ?? Type.EmptyTypes);

            var obj=factory(_serviceProvider, arguments);

            try
            {
                _serviceCollection.AddSingleton(typeof(T), obj);
            }
            finally
            {
                NewServiceProvider();
            }
        }


        public void Inject(Type serviceType, Type implementType, params object[] arguments)
        {
            Dispose();

            var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();

            var factory = ActivatorUtilities.CreateFactory(implementType, argumentTypes ?? Type.EmptyTypes);

            var obj = factory(_serviceProvider, arguments);

            try
            {
                _serviceCollection.AddSingleton(serviceType, obj);
            }
            finally
            {
                NewServiceProvider();
            }
        }

        public void Inject(Type serviceType, Type implementType, object[] arguments, Type[] argumentTypes)
        {
            Dispose();


            var factory = ActivatorUtilities.CreateFactory(implementType, argumentTypes);

            var obj = factory(_serviceProvider, arguments);

            try
            {
                _serviceCollection.AddSingleton(serviceType, obj);
            }
            finally
            {
                NewServiceProvider();
            }
        }

        public void RegisterInterceptor(params Type[] interceptorTypes)
        {
            throw new NotImplementedException();
        }

        public void RegisterInterceptor<T>(bool isOverride, params Type[] interceptorTypes)
        {
            throw new NotImplementedException();
        }

        public void RegisterInterceptor<T>(bool isOverride, MethodInfo method, params Type[] interceptorTypes)
        {
            throw new NotImplementedException();
        }

        public void Inject<T, V>(object[] arguments, Type[] argumentTypes)
            where T : class
            where V : class, T
        {
            Dispose();

            var factory = ActivatorUtilities.CreateFactory(typeof(V), argumentTypes);

            var obj = factory(_serviceProvider, arguments);

            try
            {
                _serviceCollection.AddSingleton(typeof(T), obj);
            }
            finally
            {
                NewServiceProvider();
            }
        }

        private void NewServiceProvider()
        {
            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }
    }
}
