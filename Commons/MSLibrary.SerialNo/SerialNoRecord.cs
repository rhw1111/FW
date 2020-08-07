using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.SerialNo.DAL;
using MSLibrary.LanguageTranslate;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace MSLibrary.SerialNo
{
    /// <summary>
    /// 序列号记录
    /// </summary>
    public class SerialNoRecord : EntityBase<ISerialNoRecordIMP>
    {
        private static IFactory<ISerialNoRecordIMP>? _serialNoRecordIMPFactory;

        public static IFactory<ISerialNoRecordIMP> SerialNoRecordIMPFactory
        {
            set
            {
                _serialNoRecordIMPFactory = value;
            }
        }

        public override IFactory<ISerialNoRecordIMP>? GetIMPFactory()
        {
            return _serialNoRecordIMPFactory;
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
        /// 前缀
        /// </summary>
        public string Prefix
        {
            get
            {

                return GetAttribute<string>(nameof(Prefix));
            }
            set
            {
                SetAttribute<string>(nameof(Prefix), value);
            }
        }

        /// <summary>
        /// 配置名称
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
        /// 当前值
        /// </summary>
        public long CurrentValue
        {
            get
            {

                return GetAttribute<long>(nameof(CurrentValue));
            }
            set
            {
                SetAttribute<long>(nameof(CurrentValue), value);
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

        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }

        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }

        public async Task<SerialNoRecordStepValue> GetNextValue(int addValue,CancellationToken cancellationToken = default)
        {
            return await _imp.GetNextValue(this, addValue, cancellationToken);
        }
    }

    public interface ISerialNoRecordIMP
    {
        Task Add(SerialNoRecord record, CancellationToken cancellationToken = default);
        Task Delete(SerialNoRecord record, CancellationToken cancellationToken = default);
        Task<SerialNoRecordStepValue> GetNextValue(SerialNoRecord record,int addValue, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(ISerialNoRecordIMP), Scope = InjectionScope.Transient)]
    public class SerialNoRecordIMP : ISerialNoRecordIMP
    {
        private readonly ISerialNoRecordStore _serialNoRecordStore;

        public SerialNoRecordIMP(ISerialNoRecordStore serialNoRecordStore)
        {
            _serialNoRecordStore = serialNoRecordStore;
        }

        public async Task Add(SerialNoRecord record, CancellationToken cancellationToken = default)
        {
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {

                await _serialNoRecordStore.Add(record, cancellationToken);

                var existsRecord = await _serialNoRecordStore.QueryFirstNolockByPrefix(record.Prefix,cancellationToken);

                if (existsRecord!=null && existsRecord.ID!= record.ID)
                {

                    var fragment = new TextFragment()
                    {
                        Code = SerialNoTextCodes.ExistSerialNoRecordByPrefix,
                        DefaultFormatting = "已经存在前缀为{0}的流水号记录",
                        ReplaceParameters = new List<object>() { record.Prefix }
                    };

                    throw new UtilityException((int)SerialNoErrorCodes.ExistSerialNoRecordByPrefix, fragment, 1, 0);
                }

                scope.Complete();
            }
        }

        public async Task Delete(SerialNoRecord record, CancellationToken cancellationToken = default)
        {
            await _serialNoRecordStore.Delete(record.ID, cancellationToken);
        }

        public async Task<SerialNoRecordStepValue> GetNextValue(SerialNoRecord record, int addValue, CancellationToken cancellationToken = default)
        {
            var currentValue=await _serialNoRecordStore.UpdateCurrentValue(record.ID, addValue, cancellationToken);
            return new SerialNoRecordStepValue(currentValue - addValue + 1, currentValue);
        }
    }

    public class SerialNoRecordStepValue
    {
        public SerialNoRecordStepValue(int start,int end)
        {
            Start = start;
            End = end;
        }
        public int Start { get; private set; }
        public int End { get; private set; }
    }
}
