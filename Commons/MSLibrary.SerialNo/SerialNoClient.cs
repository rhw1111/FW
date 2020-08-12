using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MongoDB.Libmongocrypt;

namespace MSLibrary.SerialNo
{
    public class SerialNoClient : EntityBase<ISerialNoClientIMP>
    {
        public override IFactory<ISerialNoClientIMP>? GetIMPFactory()
        {
            throw new NotImplementedException();
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
        /// 配置名称
        /// 对应SerialNoConfiguration中的Name
        /// </summary>
        public string ConfigurationName
        {
            get
            {

                return GetAttribute<string>(nameof(ConfigurationName));
            }
            set
            {
                SetAttribute<string>(nameof(ConfigurationName), value);
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
    }

    public interface ISerialNoClientIMP
    {
        Task<int> GetSerialNo(SerialNoClient client, string prefix, CancellationToken cancellationToken = default);
    }


    [Injection(InterfaceType = typeof(ISerialNoClientIMP), Scope = InjectionScope.Transient)]
    public class SerialNoClientIMP : ISerialNoClientIMP
    {
        private static ConcurrentDictionary<string, serialNoStepValueContainer> _stepValueBuffer = new ConcurrentDictionary<string, serialNoStepValueContainer>();
        private static SemaphoreSlim _containerLock = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim _containerCheckLock = new SemaphoreSlim(1, 1);
        private static int _containerCheckStatus = 0;

        private readonly IClientGetSerialNoStepValueService _clientGetSerialNoStepValueService;
        private readonly IClinetSerialNoPrefixCheckService _clinetSerialNoPrefixCheckService;

        public SerialNoClientIMP(IClientGetSerialNoStepValueService clientGetSerialNoStepValueService, IClinetSerialNoPrefixCheckService clinetSerialNoPrefixCheckService)
        {
            _clientGetSerialNoStepValueService = clientGetSerialNoStepValueService;
            _clinetSerialNoPrefixCheckService = clinetSerialNoPrefixCheckService;
        }
        public async Task<int> GetSerialNo(SerialNoClient client, string prefix, CancellationToken cancellationToken = default)
        {
            if (!_stepValueBuffer.TryGetValue(prefix,out serialNoStepValueContainer valueContainer))
            {
                await _containerLock.WaitAsync();
                try
                {
                    if (!_stepValueBuffer.TryGetValue(prefix, out valueContainer))
                    {
                        valueContainer = new serialNoStepValueContainer();                    
                        valueContainer.First = new serialNoStepValue(prefix, client.ConfigurationName, _clientGetSerialNoStepValueService);
                        valueContainer.Second = new serialNoStepValue(prefix, client.ConfigurationName, _clientGetSerialNoStepValueService);
                        await valueContainer.First.LoadData(cancellationToken);
                        await valueContainer.Second.LoadData(cancellationToken);
                        _stepValueBuffer[prefix] = valueContainer;
                    }
                }
                finally
                {
                    _containerLock.Release();
                }
            }

            int? serialNo = null;
            while (true)
            {
                serialNo = null;

                await valueContainer.LockObj.WaitAsync();
                try
                {
                    switch(valueContainer.First.LoadStatus)
                    {
                        case 0:
                            valueContainer.First = valueContainer.Second;
                            valueContainer.Second = new serialNoStepValue(prefix, client.ConfigurationName, _clientGetSerialNoStepValueService);
                            valueContainer.Second.LoadDataBackround(cancellationToken);
                            break;
                        case 1:
                            serialNo = valueContainer.First.GetSerialNo();
                            if (serialNo == null)
                            {
                                valueContainer.First = valueContainer.Second;
                                valueContainer.Second = new serialNoStepValue(prefix, client.ConfigurationName, _clientGetSerialNoStepValueService);
                                valueContainer.Second.LoadDataBackround(cancellationToken);
                            }
                            break;
                        case 2:
                            valueContainer.First.LoadStatus = 0;
                            throw valueContainer.First.LoadError!;
                        default:
                            await Task.Delay(50);
                            break;
                    }

                    if (serialNo != null)
                    {
                        checkContainer(client);
                        break;
                    }
                }
                finally
                {
                    valueContainer.LockObj.Release();
                }

            }


            return serialNo.Value;
        }


        private void checkContainer(SerialNoClient client)
        {
            if (_containerCheckStatus == 1)
            {
                return; 
            }

            Task.Run(async () =>
            {
                var waitResult=await _containerCheckLock.WaitAsync(200);
                if (!waitResult)
                {
                    return;
                }
                try
                {
                    _containerCheckStatus = 1;
                    List<string> deleteKeys = new List<string>();
                    foreach(var item in _stepValueBuffer)
                    {
                        var checkResult=await _clinetSerialNoPrefixCheckService.Check(client.ConfigurationName, item.Key);
                        if (!checkResult)
                        {
                            deleteKeys.Add(item.Key);
                        }
                    }

                    foreach(var item in deleteKeys)
                    {
                        _stepValueBuffer.Remove(item, out serialNoStepValueContainer container);
                        container.Dispose();
                    }
                }
                finally
                {
                    _containerCheckStatus = 0;
                    _containerCheckLock.Release();
                }
            });
        }


        private class serialNoStepValue
        {
            private string _prefix;
            private string _configurationName;
            private int _loadStatus = 0;
            private Exception? _loadError=null;

            private IClientGetSerialNoStepValueService _service;

            private object _lockObj = new object();

            public serialNoStepValue(string prefix, string configurationName, IClientGetSerialNoStepValueService service)
            {
                _prefix = prefix;
                _service = service;
                _configurationName = configurationName;
            }

            /// <summary>
            /// 加载状态
            /// 0：初始状态，未加载
            /// 1：加载完成
            /// 2：加载出错
            /// 3：正在加载
            /// </summary>
            public int LoadStatus
            {
                get
                {
                    return _loadStatus;
                }
                set
                {
                    _loadStatus = value;
                }
            }

            public Exception? LoadError
            {
                get
                {
                    return _loadError;
                }
            }
            public int Current { get; set; }
            public int End { get; set; }

            public int? GetSerialNo()
            {
                int? result = null;
                lock(_lockObj)
                {
                    if (Current<End)
                    {
                        Current++;
                        result = Current;
                    }
                }

                return result;
            }

            public bool IsUsed()
            {
                if (Current>= End)
                {
                    return true;
                }

                return false;
            }

            public async Task LoadData(CancellationToken cancellationToken = default)
            {
                try
                {
                    var stepValue = await _service.Get(_configurationName, _prefix, cancellationToken);
                    Current = stepValue.Start;
                    End = stepValue.End;
                    _loadStatus = 1;
                }
                catch(Exception ex)
                {
                    _loadStatus = 2;
                    _loadError = ex;
                }
                
            }
           
            public void LoadDataBackround(CancellationToken cancellationToken = default)
            {
                _loadStatus = 3;
                Task.Run(async () =>
                {
                    try
                    {
                        var stepValue = await _service.Get(_configurationName, _prefix, cancellationToken);
                        Current = stepValue.Start;
                        End = stepValue.End;
                        _loadStatus = 1;
                    }
                    catch (Exception ex)
                    {
                        _loadStatus = 2;
                        _loadError = ex;
                    }
                });
            }

        }

        private class serialNoStepValueContainer:IDisposable
        {
            public SemaphoreSlim LockObj { get;} = new SemaphoreSlim(1, 1);

            public serialNoStepValue First { get; set; } = null!;
            public serialNoStepValue Second { get; set; } = null!;

            public void Dispose()
            {
                LockObj.Dispose();
            }
        }
    }

    
}
