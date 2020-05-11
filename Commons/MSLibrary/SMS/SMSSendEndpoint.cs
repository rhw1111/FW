using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.SMS.DAL;

namespace MSLibrary.SMS
{
    /// <summary>
    /// 短信发送终结点
    /// 负责短信的真正发送
    /// </summary>
    public class SMSSendEndpoint : EntityBase<ISMSSendEndpointIMP>
    {
        private static IFactory<ISMSSendEndpointIMP> _smsSendEndpointIMPFactory;

        public static IFactory<ISMSSendEndpointIMP> SMSSendEndpointIMPFactory
        {
            set
            {
                _smsSendEndpointIMPFactory = value;
            }
        }

        public override IFactory<ISMSSendEndpointIMP> GetIMPFactory()
        {
            return _smsSendEndpointIMPFactory;
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
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }

        /// <summary>
        /// 终结点配置信息
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
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

    public interface ISMSSendEndpointIMP
    {
        Task Add(SMSSendEndpoint endpoint);
        Task Update(SMSSendEndpoint endpoint);
        Task Delete(SMSSendEndpoint endpoint);
        Task Send(SMSRecord record);
    }

    [Injection(InterfaceType = typeof(ISMSSendEndpointIMP), Scope = InjectionScope.Transient)]
    public class SMSSendEndpointIMP : ISMSSendEndpointIMP
    {
        private static Dictionary<string, IFactory<ISMSSendService>> _smsSendServiceFactories;

        private ISMSSendEndpointStore _smsSendEndpointStore;

        public SMSSendEndpointIMP(ISMSSendEndpointStore smsSendEndpointStore)
        {
            _smsSendEndpointStore = smsSendEndpointStore;
        }


        public static Dictionary<string, IFactory<ISMSSendService>> SMSSendServiceFactories
        {
            set
            {
                _smsSendServiceFactories = value;
            }
        }
        public async Task Add(SMSSendEndpoint endpoint)
        {
            await _smsSendEndpointStore.Add(endpoint);
        }

        public async Task Delete(SMSSendEndpoint endpoint)
        {
            await _smsSendEndpointStore.Delete(endpoint);
        }

        public async Task Send(SMSRecord record)
        {
            await Task.FromResult(0);
            throw new NotImplementedException();
        }

        public async Task Update(SMSSendEndpoint endpoint)
        {
            await _smsSendEndpointStore.Update(endpoint);
        }

        private async Task<ISMSSendService> GetService()
        {
            await Task.FromResult(0);
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 短信发送服务
    /// </summary>
    public interface ISMSSendService
    {
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        Task Send(string configuration, SMSRecord record);
    }
}
