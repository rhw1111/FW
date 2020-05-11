using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 队列实体
    /// 负责管理存储消息的队列GroupName+IsDead+Code唯一，GroupName+name唯一
    /// </summary>
    public class SQueue : EntityBase<ISQueueIMP>
    {
        private static IFactory<ISQueueIMP> _sQueueFactory;

        public static IFactory<ISQueueIMP> SQueueFactory
        {
            set
            {
                _sQueueFactory = value;
            }
        }

        public override IFactory<ISQueueIMP> GetIMPFactory()
        {
            return _sQueueFactory;
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
        /// 分组名称
        /// </summary>
        public string GroupName
        {
            get
            {
                return GetAttribute<string>("GroupName");
            }
            set
            {
                SetAttribute<string>("GroupName", value);
            }
        }
        /// <summary>
        /// 存储类型
        /// 该属性决定了消息在该队列中的存储方式
        /// 0：SqlServer表存储，1：Dynamics CRM实体存储
        /// </summary>
        public int StoreType
        {
            get
            {
                return GetAttribute<int>("StoreType");
            }
            set
            {
                SetAttribute<int>("StoreType", value);
            }
        }



        /// <summary>
        /// 所属服务器名称
        /// 在Dynamics存储中，表示CrmServiceFactory的名称，需要预设
        /// </summary>
        public string ServerName
        {
            get
            {
                return GetAttribute<string>("ServerName");
            }
            set
            {
                SetAttribute<string>("ServerName", value);
            }
        }


        /// <summary>
        /// 队列名称
        /// 该名称包含队列存储的位置信息
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
        /// 分组Code
        /// 根据该属性唯一标识同一个组内的队列
        /// </summary>
        public int Code
        {
            get
            {
                return GetAttribute<int>("Code");
            }
            set
            {
                SetAttribute<int>("Code", value);
            }
        }



        /// <summary>
        /// 是否是死队列
        /// </summary>
        public bool IsDead
        {
            get
            {
                return GetAttribute<bool>("IsDead");
            }
            set
            {
                SetAttribute<bool>("IsDead", value);
            }
        }

        /// <summary>
        /// 队列执行间隔（毫秒数）
        /// </summary>
        public int Interval
        {
            get
            {
                return GetAttribute<int>("Interval");
            }
            set
            {
                SetAttribute<int>("Interval", value);
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
    }


    public interface ISQueueIMP
    {
        Task Add(SQueue queue);
        Task Update(SQueue queue);
        Task Delete(SQueue queue);
    }


    [Injection(InterfaceType = typeof(ISQueueIMP), Scope = InjectionScope.Transient)]
    public class SQueueIMP : ISQueueIMP
    {
        public Task Add(SQueue queue)
        {
            throw new NotImplementedException();
        }

        public Task Delete(SQueue queue)
        {
            throw new NotImplementedException();
        }

        public Task Update(SQueue queue)
        {
            throw new NotImplementedException();
        }
    }
}
