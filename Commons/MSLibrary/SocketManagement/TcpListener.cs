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
    /// Tcp监听器（应答式）
    /// 用于监听处理Tcp连接
    /// </summary>
    public class TcpListener : EntityBase<ITcpListenerIMP>
    {
        private static IFactory<ITcpListenerIMP> _tcpListenerIMPFactory;
        
        public static IFactory<ITcpListenerIMP> TcpListenerIMPFactory
        {
            set
            {
                _tcpListenerIMPFactory = value;
            }
        }


        public override IFactory<ITcpListenerIMP> GetIMPFactory()
        {
            return _tcpListenerIMPFactory;
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
        /// 是否是长连接
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
        /// 该类型必须实现IFactory<ISocketDataExecute>接口
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
        /// 接收的心跳数据
        /// 只有长连接才有意义
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
        /// 启动监听
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            await _imp.Start(this);
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        /// <returns></returns>
        public async Task Stop()
        {
            await _imp.Stop(this);
        }
    }


    public interface ITcpListenerIMP
    {
        Task Add(TcpListener listener);

        Task Update(TcpListener listener);

        Task Delete(TcpListener listener);
        Task Start(TcpListener listener);

        Task Stop(TcpListener listener);
    }

    [Injection(InterfaceType = typeof(ITcpListenerIMP), Scope = InjectionScope.Transient)]
    public class TcpListenerIMP : ITcpListenerIMP
    {
        private static Dictionary<string, IFactory<ITcpDataExecute>> _tcpDataExecuteFactories = new Dictionary<string, IFactory<ITcpDataExecute>>();

        public static Dictionary<string, IFactory<ITcpDataExecute>> TcpDataExecuteFactories
        {
            get
            {
                return _tcpDataExecuteFactories;
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
        private Pool<TcpAcceptContext> _tcpAcceptContextPool;


        private TcpListener _listener;

        private ITcpListenerStore _tcpListenerStore;
        private ILoggerFactory _loggerFactory;
        
        public static string LogCategoryName
        {
            get; set;
        }

        public TcpListenerIMP(ITcpListenerStore tcpListenerStore, ILoggerFactory loggerFactory)
        {
            _tcpListenerStore = tcpListenerStore;
            _loggerFactory = loggerFactory;
            _acceptSemaphore = new SemaphoreSlim(1,1);
        }
        public async Task Add(TcpListener listener)
        {
            await _tcpListenerStore.Add(listener);
        }

        public async Task Delete(TcpListener listener)
        {
            await _tcpListenerStore.Delete(listener.ID);
        }

        public async Task Start(TcpListener listener)
        {
            _listener = listener;
            if (!_start)
            {

                //设置监听Socket
                IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, listener.Port);
                _listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                _listenSocket.Bind(localPoint);
                _listenSocket.Listen(listener.MaxConcurrencyCount);


                //设置连接参数池
                _tcpAcceptContextPool = new Pool<TcpAcceptContext>($"TcpListener-{listener.Name}-AcceptContexts", 
                 null,
                 null,
                 null,
                 null,
                async () =>
                {
                    var context = new TcpAcceptContext()
                    {
                        ListenSocket = _listenSocket
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



                if (_listener.KeepAlive)
                {
                    //启动定时检查Tcp连接
                    var t=Task.Run(async () =>
                    {
                        while (_start)
                        {
                            try
                            {
                                await ValidateConnection();
                            }
                            catch (Exception ex)
                            {
                                LoggerHelper.LogError( LogCategoryName, $"TcpListener {_listener.Name},ValidateConnection Error,message:{ex.Message},stack:{ex.StackTrace}");
                            }

                            System.Threading.Thread.Sleep(100);
                        }
                    });
                }

                await StartAccept(_listenSocket);
            }
        }

        public async Task Stop(TcpListener listener)
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

                TcpAcceptContext itemContext;

                List<TcpAcceptContext> returnList = new List<TcpAcceptContext>();



                foreach (var item in _tcpAcceptContextPool.Items)
                {
                    itemContext = (TcpAcceptContext)item.Value;

                    lock (itemContext.LockObj)
                    {
                        itemContext.Status = -1;
                    }
                    itemContext.ReleaseReceiveSemaphore();

                }
                _start = false;
                ReleaseAcceptSemaphore();


            }

            await Task.FromResult(0);
        }

        public async Task Update(TcpListener listener)
        {
            await _tcpListenerStore.Update(listener);
        }

        /// <summary>
        /// 检查Tcp连接是否已经超时
        /// </summary>
        /// <returns></returns>
        private async Task ValidateConnection()
        {
            TcpAcceptContext itemContext;
            DateTime nowUTC = DateTime.UtcNow;
            List<TcpAcceptContext> returnList = new List<TcpAcceptContext>();
            foreach(var item in _tcpAcceptContextPool.Items)
            {
                itemContext = (TcpAcceptContext)item.Value;

                var acceptArgs = itemContext.SocketAsyncEventArgs;
                if (acceptArgs!=null)
                {
                    lock(itemContext.LockObj)
                    {
                        if (itemContext.Status==1 && (nowUTC- itemContext.LatestRunning).TotalSeconds>=15)
                        {
                            itemContext.Status = -1;
                        }
                    }

                    if (itemContext.Status==-1)
                    {
                        itemContext.ReleaseReceiveSemaphore();
                    }


                }
            }


            await Task.FromResult(0);
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


        /// <summary>
        /// 开始接受连接
        /// </summary>
        /// <param name="listenSocket"></param>
        /// <param name="acceptEventArg"></param>
        private async Task StartAccept(Socket listenSocket)
        {
            

            var t=Task.Run(async () =>
            {
                while (_start)
                {
                    var acceptContext = await _tcpAcceptContextPool.GetAsync(true);

                    await _acceptSemaphore.WaitAsync();

                    try
                    {

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
                        CloseClientSocket(acceptContext.SocketAsyncEventArgs);
                        await AddLog($"StartAccept Error,message:{ex.Message},stack:{ex.StackTrace}", DateTime.UtcNow);
                        LoggerHelper.LogError( LogCategoryName, $"TcpListener {_listener.Name},StartAccept Error,message:{ex.Message},stack:{ex.StackTrace}");
                        break;
                    }
                }
            });

            await Task.FromResult(0);
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


        /// <summary>
        /// 连接处理
        /// </summary>
        /// <param name="e"></param>
        private async Task ProcessAccept(SocketAsyncEventArgs e)
        {
            TcpAcceptContext token = (TcpAcceptContext)e.UserToken;

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

                    //重新初始化接收参数
                    token.RetrieveBytes = new List<byte>();

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
                        LoggerHelper.LogError(LogCategoryName, $"TcpListener {_listener.Name},ProcessAccept Error,message:{ex.Message},stack:{ex.StackTrace}");
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

            TcpAcceptContext token = (TcpAcceptContext)e.UserToken;





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
                lock(token.LockObj)
                {
                    if (token.Status==-1)
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
                    return;
                }

                var t=Task.Run(async () =>
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
            }

            await Task.FromResult(0);
        }


        private async Task DoProcessReceive(SocketAsyncEventArgs e, bool useSocket = false, byte[] buffer = null)
        {
            TcpAcceptContext token = (TcpAcceptContext)e.UserToken;
            var innerResult = await InnerDoProcessReceive(e, useSocket, buffer);

            while (!innerResult.Complete)
            {
                innerResult = await InnerDoProcessReceive(e, true, innerResult.Buffer);
            }

            bool willRaiseEvent;
            //处理接收到的数据
            var strData = UTF8Encoding.UTF8.GetString(token.RetrieveBytes.ToArray());


            //创建接收处理上下文

            //检查是否是心跳包
            //心跳包的格式固定为字符串“HeartBeat”
            if (strData.ToLower() == _listener.HeartBeatSendData.ToLower())
            {

                //标识处理状态为成功，并停止计时               
                token.ExecuteSuccess = true;
                token.Watch.Stop();
                await AddLog(token);


                token.LatestRunning = DateTime.UtcNow;

                var responseBytes = UTF8Encoding.UTF8.GetBytes(_emptyResponse);

                e.SetBuffer(responseBytes,0,responseBytes.Length);

                willRaiseEvent = e.AcceptSocket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    await ProcessSend(e);
                }

                return;
            }

            //如果收到的是空字节，表示客户端已经关闭，服务端也需要关闭
            if (token.RetrieveBytes.Count == 0)
            {
                lock (token.LockObj)
                {
                    token.Status = -1;
                }
                token.ReleaseReceiveSemaphore();
                return;
            }

            if (!_tcpDataExecuteFactories.TryGetValue(_listener.ExecuteDataFactoryType, out IFactory<ITcpDataExecute> tcpDataExecuteFactory))
            {
                lock (_tcpDataExecuteFactories)
                {
                    Type tcpDataExecuteFactoryType = Type.GetType(_listener.ExecuteDataFactoryType);

                    if (!_tcpDataExecuteFactories.TryGetValue(_listener.ExecuteDataFactoryType, out tcpDataExecuteFactory))
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
                                Code = TextCodes.TcpDataExecuteTypeError,
                                DefaultFormatting = "Tcp监听{0}中，数据处理的工厂类型{1}未实现接口IFactory<ITcpDataExecute>",
                                ReplaceParameters = new List<object>() { _listener.Name, _listener.ExecuteDataFactoryType }
                            };

                            throw new UtilityException((int)Errors.TcpDataExecuteTypeError, fragment);
                        }

                        tcpDataExecuteFactory = (IFactory<ITcpDataExecute>)objTcpDataExecuteFactory;
                        _tcpDataExecuteFactories.Add(_listener.ExecuteDataFactoryType, tcpDataExecuteFactory);
                    }
                }
            }

            var tcpDataExecute = tcpDataExecuteFactory.Create();

            var byteResult = await tcpDataExecute.Execute(token.RetrieveBytes.ToArray());

            //标识处理状态为成功，并停止计时
            token.ExecuteSuccess = true;
            token.Watch.Stop();

            if (byteResult != null)
            {
                token.ResponseBytes = byteResult.ToList();
            }

            await AddLog(token);

            //发送返回值

            if (byteResult != null)
            {
                /*var bytes = UTF8Encoding.UTF8.GetBytes(strResult);

                var totalByteList = bytes.Length.ToBytes().ToList();

                totalByteList.AddRange(bytes);

                var totalBytes = totalByteList.ToArray();*/

                e.SetBuffer(byteResult, 0, byteResult.Length);



            }
            else
            {
                var responseBytes = UTF8Encoding.UTF8.GetBytes(_emptyResponse);
                e.SetBuffer(responseBytes,0,responseBytes.Length);
            }


            willRaiseEvent = e.AcceptSocket.SendAsync(e);
            if (!willRaiseEvent)
            {
                await ProcessSend(e);
            }

        }



        private async Task<InnerDoProcessReceiveResult> InnerDoProcessReceive(SocketAsyncEventArgs e, bool useSocket = false, byte[] buffer = null)
        {
            InnerDoProcessReceiveResult result = new InnerDoProcessReceiveResult();

            TcpAcceptContext token = (TcpAcceptContext)e.UserToken;

            if (!useSocket)
            {
                //将数据加入到接收上下文中
                token.RetrieveBytes.AddRange(e.Buffer.Take(e.BytesTransferred));
            }
            else
            {

                var bufferLength = e.AcceptSocket.Receive(buffer);

                token.RetrieveBytes.AddRange(buffer.Take(bufferLength));
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

            TcpAcceptContext token = (TcpAcceptContext)e.UserToken;

            if (e.SocketError == SocketError.Success)
            {
                if (!_listener.KeepAlive)
                {
                    lock (token.LockObj)
                    {
                        token.Status = -1;
                    }
                }
                else
                {

                    token.LatestRunning = DateTime.UtcNow;
                    lock (token.LockObj)
                    {
                        token.Status = 1;
                    }
                }
            }
            else
            {
                lock (token.LockObj)
                {
                    token.Status = -1;
                }
            }
            token.ReleaseReceiveSemaphore();


            await Task.FromResult(0);

        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            TcpAcceptContext token = (TcpAcceptContext)e.UserToken;

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
                token.ReleaseReceiveSemaphore();
                _tcpAcceptContextPool.Return(token);
            }

        }

        private async Task AddLog(TcpAcceptContext token)
        {
            TcpListenerLog log = new TcpListenerLog()
            {
                IsError = !token.ExecuteSuccess,
                ErrorMessage = token.ExecuteError?? string.Empty,
                ExecuteDuration = (int)token.Watch.ElapsedMilliseconds,
                ListenerName = _listener.Name,
                RequestContent = UTF8Encoding.UTF8.GetString(token.RetrieveBytes.ToArray()),
                RequestTime = token.RequestDateTime,
                ResponseContent = token.ResponseBytes == null ? string.Empty : UTF8Encoding.UTF8.GetString(token.ResponseBytes.ToArray()),
                ResponseTime = DateTime.UtcNow
            };
            var t=Task.Run(async() =>
            {
                try
                {
                    await log.Add();
                }
                catch(Exception ex)
                {
                    LoggerHelper.LogError(LogCategoryName, $"TcpListener {_listener.Name}, AddLog Error,message:{ex.Message},stack:{ex.StackTrace}");
                }
            });

            await Task.FromResult(0);
        }

        private async Task AddLog(string errorMessage,DateTime occurTime)
        {
            TcpListenerLog log = new TcpListenerLog()
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
            var t=Task.Run(async()=>
            {
                try
                {
                    await log.Add();
                }
                catch(Exception ex)
                {
                    LoggerHelper.LogError( LogCategoryName, $"TcpListener {_listener.Name}, AddLog Error,message:{ex.Message},stack:{ex.StackTrace}");
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
    /// 接收数据处理服务
    /// </summary>
    public interface ITcpDataExecute
    {
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="data">接收的数据</param>
        /// <returns>要发送回客户端的数据</returns>
        Task<byte[]> Execute(byte[] data);

    }


    /// <summary>
    /// 接收数据上下文
    /// 用于Socket接收完整数据流
    /// </summary>
   /* public class TcpReceieveContext
    {


        public Socket Socket { get; set; }


        public List<byte> Bytes { get; set; }

        public List<byte> ResponseBytes { get; set; }

        public bool Success { get; set; }

        public Stopwatch Watch { get; set; }

        public DateTime DateTime { get; set; }

        public string Error { get; set; }

        /// <summary>
        /// 关联的连接上下文
        /// </summary>
        public TcpAcceptContext AcceptContext { get; set; }

         

    }*/

    /// <summary>
    /// 连接接收上下文
    /// </summary>
    public class TcpAcceptContext
    {

        private object _lockObj = new object();
        private object _lockReceiveSemaphoreObj = new object();
        private int _status = 0;
        private DateTime _latestRunning = DateTime.UtcNow;


        private SemaphoreSlim _receiveSemaphore;


        public Socket ListenSocket { get; set; }


        
        public TcpAcceptContext()
        {
            _receiveSemaphore = new SemaphoreSlim(1, 1);
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







        public List<byte> RetrieveBytes { get; set; }

        public List<byte> ResponseBytes { get; set; }

        public bool ExecuteSuccess { get; set; }

        public Stopwatch Watch { get; set; }

        public DateTime RequestDateTime { get; set; }

        public string ExecuteError { get; set; }






        public SocketAsyncEventArgs SocketAsyncEventArgs
        {
            get;set;
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
    }



}
