using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using Renci.SshNet;
using Renci.SshNet.Async;

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
        public async Task DownloadFile(string configuration, Func<Stream, Task> action, string path, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);
            using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
            {
                client.Connect();
                await using (var stream=new MemoryStream())
                {
                    await client.DownloadAsync(path, stream);
                    await action(stream);
                    stream.Close();
                }                  
                client.Disconnect();
            }

        }

        public async Task<string> ExecuteCommand(string configuration, string command, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);

            string result;
            using (var sshClient = new SshClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
            {

                sshClient.Connect();

                result = await sshClient.RunCommand(command).ExecuteAsync();

                sshClient.Disconnect();
            }

            return result;
        }

        public async Task ExecuteCommand(string configuration, Func<ISSHEndpointCommandService, Task> action, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);

            using (var sshClient = new SshClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
            {

                sshClient.Connect();

                SSHEndpointCommandService service = new SSHEndpointCommandService(sshClient);
                await action(service);

                sshClient.Disconnect();
            }

        }

        public async Task<string> ExecuteCommandBatch(string configuration, IList<Func<string?, Task<string>>> commondGenerators, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);


            string? result=null;
          
            using (var sshClient = new SshClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
            {

                sshClient.Connect();

                foreach (var item in commondGenerators)
                {
                    var command = await item(result);
                    result = await sshClient.RunCommand(command).ExecuteAsync();
                }

                sshClient.Disconnect();
            }

            return result??string.Empty;
        }

        public async Task UploadFile(string configuration, Stream stream, string path, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);
            using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password)) 
            {
                client.Connect(); 
                await client.UploadAsync(stream, path);
                
                client.Disconnect();
            }
        }

        public async Task UploadFile(string configuration, Func<ISSHEndpointUploadFileService, Task> action, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);
            using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
            {
                client.Connect();

                SSHEndpointUploadFileServiceForDefault service = new SSHEndpointUploadFileServiceForDefault(client);
                await action(service);

                client.Disconnect();
            }
        }

        public async Task UploadFileBatch(string configuration, IList<(Stream, string)> uploadFileInfos, CancellationToken cancellationToken = default)
        {
            var configurationObj = getConfiguration(configuration);

            using (var client = new SftpClient(configurationObj.Address, configurationObj.Port, configurationObj.UserName, configurationObj.Password))
            {
                client.Connect();
                foreach (var item in uploadFileInfos)
                {
                    await client.UploadAsync(item.Item1, item.Item2);
                }              
                client.Disconnect();
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

        public SSHEndpointCommandService(SshClient sshClient)
        {
            _sshClient = sshClient;
        }
        public async Task<string> Do(string command)
        {
            return await _sshClient.RunCommand(command).ExecuteAsync();
        }
    }
}
