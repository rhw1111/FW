using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.SerialNumber.DAL;

namespace MSLibrary.SerialNumber
{
    /// <summary>
    /// 序列号记录
    /// 每个不同的前缀为一条序列号记录
    /// </summary>
    public class SerialNumberRecord : EntityBase<ISerialNumberRecordIMP>
    {
        private static IFactory<ISerialNumberRecordIMP> _serialNumberRecordIMPFactory;

        public static IFactory<ISerialNumberRecordIMP> SerialNumberRecordIMPFactory
        {
            set
            {
                _serialNumberRecordIMPFactory = value;
            }
        }

        public override IFactory<ISerialNumberRecordIMP> GetIMPFactory()
        {
            return _serialNumberRecordIMPFactory;
        }

        /// <summary>
        /// ID
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
        /// 前缀
        /// </summary>
        public string Prefix
        {
            get
            {
                return GetAttribute<string>("Prefix");
            }
            set
            {
                SetAttribute<string>("Prefix", value);
            }
        }


        /// <summary>
        /// 流水值
        /// </summary>
        public long Value
        {
            get
            {
                return GetAttribute<long>("Value");
            }
            set
            {
                SetAttribute<long>("Value", value);
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
        /// 刷新流水值
        /// </summary>
        /// <returns></returns>
        public async Task RefreashValue()
        {
            await _imp.RefreashValue(this);
        }
    }


    public interface ISerialNumberRecordIMP
    {
        Task Add(SerialNumberRecord record);
        Task RefreashValue(SerialNumberRecord record);
    }

    [Injection(InterfaceType = typeof(ISerialNumberRecordIMP), Scope = InjectionScope.Transient)]
    public class SerialNumberRecordIMP : ISerialNumberRecordIMP
    {
        private ISerialNumberRecordStore _serialNumberRecordStore;
        public SerialNumberRecordIMP(ISerialNumberRecordStore serialNumberRecordStore)
        {
            _serialNumberRecordStore = serialNumberRecordStore;
        }
        public async Task Add(SerialNumberRecord record)
        {
            await _serialNumberRecordStore.Add(record);
        }

        public async Task RefreashValue(SerialNumberRecord record)
        {
            await _serialNumberRecordStore.UpdateValue(record);
        }
    }
}
