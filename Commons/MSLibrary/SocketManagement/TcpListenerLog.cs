using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.SocketManagement.DAL;

namespace MSLibrary.SocketManagement
{
    /// <summary>
    /// Tcp监听日志
    /// </summary>
    public class TcpListenerLog : EntityBase<ITcpListenerLogIMP>
    {
        private static IFactory<ITcpListenerLogIMP> _tcpListenerLogIMPFactory;
        public static IFactory<ITcpListenerLogIMP> TcpListenerLogIMPFactory
        {
            set
            {
                _tcpListenerLogIMPFactory = value;
            }
        }

        public override IFactory<ITcpListenerLogIMP> GetIMPFactory()
        {
            return _tcpListenerLogIMPFactory;
        }

        /// <summary>
        /// id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// TCP监听器名称
        /// </summary>
        public string ListenerName
        {
            get
            {
                return GetAttribute<string>("ListenerName");
            }
            set
            {
                SetAttribute<string>("ListenerName", value);
            }
        }

        /// <summary>
        /// 请求时间（UTC）
        /// </summary>
        public DateTime RequestTime
        {
            get
            {
                return GetAttribute<DateTime>("RequestTime");
            }
            set
            {
                SetAttribute<DateTime>("RequestTime", value);
            }
        }

        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequestContent
        {
            get
            {
                return GetAttribute<string>("RequestContent");
            }
            set
            {
                SetAttribute<string>("RequestContent", value);
            }
        }

        /// <summary>
        /// 请求处理时间（毫秒）
        /// </summary>
        public int ExecuteDuration
        {
            get
            {
                return GetAttribute<int>("ExecuteDuration");
            }
            set
            {
                SetAttribute<int>("ExecuteDuration", value);
            }
        }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseContent
        {
            get
            {
                return GetAttribute<string>("ResponseContent");
            }
            set
            {
                SetAttribute<string>("ResponseContent", value);
            }
        }

        /// <summary>
        /// 响应时间（UTC）
        /// </summary>
        public DateTime ResponseTime
        {
            get
            {
                return GetAttribute<DateTime>("ResponseTime");
            }
            set
            {
                SetAttribute<DateTime>("ResponseTime", value);
            }
        }

        public bool IsError
        {
            get
            {
                return GetAttribute<bool>("IsError");
            }
            set
            {
                SetAttribute<bool>("IsError", value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return GetAttribute<string>("ErrorMessage");
            }
            set
            {
                SetAttribute<string>("ErrorMessage", value);
            }
        }

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }
    }

    public interface ITcpListenerLogIMP
    {
        Task Add(TcpListenerLog log);
        Task Delete(TcpListenerLog log);
    }

    [Injection(InterfaceType = typeof(ITcpListenerLogIMP), Scope = InjectionScope.Transient)]
    public class TcpListenerLogIMP : ITcpListenerLogIMP
    {
        private ITcpListenerLogStore _tcplistenerLogStore;

        public TcpListenerLogIMP(ITcpListenerLogStore tcplistenerLogStore)
        {
            _tcplistenerLogStore = tcplistenerLogStore;
        }
        public async Task Add(TcpListenerLog log)
        {
            await _tcplistenerLogStore.Add(log);
        }

        public async Task Delete(TcpListenerLog log)
        {
            await _tcplistenerLogStore.Delete(log.ListenerName, log.ID);
        }
    }
}
