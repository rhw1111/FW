using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Transaction.DAL;
using MSLibrary.LanguageTranslate;
using MSLibrary.Storge;
using MSLibrary.DAL;

namespace MSLibrary.Transaction
{
    /// <summary>
    /// 分布式操作记录
    /// 用来管理分布式事务的处理
    /// </summary>
    public class DTOperationRecord : EntityBase<IDTOperationRecordIMP>
    {
        private static IFactory<IDTOperationRecordIMP> _dtOperationRecordIMPFactory;

        public static IFactory<IDTOperationRecordIMP> DTOperationRecordIMPFactory
        {
            set
            {
                _dtOperationRecordIMPFactory = value;
            }
        }
        public override IFactory<IDTOperationRecordIMP> GetIMPFactory()
        {
            return _dtOperationRecordIMPFactory;
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
        /// 唯一名称
        /// </summary>
        public string UniqueName
        {
            get
            {
                return GetAttribute<string>("UniqueName");
            }
            set
            {
                SetAttribute<string>("UniqueName", value);
            }
        }

        /// <summary>
        /// 存储组名称
        /// 用来决定记录的存储位置
        /// 该名称对应 MSLibrary.Storge.StoreGroup的名称
        /// </summary>
        public string StoreGroupName
        {
            get
            {
                return GetAttribute<string>("StoreGroupName");
            }
            set
            {
                SetAttribute<string>("StoreGroupName", value);
            }
        }

        /// <summary>
        /// 散列值
        /// 用来控制最终存储位置的选择
        /// </summary>
        public string HashInfo
        {
            get
            {
                return GetAttribute<string>("HashInfo");
            }
            set
            {
                SetAttribute<string>("HashInfo", value);
            }
        }

        /// <summary>
        /// 处理类型
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
        /// 处理类型相关的信息
        /// 该信息将传递给DTOperationRecordService处理
        /// </summary>
        public string TypeInfo
        {
            get
            {
                return GetAttribute<string>("TypeInfo");
            }
            set
            {
                SetAttribute<string>("TypeInfo", value);
            }
        }


        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get
            {
                return GetAttribute<string>("Version");
            }
            set
            {
                SetAttribute<string>("Version", value);
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
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



        /// <summary>
        /// 状态
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
        /// 执行超时时间（单位秒）
        /// </summary>
        public int Timeout
        {
            get
            {
                return GetAttribute<int>("Timeout");
            }
            set
            {
                SetAttribute<int>("Timeout", value);
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

        public async Task<bool> NeedCancel()
        {
            return await _imp.NeedCancel(this);
        }

        public async Task Cancel()
        {
            await _imp.Cancel(this);
        }

        public async Task Execute(Func<Task> action)
        {
            await _imp.Execute(this, action);
        }

        public async Task Add()
        {
            await _imp.Add(this);
        }
        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        public async Task UpdateErrorMessage(string errorMessage)
        {
            await _imp.UpdateErrorMessage(this,errorMessage);
        }
    }

    public enum DTOperationRecordStatus
    {
        UnComplete=0,
        Complete=1
    }
    public interface IDTOperationRecordIMP
    {
        Task<bool> NeedCancel(DTOperationRecord record);

        Task Cancel(DTOperationRecord record);

        Task Execute(DTOperationRecord record, Func<Task> action);

        Task Add(DTOperationRecord record);
        Task Delete(DTOperationRecord record);

        Task UpdateErrorMessage(DTOperationRecord record,string errorMessage);
    }


    [Injection(InterfaceType = typeof(IDTOperationRecordIMP), Scope = InjectionScope.Transient)]
    public class DTOperationRecordIMP : IDTOperationRecordIMP
    {
        private const string _entityName = "DTOperationRecord";

        public static IDictionary<string, IFactory<IDTOperationRecordService>> DTOperationRecordServiceFactories = new Dictionary<string, IFactory<IDTOperationRecordService>>();

        private IDTOperationRecordProcessDataStore _dtOperationRecordProcessDataStore;
        private IDTOperationRecordStore _dtOperationRecordStore;
        private IStoreGroupRepositoryCacheProxy _storeGroupRepositoryCacheProxy;

        public DTOperationRecordIMP(IDTOperationRecordProcessDataStore dtOperationRecordProcessDataStore, IDTOperationRecordStore dtOperationRecordStore, IStoreGroupRepositoryCacheProxy storeGroupRepositoryCacheProxy)
        {
            _dtOperationRecordProcessDataStore = dtOperationRecordProcessDataStore;
            _dtOperationRecordStore = dtOperationRecordStore;
            _storeGroupRepositoryCacheProxy = storeGroupRepositoryCacheProxy;
        }


        public async Task Add(DTOperationRecord record)
        {
            await _dtOperationRecordStore.Add(record);
        }

        public async Task Cancel(DTOperationRecord record)
        {
            var service = getService(record.Type);
            await service.Cancel(record.TypeInfo, record.UniqueName);

            await _dtOperationRecordStore.UpdateStatus(record.StoreGroupName, record.HashInfo, record.ID, (int)DTOperationRecordStatus.Complete);
        }

        public async Task Delete(DTOperationRecord record)
        {
            await _dtOperationRecordStore.Delete(record.StoreGroupName,record.HashInfo, record.ID);
        }

        public async Task Execute(DTOperationRecord record, Func<Task> action)
        {
            try
            {
                await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 10) }))
                {
                    await _dtOperationRecordStore.UpdateVersion(record.StoreGroupName,record.HashInfo,record.ID, Guid.NewGuid().ToString());

                    await action();

                    await _dtOperationRecordStore.UpdateStatus(record.StoreGroupName, record.HashInfo, record.ID, (int)DTOperationRecordStatus.Complete);
                    scope.Complete();
                }
            }
            catch
            {
                var nRecord=await _dtOperationRecordStore.QueryByID(record.StoreGroupName, record.HashInfo, record.ID);
                if (nRecord!=null)
                {
                    await nRecord.Cancel();
                }
                throw;
            }
        }

        public async Task<bool> NeedCancel(DTOperationRecord record)
        {
            //先判断是否完成
            if (record.Status== (int)DTOperationRecordStatus.Complete)
            {
                return await Task.FromResult(false);
            }
            //再判断是否在处理中
            var latestRecord = await _dtOperationRecordStore.QueryByIDNoLock(record.StoreGroupName, record.HashInfo, record.ID);
            if (record.Version!= latestRecord.Version)
            {
                return await Task.FromResult(false);
            }
            //再判断是否超时
            if((DateTime.UtcNow-record.ModifyTime).TotalSeconds<record.Timeout)
            {
                return await Task.FromResult(false);
            }


            return await Task.FromResult(true);
        }

        public async Task UpdateErrorMessage(DTOperationRecord record, string errorMessage)
        {
            await _dtOperationRecordStore.UpdateErroeMessage(record.StoreGroupName, record.HashInfo, record.ID, errorMessage);
        }

        private IDTOperationRecordService getService(string type)
        {
            if (!DTOperationRecordServiceFactories.TryGetValue(type,out IFactory<IDTOperationRecordService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundDTOperationRecordServiceByType,
                    DefaultFormatting = "找不到类型为{0}的分布式操作记录服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.DTOperationRecordServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundDTOperationRecordServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }
}
