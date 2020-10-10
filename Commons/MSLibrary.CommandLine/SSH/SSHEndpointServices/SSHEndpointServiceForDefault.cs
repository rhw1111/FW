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
    [Injection(InterfaceType = typeof(SSHEndpointServiceForDefault), Scope = InjectionScope.Singleton)]
    public class SSHEndpointServiceForDefault : ISSHEndpointService
    {
        private static ConcurrentDictionary<string, Pool<SftpClient>> _sftpClientPools = new ConcurrentDictionary<string, Pool<SftpClient>>();
        private static ConcurrentDictionary<string, Pool<SshClient>> _sshClientPools = new ConcurrentDictionary<string, Pool<SshClient>>();

        public static int SftpClientPoolLength { get; set; } = 1;
        public static int SshClientPoolLength { get; set; } = 1;

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
            string result=string.Empty;
            await exceptionHandle(async () =>
            {
                await sshClientExecute(configuration, async (client) =>
                 {
                     var sshCommand = client.CreateCommand(command);

                     if (timeoutSeconds != -1)
                     {
                         sshCommand.CommandTimeout = new TimeSpan(0, 0, timeoutSeconds);
                     }

                     int repeatTimes = 0;

                     while (repeatTimes <= 2)
                     {
                         try
                         {
                             result = await sshCommand.ExecuteAsync();

                             break;
                         }
                         catch (InvalidOperationException)
                         {
                             repeatTimes++;

                             if (repeatTimes > 2)
                             {
                                 throw;
                             }
                         }
                     }
                 });
            }
            );
            return result;
        }

        public async Task ExecuteCommand(string configuration, Func<ISSHEndpointCommandService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                await sshClientExecute(configuration, async (client) =>
                 {
                     SSHEndpointCommandService service = new SSHEndpointCommandService(client, timeoutSeconds);

                     int repeatTimes = 0;

                     while (repeatTimes <= 2)
                     {
                         try
                         {
                             await action(service);

                             break;
                         }
                         catch (InvalidOperationException)
                         {
                             repeatTimes++;

                             if (repeatTimes > 2)
                             {
                                 throw;
                             }
                         }
                     }
                 });
            }
            );


        }

        public async Task<string> ExecuteCommandBatch(string configuration, IList<Func<string?, Task<string>>> commondGenerators, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            string? result = null;

            await exceptionHandle(async () =>
            {
                await sshClientExecute(configuration, async (client) =>
                 {
                     foreach (var item in commondGenerators)
                     {
                         var command = await item(result);
                         var sshCommond = client.CreateCommand(command);

                         if (timeoutSeconds != -1)
                         {
                             sshCommond.CommandTimeout = new TimeSpan(0, 0, timeoutSeconds);
                         }

                         int repeatTimes = 0;

                         while (repeatTimes <= 2)
                         {
                             try
                             {
                                 result = await sshCommond.ExecuteAsync();

                                 break;
                             }
                             catch (InvalidOperationException)
                             {
                                 repeatTimes++;

                                 if (repeatTimes > 2)
                                 {
                                     throw;
                                 }
                             }
                         }
                     }
                 });
            });

            return result??string.Empty;
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
            await exceptionHandle(async () =>
            {
                await sftpClientExecute(configuration, async (client)=>
                 {
                     if (timeoutSeconds != -1)
                     {
                         client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                     }

                     int repeatTimes = 0;

                     while (repeatTimes <= 2)
                     {
                         try
                         {
                             await client.UploadAsync(stream, path);
                             break;
                         }
                         catch (InvalidOperationException)
                         {
                             repeatTimes++;

                             if (repeatTimes > 2)
                             {
                                 throw;
                             }
                         }
                     }
                 }
                );
            });

        }

        public async Task UploadFile(string configuration, Func<ISSHEndpointUploadFileService, Task> action, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                await sftpClientExecute(configuration, async (client) =>
                 {
                     if (timeoutSeconds != -1)
                     {
                         client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                     }

                     SSHEndpointUploadFileServiceForDefault service = new SSHEndpointUploadFileServiceForDefault(client);

                     int repeatTimes = 0;

                     while (repeatTimes <= 2)
                     {
                         try
                         {
                             await action(service);

                             break;
                         }
                         catch (InvalidOperationException)
                         {
                             repeatTimes++;

                             if (repeatTimes > 2)
                             {
                                 throw;
                             }
                         }
                     }
                 });
            }
            );

        }

        public async Task UploadFileBatch(string configuration, IList<(Stream, string)> uploadFileInfos, int timeoutSeconds = -1, CancellationToken cancellationToken = default)
        {
            await exceptionHandle(async () =>
            {
                await sftpClientExecute(configuration, async (client) =>
                 {
                     if (timeoutSeconds != -1)
                     {
                         client.OperationTimeout = new TimeSpan(0, 0, timeoutSeconds);
                     }

                     foreach (var item in uploadFileInfos)
                     {
                         int repeatTimes = 0;

                         while (repeatTimes <= 2)
                         {
                             try
                             {
                                 await client.UploadAsync(item.Item1, item.Item2);

                                 break;
                             }
                             catch (InvalidOperationException)
                             {
                                 repeatTimes++;

                                 if (repeatTimes > 2)
                                 {
                                     throw;
                                 }
                             }
                         }
                     }
                 });
            });
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

        private async Task sftpClientExecute(string configuration,Func<SftpClient,Task> action)
        {
            if (!_sftpClientPools.TryGetValue(configuration,out Pool<SftpClient>? pool))
            {
                var configurationObj = getConfiguration(configuration);
                lock (_sftpClientPools)
                {
                    if (!_sftpClientPools.TryGetValue(configuration, out pool))
                    {
                        pool = new Pool<SftpClient>($"SftpClient-{configuration}",
            null,
            null,
            null,
            null,
           async () =>
           {
               List<AuthenticationMethod> authMethods = new List<AuthenticationMethod>();
               authMethods.Add(new PasswordAuthenticationMethod(configurationObj.UserName, configurationObj.Password));

               ConnectionInfo sshConnectionInfo = new ConnectionInfo(configurationObj.Address, configurationObj.Port, configurationObj.UserName, authMethods.ToArray());
               sshConnectionInfo.MaxSessions = 200;
               sshConnectionInfo.Timeout = new TimeSpan(0, 0, 5);
               var sshClient = new SftpClient(sshConnectionInfo);
               sshClient.KeepAliveInterval = new TimeSpan(0,0,1);
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

               return await Task.FromResult(sshClient);
           },
           async (item) =>
           {
              
               if (!item.IsConnected)
               {

                   return await Task.FromResult(false);
               }
               return await Task.FromResult(true);
           },
           null,
           null
           , SftpClientPoolLength);
                        _sftpClientPools[configuration] = pool;
                    }
                }
            }

            var client = await pool.GetAsync(true);
            try
            {
                await action(client);
            }
            finally
            {
                await pool.ReturnAsync(client);
            }
        }

        private async Task sshClientExecute(string configuration, Func<SshClient, Task> action)
        {
            if (!_sshClientPools.TryGetValue(configuration, out Pool<SshClient>? pool))
            {
                var configurationObj = getConfiguration(configuration);
                lock (_sshClientPools)
                {
                    if (!_sshClientPools.TryGetValue(configuration, out pool))
                    {
                        pool = new Pool<SshClient>($"SshClient-{configuration}",
            null,
            null,
            null,
            null,
           async () =>
           {
               List<AuthenticationMethod> authMethods = new List<AuthenticationMethod>();
               authMethods.Add(new PasswordAuthenticationMethod(configurationObj.UserName, configurationObj.Password));

               ConnectionInfo sshConnectionInfo = new ConnectionInfo(configurationObj.Address, configurationObj.Port, configurationObj.UserName, authMethods.ToArray());
               sshConnectionInfo.Timeout = new TimeSpan(0, 0, 5);
               sshConnectionInfo.MaxSessions = 200;
               var sshClient = new SshClient(sshConnectionInfo);
               sshClient.KeepAliveInterval = new TimeSpan(0, 0, 1);
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

               return await Task.FromResult(sshClient);
           },
           async (item) =>
           {
               
               if (!item.IsConnected)
               {

                   return await Task.FromResult(false);
               }
               return await Task.FromResult(true);
           },
           null,
           null
           , SshClientPoolLength);
                        _sshClientPools[configuration] = pool;
                    }
                }


            }

            var client = await pool.GetAsync(true);
            try
            {
                await action(client);
            }
            finally
            {
                await pool.ReturnAsync(client);
            }
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
