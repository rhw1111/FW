using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MSLibrary.Collections;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.SocketManagement.DAL;

namespace MSLibrary.SocketManagement
{
    /// <summary>
    /// Tcp监听器（双工）
    /// 用于监听处理Tcp连接
    /// </summary>
    public class TcpDuplexListener : EntityBase<ITcpDuplexListenerIMP>
    {
        private static IFactory<ITcpDuplexListenerIMP> _tcpDuplexListenerIMPFactory;

        public static IFactory<ITcpDuplexListenerIMP> TcpDuplexListenerIMPFactory
        {
            set
            {
                _tcpDuplexListenerIMPFactory = value;
            }
        }

        public override IFactory<ITcpDuplexListenerIMP> GetIMPFactory()
        {
            return _tcpDuplexListenerIMPFactory;
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
        /// 监听器名称
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
        /// 监听端口
        /// </summary>
        public int Port
        {
            get
            {
                return GetAttribute<int>("Port");
            }
            set
            {
                SetAttribute<int>("Port", value);
            }
        }

        /// <summary>
        /// 最大并发数量
        /// </summary>
        public int MaxConcurrencyCount
        {
            get
            {
                return GetAttribute<int>("MaxConcurrencyCount");
            }
            set
            {
                SetAttribute<int>("MaxConcurrencyCount", value);
            }
        }


        /// <summary>
        /// 缓冲区最大字节数
        /// </summary>
        public int MaxBufferCount
        {
            get
            {
                return GetAttribute<int>("MaxBufferCount");
            }
            set
            {
                SetAttribute<int>("MaxBufferCount", value);
            }
        }

        /// <summary>
        /// 针对收到的数据做处理的完整类型名称（包括程序集名称）
        /// 该类型必须实现IFactory<ITcpDuplexDataExecute>接口
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
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return GetAttribute<string>("Description");
            }
            set
            {
                SetAttribute<string>("Description", value);
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
        /// 启动
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            await _imp.Start(this);
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        public async Task Stop()
        {
            await _imp.Stop(this);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="dataExecute">数据处理</param>
        /// <returns></returns>
        public async Task Send(Func<List<TcpDuplexListenerConnection>, Task<TcpDuplexDataExecuteResult>> dataExecute)
        {
            await _imp.Send(this, dataExecute);
        }

        public async Task Dispose()
        {
             await _imp.Dispose(this);
        }
    }


    public interface ITcpDuplexListenerIMP
    {
        Task Start(TcpDuplexListener listener);

        Task Stop(TcpDuplexListener listener);

        Task Send(TcpDuplexListener listener,Func<List<TcpDuplexListenerConnection>, Task<TcpDuplexDataExecuteResult>> dataExecute);
        Task Dispose(TcpDuplexListener listener);
    }

    [Injection(InterfaceType = typeof(ITcpDuplexListenerIMP), Scope = InjectionScope.Transient)]
    public class TcpDuplexListenerIMP : ITcpDuplexListenerIMP
    {
        private static Dictionary<string, IFactory<ITcpDuplexDataExecute>> _tcpDuplexDataExecuteFactories = new Dictionary<string, IFactory<ITcpDuplexDataExecute>>();

        public static Dictionary<string, IFactory<ITcpDuplexDataExecute>> TcpDuplexDataExecuteFactories
        {
            get
            {
                return _tcpDuplexDataExecuteFactories;
            }
        }

        /// <summary>
        /// Tcp响应无法发送空字节数组
        /// 因此设置该值为当响应为空的时候需要发送的字符串
        /// </summary>
        private const string _emptyResponse = "R";

        private bool _start = false;

        private Socket _listenSocket;

        private SemaphoreSlim _acceptSemaphore;
        private object _lockAcceptSemaphoreObj = new object();

        /// <summary>
        /// 连接上下文池
        /// </summary>
        private Pool<TcpDuplexAcceptContext> _tcpDuplexAcceptContextPool;


        private TcpDuplexListener _listener;


        private ILoggerFactory _loggerFactory;

        public static string LogCategoryName
        {
            get; set;
        }


        public TcpDuplexListenerIMP(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _acceptSemaphore = new SemaphoreSlim(1, 1);
        }


        public async Task Send(TcpDuplexListener listener, Func<List<TcpDuplexListenerConnection>, Task<TcpDuplexDataExecuteResult>> dataExecute)
        {
            var connections = GetConnections();
            if (_start)
            {
                var byteResult=await dataExecute(connections);

                if (byteResult.ResponseData != null && byteResult.ResponseData.Length > 0)
                {
                    List<TcpDuplexAcceptContext> sendContexts = null;
                    if (byteResult.ResponseAll)
                    {
                        sendContexts = GetAllAcceptContext();
                    }
                    else
                    {
                        sendContexts = GetAcceptContext(byteResult.ResponseConnections);
                    }

                    if (sendContexts != null)
                    {
                        foreach (var itemSendContext in sendContexts)
                        {
                            await itemSendContext.ReceiveSemaphore.WaitAsync();

                            itemSendContext.SocketAsyncEventArgs.SetBuffer(byteResult.ResponseData, 0, byteResult.ResponseData.Length);

                            try
                            {
                                var willRaiseEvent = itemSendContext.SocketAsyncEventArgs.AcceptSocket.SendAsync(itemSendContext.SocketAsyncEventArgs);
                                if (!willRaiseEvent)
                                {
                                    await ProcessSend(itemSendContext.SocketAsyncEventArgs);
                                }
                            }
                            catch (Exception ex)
                            {
                                CloseClientSocket(itemSendContext.SocketAsyncEventArgs);
                                await AddLog($"Send Error,message:{ex.Message},stack:{ex.StackTrace}", DateTime.UtcNow);
                                LoggerHelper.LogError(LogCategoryName, $"TcpDuplexListener {_listener.Name},Send Error,message:{ex.Message},stack:{ex.StackTrace}");
                            }

                        }
                    }
                }
            }

        }

        public async Task Start(TcpDuplexListener listener)
        {

            if (!_tcpDuplexDataExecuteFactories.TryGetValue(_listener.ExecuteDataFactoryType, out IFactory<ITcpDuplexDataExecute> tcpDataExecuteFactory))
            {
                lock (_tcpDuplexDataExecuteFactories)
                {
                    Type tcpDataExecuteFactoryType = Type.GetType(_listener.ExecuteDataFactoryType);

                    if (!_tcpDuplexDataExecuteFactories.TryGetValue(_listener.ExecuteDataFactoryType, out tcpDataExecuteFactory))
                    {
                        object objTcpDataExecuteFactory;
                        if (_listener.ExecuteDataFactoryTypeUseDI == true)
                        {
                            //通过DI容器创建
                            objTcpDataExecuteFactory = DIContainerContainer.Get(tcpDataExecuteFactoryType);
                        }
                        else
                        {
                            //通过反射创建
                            objTcpDataExecuteFactory = tcpDataExecuteFactoryType.Assembly.CreateInstance(tcpDataExecuteFactoryType.FullName);
                        }

                        if (!(objTcpDataExecuteFactory is IFactory<ITcpDataExecute>))
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.TcpDuplexDataExecuteTypeError,
                                DefaultFormatting = "双工Tcp监听{0}中，数据处理的工厂类型{1}未实现接口IFactory<ITcpDuplexDataExecute>",
                                ReplaceParameters = new List<object>() { _listener.Name, _listener.ExecuteDataFactoryType }
                            };

                            throw new UtilityException((int)Errors.TcpDuplexDataExecuteTypeError, fragment);
                        }

                        tcpDataExecuteFactory = (IFactory<ITcpDuplexDataExecute>)objTcpDataExecuteFactory;
                        _tcpDuplexDataExecuteFactories.Add(_listener.ExecuteDataFactoryType, tcpDataExecuteFactory);
                    }
                }
            }



            _listener = listener;
            if (!_start)
            {

                //设置监听Socket
                IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, listener.Port);
                _listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                _listenSocket.Bind(localPoint);
                _listenSocket.Listen(listener.MaxConcurrencyCount);


                //设置连接参数池
                _tcpDuplexAcceptContextPool = new Pool<TcpDuplexAcceptContext>($"TcpDuplexListener-{listener.Name}-AcceptContexts",
                 null,
                 null,
                 null,
                 null,
                async () =>
                {
                    var connection = new TcpDuplexListenerConnection(Guid.NewGuid(),await tcpDataExecuteFactory.Create().GetInitExtensionInfo());
                    

                     var context = new TcpDuplexAcceptContext(connection.ID)
                    {
                        ListenSocket = _listenSocket,
                        Connection = connection                  
                    };

                    var acceptEventArg = new SocketAsyncEventArgs()
                    {
                        UserToken = context
                    };
                    acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                    context.SocketAsyncEventArgs = acceptEventArg;

                    return await Task.FromResult(context);
                },
                async (item) =>
                {
                    return await Task.FromResult(true);
                },
                null,
                null
                , listener.MaxConcurrencyCount);


                _start = true;



                //启动定时检查Tcp连接
                var t = Task.Run(async () =>
                {
                    while (_start)
                    {
                        try
                        {
                            await ValidateConnection();
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.LogError( LogCategoryName, $"TcpDuplexListener {_listener.Name},ValidateConnection Error,message:{ex.Message},stack:{ex.StackTrace}");
                        }

                        System.Threading.Thread.Sleep(100);
                    }
                });


                await StartAccept(_listenSocket);
            }
        }


