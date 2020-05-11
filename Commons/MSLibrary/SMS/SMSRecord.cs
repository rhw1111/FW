using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.SMS.DAL;

namespace MSLibrary.SMS
{
    /// <summary>
    /// 短信记录
    /// 负责关联发送的短信信息
    /// </summary>
    public class SMSRecord : EntityBase<ISMSRecordIMP>
    {
        private static IFactory<ISMSRecordIMP> _smsRecordIMPFactory;

        public static IFactory<ISMSRecordIMP> SMSRecordIMPFactory
        {
            set
            {
                _smsRecordIMPFactory = value;
            }
        }

        public override IFactory<ISMSRecordIMP> GetIMPFactory()
        {
            return _smsRecordIMPFactory;
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
        /// 发送的号码列表
        /// 使用,间隔
        /// </summary>
        public string PhoneNumbers
        {
            get
            {
                return GetAttribute<string>("PhoneNumbers");
            }
            set
            {
                SetAttribute<string>("PhoneNumbers", value);
            }
        }

        /// <summary>
        /// 发送的内容
        /// </summary>
        public string Content
        {
            get
            {
                return GetAttribute<string>("Content");
            }
            set
            {
                SetAttribute<string>("Content", value);
            }
        }

        /// <summary>
        /// 发送方终结点名称
        /// </summary>
        public string SendEndpointName
        {
            get
            {
                return GetAttribute<string>("SendEndpointName");
            }
            set
            {
                SetAttribute<string>("SendEndpointName", value);
            }
        }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string ExtensionInfo
        {
            get
            {
                return GetAttribute<string>("ExtensionInfo");
            }
            set
            {
                SetAttribute<string>("ExtensionInfo", value);
            }
        }

        /// <summary>
        /// 状态
        /// 0：未发送，1：发送成功，2：发送失败
        /// </summary>
        public int Status
        {
            get
            {
                return GetAttribute<int>("Status");
            }
            set
            {
                SetAttribute<int>("Status", value);
            }
        }

        /// <summary>
        /// 针对状态的描述
        /// 例如发送失败的原因
        /// </summary>
        public string StatusDescription
        {
            get
            {
                return GetAttribute<string>("StatusDescription");
            }
            set
            {
                SetAttribute<string>("StatusDescription", value);
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


    public interface ISMSRecordIMP
    {
        Task Add(SMSRecord record);
        Task Send(SMSRecord record);
    }

    [Injection(InterfaceType = typeof(ISMSRecordIMP), Scope = InjectionScope.Transient)]
    public class SMSRecordIMP : ISMSRecordIMP
    {
        private ISMSRecordStore _smsRecordStore;
        public SMSRecordIMP(ISMSRecordStore smsRecordStore)
        {
            _smsRecordStore = smsRecordStore;
        }

        public async Task Add(SMSRecord record)
        {
            await _smsRecordStore.Add(record);
        }

        public Task Send(SMSRecord record)
        {
            throw new NotImplementedException();
        }
    }
}
