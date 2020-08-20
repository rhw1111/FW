using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Template;
using MSLibrary.CommandLine.SSH.DAL;

namespace MSLibrary.CommandLine.SSH
{
    public class SSHEndpoint : EntityBase<ISSHEndpointIMP>
    {
        private static IFactory<ISSHEndpointIMP>? _sshEndpointIMPFactory;

        public static IFactory<ISSHEndpointIMP> SSHEndpointIMPFactory
        {
            set
            {
                _sshEndpointIMPFactory = value;
            }
        }
        public override IFactory<ISSHEndpointIMP>? GetIMPFactory()
        {
            return _sshEndpointIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {

                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {

                return GetAttribute<string>(nameof(Type));
            }
            set
            {
                SetAttribute<string>(nameof(Type), value);
            }
        }

        /// <summary>
        /// 配置
        /// </summary>
        public string Configuration
        {
            get
            {

                return GetAttribute<string>(nameof(Configuration));
            }
            set
            {
                SetAttribute<string>(nameof(Configuration), value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(CreateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreateTime), value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> ExecuteCommand(string command, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            return await _imp.ExecuteCommand(this,command,timeoutSeconds, cancellationToken);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UploadFile(Stream stream, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await _imp.UploadFile(this,stream,path,timeoutSeconds,cancellationToken);
        }

        /// <summary>
        /// 批量执行命令
        /// </summary>
        /// <param name="commondGenerators"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> ExecuteCommandBatch( IList<Func<string?, Task<string>>> commondGenerators, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            return await _imp.ExecuteCommandBatch(this, commondGenerators,timeoutSeconds, cancellationToken);
        }
        /// <summary>
        /// 批量上传文件
        /// </summary>
        /// <param name="uploadFileInfos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UploadFileBatch(IList<(Stream, string)> uploadFileInfos, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await _imp.UploadFileBatch(this, uploadFileInfos, timeoutSeconds, cancellationToken);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DownloadFile(Func<Stream, Task> action, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await _imp.DownloadFile(this,action, path,timeoutSeconds, cancellationToken);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UploadFile(Func<ISSHEndpointUploadFileService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await _imp.UploadFile(this, action,timeoutSeconds, cancellationToken);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteCommand(Func<ISSHEndpointCommandService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await _imp.ExecuteCommand(this, action, timeoutSeconds, cancellationToken);
        }

        public async Task TransferFile(Func<string, Task<string>> fileNameGenerateAction, string fromPath, string toPath, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await _imp.TransferFile(this, fileNameGenerateAction, fromPath,toPath, timeoutSeconds, cancellationToken);
        }

        public async Task<bool> ExistsFile(string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            return await _imp.ExistsFile(this, path, timeoutSeconds, cancellationToken);
        }
        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }
        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }
        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }
    }

