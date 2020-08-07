using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.SerialNo.DAL;
using MSLibrary.Transaction;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.SerialNo
{
    /// <summary>
    /// 流水号配置
    /// </summary>
    public class SerialNoConfiguration : EntityBase<ISerialNoConfigurationIMP>
    {
        private static IFactory<ISerialNoConfigurationIMP>? _serialNoConfigurationIMPFactory;

        public static IFactory<ISerialNoConfigurationIMP> SerialNoConfigurationIMPFactory
        {
            set
            {
                _serialNoConfigurationIMPFactory = value;
            }
        }

        public override IFactory<ISerialNoConfigurationIMP>? GetIMPFactory()
        {
            return _serialNoConfigurationIMPFactory;
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
        /// 起始值
        /// </summary>
        public int StartValue
        {
            get
            {

                return GetAttribute<int>(nameof(StartValue));
            }
            set
            {
                SetAttribute<int>(nameof(StartValue), value);
            }
        }

        /// <summary>
        /// 步长
        /// </summary>
        public int Step
        {
            get
            {

                return GetAttribute<int>(nameof(Step));
            }
            set
            {
                SetAttribute<int>(nameof(Step), value);
            }
        }

        /// <summary>
        /// 是否混淆
        /// </summary>
        public bool Confusion
        {
            get
            {

                return GetAttribute<bool>(nameof(Confusion));
            }
            set
            {
                SetAttribute<bool>(nameof(Confusion), value);
            }
        }

        /// <summary>
        /// 最小混淆值
        /// </summary>
        public int ConfusionMin
        {
            get
            {

                return GetAttribute<int>(nameof(ConfusionMin));
            }
            set
            {
                SetAttribute<int>(nameof(ConfusionMin), value);
            }
        }

        /// <summary>
        /// 最大混淆值
        /// </summary>
        public int ConfusionMax
        {
            get
            {

                return GetAttribute<int>(nameof(ConfusionMax));
            }
            set
            {
                SetAttribute<int>(nameof(ConfusionMax), value);
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

    public interface ISerialNoConfigurationIMP
    {
        Task Add(SerialNoConfiguration configuration, CancellationToken cancellationToken = default);
        Task Update(SerialNoConfiguration configuration, CancellationToken cancellationToken = default);
        Task Delete(SerialNoConfiguration configuration, CancellationToken cancellationToken = default);

        Task<SerialNoRecordStepValue> GetSerialNoStepValue(SerialNoConfiguration configuration,string prefix, CancellationToken cancellationToken = default);
    }


    [Injection(InterfaceType = typeof(ISerialNoConfigurationIMP), Scope = InjectionScope.Transient)]
    public class SerialNoConfigurationIMP : ISerialNoConfigurationIMP
    {
        private readonly ISerialNoRecordRepository _serialNoRecordRepository;
        private readonly ISerialNoConfigurationStore _serialNoConfigurationStore;

        public SerialNoConfigurationIMP(ISerialNoRecordRepository serialNoRecordRepository, ISerialNoConfigurationStore serialNoConfigurationStore)
        {
            _serialNoRecordRepository = serialNoRecordRepository;
            _serialNoConfigurationStore = serialNoConfigurationStore;
        }

        public async Task Add(SerialNoConfiguration configuration, CancellationToken cancellationToken = default)
        {
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {

                await _serialNoConfigurationStore.Add(configuration, cancellationToken);

                var existsConfiguration = await _serialNoConfigurationStore.QueryFirstNolockByName(configuration.Name, cancellationToken);

                if (existsConfiguration != null && existsConfiguration.ID != configuration.ID)
                {

                    var fragment = new TextFragment()
                    {
                        Code = SerialNoTextCodes.ExistSerialNoConfigurationByName,
                        DefaultFormatting = "已经存在名称为{0}的流水号配置",
                        ReplaceParameters = new List<object>() { configuration.Name }
                    };

                    throw new UtilityException((int)SerialNoErrorCodes.ExistSerialNoConfigurationByName, fragment, 1, 0);
                }

                scope.Complete();
            }
        }

        public async Task Delete(SerialNoConfiguration configuration, CancellationToken cancellationToken = default)
        {
            await _serialNoConfigurationStore.Delete(configuration.ID, cancellationToken);
        }

        public async Task<SerialNoRecordStepValue> GetSerialNoStepValue(SerialNoConfiguration configuration, string prefix, CancellationToken cancellationToken = default)
        {
            SerialNoRecord? record = null;
            while(true)
            {
                record = await _serialNoRecordRepository.QueryByPrefix(prefix, cancellationToken);
                if (record == null)
                {
                    record = new SerialNoRecord()
                    {
                        ID = Guid.NewGuid(),
                        Prefix = prefix,
                        ConfigurationName = configuration.Name,
                        CurrentValue = configuration.StartValue,
                    };

                    try
                    {
                        await record.Add(cancellationToken);
                    }
                    catch (UtilityException ex)
                    {
                        if (ex.Code == (int)SerialNoErrorCodes.ExistSerialNoRecordByPrefix)
                        {
                            await Task.Delay(200);
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            int addValue = configuration.Step;
            if (configuration.Confusion)
            {
                Random ran = new Random(DateTime.Now.Millisecond);
                addValue+=ran.Next(configuration.ConfusionMin, configuration.ConfusionMax + 1);
            }

            return await record.GetNextValue(addValue,cancellationToken);
        }

        public async Task Update(SerialNoConfiguration configuration, CancellationToken cancellationToken = default)
        {
            await _serialNoConfigurationStore.Update(configuration, cancellationToken);
        }
    }
}
