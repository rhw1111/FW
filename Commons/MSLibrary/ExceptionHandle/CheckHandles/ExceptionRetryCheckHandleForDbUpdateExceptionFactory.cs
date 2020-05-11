using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.ExceptionHandle.CheckHandles
{

    [Injection(InterfaceType = typeof(ExceptionRetryCheckHandleForDbUpdateExceptionFactory), Scope = InjectionScope.Singleton)]
    public class ExceptionRetryCheckHandleForDbUpdateExceptionFactory : IFactory<IExceptionRetryCheckHandle>
    {
        private ExceptionRetryCheckHandleForDbUpdateException _exceptionRetryCheckHandleForDbUpdateException;

        public ExceptionRetryCheckHandleForDbUpdateExceptionFactory(ExceptionRetryCheckHandleForDbUpdateException exceptionRetryCheckHandleForDbUpdateException)
        {
            _exceptionRetryCheckHandleForDbUpdateException = exceptionRetryCheckHandleForDbUpdateException;
        }
        public IExceptionRetryCheckHandle Create()
        {
            return _exceptionRetryCheckHandleForDbUpdateException;
        }
    }
}