    public interface ISSHEndpointIMP
    {
        Task<string> ExecuteCommand(SSHEndpoint endpoint,string command, int timeoutSeconds = -1, CancellationToken cancellationToken = default);
        Task UploadFile(SSHEndpoint endpoint,Stream stream,string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task<string> ExecuteCommandBatch(SSHEndpoint endpoint, IList<Func<string?, Task<string>>> commondGenerators, int timeoutSeconds = -1, CancellationToken cancellationToken = default);
        Task UploadFileBatch(SSHEndpoint endpoint, IList<(Stream,string)> uploadFileInfos, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task DownloadFile(SSHEndpoint endpoint, Func<Stream, Task> action, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task UploadFile(SSHEndpoint endpoint, Func<ISSHEndpointUploadFileService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task ExecuteCommand(SSHEndpoint endpoint, Func<ISSHEndpointCommandService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task TransferFile(SSHEndpoint endpoint, Func<string, Task<string>> fileNameGenerateAction, string fromPath, string toPath, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task Add(SSHEndpoint sshEndPoint, CancellationToken cancellationToken = default);
        Task Delete(SSHEndpoint sshEndPoint, CancellationToken cancellationToken = default);
        Task Update(SSHEndpoint sshEndPoint, CancellationToken cancellationToken = default);
        Task<bool> ExistsFile(SSHEndpoint sshEndPoint, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default);
    }

    public interface ISSHEndpointService
    {
        Task<string> ExecuteCommand(string configuration, string command,int timeoutSeconds=-1, CancellationToken cancellationToken = default);
        Task UploadFile(string configuration, Stream stream, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task DownloadFile(string configuration,Func<Stream,Task> action, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task<string> ExecuteCommandBatch(string configuration, IList<Func<string?, Task<string>>> commondGenerators, int timeoutSeconds = -1, CancellationToken cancellationToken = default);
        Task UploadFileBatch(string configuration, IList<(Stream, string)> uploadFileInfos, int timeoutSeconds = -1, CancellationToken cancellationToken = default);

        Task UploadFile(string configuration, Func<ISSHEndpointUploadFileService,Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default);
        Task ExecuteCommand(string configuration, Func<ISSHEndpointCommandService,Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default);
        Task TransferFile(string configuration, Func<string,Task<string>> fileNameGenerateAction, string fromPath, string toPath, int timeoutSeconds = -1, CancellationToken cancellationToken = default);
        Task<bool> ExistsFile(string configuration, string path,int timeoutSeconds = -1, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(ISSHEndpointIMP), Scope = InjectionScope.Transient)]
    public class SSHEndpointIMP : ISSHEndpointIMP
    {
        private readonly ISSHEndpointStore _sshEndpointStore;
        public SSHEndpointIMP(ISSHEndpointStore sshEndpointStore)
        {
            _sshEndpointStore = sshEndpointStore;
        }
        /// <summary>
        /// 文本替换服务
        /// 如果该属性赋值，则configuration中的内容将首先使用该服务来替换占位符
        /// </summary>
        public static ITextReplaceService? TextReplaceService { set; get; }

        public static IDictionary<string, IFactory<ISSHEndpointService>> SSHEndpointServiceFactories { get; } = new Dictionary<string, IFactory<ISSHEndpointService>>();

        public async Task DownloadFile(SSHEndpoint endpoint, Func<Stream, Task> action, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(endpoint.Type);
            await service.DownloadFile(await getContent(endpoint.Configuration),action,path,timeoutSeconds, cancellationToken);
        }

        public async Task<string> ExecuteCommand(SSHEndpoint endpoint, string command, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(endpoint.Type);
            return await service.ExecuteCommand(await getContent(endpoint.Configuration), command,timeoutSeconds, cancellationToken);
        }

        public async Task<string> ExecuteCommandBatch(SSHEndpoint endpoint, IList<Func<string?, Task<string>>> commondGenerators, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(endpoint.Type);
            return await service.ExecuteCommandBatch(await getContent(endpoint.Configuration), commondGenerators,timeoutSeconds, cancellationToken);
        }

        public async Task UploadFile(SSHEndpoint endpoint, Stream stream, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(endpoint.Type);
            await service.UploadFile(await getContent(endpoint.Configuration), stream,path,timeoutSeconds, cancellationToken);
        }

        public async Task UploadFileBatch(SSHEndpoint endpoint, IList<(Stream, string)> uploadFileInfos, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(endpoint.Type);
            await service.UploadFileBatch(await getContent(endpoint.Configuration), uploadFileInfos,timeoutSeconds, cancellationToken);
        }

        public async Task UploadFile(SSHEndpoint endpoint, Func<ISSHEndpointUploadFileService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(endpoint.Type);
            await service.UploadFile(await getContent(endpoint.Configuration), action,timeoutSeconds, cancellationToken);
        }

        private ISSHEndpointService getService(string type)
        {
            if (!SSHEndpointServiceFactories.TryGetValue(type,out IFactory<ISSHEndpointService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = CommandLineTextCodes.NotFoundISSHEndpointServiceByType,
                    DefaultFormatting = "找不到类型为{0}的SSH终结点服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.SSHEndpointServiceFactories" }
                };

                throw new UtilityException((int)CommandLineErrorCodes.NotFoundISSHEndpointServiceByType, fragment, 1, 0);
            }

            return serviceFactory.Create();
        }

        public async Task ExecuteCommand(SSHEndpoint endpoint, Func<ISSHEndpointCommandService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(endpoint.Type);
            await service.ExecuteCommand(await getContent(endpoint.Configuration), action,timeoutSeconds, cancellationToken);
        }

        public async Task TransferFile(SSHEndpoint endpoint, Func<string, Task<string>> fileNameGenerateAction, string fromPath, string toPath, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(endpoint.Type);
            await service.TransferFile(await getContent(endpoint.Configuration), fileNameGenerateAction, fromPath, toPath, timeoutSeconds, cancellationToken);
        }
        private async Task<string> getContent(string content)
        {
            if (TextReplaceService != null)
            {
                return await TextReplaceService.Replace(content);
            }
            else
            {
                return content;
            }
        }

        public async Task Add(SSHEndpoint sshEndPoint, CancellationToken cancellationToken = default)
        {
            await _sshEndpointStore.Add(sshEndPoint, cancellationToken);
        }
        public async Task Delete(SSHEndpoint sshEndPoint, CancellationToken cancellationToken = default)
        {
            await _sshEndpointStore.Delete(sshEndPoint.ID, cancellationToken);
        }
        public async Task Update(SSHEndpoint sshEndPoint, CancellationToken cancellationToken = default)
        {
            await _sshEndpointStore.Update(sshEndPoint, cancellationToken);
        }

        public async Task<bool> ExistsFile(SSHEndpoint sshEndPoint, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var service = getService(sshEndPoint.Type);
            return await service.ExistsFile(await getContent(sshEndPoint.Configuration), path, timeoutSeconds, cancellationToken);
        }
    }

    public interface ISSHEndpointUploadFileService
    {
        Task Upload(Stream fileStream,string path);
    }

    public interface ISSHEndpointCommandService
    {
        Task<string> Do(string command);
    }
}
