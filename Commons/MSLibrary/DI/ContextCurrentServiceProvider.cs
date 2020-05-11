using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MSLibrary.DI
{
    public class ContextCurrentServiceProvider : IContext<IServiceProvider>
    {
        private static AsyncLocal<IServiceProvider> _asyncLocal = new AsyncLocal<IServiceProvider>();
        private static ThreadLocal<IServiceProvider> _threadLocal = new ThreadLocal<IServiceProvider>();

        public IServiceProvider Value
        {
            get
            {
                return _asyncLocal.Value;
            }

            set
            {
                _asyncLocal.Value = value;
                _threadLocal.Value = value;
            }
        }

        public bool IsAuto()
        {
            return !_threadLocal.IsValueCreated;
        }
    }

}
