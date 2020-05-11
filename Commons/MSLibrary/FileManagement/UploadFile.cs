using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSLibrary.DI;
using MSLibrary.Entity.DAL;
using MSLibrary.LanguageTranslate;
using MSLibrary.FileManagement.DAL;

namespace MSLibrary.FileManagement
{
    /// <summary>
    /// 上传文件实体
    /// 存储需要系统管理的文件信息
    /// </summary>
    public class UploadFile : EntityBase<IUploadFileIMP>
    {
        private static IFactory<IUploadFileIMP> _uploadFileIMPFactory;
        public static IFactory<IUploadFileIMP> UploadFileIMPFactory
        {
            set
            {
                _uploadFileIMPFactory = value;
            }
        }
        public override IFactory<IUploadFileIMP> GetIMPFactory()
        {
            return _uploadFileIMPFactory;
        }

        /// <summary>
        /// 文件Id
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
        /// 文件显示名称
        /// </summary>
        public string DisplayName
        {
            get
            {

                return GetAttribute<string>("DisplayName");
            }
            set
            {
                SetAttribute<string>("DisplayName", value);
            }
        }

        /// <summary>
        /// 唯一文件名
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
        /// 文件类型
        /// </summary>
        public FileType? FileType
        {
            get
            {

                return GetAttribute<FileType?>("FileType");
            }
            set
            {
                SetAttribute<FileType?>("FileType", value);
            }
        }


        /// <summary>
        /// 文件的后缀名
        /// </summary>
        public string Suffix
        {
            get
            {

                return GetAttribute<string>("Suffix");
            }
            set
            {
                SetAttribute<string>("Suffix", value);
            }
        }

        /// <summary>
        /// 文件长度
        /// </summary>
        public long Size
        {
            get
            {

                return GetAttribute<long>("Size");
            }
            set
            {
                SetAttribute<long>("Size", value);
            }
        }

        /// <summary>
        /// 状态
        /// 0，草稿
        /// 1，已提交
        /// 2，已删除
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
        /// 文件关联的类型
        /// </summary>
        public string RegardingType
        {
            get
            {

                return GetAttribute<string>("RegardingType");
            }
            set
            {
                SetAttribute<string>("RegardingType", value);
            }
        }


        /// <summary>
        /// 文件关联的关键信息
        /// </summary>
        public string RegardingKey
        {
            get
            {

                return GetAttribute<string>("RegardingKey");
            }
            set
            {
                SetAttribute<string>("RegardingKey", value);
            }
        }


        /// <summary>
        /// 文件来源类型
        /// </summary>
        public string SourceType
        {
            get
            {

                return GetAttribute<string>("SourceType");
            }
            set
            {
                SetAttribute<string>("SourceType", value);
            }
        }

        /// <summary>
        /// 文件来源关键信息
        /// </summary>
        public string SourceKey
        {
            get
            {

                return GetAttribute<string>("SourceKey");
            }
            set
            {
                SetAttribute<string>("SourceKey", value);
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
        /// 新增上传文件
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }

        /// <summary>
        /// 提交上传文件
        /// </summary>
        /// <returns></returns>
        public async Task Submit()
        {
            await _imp.Submit(this);
        }

        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        /// <summary>
        /// 写入流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task Write(Stream stream)
        {
            await _imp.Write(this, stream);
        }

        /// <summary>
        /// 读取指定范围的流
        /// 如果范围超出文件长度，返回false
        /// </summary>
        /// <returns></returns>
        public async Task<ValidateResult<Stream>> Read(long start,long? end)
        {
            return await _imp.Read(this,start,end);
        }

        /// <summary>
        /// 读取文件流
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task Read(Func<Stream,Task> action)
        {
            await _imp.Read(this, action);
        }

        /// <summary>
        /// 获取上传文件对应的MimeType
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMimeType()
        {
            return await _imp.GetMimeType(this);
        }
        /// <summary>
        /// 修改上传文件可修改的属性
        /// </summary>
        /// <returns></returns>
        public async Task Update()
        {
            await _imp.Update(this);
        }

        /// <summary>
        /// 获取上传文件的文件类型
        /// </summary>
        /// <returns></returns>
        public async Task<FileType> GetFileType()
        {
            return await _imp.GetFileType(this);
        }
    }


    public interface IUploadFileIMP
    {
        /// <summary>
        /// 新增上传文件
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        Task Add(UploadFile uploadFile);
        /// <summary>
        /// 提交上传文件
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        Task Submit(UploadFile uploadFile);
        /// <summary>
        /// 删除上传文件 
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        Task Delete(UploadFile uploadFile);
        /// <summary>
        /// 写入流
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task Write(UploadFile uploadFile, Stream stream);
        /// <summary>
        /// 读取流
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<ValidateResult<Stream>> Read(UploadFile uploadFile, long start, long? end);

        /// <summary>
        /// 读取流
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task Read(UploadFile uploadFile, Func<Stream,Task> action);


