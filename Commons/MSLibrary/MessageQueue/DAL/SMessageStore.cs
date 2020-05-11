using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 消息数据操作
    /// 消息依据不同的队列的存储类型，存储在不同的数据点中
    /// </summary>
    [Injection(InterfaceType = typeof(ISMessageStore), Scope = InjectionScope.Singleton)]
    public class SMessageStore : ISMessageStore
    {
        private static Dictionary<int, IFactory<ISMessageStore>> _sMessageStoreFactories = new Dictionary<int, IFactory<ISMessageStore>>();
        
        public static Dictionary<int, IFactory<ISMessageStore>> SMessageStoreFactories
        {
            get
            {
                return _sMessageStoreFactories;
            }
        }


        public async Task Add(SQueue queue, SMessage message)
        {
            var store=GetRealSMessageStore(queue.StoreType);
            await store.Add(queue, message);
        }

        public async Task AddRetry(SQueue queue, Guid id,string exceptionMessage)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            await store.AddRetry(queue,id, exceptionMessage);
        }

        public async Task AddToDead(SQueue queue, SMessage message)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            await store.AddToDead(queue,message);
        }

        public async Task Delete(SQueue queue, Guid id)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            await store.Delete(queue,id);
        }

        public async Task QueryAllByQueue(SQueue queue, int pageSize, Func<List<SMessage>, Task<bool>> callBack)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            await store.QueryAllByQueue(queue,pageSize,callBack);
        }

        public async Task<SMessage> QueryByDelayID(SQueue queue, Guid delayMessageID)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            return await store.QueryByDelayID(queue, delayMessageID);
        }

        public async Task<SMessage> QueryByKeyAndBeforeExpectTime(SQueue queue, string key, DateTime expectTime)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            return await store.QueryByKeyAndBeforeExpectTime(queue,key,expectTime);
        }

        public async Task<SMessage> QueryByOriginalID(SQueue queue, Guid originalMessageID, Guid listenerID)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            return await store.QueryByOriginalID(queue, originalMessageID, listenerID);
        }

        public async Task<QueryResult<SMessage>> QueryByQueue(SQueue queue, int page, int pageSize)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            return await store.QueryByQueue(queue, page, pageSize);
        }

        public async Task UpdateLastExecuteTime(SQueue queue, Guid id)
        {
            var store = GetRealSMessageStore(queue.StoreType);
            await store.UpdateLastExecuteTime(queue,id);
        }

        private ISMessageStore GetRealSMessageStore(int storeType)
        {
            if (!_sMessageStoreFactories.TryGetValue(storeType,out IFactory<ISMessageStore> facroty))
            {
                throw new Exception($"not fount {storeType.ToString()} StoreType IFactory<ISMessageStore> in SMessageStore.SMessageStoreFactories");
            }

            return facroty.Create();
        }
    }
}
