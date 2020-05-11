using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Core;

namespace MSLibrary.DI
{
    /// <summary>
    /// 针对Autofac的DI容器
    /// </summary>
    public class DIContainerForAutofac : IDIContainer
    {
        private readonly  ContainerBuilder _containerBuilder;
        private  ILifetimeScope _lifetimeScope = null;

        public DIContainerForAutofac(ContainerBuilder containerBuilder)
        {
           
            _containerBuilder = containerBuilder;

        }

        private DIContainerForAutofac(ContainerBuilder containerBuilder,ILifetimeScope lifetimeScope)
        {
            _containerBuilder = containerBuilder;
            _lifetimeScope = lifetimeScope;
        }


        public IDIContainer CreateContainer()
        {
            return new DIContainerForAutofac(_containerBuilder,_lifetimeScope.BeginLifetimeScope());
        }

        public void Dispose()
        {
            if (_lifetimeScope != null)
            {
                _lifetimeScope.Dispose();
            }
        }

        public T Get<T>()
        {
            return _lifetimeScope.Resolve<T>();
        }

        public object Get(Type serviceType)
        {
            return _lifetimeScope.Resolve(serviceType);
        }

        public object Get(Type serviceType, params object[] arguments)
        {
            List<PositionalParameter> parameters = new List<PositionalParameter>();
            int index = 0;
            foreach (var item in arguments)
            {
                index++;
                parameters.Add(new PositionalParameter(index, item));
            }

            return _lifetimeScope.Resolve(serviceType, parameters);
        }

        public object Get(Type serviceType, object[] arguments, Type[] argumentTypes)
        {
            return Get(serviceType, arguments);
        }

        public T Get<T>(params object[] arguments)
        {
            List<PositionalParameter> parameters = new List<PositionalParameter>();
            int index = 0;
            foreach (var item in arguments)
            {
                index++;
                parameters.Add(new PositionalParameter(index, item));
            }

            return _lifetimeScope.Resolve<T>(parameters);
        }

        public T Get<T>(object[] arguments, Type[] argumentTypes)
        {
            return Get<T>(arguments);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _lifetimeScope.Resolve<IEnumerable<T>>();
        }

        public IEnumerable<object> GetAll(Type serviceType)
        {
            Type type = typeof(IEnumerable<>);
            type = type.MakeGenericType(serviceType);
            return (IEnumerable<object>)_lifetimeScope.Resolve(type);
        }

        public IEnumerable<Type> GetInterceptorType<T>(MethodInfo method)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Type> GetInterceptorType(Type targetType, MethodInfo method)
        {
            throw new NotImplementedException();
        }

        public void Inject(Type serviceType, Type implementType, InjectionScope scope)
        {
            var builder = _containerBuilder.RegisterType(implementType).As(serviceType);
            switch (scope)
            {
                case InjectionScope.Singleton:
                    builder.SingleInstance();
                    break;
                case InjectionScope.Transient:
                    builder.InstancePerDependency();
                    break;
                default:
                    builder.InstancePerLifetimeScope();
                    break;
            }

            
        }

        public void Inject<T>(InjectionScope scope, IFactory<T> factory) where T : class
        {
            var builder = _containerBuilder.Register((context) =>
            {
                return factory.Create();
            }).As<T>();
            switch (scope)
            {
                case InjectionScope.Singleton:
                    builder.SingleInstance();
                    break;
                case InjectionScope.Transient:
                    builder.InstancePerDependency();
                    break;
                default:
                    builder.InstancePerLifetimeScope();
                    break;
            }

          
        }

        public void Inject<T>(object obj) where T : class
        {
            _containerBuilder.RegisterInstance(obj).As<T>().SingleInstance();

           
        }

        public void Inject(Type serviceType, object obj)
        {
            _containerBuilder.RegisterInstance(obj).As(serviceType).SingleInstance();

           
        }

        public void Inject(Type serviceType, Type implementType, params object[] arguments)
        {
            List<PositionalParameter> parameters = new List<PositionalParameter>();
            int index = 0;
            foreach (var item in arguments)
            {
                index++;
                parameters.Add(new PositionalParameter(index, item));
            }

            _containerBuilder.RegisterType(implementType).WithParameters(parameters).As(serviceType).SingleInstance();

            
        }

        public void Inject(Type serviceType, Type implementType, object[] arguments, Type[] argumentTypes)
        {
            Inject(serviceType, implementType, arguments);
            
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

        public void Inject<T, V>(InjectionScope scope)
            where T : class
            where V : class, T
        {
            var builder = _containerBuilder.RegisterType<V>().As<T>();
            switch (scope)
            {
                case InjectionScope.Singleton:
                    builder.SingleInstance();
                    break;
                case InjectionScope.Transient:
                    builder.InstancePerDependency();
                    break;
                default:
                    builder.InstancePerLifetimeScope();
                    break;
            }
            
        }

        public void Inject<T, V>(params object[] arguments)
            where T : class
            where V : class, T
        {
            List<PositionalParameter> parameters = new List<PositionalParameter>();
            int index = 0;
            foreach (var item in arguments)
            {
                index++;
                parameters.Add(new PositionalParameter(index, item));
            }

            _containerBuilder.RegisterType<T>().WithParameters(parameters).As<V>().SingleInstance();
           
        }

        public void Inject<T, V>(object[] arguments, Type[] argumentTypes)
            where T : class
            where V : class, T
        {
            Inject<T,V>(arguments);
      
        }

        public void CompleteInit(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }
    }
}
