using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.CommonQueue.DAL;

namespace MSLibrary.CommonQueue
{
    public class CommonMessageClientType : EntityBase<ICommonMessageClientTypeIMP>
    {
        public override IFactory<ICommonMessageClientTypeIMP> GetIMPFactory()
        {
            throw new NotImplementedException();
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
        /// 消息名称
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
        /// 该类型执行处理的生产终结点
        /// </summary>
        public Guid EndpointID
        {
            get
            {
                return GetAttribute<Guid>("EndpointID");
            }
            set
            {
                SetAttribute<Guid>("EndpointID", value);
            }
        }

        /// <summary>
        /// 该类型执行处理的生产终结点
        /// </summary>
        public CommonQueueProductEndpoint Endpoint
        {
            get
            {
                return GetAttribute<CommonQueueProductEndpoint>("Endpoint");
            }
            set
            {
                SetAttribute<CommonQueueProductEndpoint>("Endpoint", value);
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

    public interface ICommonMessageClientTypeIMP
    {
        Task Add(CommonMessageClientType type);
        Task Update(CommonMessageClientType type);
        Task Delete(CommonMessageClientType type);
    }

    [Injection(InterfaceType = typeof(ICommonMessageClientTypeIMP), Scope = InjectionScope.Transient)]
    public class CommonMessageClientTypeIMP : ICommonMessageClientTypeIMP
    {
        private ICommonMessageClientTypeStore _commonMessageClientTypeStore;

        public CommonMessageClientTypeIMP(ICommonMessageClientTypeStore commonMessageClientTypeStore)
        {
            _commonMessageClientTypeStore = commonMessageClientTypeStore;
        }

        public async Task Add(CommonMessageClientType type)
        {
            await _commonMessageClientTypeStore.Add(type);
        }

        public async Task Delete(CommonMessageClientType type)
        {
            await _commonMessageClientTypeStore.Delete(type.ID);
        }

        public async Task Update(CommonMessageClientType type)
        {
            await _commonMessageClientTypeStore.Update(type);
        }
    }
}
