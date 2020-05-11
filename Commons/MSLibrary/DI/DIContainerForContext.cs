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
    /// 基于Provider上下文的依赖注入容器接口
    /// 需要在外部设置Provider上下文
    /// </summary>
    public class DIContainerForContext : IDIContainer
    {
        private IServiceCollection _serviceCollection;

        public DIContainerForContext(IServiceCollection serviceCollection)
        {

            _serviceCollection = serviceCollection;
        }




        public IDIContainer CreateContainer()
        {
            var serviceProvider=ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            var scope = serviceProvider.CreateScope();

            var newContainer = new DIContainerDefault(_serviceCollection, scope);
            return newContainer;
        }

        public void Dispose()
        {

        }

        public T Get<T>()
        {
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            return serviceProvider.GetService<T>();
        }

        public object Get(Type serviceType)
        {
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            return serviceProvider.GetService(serviceType);
        }

        public T Get<T>(params object[] arguments)
        {
            var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();

            var factory = ActivatorUtilities.CreateFactory(typeof(T), argumentTypes ?? Type.EmptyTypes);
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            return (T)factory(serviceProvider, arguments);
        }

        public T Get<T>(object[] arguments, Type[] argumentTypes)
        {
            //var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();

            var factory = ActivatorUtilities.CreateFactory(typeof(T), argumentTypes);
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            return (T)factory(serviceProvider, arguments);
        }


        public object Get(Type serviceType, params object[] arguments)
        {
            var argumentTypes = arguments?.Select(a => a.GetType())?.ToArray();

            var factory = ActivatorUtilities.CreateFactory(serviceType, argumentTypes ?? Type.EmptyTypes);
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            return factory(serviceProvider, arguments);
        }

        public object Get(Type serviceType, object[] arguments, Type[] argumentTypes)
        {
            var factory = ActivatorUtilities.CreateFactory(serviceType, argumentTypes);
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            return factory(serviceProvider, arguments);
        }

        public IEnumerable<T> GetAll<T>()
        {
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            return serviceProvider.GetServices<T>();
        }

        public IEnumerable<object> GetAll(Type serviceType)
        {
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            return serviceProvider.GetServices(serviceType);
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
            where V : class, T
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
                        _serviceCollection.AddSingleton(serviceType, implementType);
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

        public void Inject(Type serviceType, object obj)
        {
            Dispose();
            try
            {
                _serviceCollection.AddSingleton(serviceType, obj);
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
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            var obj = factory(serviceProvider, arguments);

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
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            var obj = factory(serviceProvider, arguments);

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
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            var obj = factory(serviceProvider, arguments);

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
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            var obj = factory(serviceProvider, arguments);

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
            var serviceProvider = ContextContainer.GetValue<IServiceProvider>(ContextTypes.ServiceProvider);
            serviceProvider = _serviceCollection.BuildServiceProvider();
        }
    }
}
