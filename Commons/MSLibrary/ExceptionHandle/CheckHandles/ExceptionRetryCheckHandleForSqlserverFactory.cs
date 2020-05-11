using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.ExceptionHandle.CheckHandles
{
    [Injection(InterfaceType = typeof(ExceptionRetryCheckHandleForSqlserverFactory), Scope = InjectionScope.Singleton)]
    public class ExceptionRetryCheckHandleForSqlserverFactory : IFactory<IExceptionRetryCheckHandle>
    {
        public IExceptionRetryCheckHandle Create()
        {
            ExceptionRetryCheckHandleForSqlserver exceptionRetryCheckHandleForSqlserver = new ExceptionRetryCheckHandleForSqlserver();
            return exceptionRetryCheckHandleForSqlserver;
        }
    }
}