        public async Task Dispose(TcpDuplexListener listener)
        {
            _acceptSemaphore.Dispose();
            _tcpDuplexAcceptContextPool.Dispose();
            await Task.CompletedTask;
        }
      

        private async void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {

                case SocketAsyncOperation.Receive:
                    await ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    await ProcessSend(e);
                    break;
                case SocketAsyncOperation.Accept:
                    await ProcessAccept(e);
                    break;
                default:
                    break;
            }


        }


        public async Task Stop(TcpDuplexListener listener)
        {
            _listener = listener;
            if (_start)
            {
                try
                {
                    _listenSocket.Close();
                }
                catch
                {

                }

                _listenSocket = null;

                TcpDuplexAcceptContext itemContext;

                List<TcpDuplexAcceptContext> returnList = new List<TcpDuplexAcceptContext>();



                foreach (var item in _tcpDuplexAcceptContextPool.Items)
                {
                    itemContext = (TcpDuplexAcceptContext)item.Value;

                    lock (itemContext.LockObj)
                    {
                        itemContext.Status = -1;
                    }
                    itemContext.ReleaseReceiveSemaphore();
                    itemContext.ReleaseSendSemaphore();

                }
                _start = false;
                ReleaseAcceptSemaphore();


            }

            await Task.FromResult(0);
        }


        /// <summary>
        /// 检查Tcp连接是否已经超时
        /// </summary>
        /// <returns></returns>
        private async Task ValidateConnection()
        {
            TcpDuplexAcceptContext itemContext;
            DateTime nowUTC = DateTime.UtcNow;
            List<TcpDuplexAcceptContext> returnList = new List<TcpDuplexAcceptContext>();
            foreach (var item in _tcpDuplexAcceptContextPool.Items)
            {
                itemContext = (TcpDuplexAcceptContext)item.Value;

                var acceptArgs = itemContext.SocketAsyncEventArgs;
                if (acceptArgs != null)
                {
                    lock (itemContext.LockObj)
                    {
                        if (itemContext.Status == 1 && (nowUTC - itemContext.LatestRunning).TotalSeconds >= 15)
                        {
                            itemContext.Status = -1;
                        }
                    }

                    if (itemContext.Status == -1)
                    {
                        itemContext.ReleaseReceiveSemaphore();
                        itemContext.ReleaseSendSemaphore();
                    }


                }
            }


            await Task.FromResult(0);
        }

        /// <summary>
        /// 开始接受连接
        /// </summary>
        /// <param name="listenSocket"></param>
        /// <param name="acceptEventArg"></param>
        private async Task StartAccept(Socket listenSocket)
        {


            var t = Task.Run(async () =>
            {
                while (_start)
                {
                    TcpDuplexAcceptContext acceptContext = null;

                    try
                    {
                        acceptContext = await _tcpDuplexAcceptContextPool.GetAsync(true);

                        await _acceptSemaphore.WaitAsync();

                        bool willRaiseEvent = listenSocket.AcceptAsync(acceptContext.SocketAsyncEventArgs);
                        if (!willRaiseEvent)
                        {
                            //Logger.WriteLog("AcceptAsync direct", EventLogEntryType.Information);
                            await ProcessAccept(acceptContext.SocketAsyncEventArgs);
                        }
                    }
                    catch (Exception ex)
                    {
                        ReleaseAcceptSemaphore();
                        if (acceptContext != null)
                        {
                            CloseClientSocket(acceptContext.SocketAsyncEventArgs);
                        }
                        await AddLog($"StartAccept Error,message:{ex.Message},stack:{ex.StackTrace}", DateTime.UtcNow);
                        LoggerHelper.LogError(LogCategoryName, $"TcpDuplexListener {_listener.Name},StartAccept Error,message:{ex.Message},stack:{ex.StackTrace}");
                        break;
                    }
                }
            });

            await Task.FromResult(0);
        }

        /// <summary>
        /// 连接处理
        /// </summary>
        /// <param name="e"></param>
        private async Task ProcessAccept(SocketAsyncEventArgs e)
        {
            TcpDuplexAcceptContext token = (TcpDuplexAcceptContext)e.UserToken;

            ReleaseAcceptSemaphore();

            if (e.SocketError == SocketError.Success)
            {
                lock (token.LockObj)
                {
                    token.Status = 1;
                    token.LatestRunning = DateTime.UtcNow;
                }


                while (true)
                {

                    await token.ReceiveSemaphore.WaitAsync();

                    //判断是否已经处于关闭状态
                    bool needClose = false;
                    lock (token.LockObj)
                    {
                        if (token.Status == -1)
                        {
                            needClose = true;
                        }
                    }

                    if (needClose)
                    {
                        CloseClientSocket(e);
                        break;
                    }



                    //设置缓冲区最大字节数
                    var bufferCount = _listener.MaxBufferCount;
                    byte[] buffer = new byte[bufferCount];

                    e.SetBuffer(buffer, 0, bufferCount);

                    try
                    {
                        bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(e);
                        if (!willRaiseEvent)
                        {
                            await ProcessReceive(e);
                        }
                    }
                    catch (Exception ex)
                    {
                        CloseClientSocket(e);
                        await AddLog($"ProcessAccept Error,message:{ex.Message},stack:{ex.StackTrace}", DateTime.UtcNow);
                        LoggerHelper.LogError(LogCategoryName, $"TcpDuplexListener {_listener.Name},ProcessAccept Error,message:{ex.Message},stack:{ex.StackTrace}");
                        break;
                    }
                }




            }
            else
            {
                //await AddLog($"ProcessAccept Error", DateTime.UtcNow);
                //LoggerHelper.LogError(_loggerFactory, LogCategoryName, $"TcpListener {_listener.Name},ProcessAccept Error");
                CloseClientSocket(e);
            }
        }


        /// <summary>
        /// 数据接收处理
        /// </summary>
        /// <param name="e"></param>
        private async Task ProcessReceive(SocketAsyncEventArgs e)
        {

            TcpDuplexAcceptContext token = (TcpDuplexAcceptContext)e.UserToken;





            //从开始接受数据计时,默认执行状态设置为false
            token.ExecuteSuccess = false;
            token.ExecuteError = string.Empty;
            token.RequestDateTime = DateTime.UtcNow;
            token.Watch = new Stopwatch();
            token.Watch.Start();




            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                //判断是否已经处于关闭状态，是，直接关闭，否，将状态改为正在处理
                bool needClose = false;
                lock (token.LockObj)
                {
                    if (token.Status == -1)
                    {
                        needClose = true;
                    }
                    else
                    {
                        token.Status = 2;
                        token.LatestRunning = DateTime.UtcNow;
                    }
                }

                if (needClose)
                {
                    token.ReleaseReceiveSemaphore();
                    token.ReleaseSendSemaphore();
                    return;
                }

                var t = Task.Run(async () =>
                {

                    try
                    {
                        await DoProcessReceive(e);

                    }
                    catch (Exception ex)
                    {
                        token.ExecuteError = $"error message:{ex.Message},stack:{ex.StackTrace}";
                        token.Watch.Stop();
                        lock (token.LockObj)
                        {
                            token.Status = -1;
                        }
                        await AddLog(token);
                        token.ReleaseReceiveSemaphore();
                        token.ReleaseSendSemaphore();
                    }
                }
                );

            }
            else
            {
                lock (token.LockObj)
                {
                    token.Status = -1;
                }
                token.ReleaseReceiveSemaphore();
                token.ReleaseSendSemaphore();
            }

            await Task.FromResult(0);
        }


        private async Task DoProcessReceive(SocketAsyncEventArgs e, bool useSocket = false, byte[] buffer = null)
        {
            TcpDuplexAcceptContext token = (TcpDuplexAcceptContext)e.UserToken;
            var innerResult = await InnerDoProcessReceive(e, useSocket, buffer);

            while (!innerResult.Complete)
            {
                innerResult = await InnerDoProcessReceive(e, true, innerResult.Buffer);
            }



            //如果收到的是空字节，表示客户端已经关闭，服务端也需要关闭
            if (token.RequestData.Count == 0)
            {
                lock (token.LockObj)
                {
                    token.Status = -1;
                }
                token.ReleaseReceiveSemaphore();
                token.ReleaseSendSemaphore();
                return;
            }

            if (!_tcpDuplexDataExecuteFactories.TryGetValue(_listener.ExecuteDataFactoryType, out IFactory<ITcpDuplexDataExecute> tcpDataExecuteFactory))
            {
                lock (_tcpDuplexDataExecuteFactories)
                {
                    Type tcpDataExecuteFactoryType = Type.GetType(_listener.ExecuteDataFactoryType);

                    if (!_tcpDuplexDataExecuteFactories.TryGetValue(_listener.ExecuteDataFactoryType, out tcpDataExecuteFactory))
                    {
                        object objTcpDataExecuteFactory;
                        if (_listener.ExecuteDataFactoryTypeUseDI == true)
                        {
                            //通过DI容器创建
                            objTcpDataExecuteFactory = DIContainerContainer.Get(tcpDataExecuteFactoryType);
                        }
                        else
                        {
                            //通过反射创建
                            objTcpDataExecuteFactory = tcpDataExecuteFactoryType.Assembly.CreateInstance(tcpDataExecuteFactoryType.FullName);
                        }

                        if (!(objTcpDataExecuteFactory is IFactory<ITcpDataExecute>))
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.TcpDuplexDataExecuteTypeError,
                                DefaultFormatting = "双工Tcp监听{0}中，数据处理的工厂类型{1}未实现接口IFactory<ITcpDuplexDataExecute>",
                                ReplaceParameters = new List<object>() { _listener.Name, _listener.ExecuteDataFactoryType }
                            };

                            throw new UtilityException((int)Errors.TcpDuplexDataExecuteTypeError, fragment);
                        }

                        tcpDataExecuteFactory = (IFactory<ITcpDuplexDataExecute>)objTcpDataExecuteFactory;
                        _tcpDuplexDataExecuteFactories.Add(_listener.ExecuteDataFactoryType, tcpDataExecuteFactory);
                    }
                }
            }

            var tcpDataExecute = tcpDataExecuteFactory.Create();

            var byteResult = await tcpDataExecute.Execute(token.RequestData.ToArray(),token.Connection, GetConnections());

            //标识处理状态为成功，并停止计时
            token.ExecuteSuccess = true;
            token.Watch.Stop();

            await AddLog(token);

            //更新状态，解除接收锁
            token.LatestRunning = DateTime.UtcNow;
            token.RequestData.Clear();
            token.RequestData.AddRange(byteResult.RestRequestData);
            lock (token.LockObj)
            {
                token.Status = 1;
            }
            token.ReleaseReceiveSemaphore();



            //向要发送的连接发送返回信息
            if (byteResult.ResponseData != null && byteResult.ResponseData.Length > 0)
            {
                List<TcpDuplexAcceptContext> sendContexts =null;
                if (byteResult.ResponseAll)
                {
                    sendContexts= GetAllAcceptContext();
                }
                else
                {
                    sendContexts = GetAcceptContext(byteResult.ResponseConnections);
                }

                if (sendContexts!=null)
                {
                    foreach(var itemSendContext in sendContexts)
                    {
                        await itemSendContext.ReceiveSemaphore.WaitAsync();

                        itemSendContext.SocketAsyncEventArgs.SetBuffer(byteResult.ResponseData, 0, byteResult.ResponseData.Length);

                        try
                        {
                            var willRaiseEvent = itemSendContext.SocketAsyncEventArgs.AcceptSocket.SendAsync(itemSendContext.SocketAsyncEventArgs);
                            if (!willRaiseEvent)
                            {
                                await ProcessSend(itemSendContext.SocketAsyncEventArgs);
                            }
                        }
                        catch (Exception ex)
                        {
                            CloseClientSocket(itemSendContext.SocketAsyncEventArgs);
                            await AddLog($"Send Error,message:{ex.Message},stack:{ex.StackTrace}", DateTime.UtcNow);
                            LoggerHelper.LogError( LogCategoryName, $"TcpDuplexListener {_listener.Name},Send Error,message:{ex.Message},stack:{ex.StackTrace}");
                        }

                    }
                }
            }

        }



        private async Task<InnerDoProcessReceiveResult> InnerDoProcessReceive(SocketAsyncEventArgs e, bool useSocket = false, byte[] buffer = null)
        {
            InnerDoProcessReceiveResult result = new InnerDoProcessReceiveResult();

            TcpDuplexAcceptContext token = (TcpDuplexAcceptContext)e.UserToken;

            if (!useSocket)
            {
                //将数据加入到接收上下文中
                token.RequestData.AddRange(e.Buffer.Take(e.BytesTransferred));
            }
            else
            {

                var bufferLength = e.AcceptSocket.Receive(buffer);

                token.RequestData.AddRange(buffer.Take(bufferLength));
            }


            //如果有尚未接收完成的数据，继续接收


            if (e.AcceptSocket.Available > 0)
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


        //数据发送处理
        private async Task ProcessSend(SocketAsyncEventArgs e)
        {

            TcpDuplexAcceptContext token = (TcpDuplexAcceptContext)e.UserToken;

            if (e.SocketError == SocketError.Success)
            {
                token.LatestRunning = DateTime.UtcNow;
                lock (token.LockObj)
                {
                    token.Status = 1;
                }

                token.ReleaseSendSemaphore();
            }
            else
            {
                lock (token.LockObj)
                {
                    token.Status = -1;
                }
                token.ReleaseReceiveSemaphore();
                token.ReleaseSendSemaphore();
            }
            
            await Task.FromResult(0);

        }


        private List<TcpDuplexListenerConnection> GetConnections()
        {
            List<TcpDuplexListenerConnection> connections = new List<TcpDuplexListenerConnection>();

            foreach (var item in _tcpDuplexAcceptContextPool.Items)
            {
                var itemContext = (TcpDuplexAcceptContext)item.Value;

                var acceptArgs = itemContext.SocketAsyncEventArgs;
                if (acceptArgs != null)
                {
                    if (itemContext.Status == 1 || itemContext.Status == 2)
                    {
                        connections.Add(itemContext.Connection);
                    }
                }
            }

            return connections;

        }


        private List<TcpDuplexAcceptContext> GetAllAcceptContext()
        {
            List<TcpDuplexAcceptContext> contexts = new List<TcpDuplexAcceptContext>();

            foreach (var item in _tcpDuplexAcceptContextPool.Items)
            {
                var itemContext = (TcpDuplexAcceptContext)item.Value;

                var acceptArgs = itemContext.SocketAsyncEventArgs;
                if (acceptArgs != null)
                {
                    if (itemContext.Status == 1 || itemContext.Status == 2)
                    {
                        contexts.Add(itemContext);
                    }
                }
            }

            return contexts;
        }



        private List<TcpDuplexAcceptContext> GetAcceptContext(List<Guid> ids)
        {
            List<TcpDuplexAcceptContext> contexts = new List<TcpDuplexAcceptContext>();

            foreach (var item in _tcpDuplexAcceptContextPool.Items)
            {
                
                var itemContext = (TcpDuplexAcceptContext)item.Value;

                if (ids.Contains(itemContext.ID))
                {

                    var acceptArgs = itemContext.SocketAsyncEventArgs;
                    if (acceptArgs != null)
                    {
                        if (itemContext.Status == 1 || itemContext.Status == 2)
                        {
                            contexts.Add(itemContext);
                        }
                    }
                }
            }

            return contexts;
        }




        private void ReleaseAcceptSemaphore()
        {
            lock (_lockAcceptSemaphoreObj)
            {
                if (_acceptSemaphore.CurrentCount == 0)
                {
                    try
                    {
                        _acceptSemaphore.Release();
                    }
                    catch (SemaphoreFullException)
                    {

                    }
                }
            }
        }


        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            TcpDuplexAcceptContext token = (TcpDuplexAcceptContext)e.UserToken;

            lock (token)
            {

                Socket currentSocket = e.AcceptSocket;

                try
                {
                    if (currentSocket != null)
                    {
                        currentSocket.Close();
                    }
                }
                catch
                {

                }

                //重新设置连接上下文
                token.SocketAsyncEventArgs = null;
                token.Status = 0;
                token.RequestData.Clear();
                token.ReleaseReceiveSemaphore();
                token.ReleaseSendSemaphore();
                _tcpDuplexAcceptContextPool.Return(token);
            }

        }

        private async Task AddLog(TcpDuplexAcceptContext token)
        {
            TcpDuplexListenerLog log = new TcpDuplexListenerLog()
            {
                IsError = !token.ExecuteSuccess,
                ErrorMessage = token.ExecuteError ?? string.Empty,
                ExecuteDuration = (int)token.Watch.ElapsedMilliseconds,
                ListenerName = _listener.Name,
                RequestContent = UTF8Encoding.UTF8.GetString(token.RequestData.ToArray()),
                RequestTime = token.RequestDateTime,
                ResponseContent = token.ResponseBytes == null ? string.Empty : UTF8Encoding.UTF8.GetString(token.ResponseBytes.ToArray()),
                ResponseTime = DateTime.UtcNow
            };
            var t = Task.Run(async () =>
            {
                try
                {
                    await log.Add();
                }
                catch (Exception ex)
                {
                    LoggerHelper.LogError(LogCategoryName, $"TcpDuplexListener {_listener.Name}, AddLog Error,message:{ex.Message},stack:{ex.StackTrace}");
                }
            });

            await Task.FromResult(0);
        }

        private async Task AddLog(string errorMessage, DateTime occurTime)
        {
            TcpDuplexListenerLog log = new TcpDuplexListenerLog()
            {
                IsError = true,
                ErrorMessage = errorMessage,
                ExecuteDuration = 0,
                ListenerName = _listener.Name,
                RequestContent = string.Empty,
                RequestTime = occurTime,
                ResponseContent = string.Empty,
                ResponseTime = DateTime.UtcNow
            };
            var t = Task.Run(async () =>
            {
                try
                {
                    await log.Add();
                }
                catch (Exception ex)
                {
                    LoggerHelper.LogError(LogCategoryName, $"TcpDuplexListener {_listener.Name}, AddLog Error,message:{ex.Message},stack:{ex.StackTrace}");
                }
            });

            await Task.FromResult(0);
        }



        class InnerDoProcessReceiveResult
        {
            public bool Complete { get; set; }
            public byte[] Buffer { get; set; }
        }
    }



    /// <summary>
    /// Tcp监听器管理的客户端连接（双工）
    /// </summary>
    public class TcpDuplexListenerConnection
    {
        private Guid _id;
        private object _extensionInfo;

        public TcpDuplexListenerConnection(Guid id,object extensionInfo)
        {
            _id = id;
            _extensionInfo = extensionInfo;
        }

        /// <summary>
        /// 客户端连接的唯一ID
        /// </summary>
        public Guid ID
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// 自定义附加信息
        /// </summary>
        public object ExtensionInfo
        {
            get
            {
                return _extensionInfo;
            }
        }
    }

    /// <summary>
    /// Tcp数据处理接口（双工）
    /// </summary>
    public interface ITcpDuplexDataExecute
    {
        /// <summary>
        /// 获取初始化扩展信息
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitExtensionInfo();
        /// <summary>
        /// 执行处理
        /// </summary>
        /// <param name="requestData">请求数据</param>
        /// <param name="requestConnection">发起请求的连接</param>
        /// <param name="connections">当前管理的所有连接</param>
        /// <returns></returns>
        Task<TcpDuplexDataExecuteResult> Execute(byte[] requestData, TcpDuplexListenerConnection requestConnection, List<TcpDuplexListenerConnection> connections);
    }

    /// <summary>
    /// Tcp数据处理结果
    /// </summary>
    public class TcpDuplexDataExecuteResult
    {
        /// <summary>
        /// 响应数据
        /// </summary>
        public byte[] ResponseData
        {
            get;set;
        }

        /// <summary>
        /// 剩余的请求数据
        /// </summary>
        public byte[] RestRequestData
        {
            get; set;
        }

        /// <summary>
        /// 是否响应所有连接
        /// </summary>
        public bool ResponseAll
        {
            get;set;
        }

        /// <summary>
        /// 要响应的连接Id列表
        /// </summary>
        public List<Guid> ResponseConnections
        {
            get;set;
        }
    }



    /// <summary>
    /// 连接接收上下文
    /// </summary>
    public class TcpDuplexAcceptContext
    {
        private Guid _id;
        private object _lockObj = new object();
        private object _lockReceiveSemaphoreObj = new object();
        private object _lockSendSemaphoreObj = new object();
        private int _status = 0;
        private List<byte> _requestData = new List<byte>();
        
        private DateTime _latestRunning = DateTime.UtcNow;
        private TcpDuplexListenerConnection _connection;

        private SemaphoreSlim _receiveSemaphore;
        private SemaphoreSlim _sendSemaphore;


        public Socket ListenSocket { get; set; }



        public TcpDuplexAcceptContext(Guid id)
        {
            _id = id;
            _receiveSemaphore = new SemaphoreSlim(1, 1);
            _sendSemaphore = new SemaphoreSlim(1,1);
        }

        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public TcpDuplexListenerConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                 _connection=value;
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
        /// 用于锁定
        /// </summary>
        public object LockObj
        {
            get
            {
                return _lockObj;
            }
        }

        /// <summary>
        /// 状态
        /// 0：初始化，-1：已经关闭，1：已连接
        /// </summary>
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }


        public List<byte> RequestData
        {
            get
            {
                return _requestData;
            }
        }



        /// <summary>
        /// 最后处理时间
        /// </summary>
        public DateTime LatestRunning
        {
            get
            {
                return _latestRunning;
            }
            set
            {
                _latestRunning = value;
            }
        }







 

        public List<byte> ResponseBytes { get; set; }

        public bool ExecuteSuccess { get; set; }

        public Stopwatch Watch { get; set; }

        public DateTime RequestDateTime { get; set; }

        public string ExecuteError { get; set; }






        public SocketAsyncEventArgs SocketAsyncEventArgs
        {
            get; set;
        }


        public void ReleaseReceiveSemaphore()
        {
            lock (_lockReceiveSemaphoreObj)
            {
                if (_receiveSemaphore.CurrentCount == 0)
                {
                    try
                    {
                        _receiveSemaphore.Release();
                    }
                    catch (SemaphoreFullException)
                    {

                    }
                }
            }
        }


        public void ReleaseSendSemaphore()
        {
            lock (_lockSendSemaphoreObj)
            {
                if (_sendSemaphore.CurrentCount == 0)
                {
                    try
                    {
                        _sendSemaphore.Release();
                    }
                    catch (SemaphoreFullException)
                    {

                    }
                }
            }
        }
    }

}




