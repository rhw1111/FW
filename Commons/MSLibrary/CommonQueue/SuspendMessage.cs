using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.CommonQueue
{
    /// <summary>
    /// 搁置消息
    /// 用来记录暂时搁置未处理的消息
    /// </summary>
    public class SuspendMessage : EntityBase<ISuspendMessageIMP>
    {
        private static IFactory<ISuspendMessageIMP> _suspendMessageIMPFactory;

        public static IFactory<ISuspendMessageIMP> SuspendMessageIMPFactory
        {
            set 
            {
                _suspendMessageIMPFactory = value;
            }
        }

        public override IFactory<ISuspendMessageIMP> GetIMPFactory()
        {
            return _suspendMessageIMPFactory;
        }

        /// <summary>
        /// 消息ID
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
        /// 消息Key
        /// </summary>
        public string Key
        {
            get
            {
                return GetAttribute<string>(nameof(Key));
            }
            set
            {
                SetAttribute<string>(nameof(Key), value);
            }
        }


        /// <summary>
        /// 搁置时间
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

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }

    }

    public interface ISuspendMessageIMP
    {
        Task Add(SuspendMessage message);
        Task Delete(SuspendMessage message);
    }
}
