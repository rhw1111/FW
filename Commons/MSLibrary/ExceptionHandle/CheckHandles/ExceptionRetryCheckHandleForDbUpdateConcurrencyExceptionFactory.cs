using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.ExceptionHandle.CheckHandles
{

    [Injection(InterfaceType = typeof(ExceptionRetryCheckHandleForDbUpdateConcurrencyExceptionFactory), Scope = InjectionScope.Singleton)]
    public class ExceptionRetryCheckHandleForDbUpdateConcurrencyExceptionFactory : IFactory<IExceptionRetryCheckHandle>
    {
        private ExceptionRetryCheckHandleForDbUpdateConcurrencyException _exceptionRetryCheckHandleForDbUpdateConcurrencyException;

        public ExceptionRetryCheckHandleForDbUpdateConcurrencyExceptionFactory(ExceptionRetryCheckHandleForDbUpdateConcurrencyException exceptionRetryCheckHandleForDbUpdateConcurrencyException)
        {
            _exceptionRetryCheckHandleForDbUpdateConcurrencyException = exceptionRetryCheckHandleForDbUpdateConcurrencyException;
        }
        public IExceptionRetryCheckHandle Create()
        {
            return _exceptionRetryCheckHandleForDbUpdateConcurrencyException;
        }
    }
}
