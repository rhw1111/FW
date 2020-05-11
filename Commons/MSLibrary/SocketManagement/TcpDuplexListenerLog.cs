using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.SocketManagement.DAL;

namespace MSLibrary.SocketManagement
{
    public class TcpDuplexListenerLog : EntityBase<ITcpDuplexListenerLogIMP>
    {
        private static IFactory<ITcpDuplexListenerLogIMP> _tcpDuplexListenerLogIMPFactory;
        public static IFactory<ITcpDuplexListenerLogIMP> TcpDuplexListenerLogIMPFactory
        {
            set
            {
                _tcpDuplexListenerLogIMPFactory = value;
            }
        }

        public override IFactory<ITcpDuplexListenerLogIMP> GetIMPFactory()
        {
            return _tcpDuplexListenerLogIMPFactory;
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



    public interface ITcpDuplexListenerLogIMP
    {
        Task Add(TcpDuplexListenerLog log);
        Task Delete(TcpDuplexListenerLog log);
    }

}
