using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.Collections;
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
    [Injection(InterfaceType = typeof(SSHEndpointServiceForOnce), Scope = InjectionScope.Singleton)]
    public class SSHEndpointServiceForOnce : ISSHEndpointService
    {
        public static string UploadServicePath { get; set; } = "";
        public static string CommandServicePath { get; set; } = "";
        public static int ServicePort { get; set; } = 5002;

        public async Task DownloadFile(string configuration, Func<Stream, Task> action, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                await sftpClientExecute(configuration, async (client) =>
                {
                    if (timeoutSeconds != -1)
                    {
                        client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    }

                    await using (var stream = new MemoryStream())
                    {
                        await client.DownloadAsync(path, stream);
                        await action(stream);
                        stream.Close();
                    }
                });
            });


        }

        public async Task<string> ExecuteCommand(string configuration, string command, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            string result = string.Empty;
            var configurationObj = getConfiguration(configuration);

            SSHEndpointCommandServiceForOnce service = new SSHEndpointCommandServiceForOnce(getCommandServiceUrl(configurationObj.Address));
            return await service.Do(command);
        }

        public async Task ExecuteCommand(string configuration, Func<ISSHEndpointCommandService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            string result = string.Empty;
            var configurationObj = getConfiguration(configuration);

            SSHEndpointCommandServiceForOnce service = new SSHEndpointCommandServiceForOnce(getCommandServiceUrl(configurationObj.Address));
            await action(service);

        }

        public async Task<string> ExecuteCommandBatch(string configuration, IList<Func<string?, Task<string>>> commondGenerators, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            string? result = null;
            var configurationObj = getConfiguration(configuration);
            foreach (var item in commondGenerators)
            {
                var command = await item(result);

                SSHEndpointCommandServiceForOnce service = new SSHEndpointCommandServiceForOnce(getCommandServiceUrl(configurationObj.Address));
                result=await service.Do(command);
            }

            return result ?? string.Empty;
        }

        public async Task<bool> ExistsFile(string configuration, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            bool result = true;
            await exceptionHandle(async () =>
            {
                await sftpClientExecute(configuration, async (client) =>
                {
                    if (timeoutSeconds != -1)
                    {
                        client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    }

                    result = client.Exists(path);
                    await Task.FromResult(0);
                });

            });

            return result;
        }

        public async Task UploadFile(string configuration, Stream stream, string path, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {

            var configurationObj = getConfiguration(configuration);
            SSHEndpointUploadFileServiceForOnce service = new SSHEndpointUploadFileServiceForOnce(getUploadServiceUrl(configurationObj.Address));
            await service.Upload(stream, path);
        }

        public async Task UploadFile(string configuration, Func<ISSHEndpointUploadFileService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);
            SSHEndpointUploadFileServiceForOnce service = new SSHEndpointUploadFileServiceForOnce(getUploadServiceUrl(configurationObj.Address));
            await action(service);

        }

        public async Task UploadFileBatch(string configuration, IList<(Stream, string)> uploadFileInfos, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);
            SSHEndpointUploadFileServiceForOnce service = new SSHEndpointUploadFileServiceForOnce(getUploadServiceUrl(configurationObj.Address));

            foreach (var item in uploadFileInfos)
            {
                await service.Upload(item.Item1, item.Item2);
            }

        }

        public async Task<int> TransferFile(string configuration, Func<string, Task<string>> fileNameGenerateAction, string fromPath, string toPath, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            int fileCount = 0;
            await exceptionHandle(async () =>
            {
                await sftpClientExecute(configuration, async (client) =>
                {
                    if (timeoutSeconds != -1)
                    {
                        client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                    }

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
                });

            });
            return fileCount;
        }
        private async Task exceptionHandle(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (SshOperationTimeoutException)
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
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);
            return configurationObj;
        }

        private async Task sftpClientExecute(string configuration, Func<SftpClient, Task> action)
        {
            var configurationObj = getConfiguration(configuration);
            List<AuthenticationMethod> authMethods = new List<AuthenticationMethod>();
            authMethods.Add(new PasswordAuthenticationMethod(configurationObj.UserName, configurationObj.Password));

            ConnectionInfo sshConnectionInfo = new ConnectionInfo(configurationObj.Address, configurationObj.Port, configurationObj.UserName, authMethods.ToArray());
            sshConnectionInfo.MaxSessions = 200;
            sshConnectionInfo.Timeout = new TimeSpan(0, 0, 5);
            using (var sshClient = new SftpClient(sshConnectionInfo))
            {
                //sshClient.KeepAliveInterval = new TimeSpan(0, 0, 1);
                var replay = 0;
                while (true)
                {
                    try
                    {
                        if (!sshClient.IsConnected)
                        {
                            sshClient.Connect();
                        }

                        break;
                    }
                    catch (SshOperationTimeoutException)
                    {
                        replay++;
                        if (replay == 3)
                        {
                            throw;
                        }
                    }
                }

                try
                {
                    await action(sshClient);
                }
                finally
                {
                    try
                    {
                        sshClient.Disconnect();
                    }
                    catch
                    {

                    }
                }
            }

        }

        private async Task sshClientExecute(string configuration, Func<SshClient, Task> action)
        {

            var configurationObj = getConfiguration(configuration);
            List<AuthenticationMethod> authMethods = new List<AuthenticationMethod>();
            authMethods.Add(new PasswordAuthenticationMethod(configurationObj.UserName, configurationObj.Password));

            ConnectionInfo sshConnectionInfo = new ConnectionInfo(configurationObj.Address, configurationObj.Port, configurationObj.UserName, authMethods.ToArray());
            sshConnectionInfo.Timeout = new TimeSpan(0, 0, 5);
            sshConnectionInfo.MaxSessions = 200;
            using (var sshClient = new SshClient(sshConnectionInfo))
            {
                //sshClient.KeepAliveInterval = new TimeSpan(0, 0, 1);
                var replay = 0;
                while (true)
                {
                    try
                    {
                        if (!sshClient.IsConnected)
                        {
                            sshClient.Connect();
                        }

                        break;
                    }
                    catch (SshOperationTimeoutException)
                    {

                        replay++;
                        if (replay == 3)
                        {
                            throw;
                        }
                    }
                }

                try
                {
                    await action(sshClient);
                }
                finally
                {
                    try
                    {
                        sshClient.Disconnect();
                    }
                    catch
                    {

                    }
                }
            }

        }

        private string getUploadServiceUrl(string host)
        {
            return $"http://{host}:{ServicePort.ToString()}{UploadServicePath}";
        }

        private string getCommandServiceUrl(string host)
        {
            return $"http://{host}:{ServicePort.ToString()}{CommandServicePath}";
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

    public class SSHEndpointUploadFileServiceForOnce : ISSHEndpointUploadFileService
    {

        private string _serviceUrl;

        public SSHEndpointUploadFileServiceForOnce(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }
        public async Task Upload(Stream fileStream, string path)
        {
            var bytes=await fileStream.ReadAll(10240);
            var text=UTF8Encoding.UTF8.GetString(bytes);
            UploadFileData data = new UploadFileData()
            {
                Data = text,
                Path = path
            };
            await HttpClinetHelper.PostAsync(data, _serviceUrl);
        }

        [DataContract]
        private class UploadFileData
        {
            public string Data { get; set; } = null!;
            public string Path { get; set; } = null!;
        }
    }

    public class SSHEndpointCommandServiceForOnce : ISSHEndpointCommandService
    {
        private string _serviceUrl;

        public SSHEndpointCommandServiceForOnce(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }
        public async Task<string> Do(string command)
        {
            return await HttpClinetHelper.PostAsync<string,string>(command, _serviceUrl);
        }
    }
}
