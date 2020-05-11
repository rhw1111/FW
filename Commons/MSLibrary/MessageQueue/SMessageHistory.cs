using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.MessageQueue.DAL;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 消息历史记录
    /// 负责管理所有已经处理的消息以及监听对象
    /// </summary>
    public class SMessageHistory : EntityBase<ISMessageHistoryIMP>
    {
        private static IFactory<ISMessageHistoryIMP> _smessageHistoryIMPFactory;

        public static IFactory<ISMessageHistoryIMP> SMessageHistoryIMPFactory
        {
            set
            {
                _smessageHistoryIMPFactory = value;
            }
        }

        public override IFactory<ISMessageHistoryIMP> GetIMPFactory()
        {
            return _smessageHistoryIMPFactory;
        }

        /// <summary>
        /// 消息ID
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
        /// 消息关键字
        /// 将基于该关键字分布消息
        /// </summary>
        public string Key
        {
            get
            {
                return GetAttribute<string>("Key");
            }
            set
            {
                SetAttribute<string>("Key", value);
            }
        }

        /// <summary>
        /// 消息类型
        /// 处理器将根据不同的消息类型做不同处理
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }
        /// <summary>
        /// 初始消息ID
        /// </summary>
        public Guid? OriginalMessageID
        {
            get
            {
                return GetAttribute<Guid?>("OriginalMessageID");
            }
            set
            {
                SetAttribute<Guid?>("OriginalMessageID", value);
            }
        }

        /// <summary>
        /// 延迟消息所属的来源消息ID
        /// </summary>
        public Guid? DelayMessageID
        {
            get
            {
                return GetAttribute<Guid?>("DelayMessageID");
            }
            set
            {
                SetAttribute<Guid?>("DelayMessageID", value);
            }
        }

        

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Data
        {
            get
            {
                return GetAttribute<string>("Data");
            }
            set
            {
                SetAttribute<string>("Data", value);
            }
        }



        /// <summary>
        /// 状态
        /// 0：未完成，1：已完成
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


        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }

        /// <summary>
        /// 新增监听明细
        /// </summary>
        /// <returns></returns>
        public async Task AddListenerDetail(SMessageHistoryListenerDetail listenerDetail)
        {
            await _imp.AddListenerDetail(this, listenerDetail);
        }

        /// <summary>
        /// 获取监听明细
        /// </summary>
        /// <param name="listenerName"></param>
        /// <returns></returns>
        public async Task<SMessageHistoryListenerDetail> GetListenerDetail(string listenerName)
        {
            return await _imp.GetListenerDetail(this, listenerName);
        }

        /// <summary>
        /// 标识完成
        /// </summary>
        /// <returns></returns>
        public async Task Complete()
        {
            await _imp.Complete(this);
        }
    }

    public interface ISMessageHistoryIMP
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        Task Add(SMessageHistory history);
        /// <summary>
        /// 新增监听明细
        /// </summary>
        /// <param name="history"></param>
        /// <param name="listenerDetail"></param>
        /// <returns></returns>
        Task AddListenerDetail(SMessageHistory history,SMessageHistoryListenerDetail listenerDetail);
        /// <summary>
        /// 根据监听名称查询监听明细
        /// </summary>
        /// <param name="history"></param>
        /// <param name="listenerName"></param>
        /// <returns></returns>
        Task<SMessageHistoryListenerDetail> GetListenerDetail(SMessageHistory history,string listenerName);

        /// <summary>
        /// 标识完成
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        Task Complete(SMessageHistory history);
    }

    [Injection(InterfaceType = typeof(ISMessageHistoryIMP), Scope = InjectionScope.Transient)]
    public class SMessageHistoryIMP : ISMessageHistoryIMP
    {
        private ISMessageHistoryStore _sMessageHistoryStore;
        private ISMessageHistoryListenerDetailStore _sMessageHistoryListenerDetailStore;

        public SMessageHistoryIMP(ISMessageHistoryStore sMessageHistoryStore, ISMessageHistoryListenerDetailStore sMessageHistoryListenerDetailStore)
        {
            _sMessageHistoryStore = sMessageHistoryStore;
            _sMessageHistoryListenerDetailStore = sMessageHistoryListenerDetailStore;
        }
        public async Task Add(SMessageHistory history)
        {
            history.Status = 0;
            await _sMessageHistoryStore.Add(history);
        }

        public async Task AddListenerDetail(SMessageHistory history, SMessageHistoryListenerDetail listenerDetail)
        {
            await _sMessageHistoryListenerDetailStore.Add(listenerDetail);
        }

        public async Task Complete(SMessageHistory history)
        {
            await _sMessageHistoryStore.UpdateStatus(history.ID, 1);
        }

        public async Task<SMessageHistoryListenerDetail> GetListenerDetail(SMessageHistory history, string listenerName)
        {
            return await _sMessageHistoryListenerDetailStore.QueryByName(history.ID, listenerName);
        }
    }
}
