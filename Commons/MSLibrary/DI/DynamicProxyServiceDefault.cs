using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace MSLibrary.DI
{
    /// <summary>
    /// 动态代理服务的默认实现
    /// 通过Castle实现
    /// </summary>
    public class DynamicProxyServiceDefault : IDynamicProxyService
    {
        public T CreateAopProxy<T>(T target, params Type[] interceptorTypes)
        {
            List<IInterceptor> interceptorList = new List<IInterceptor>();
            //从DI容器中获取拦截器对象
            foreach (var typeItem in interceptorTypes)
            {
                
                var interceptorObj=DIContainerContainer.Get(typeItem);
                if (interceptorObj==null)
                {
                    throw new Exception($"not fount servicetype {typeItem.FullName} in DIContainerContainer");
                }
                var interceptorItem = interceptorObj as IInterceptorItem;
                if (interceptorItem==null)
                {
                    throw new Exception($"servicetype {typeItem.FullName} in DIContainerContainer doesn't convert to {typeof(IInterceptorItem).FullName}");
                }

                interceptorList.Add(new InterceptorItemAdapter(interceptorItem));
            }


            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 拦截器上下文适配Castle的IInvocation的适配器
    /// </summary>
    public class InvocationAdapter : IInterceptorItemContext
    {
        private IInvocation _invocation;
        public InvocationAdapter(IInvocation invocation)
        {
            _invocation = invocation;
        }
        public object[] Arguments
        {
            get
            {
                return _invocation.Arguments;
            }
        }

        public MethodInfo Method
        {
            get
            {
                return _invocation.Method;
            }
        }

        public object ReturnValue
        {
            get
            {
                return _invocation.ReturnValue;
            }
            set
            {
                _invocation.ReturnValue = value;
            }
        }

        public void Next()
        {
            _invocation.Proceed();
        }
    }

    /// <summary>
    /// Castle拦截器适配IInterceptorItem的适配器
    /// </summary>
    public class InterceptorItemAdapter : IInterceptor
    {
        private IInterceptorItem _interceptorItem;

        public InterceptorItemAdapter(IInterceptorItem interceptorItem)
        {
            _interceptorItem = interceptorItem;
        }
        public void Intercept(IInvocation invocation)
        {
            InvocationAdapter invocationAdapter = new InvocationAdapter(invocation);
            _interceptorItem.Execute(invocationAdapter).Wait();
        }
    }
}
