using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MSLibrary.DI
{
    /// <summary>
    /// DI容器上下文
    /// </summary>
    public class ContextDIContainer : IContext<IDIContainer>
    {
        private static AsyncLocal<IDIContainer> _asyncLocal = new AsyncLocal<IDIContainer>();
        private static ThreadLocal<IDIContainer> _threadLocal = new ThreadLocal<IDIContainer>();

        public IDIContainer Value
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
