using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 消息历史监听明细
    /// 记录每条消息已经完成的监听
    /// </summary>
    public class SMessageHistoryListenerDetail : EntityBase<ISMessageHistoryListenerDetailIMP>
    {
        private static IFactory<ISMessageHistoryListenerDetailIMP> _smessageHistoryListenerDetailIMPFactory;

        public static IFactory<ISMessageHistoryListenerDetailIMP> SMessageHistoryListenerDetailIMPFactory
        {
            set
            {
                _smessageHistoryListenerDetailIMPFactory = value;
            }
        }

        public override IFactory<ISMessageHistoryListenerDetailIMP> GetIMPFactory()
        {
            return _smessageHistoryListenerDetailIMPFactory;
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
        /// 消息历史ID
        /// </summary>
        public Guid SMessageHistoryID
        {
            get
            {
                return GetAttribute<Guid>("SMessageHistoryID");
            }
            set
            {
                SetAttribute<Guid>("SMessageHistoryID", value);
            }
        }

        /// <summary>
        /// 消息历史
        /// </summary>
        public SMessageHistory SMessageHistory
        {
            get
            {
                return GetAttribute<SMessageHistory>("SMessageHistory");
            }
            set
            {
                SetAttribute<SMessageHistory>("SMessageHistory", value);
            }
        }

        /// <summary>
        /// 监听名称
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
        /// 监听模式
        /// </summary>
        public int ListenerMode
        {
            get
            {
                return GetAttribute<int>("ListenerMode");
            }
            set
            {
                SetAttribute<int>("ListenerMode", value);
            }
        }

        /// <summary>
        /// 监听工厂类型
        /// </summary>
        public string ListenerFactoryType
        {
            get
            {
                return GetAttribute<string>("ListenerFactoryType");
            }
            set
            {
                SetAttribute<string>("ListenerFactoryType", value);
            }
        }

        /// <summary>
        /// 监听的web服务地址
        /// </summary>
        public string ListenerWebUrl
        {
            get
            {
                return GetAttribute<string>("ListenerWebUrl");
            }
            set
            {
                SetAttribute<string>("ListenerWebUrl", value);
            }
        }


        /// <summary>
        /// 监听的web服务实际访问地址
        /// </summary>
        public string ListenerRealWebUrl
        {
            get
            {
                return GetAttribute<string>("ListenerRealWebUrl");
            }
            set
            {
                SetAttribute<string>("ListenerRealWebUrl", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }
    }


    public interface ISMessageHistoryListenerDetailIMP
    {

    }


    [Injection(InterfaceType = typeof(ISMessageHistoryListenerDetailIMP), Scope = InjectionScope.Transient)]
    public class SMessageHistoryListenerDetailIMP:ISMessageHistoryListenerDetailIMP
    {

    }
}