        /// <summary>
        /// 获取上传文件对应的MimeType
        /// </summary>
        /// <returns></returns>
        Task<string> GetMimeType(UploadFile uploadFile);
        /// <summary>
        /// 获取上传文件的文件类型
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        Task<FileType> GetFileType(UploadFile uploadFile);
        /// <summary>
        /// 修改上传文件可修改的属性
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        Task Update(UploadFile uploadFile);
    }

    /// <summary>
    /// 针对源文件的上传、修改、删除操作将根据UploadFile的SourceType找到对应的IUploadFileSourceExecuteService来处理
    /// </summary>
    [Injection(InterfaceType = typeof(IUploadFileIMP), Scope = InjectionScope.Transient)]
    public class UploadFileIMP : IUploadFileIMP
    {

        private static Dictionary<string, IFactory<IUploadFileSourceExecuteService>> _uploadFileSourceExecuteServiceFactories = new Dictionary<string, IFactory<IUploadFileSourceExecuteService>>();

        public static Dictionary<string, IFactory<IUploadFileSourceExecuteService>> UploadFileSourceExecuteServiceFactories
        {
            get
            {
                return _uploadFileSourceExecuteServiceFactories;
            }
        }

        private IUploadFileStore _uploadFileStore;


        public UploadFileIMP(IUploadFileStore uploadFileStore)
        {
            _uploadFileStore = uploadFileStore;
        }

        public async Task Add(UploadFile uploadFile)
        {
            uploadFile.FileType = await uploadFile.GetFileType();
            uploadFile.Status = 0;
            uploadFile.CreateTime = DateTime.UtcNow;
            await _uploadFileStore.Add(uploadFile);
        }

        public async Task Delete(UploadFile uploadFile)
        {
            var sourceService = GetSourceService(uploadFile.SourceType);

            if (uploadFile.Status == 1)
            {
                //先修改状态为删除
                await _uploadFileStore.UpdateStatus(uploadFile, 2);
            }

            await sourceService.Delete(uploadFile);

            //删除上传文件记录
            await _uploadFileStore.Delete(uploadFile.RegardingType, uploadFile.RegardingKey, uploadFile.ID);
        }

        public async Task<FileType> GetFileType(UploadFile uploadFile)
        {
            if (uploadFile.FileType.HasValue)
            {
                return await Task.FromResult(uploadFile.FileType.Value);
            }
            return await Task.FromResult(FileSuffixFileTypeMapContainer.GetFileDefaultType(uploadFile.Suffix));
        }

        public async Task<string> GetMimeType(UploadFile uploadFile)
        {
            return await Task.FromResult(FileSuffixMimeMapContainer.GetMime(uploadFile.Suffix));
        }

        public async Task<ValidateResult<Stream>> Read(UploadFile uploadFile,long start,long? end)
        {
            var sourceService = GetSourceService(uploadFile.SourceType);
            return await sourceService.Read(uploadFile,start,end);
        }

        public async Task Read(UploadFile uploadFile, Func<Stream,Task> action)
        {
            var sourceService = GetSourceService(uploadFile.SourceType);
            await sourceService.Read(uploadFile,action);
        }


        public async Task Submit(UploadFile uploadFile)
        {
            await _uploadFileStore.UpdateStatus(uploadFile, 1);
        }

        public async Task Update(UploadFile uploadFile)
        {
            await _uploadFileStore.Update(uploadFile);
        }

        public async Task Write(UploadFile uploadFile, Stream stream)
        {

            //只有草稿状态才能写
            //if (uploadFile.Status == 0)
            //{
            var sourceService = GetSourceService(uploadFile.SourceType);
            await sourceService.Write(uploadFile, stream);
            //}
        }

        private IUploadFileSourceExecuteService GetSourceService(string sourceType)
        {
            if (!_uploadFileSourceExecuteServiceFactories.TryGetValue(sourceType, out IFactory<IUploadFileSourceExecuteService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundUploadFileSourceExecuteServiceByType,
                    DefaultFormatting = "找不到类型为{0}的上传文件源处理服务",
                    ReplaceParameters = new List<object>() { sourceType }
                };

                throw new UtilityException((int)Errors.NotFoundUploadFileSourceExecuteServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }

    /// <summary>
    /// 上传文件源处理服务
    /// 负责实际文件的读、写、删除操作
    /// </summary>
    public interface IUploadFileSourceExecuteService
    {
        /// <summary>
        /// 读取指定范围的流
        /// 如果范围超出实际文件长度，返回false
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<ValidateResult<Stream>> Read(UploadFile uploadFile,long start,long? end);
        /// <summary>
        /// 读取文件流
        /// 会多次调用action
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Read(UploadFile uploadFile, Func<Stream,Task> action);
        Task Write(UploadFile uploadFile, Stream stream);
        Task Delete(UploadFile uploadFile);
    }
}
