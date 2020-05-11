using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.ExceptionHandle.CheckHandles
{
    [Injection(InterfaceType = typeof(ExceptionRetryCheckHandleForMongoExceptionFactory), Scope = InjectionScope.Singleton)]
    public class ExceptionRetryCheckHandleForMongoExceptionFactory : IFactory<IExceptionRetryCheckHandle>
    {
        private ExceptionRetryCheckHandleForMongoException _exceptionRetryCheckHandleForMongoException;

        public ExceptionRetryCheckHandleForMongoExceptionFactory(ExceptionRetryCheckHandleForMongoException exceptionRetryCheckHandleForMongoException)
        {
            _exceptionRetryCheckHandleForMongoException = exceptionRetryCheckHandleForMongoException;
        }
        public IExceptionRetryCheckHandle Create()
        {
            return _exceptionRetryCheckHandleForMongoException;
        }
    }
}
