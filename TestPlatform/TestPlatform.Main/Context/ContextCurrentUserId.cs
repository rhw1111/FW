using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MSLibrary;

namespace FW.TestPlatform.Main.Context
{
    /// <summary>
    /// 当前用户ID上下文
    /// </summary>
    public class ContextCurrentUserId : IContext<Guid>
    {
        private static AsyncLocal<Guid> _asyncLocal = new AsyncLocal<Guid>();
        private static ThreadLocal<Guid> _threadLocal = new ThreadLocal<Guid>();

        public Guid Value
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
