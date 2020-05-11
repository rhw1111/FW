using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Transactions;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using MSLibrary.FileManagement.DAL;


namespace MSLibrary.FileManagement
{
    /// <summary>
    /// 上传文件处理记录
    /// </summary>
    public class UploadFileHandleRecord : EntityBase<IUploadFileHandleRecordIMP>
    {
        private static IFactory<IUploadFileHandleRecordIMP> _uploadFileHandleRecordIMPFactory;
        public static IFactory<IUploadFileHandleRecordIMP> UploadFileHandleRecordIMPFactory
        {
            set
            {
                _uploadFileHandleRecordIMPFactory = value;
            }
        }
        public override IFactory<IUploadFileHandleRecordIMP> GetIMPFactory()
        {
            return _uploadFileHandleRecordIMPFactory;
        }

        /// <summary>
        /// 记录Id
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
        /// 上传文件Id
        /// </summary>
        public Guid UploadFileId
        {
            get
            {

                return GetAttribute<Guid>("UploadFileId");
            }
            set
            {
                SetAttribute<Guid>("UploadFileId", value);
            }
        }


        /// <summary>
        /// 上传文件的RegardingType
        /// </summary>
        public string UploadFileRegardingType
        {
            get
            {

                return GetAttribute<string>("UploadFileRegardingType");
            }
            set
            {
                SetAttribute<string>("UploadFileRegardingType", value);
            }
        }

        /// <summary>
        /// 上传文件的RegardingKey
        /// </summary>
        public string UploadFileRegardingKey
        {
            get
            {

                return GetAttribute<string>("UploadFileRegardingKey");
            }
            set
            {
                SetAttribute<string>("UploadFileRegardingKey", value);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        public UploadFile UploadFile
        {
            get
            {
                if (!Attributes.ContainsKey("UploadFile"))
                {
                    var uploadFile = _imp.GetUploadFile(this);
                    SetAttribute<UploadFile>("UploadFile", uploadFile);
                }

                return GetAttribute<UploadFile>("UploadFile");
            }
            set
            {
                SetAttribute<UploadFile>("UploadFile", value);
            }
        }

        /// <summary>
        /// 处理配置名称
        /// </summary>
        public string ConfigurationName
        {
            get
            {

                return GetAttribute<string>("ConfigurationName");
            }
            set
            {
                SetAttribute<string>("ConfigurationName", value);
            }
        }

        /// <summary>
        /// 额外信息
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
        /// 结果信息
        /// </summary>
        public string ResultInfo
        {
            get
            {

                return GetAttribute<string>("ResultInfo");
            }
            set
            {
                SetAttribute<string>("ResultInfo", value);
            }
        }

        /// <summary>
        /// 出错信息
        /// </summary>
        public string Error
        {
            get
            {

                return GetAttribute<string>("Error");
            }
            set
            {
                SetAttribute<string>("Error", value);
            }
        }

        /// <summary>
        /// 状态
        /// 0，未处理
        /// 1，正在处理
        /// 2，已处理
        /// 3，出错
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
        public async Task UpdateToProcessStatus()
        {
            await _imp.UpdateToProcessStatus(this);
        }
        public async Task Execute()
        {
            await _imp.Execute(this);
        }

    }

    public interface IUploadFileHandleRecordIMP
    {
        Task Add(UploadFileHandleRecord record);
        Task Delete(UploadFileHandleRecord record);
        Task UpdateToProcessStatus(UploadFileHandleRecord record);
        Task Execute(UploadFileHandleRecord record);

        UploadFile GetUploadFile(UploadFileHandleRecord record);
    }

    [Injection(InterfaceType = typeof(IUploadFileHandleRecordIMP), Scope = InjectionScope.Transient)]
    public class UploadFileHandleRecordIMP : IUploadFileHandleRecordIMP
    {
        private static Dictionary<string, IFactory<IUploadFileHandleService>> _uploadFileHandleServiceFactories = new Dictionary<string, IFactory<IUploadFileHandleService>>();

        /// <summary>
        /// 处理服务工厂键值对
        /// 键为ConfigurationName-关联上传文件的Suffix
        /// </summary>
        public static Dictionary<string, IFactory<IUploadFileHandleService>> UploadFileHandleServiceFactories
        {
            get
            {
                return _uploadFileHandleServiceFactories;
            }
        }

        private IUploadFileHandleRecordStore _uploadFileHandleRecordStore;
        private IUploadFileStore _uploadFileStore;

        public UploadFileHandleRecordIMP(IUploadFileHandleRecordStore uploadFileHandleRecordStore, IUploadFileStore uploadFileStore)
        {
            _uploadFileHandleRecordStore = uploadFileHandleRecordStore;
            _uploadFileStore = uploadFileStore;
        }

        public async Task Add(UploadFileHandleRecord record)
        {
            await _uploadFileHandleRecordStore.Add(record);
        }

        public async Task Delete(UploadFileHandleRecord record)
        {
            await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                await _uploadFileHandleRecordStore.Delete(record.ID);
                if (record.UploadFile != null)
                {
                    await record.UploadFile.Delete();
                }

                scope.Complete();
            }
        }

        public async Task Execute(UploadFileHandleRecord record)
        {
            if (record.UploadFile != null)
            {
                if (!_uploadFileHandleServiceFactories.TryGetValue($"{record.ConfigurationName}-{record.UploadFile.Suffix}", out IFactory<IUploadFileHandleService> serviceFactory))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundUploadFileHandleServiceByKey,
                        DefaultFormatting = "找不到指定key的上传文件处理服务，key为{0}，发生位置：{1}",
                        ReplaceParameters = new List<object>() { $"{record.ConfigurationName}-{record.UploadFile.Suffix}", $"{this.GetType().FullName}.Execute" }
                    };

                    throw new UtilityException((int)Errors.NotFoundUploadFileHandleServiceByKey, fragment);
                }
                var service = serviceFactory.Create();

                await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    try
                    {
                        var result = await service.Execute(record, record.ExtensionInfo, record.UploadFile);
                        await _uploadFileHandleRecordStore.UpdateStatus(record.ID, 2, result, null);
                    }
                    catch (Exception ex)
                    {
                        await _uploadFileHandleRecordStore.UpdateStatus(record.ID, 3, null, $"{ex.Message},stacktrace:{ex.StackTrace}");
                    }
                    scope.Complete();
                }
            }

        }

        public async Task UpdateToProcessStatus(UploadFileHandleRecord record)
        {
            await _uploadFileHandleRecordStore.UpdateStatus(record.ID, 1, null, null);
        }

        public UploadFile GetUploadFile(UploadFileHandleRecord record)
        {
            return _uploadFileStore.QueryByIdSync(record.UploadFileRegardingType, record.UploadFileRegardingKey, record.UploadFileId);
        }
    }


    public interface IUploadFileHandleService
    {
        Task<string> Execute(UploadFileHandleRecord record, string extensionInfo, UploadFile file);
    }

}
