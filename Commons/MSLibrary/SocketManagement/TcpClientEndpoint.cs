using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using MSLibrary.Collections;
using MSLibrary.Logger;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.SocketManagement.DAL;

namespace MSLibrary.SocketManagement
{
    /// <summary>
    /// Tcp客户端终结点（应答式）
    /// 用于与服务器进行Tcp通信
    /// 支持短连接和长连接
    /// </summary>
    public class TcpClientEndpoint : EntityBase<ITcpClientEndpointIMP>
    {
        private static IFactory<ITcpClientEndpointIMP> _tcpClientEndpointIMPFactory;

        public static IFactory<ITcpClientEndpointIMP> TcpClientEndpointIMPFactory
        {
            set
            {
                _tcpClientEndpointIMPFactory = value;
            }
        }
        public override IFactory<ITcpClientEndpointIMP> GetIMPFactory()
        {
            return _tcpClientEndpointIMPFactory;
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
        /// 终结点名称
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
        /// 需要连接的服务器地址
        /// </summary>
        public string ServerAddress
        {
            get
            {
                return GetAttribute<string>("ServerAddress");
            }
            set
            {
                SetAttribute<string>("ServerAddress", value);
            }
        }


        /// <summary>
        /// 监听端口
        /// </summary>
        public int ServerPort
        {
            get
            {
                return GetAttribute<int>("ServerPort");
            }
            set
            {
                SetAttribute<int>("ServerPort", value);
            }
        }

        /// <summary>
        /// 是否长连接
        /// </summary>
        public bool KeepAlive
        {
            get
            {
                return GetAttribute<bool>("KeepAlive");
            }
            set
            {
                SetAttribute<bool>("KeepAlive", value);
            }
        }

        /// <summary>
        /// 连接池的最大连接保持数
        /// 只有是长连接时才有意义
        /// </summary>
        public int PoolMaxSize
        {
            get
            {
                return GetAttribute<int>("PoolMaxSize");
            }
            set
            {
                SetAttribute<int>("PoolMaxSize", value);
            }
        }

        /// <summary>
        /// 针对收到的数据做处理的完整类型名称（包括程序集名称）
        /// 该类型必须实现IFactory<ITcpClientDataExecute>接口
        /// </summary>
        public string ExecuteDataFactoryType
        {
            get
            {
                return GetAttribute<string>("ExecuteDataFactoryType");
            }
            set
            {
                SetAttribute<string>("ExecuteDataFactoryType", value);
            }
        }

        /// <summary>
        /// 执行类型是否使用DI容器
        /// </summary>
        public bool ExecuteDataFactoryTypeUseDI
        {
            get
            {
                return GetAttribute<bool>("ExecuteDataFactoryTypeUseDI");
            }
            set
            {
                SetAttribute<bool>("ExecuteDataFactoryTypeUseDI", value);
            }
        }


        /// <summary>
        /// 向服务器发送的心跳数据
        /// 只有是长连接才有意义
        /// </summary>
        public string HeartBeatSendData
        {
            get
            {
                return GetAttribute<string>("HeartBeatSendData");
            }
            set
            {
                SetAttribute<string>("HeartBeatSendData", value);
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
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<byte[]> Send(byte[] data)
        {
            return await _imp.Send(this,data);
        }

        /// <summary>
        /// 清理
        /// </summary>
        /// <returns></returns>
        public async Task Dispose()
        {
            await _imp.Dispose(this);
        }
    }


    public interface ITcpClientEndpointIMP
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Add(TcpClientEndpoint endpoint);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Update(TcpClientEndpoint endpoint);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Delete(TcpClientEndpoint endpoint);
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<byte[]> Send(TcpClientEndpoint endpoint,byte[] data);
        /// <summary>
        /// 清理
        /// </summary>
        /// <returns></returns>
        Task Dispose(TcpClientEndpoint endpoint);
    }

    [Injection(InterfaceType = typeof(ITcpClientEndpointIMP), Scope = InjectionScope.Transient)]
    public class TcpClientEndpointIMP : ITcpClientEndpointIMP
    {
        private static string _logCategoryName;
 
        public static string LogCategoryName
        {
            set
            {
                _logCategoryName = value;
            }
        }



        private bool _start = false;

        /// <summary>
        /// 连接上下文池
        /// </summary>
        private Pool<TcpClientContext> _tcpClientContextPool;
        private ILoggerFactory _loggerFactory;

        private TcpClientEndpoint _endpoint;

        private ITcpClientEndpointStore _tcpClientEndpointStore;

        public TcpClientEndpointIMP(ILoggerFactory loggerFactory, ITcpClientEndpointStore tcpClientEndpointStore)
        {
            _loggerFactory = loggerFactory;
            _tcpClientEndpointStore = tcpClientEndpointStore;
        }


        public async Task Add(TcpClientEndpoint endpoint)
        {
            await _tcpClientEndpointStore.Add(endpoint);
        }

        public async Task Delete(TcpClientEndpoint endpoint)
        {
            await _tcpClientEndpointStore.Delete(endpoint.ID);
        }

        public async Task Dispose(TcpClientEndpoint endpoint)
        {
            //将池中的每个上下文都执行Dispose操作
            _start = false;
            var contextItems = _tcpClientContextPool.Items;
            foreach(var item in contextItems)
            {
                if (!item.Value.ConnectionIsClose)
                {
                    await item.Value.Dispose();
                }
            }
            _tcpClientContextPool.Dispose();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<byte[]> Send(TcpClientEndpoint endpoint, byte[] data)
        {
            //初始化终结点
            await InitEndpoint(endpoint);
            TcpClientContext context=null;
            //从池中获取Tcp客户端上下文
            try
            {
                context = await _tcpClientContextPool.GetAsync(true);
                //判断是否已经断开连接，如果已经断开连接，则直接抛出异常
                if (context.ConnectionIsClose)
                {
                    throw context.MainException;
                }

                return await context.Send(data);
                
            }
            finally
            {
                if (context != null)
                {
                    _tcpClientContextPool.Return(context);
                }
            }

        }

        public async Task Update(TcpClientEndpoint endpoint)
        {
            await _tcpClientEndpointStore.Update(endpoint);
        }

        /// <summary>
        /// 初始化终结点
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        private async Task InitEndpoint(TcpClientEndpoint endpoint)
        {
            if (!_start)
            {
                _start = true;
                _endpoint = endpoint;
                try
                {
                    _tcpClientContextPool = new Pool<TcpClientContext>($"TcpClient-{endpoint.Name}",
                        null,
                        null,
                        null,
                        null,
                        async () =>
                        {
                            TcpClientContext context = new TcpClientContext(endpoint, _loggerFactory, _logCategoryName);
                            await context.Init();
                            return context;
                        },
                        async (context) =>
                        {
                            if (context.ConnectionIsClose)
                            {

                                return await Task.FromResult(false);
                            }
                            else
                            {
                                try
                                {
                                    await context.Send(UTF8Encoding.UTF8.GetBytes(_endpoint.HeartBeatSendData));
                                }
                                catch (Exception ex)
                                {
                                    LoggerHelper.LogError( _logCategoryName, $"TcpClientEndpoint {_endpoint.Name} send HeartBeat error,server address:{_endpoint.ServerAddress},port:{_endpoint.ServerPort.ToString()},error message:{ex.Message},stack:{ex.StackTrace}");
                                }

                                if (context.ConnectionIsClose)
                                {
                                    return await Task.FromResult(false);
                                }
                                else
                                {
                                    return await Task.FromResult(true);
                                }
                            }
                        },
                        null,
                        null,
                        endpoint.PoolMaxSize
                        );

                    if (_endpoint.KeepAlive)
                    {
                        //启动一个循环执行的方法，每隔一秒为所有可用的连接发送心跳信息
                        var t = Task.Run(async () =>
                          {
                              while (_start)
                              {
                                  await _tcpClientContextPool.InvokeEveryUnUseItem(async (context) =>
                                  {
                                      if (!context.ConnectionIsClose)
                                      {
                                          try
                                          {
                                              await context.Send(UTF8Encoding.UTF8.GetBytes(_endpoint.HeartBeatSendData));
                                          }
                                          catch (Exception ex)
                                          {
                                              LoggerHelper.LogError( _logCategoryName, $"TcpClientEndpoint {_endpoint.Name} send HeartBeat error,server address:{_endpoint.ServerAddress},port:{_endpoint.ServerPort.ToString()},error message:{ex.Message},stack:{ex.StackTrace}");
                                          }
                                      }
                                  });

                                  System.Threading.Thread.Sleep(1000);
                              }
                          });
                    }
                }
                catch
                {
                    _start = false;
                    throw;
                }
            }

            await Task.FromResult(0);
        }


    }

    /// <summary>
    /// Tcp客户端上下文
    /// </summary>
    public class TcpClientContext
    {
        private static Dictionary<string, IFactory<ITcpClientDataExecute>> _tcpDataExecuteFactories = new Dictionary<string, IFactory<ITcpClientDataExecute>>();

        public static Dictionary<string, IFactory<ITcpClientDataExecute>> TcpDataExecuteFactories
        {
            get
            {
                return _tcpDataExecuteFactories;
            }
        }

        private TcpClientEndpoint _endpoint;
        private Socket _tcpClient;
        private ILoggerFactory _loggerFactory;
        private string _logCategoryName;
        private SocketAsyncEventArgs _args;
        private SemaphoreSlim _connectSemaphore;
        private SemaphoreSlim _sendSemaphore;
        private SemaphoreSlim _receiveSemaphore;

        private List<byte> _receiveBytes;

        private object _lockObje = new object();


        public TcpClientContext(TcpClientEndpoint endpoint, ILoggerFactory loggerFactory, string logCategoryName)
        {
            _loggerFactory = loggerFactory;
            _logCategoryName = logCategoryName;
            _endpoint = endpoint;
            IPEndPoint serverPoint = new IPEndPoint(IPAddress.Parse(_endpoint.ServerAddress), _endpoint.ServerPort);
            _tcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            
            _receiveBytes = new List<byte>();

            _args = new SocketAsyncEventArgs()
            {
                UserToken = this,
                 RemoteEndPoint=serverPoint
            };

            _args.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

            _connectSemaphore = new SemaphoreSlim(0, 1);
            _sendSemaphore = new SemaphoreSlim(0,1);
            _receiveSemaphore = new SemaphoreSlim(0,1);
            ConnectionIsClose = false;
        }

        /// <summary>
        /// 连接是否已经关闭
        /// </summary>
        public bool ConnectionIsClose
        {
            get;set;
        }

        /// <summary>
        /// 发送的数据
        /// </summary>
        public byte[] SendData
        {
            get;set;
        }

        /// <summary>
        /// 发送时的异常
        /// </summary>
        public Exception SendException
        {
            get;set;
        }

        /// <summary>
        /// 总异常
        /// </summary>
        public Exception MainException
        {
            get; set;
        }
        /// <summary>
        /// 接收操作时，接收到的字节列表
        /// </summary>
        public List<byte> ReceiveBytes
        {
            get
            {
                return _receiveBytes;
            }
        }

        /// <summary>
        /// 接收时的异常
        /// </summary>
        public Exception ReceiveException
        {
            get;set;
        }

        public SemaphoreSlim ConnectSemaphore
        {
            get
            {
                return _connectSemaphore;
            }
        }

        public SemaphoreSlim SendSemaphore
        {
            get
            {
                return _sendSemaphore;
            }
        }


        public SemaphoreSlim ReceiveSemaphore
        {
            get
            {
                return _receiveSemaphore;
            }
        }

        /// <summary>
        /// 上下文初始化
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            try
            {
                var willRaiseEvent = _tcpClient.ConnectAsync(_args);
                if (!willRaiseEvent)
                {
                    await ProcessConnect(_args);
                }
                //等待连接完成，超时时间为1秒，
                var waitResult= await _connectSemaphore.WaitAsync(1000);
                if (!waitResult)
                {
                    //表示发生连接超时
                    await CloseConnection(new Exception($"TcpClientEndpoint {_endpoint.Name} connect server {_endpoint.ServerAddress}:{_endpoint.ServerPort.ToString()} connect timeout"));
                    return;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"TcpClientEndpoint {_endpoint.Name} connect server {_endpoint.ServerAddress}:{_endpoint.ServerPort.ToString()} error,message:{ex.Message},stack:{ex.StackTrace}";
                LoggerHelper.LogError( _logCategoryName, errorMessage);
                await CloseConnection(new Exception(errorMessage));
            }
        }


        /// <summary>
        /// 执行发送
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<byte[]> Send(byte[] data)
        {
            try
            {
                //清空发送与接收的相关参数
                SendException = null;
                ReceiveException = null;
                MainException = null;
                _receiveBytes.Clear();
                SendData = data;

                _args.SetBuffer(data, 0, data.Length);

                var willRaiseEvent = _tcpClient.SendAsync(_args);
                if (!willRaiseEvent)
                {
                    await ProcessSend(_args);
                }

                //等待发送完成
                await _sendSemaphore.WaitAsync();

                if (SendException!=null)
                {
                    throw SendException;
                }

                byte[] responseBuffer = new byte[1024];

                _args.SetBuffer(responseBuffer, 0, responseBuffer.Length);

                //接收服务器的返回数据
                willRaiseEvent = _tcpClient.ReceiveAsync(_args);
                if (!willRaiseEvent)
                {
                    await ProcessReceive(_args);
                }
                //等待接收完成
                await _receiveSemaphore.WaitAsync();
                if (ReceiveException!=null)
                {
                    throw ReceiveException;
                }

                if (!_endpoint.KeepAlive)
                {
                    await CloseConnection(new Exception("connect close"));
                }
                return _receiveBytes.ToArray();
            }
            catch (Exception ex)
            {
                string errorMessage = $"TcpClientEndpoint {_endpoint.Name} SendData error, server {_endpoint.ServerAddress}:{_endpoint.ServerPort.ToString()} error,message:{ex.Message},stack:{ex.StackTrace}";
                LoggerHelper.LogError( _logCategoryName, errorMessage);
                await CloseConnection(new Exception(errorMessage));
                throw ex;
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        /// <returns></returns>
        public async Task Dispose()
        {
            if (!ConnectionIsClose)
            {
                await CloseConnection(new Exception("connection close"));
            }
        }

        private async void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    await ProcessConnect(e);
                    break;
                case SocketAsyncOperation.Send:
                    await ProcessSend(e);
                    break;
                case SocketAsyncOperation.Receive:
                    await ProcessReceive(e);
                    break;
                default:
                    break;
            }


        }

        /// <summary>
        /// 连接处理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task ProcessConnect(SocketAsyncEventArgs e)
        {
            var context = (TcpClientContext)e.UserToken;
            if (e.SocketError != SocketError.Success)
            {
                string errorMessage = $"TcpClientEndpoint {_endpoint.Name} ProcessConnect error, server {_endpoint.ServerAddress}:{_endpoint.ServerPort.ToString()} error";
                LoggerHelper.LogError(_logCategoryName, errorMessage);
                await CloseConnection(new Exception(errorMessage));
            }

            context.ConnectSemaphore.Release();
        }


        /// <summary>
        /// 发送处理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task ProcessSend(SocketAsyncEventArgs e)
        {
            var context = (TcpClientContext)e.UserToken;

            if (e.SocketError == SocketError.Success)
            {
                context._sendSemaphore.Release();
            }
            else
            {
                var ex = new Exception($"TcpClientEndpoint {_endpoint.Name} receive error,send data is {context.SendData}");
                context.SendException = ex;
                context.SendSemaphore.Release();
            }

            await Task.FromResult(0);
        }

        /// <summary>
        /// 接收处理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task ProcessReceive(SocketAsyncEventArgs e)
        {
            var context = (TcpClientContext)e.UserToken;

            if (e.SocketError == SocketError.Success)
            {
                await DoProcessReceive(e);
            }
            else
            {
                var ex = new Exception($"TcpClientEndpoint {_endpoint.Name} receive error,send data is {context.SendData}");
                context.ReceiveException = ex;
                context.ReceiveSemaphore.Release();
                await CloseConnection(ex);
            }
        }


        private async Task<InnerDoProcessReceiveResult> InnerDoProcessReceive(SocketAsyncEventArgs e, bool useSocket = false, byte[] buffer = null)
        {
            InnerDoProcessReceiveResult result = new InnerDoProcessReceiveResult();

            var context = (TcpClientContext)e.UserToken;

            if (!useSocket)
            {
                //将数据加入到接收上下文中
                context.ReceiveBytes.AddRange(e.Buffer.Take(e.BytesTransferred));
            }
            else
            {

                var bufferLength = e.ConnectSocket.Receive(buffer);

                context.ReceiveBytes.AddRange(buffer.Take(bufferLength));
            }


            //如果有尚未接收完成的数据，继续接收


            if (e.ConnectSocket.Available > 0)
            {
                if (buffer == null)
                {
                    buffer = new byte[e.Buffer.Length];
                }

                result.Complete = false;
                result.Buffer = buffer;
                return await Task.FromResult(result);

            }
            else
            {
                result.Complete = true;
                result.Buffer = buffer;
                return await Task.FromResult(result);
            }
        }



        private async Task DoProcessReceive(SocketAsyncEventArgs e, bool useSocket = false, byte[] buffer = null)
        {
            var context = (TcpClientContext)e.UserToken;

            var innerResult = await InnerDoProcessReceive(e, useSocket, buffer);

            while (!innerResult.Complete)
            {
                innerResult = await InnerDoProcessReceive(e, true, innerResult.Buffer);
            }

            //处理接收到的数据
            //var strData = UTF8Encoding.UTF8.GetString(context.ReceiveBytes.ToArray());

            

            if (!_tcpDataExecuteFactories.TryGetValue(_endpoint.ExecuteDataFactoryType, out IFactory<ITcpClientDataExecute> tcpDataExecuteFactory))
            {
                lock (_tcpDataExecuteFactories)
                {
                    Type tcpDataExecuteFactoryType = Type.GetType(_endpoint.ExecuteDataFactoryType);

                    if (!_tcpDataExecuteFactories.TryGetValue(_endpoint.ExecuteDataFactoryType, out tcpDataExecuteFactory))
                    {
                        object objTcpDataExecuteFactory;
                        if (_endpoint.ExecuteDataFactoryTypeUseDI == true)
                        {
                            //通过DI容器创建
                            objTcpDataExecuteFactory = DIContainerContainer.Get(tcpDataExecuteFactoryType);
                        }
                        else
                        {
                            //通过反射创建
                            objTcpDataExecuteFactory = tcpDataExecuteFactoryType.Assembly.CreateInstance(tcpDataExecuteFactoryType.FullName);
                        }

                        if (!(objTcpDataExecuteFactory is IFactory<ITcpClientDataExecute>))
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.TcpClientDataExecuteTypeError,
                                DefaultFormatting = "Tcp客户端{0}中，数据处理的工厂类型{1}未实现接口IFactory<ITcpClientDataExecute>",
                                ReplaceParameters = new List<object>() { _endpoint.Name, _endpoint.ExecuteDataFactoryType }
                            };

                            throw new UtilityException((int)Errors.TcpClientDataExecuteTypeError, fragment);
                        }

                        tcpDataExecuteFactory = (IFactory<ITcpClientDataExecute>)objTcpDataExecuteFactory;
                        _tcpDataExecuteFactories.Add(_endpoint.ExecuteDataFactoryType, tcpDataExecuteFactory);
                    }
                }
            }

            var tcpDataExecute = tcpDataExecuteFactory.Create();

            var receiceResult=await tcpDataExecute.ReceiveExecute(context.ReceiveBytes.ToArray());
            if (receiceResult.Exception == null)
            {
                context.ReceiveException = null;
                context.ReceiveBytes.Clear();
                context.ReceiveBytes.AddRange(receiceResult.Data);
                context.ReceiveSemaphore.Release();
            }
            else
            {
                context.ReceiveException = receiceResult.Exception;
                context.ReceiveSemaphore.Release();
                await CloseConnection(receiceResult.Exception);
            }
        }


        private  async Task CloseConnection(Exception ex)
        {
            if (!ConnectionIsClose)
            {
                lock (_lockObje)
                {
                    if (!ConnectionIsClose)
                    {
                        //关闭Socket连接
                        try
                        {
                            _tcpClient.Close();
                        }
                        catch
                        {

                        }

                        ConnectionIsClose = true;
                        MainException = ex;
                    }
                }
            }

            await Task.FromResult(0);
        }



        class InnerDoProcessReceiveResult
        {
            public bool Complete { get; set; }
            public byte[] Buffer { get; set; }
        }


    }

    /// <summary>
    /// Tcp客户端数据处理服务
    /// 负责服务端响应数据的处理
    /// </summary>
    public interface ITcpClientDataExecute
    {
        /// <summary>
        /// 服务端响应数据的处理
        /// </summary>
        /// <param name="receiveData"></param>
        /// <returns></returns>
        Task<TcpClientReceiveDataResult> ReceiveExecute(byte[] receiveData);
    }
    public class TcpClientReceiveDataResult
    {
        public Exception Exception { get; set; }
        public byte[] Data { get; set; }
    }
}
