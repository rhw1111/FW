using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using Renci.SshNet;
using Renci.SshNet.Async;
using Renci.SshNet.Common;

namespace MSLibrary.CommandLine.SSH.SSHEndpointServices
{
    /// <summary>
    /// SSH终结点服务的默认实现
    /// 要求的配置格式为
    /// {
    ///     "Address":"服务器地址",
    ///     "Port":服务器端口,
    ///     "UserName":"用户名",
    ///     "Password":"密码"
    /// }
    /// 
    /// </summary>
    [Injection(InterfaceType = typeof(SSHEndpointServiceForDefault), Scope = InjectionScope.Singleton)]
    public class SSHEndpointServiceForDefault : ISSHEndpointService
    {
        public async Task DownloadFile(string configuration, Func<Stream, Task> action, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);
                using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {
                    client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    client.Connect();
                    await using (var stream = new MemoryStream())
                    {
                        await client.DownloadAsync(path, stream);
                        await action(stream);
                        stream.Close();
                    }
                    client.Disconnect();
                }
            });


        }

        public async Task<string> ExecuteCommand(string configuration, string command, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            string result=string.Empty;
            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);

                using (var sshClient = new SshClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {

                    sshClient.Connect();
                    var sshCommand = sshClient.CreateCommand(command);
                    sshCommand.CommandTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    result = await sshCommand.ExecuteAsync();

                    sshClient.Disconnect();
                }
            }
            );
            return result;
        }

        public async Task ExecuteCommand(string configuration, Func<ISSHEndpointCommandService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);

                using (var sshClient = new SshClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {
                    sshClient.Connect();

                    SSHEndpointCommandService service = new SSHEndpointCommandService(sshClient, timeoutSeconds);
                    await action(service);

                    sshClient.Disconnect();
                }
            }
            );


        }

        public async Task<string> ExecuteCommandBatch(string configuration, IList<Func<string?, Task<string>>> commondGenerators, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            string? result = null;

            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);

                using (var sshClient = new SshClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {

                    sshClient.Connect();

                    foreach (var item in commondGenerators)
                    {
                        var command = await item(result);
                        var sshCommond = sshClient.CreateCommand(command);
                        sshCommond.CommandTimeout = new TimeSpan(0, 0, timeoutSeconds);
                        result = await sshCommond.ExecuteAsync();
                    }

                    sshClient.Disconnect();
                }
            });

            return result??string.Empty;
        }

        public async Task<bool> ExistsFile(string configuration, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            bool result = true;
            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);
                using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {
                    client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    client.Connect();
                    result=client.Exists(path);
                    client.Disconnect();
                }
                await Task.FromResult(0);
            });

            return result;
        }

        public async Task UploadFile(string configuration, Stream stream, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);
                using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {
                    client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    client.Connect();
                    await client.UploadAsync(stream, path);

                    client.Disconnect();
                }
            });

        }

        public async Task UploadFile(string configuration, Func<ISSHEndpointUploadFileService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);
                using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {
                    client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    client.Connect();

                    SSHEndpointUploadFileServiceForDefault service = new SSHEndpointUploadFileServiceForDefault(client);
                    await action(service);

                    client.Disconnect();
                }
            }
            );

        }

        public async Task UploadFileBatch(string configuration, IList<(Stream, string)> uploadFileInfos, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);
                using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {
                    client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    client.Connect();
                    foreach (var item in uploadFileInfos)
                    {
                        await client.UploadAsync(item.Item1, item.Item2);
                    }
                    client.Disconnect();
                }
            });
        }

        public async Task<int> TransferFile(string configuration, Func<string, Task<string>> fileNameGenerateAction, string fromPath, string toPath, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            int fileCount = 0;
            await exceptionHandle(async () =>
            {
                var configurationObj = getConfiguration(configuration);
                using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
                {
                    client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    client.Connect();
                    var list = await client.ListDirectoryAsync(fromPath);
                    foreach (var item in list)
                    {
                        if (!item.IsDirectory && !item.IsSymbolicLink)
                        {
                            var newFileName = await fileNameGenerateAction(item.FullName);
                            string fileFullName = $"{toPath}{Path.DirectorySeparatorChar}{newFileName}";
                            item.MoveTo(fileFullName);
                            fileCount++;
                        }
                    }
                    client.Disconnect();
                }
            });
            return fileCount;
        }
        private async Task exceptionHandle(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch(SshOperationTimeoutException)
            {
                var fragment = new TextFragment()
                {
                    Code = CommandLineTextCodes.SSHOperationTimeout,
                    DefaultFormatting = "SSH执行操作超时",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)CommandLineErrorCodes.SSHOperationTimeout, fragment, 1, 0);
            }
        }

        private Configuration getConfiguration(string configuration)
        {
            var configurationObj=JsonSerializerHelper.Deserialize<Configuration>(configuration);
            return configurationObj;
        }

        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string Address { get; set; } = null!;
            [DataMember]
            public int Port { get; set; }
            [DataMember]
            public string UserName { get; set; } = null!;
            [DataMember]
            public string Password { get; set; } = null!;

        }
    }

    public class SSHEndpointUploadFileServiceForDefault : ISSHEndpointUploadFileService
    {
        private readonly SftpClient _sftpClient;

        public SSHEndpointUploadFileServiceForDefault(SftpClient sftpClient)
        {       
            _sftpClient = sftpClient;
        }
        public async Task Upload(Stream fileStream, string path)
        {
          
            await _sftpClient.UploadAsync(fileStream, path);
        }
    }

    public class SSHEndpointCommandService : ISSHEndpointCommandService
    {
        private readonly SshClient _sshClient;
        private int _timeoutSeconds;
        public SSHEndpointCommandService(SshClient sshClient,int timeoutSecond=-1)
        {
            _timeoutSeconds = timeoutSecond;
            _sshClient = sshClient;
        }
        public async Task<string> Do(string command)
        {
            var sshCommand = _sshClient.CreateCommand(command);
            sshCommand.CommandTimeout = new TimeSpan(0, 0, _timeoutSeconds);
            return await sshCommand.ExecuteAsync();
        }
    }
}
