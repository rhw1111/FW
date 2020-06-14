using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MSLibrary.Thread;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction.DAL;

namespace MSLibrary.Transaction
{
    /// <summary>
    /// 分布式操作的操作数据记录
    /// </summary>
    public class DTOperationData : EntityBase<IDTOperationDataIMP>
    {
        public override IFactory<IDTOperationDataIMP> GetIMPFactory()
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
        /// 所属操作记录的唯一名称
        /// </summary>
        public string RecordUniqueName
        {
            get
            {
                return GetAttribute<string>("RecordUniqueName");
            }
            set
            {
                SetAttribute<string>("RecordUniqueName", value);
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
        /// 处理数据
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

        public byte[] Version
        {
            get
            {
                return GetAttribute<byte[]>("Version");
            }
            set
            {
                SetAttribute<byte[]>("Version", value);
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

        public async Task Add()
        {
            await _imp.Add(this);
        }
        public async Task Delete()
        {
            await _imp.Delete(this);
        }


        public async Task Cancel()
        {
            await _imp.Cancel(this);
        }
    }

    public enum DTOperationDataStatus
    {
        UnCancel = 0,
        Cancel = 1
    }

    public interface IDTOperationDataIMP
    {
        Task Add(DTOperationData data);
        Task Delete(DTOperationData data);
        Task Cancel(DTOperationData data);
    }

    public interface IDTOperationDataService
    {
        Task Cancel(string data);
    }

    [Injection(InterfaceType = typeof(IDTOperationDataIMP), Scope = InjectionScope.Transient)]
    public class DTOperationDataIMP : IDTOperationDataIMP
    {
        public static IDictionary<string, IFactory<IDTOperationDataService>> DTOperationDataServiceFactories { get; } = new Dictionary<string, IFactory<IDTOperationDataService>>();

        private IApplicationLockService _applicationLockService;
        private IDTOperationDataStore _dtOperationDataStore;

        public DTOperationDataIMP(IApplicationLockService applicationLockService, IDTOperationDataStore dtOperationDataStore)
        {
            _applicationLockService = applicationLockService;
            _dtOperationDataStore = dtOperationDataStore;
        }

        public async Task Add(DTOperationData data)
        {
            await _dtOperationDataStore.Add(data);
        }

       

        public async Task Cancel(DTOperationData data)
        {
            if (data.Status==(int)DTOperationDataStatus.Cancel)
            {
                return;
            }
            var service = getService(data.Type);

            await using (DBTransactionScope transactionScope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 30, 0) }))
            {
               
                await service.Cancel(data.Data);

                var updateResult = await _dtOperationDataStore.UpdateStatus(data.StoreGroupName, data.HashInfo, data.ID, data.Version, (int)DTOperationDataStatus.Cancel);

                if (!updateResult)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.DTOperationDataConcurrenceErrorInCancel,
                        DefaultFormatting = "分布式操作数据在Cancel时发生并发错误，StoreGroupName:{0},HashInfo:{1}",
                        ReplaceParameters = new List<object>() { data.StoreGroupName,data.HashInfo,data.ID.ToString() }
                    };

                    throw new UtilityException((int)Errors.DTOperationDataConcurrenceErrorInCancel, fragment);
                }

                transactionScope.Complete();
            }
        }

        public async Task Delete(DTOperationData data)
        {
            await _dtOperationDataStore.Delete(data.StoreGroupName,data.HashInfo,data.ID);
        }

        private IDTOperationDataService getService(string type)
        {
            if (!DTOperationDataServiceFactories.TryGetValue(type,out IFactory<IDTOperationDataService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundDTOperationDataServiceByType,
                    DefaultFormatting = "找不到类型为{0}的分布式操作数据服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.DTOperationDataServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundDTOperationDataServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }


}
