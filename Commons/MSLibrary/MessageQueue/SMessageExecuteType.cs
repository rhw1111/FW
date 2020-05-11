using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.MessageQueue.DAL;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 消息执行类型实体
    /// 负责存储消息类型、管理类型监听记录
    /// </summary>
    public class SMessageExecuteType : EntityBase<ISMessageExecuteTypeIMP>
    {
        private static IFactory<ISMessageExecuteTypeIMP> _sMessageExecuteTypeIMPFactory;

        public static IFactory<ISMessageExecuteTypeIMP> SMessageExecuteTypeIMPFactory
        {
            set
            {
                _sMessageExecuteTypeIMPFactory = value;
            }
        }
        public override IFactory<ISMessageExecuteTypeIMP> GetIMPFactory()
        {
            return _sMessageExecuteTypeIMPFactory;
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
        /// 类型名称
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
        /// 修改
        /// </summary>
        /// <returns></returns>
        public async Task Update()
        {
            await _imp.Update(this);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        /// <summary>
        /// 为类型添加监听
        /// </summary>
        /// <param name="listenerId">监听者Id</param>
        /// <returns></returns>
        public async Task AddListener(SMessageTypeListener listener)
        {
            await _imp.AddListener(this, listener);
        }
        /// <summary>
        /// 为类型取消监听
        /// </summary>
        /// <param name="listenerId">监听者id</param>
        /// <returns></returns>
        public async Task DeleteListener(Guid listenerId)
        {
            await _imp.DeleteListener(this, listenerId);
        }

        /// <summary>
        /// 为类型修改监听
        /// </summary>
        /// <param name="listenerId">监听者id</param>
        /// <returns></returns>
        public async Task UpdateListener(SMessageTypeListener listener)
        {
            await _imp.UpdateListener(this, listener);
        }


        /// <summary>
        /// 获取所有的关联监听
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetAllListener(Func<SMessageTypeListener,Task> callback)
        {
            await _imp.GetAllListener(this,callback);
        }
        /// <summary>
        /// 分页获取关联的监听
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<SMessageTypeListener>> GetListener(int page,int pageSize)
        {
            return await _imp.GetListener(this,page,pageSize);
        }

        public async Task<SMessageTypeListener> GetListener(Guid listenerId)
        {
            return await _imp.GetListener(this, listenerId);
        }
    }

    public interface ISMessageExecuteTypeIMP
    {
        Task Add(SMessageExecuteType messageType);
        Task Update(SMessageExecuteType messageType);
        Task Delete(SMessageExecuteType messageType);
        Task AddListener(SMessageExecuteType messageType, SMessageTypeListener listener);
        Task DeleteListener(SMessageExecuteType messageType, Guid listenerId);
        Task UpdateListener(SMessageExecuteType messageType, SMessageTypeListener listener);
        Task GetAllListener(SMessageExecuteType messageType,Func<SMessageTypeListener, Task> callback);
        Task<QueryResult<SMessageTypeListener>> GetListener(SMessageExecuteType messageType,int page, int pageSize);
        Task<SMessageTypeListener> GetListener(SMessageExecuteType messageType, Guid listenerId);

    }

    [Injection(InterfaceType = typeof(ISMessageExecuteTypeIMP), Scope = InjectionScope.Transient)]
    public class SMessageExecuteTypeIMP : ISMessageExecuteTypeIMP
    {
        private ISMessageExecuteTypeStore _sMessageExecuteTypeStore;
        private ISMessageTypeListenerStore _sMessageTypeListenerStore;

        public SMessageExecuteTypeIMP(ISMessageExecuteTypeStore sMessageExecuteTypeStore, ISMessageTypeListenerStore sMessageTypeListenerStore)
        {
            _sMessageExecuteTypeStore = sMessageExecuteTypeStore;
            _sMessageTypeListenerStore = sMessageTypeListenerStore;
        }
        public async Task Add(SMessageExecuteType messageType)
        {
            await _sMessageExecuteTypeStore.Add(messageType);
        }

        public async Task AddListener(SMessageExecuteType messageType, SMessageTypeListener listener)
        {
            listener.MessageType = messageType;
            await _sMessageTypeListenerStore.Add(listener);
        }

        public async Task DeleteListener(SMessageExecuteType messageType, Guid listenerId)
        {
            await _sMessageTypeListenerStore.DeleteByTypeRelation(messageType.ID, listenerId);
        }


        public async Task Delete(SMessageExecuteType messageType)
        {
            await _sMessageExecuteTypeStore.Delete(messageType.ID);
        }



        public async Task GetAllListener(SMessageExecuteType messageType,Func<SMessageTypeListener, Task> callback)
        {
            await _sMessageTypeListenerStore.QueryByType(messageType.ID, callback);
        }

        public async Task<QueryResult<SMessageTypeListener>> GetListener(SMessageExecuteType messageType, int page, int pageSize)
        {
            return await _sMessageTypeListenerStore.QueryByType(messageType.ID, page, pageSize);
        }


        public async Task Update(SMessageExecuteType messageType)
        {
            await _sMessageExecuteTypeStore.Update(messageType);
        }

        public async Task UpdateListener(SMessageExecuteType messageType, SMessageTypeListener listener)
        {
            listener.MessageType = messageType;
            await _sMessageTypeListenerStore.UpdateByTypeRelation(listener);
        }

        public async Task<SMessageTypeListener> GetListener(SMessageExecuteType messageType, Guid listenerId)
        {
            return await _sMessageTypeListenerStore.QueryByTypeRelation(messageType.ID, listenerId);
        }
    }
}
