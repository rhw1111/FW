using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Logger.DAL;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 表示通用日志
    /// </summary>
    public class CommonLog : EntityBase<ICommonLogIMP>
    {
        private static IFactory<ICommonLogIMP> _commonLogIMPFactory;

        public static IFactory<ICommonLogIMP> CommonLogIMPFactory
        {
            set
            {
                _commonLogIMPFactory = value;
            }
        }


        public override IFactory<ICommonLogIMP> GetIMPFactory()
        {
            return _commonLogIMPFactory;
        }


        /// <summary>
        /// Id
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
        /// 父记录ID
        /// </summary>
        public Guid ParentID
        {
            get
            {

                return GetAttribute<Guid>("ParentID");
            }
            set
            {
                SetAttribute<Guid>("ParentID", value);
            }
        }

        /// <summary>
        /// 上一层ID
        /// </summary>
        public Guid PreLevelID
        {
            get
            {

                return GetAttribute<Guid>("PreLevelID");
            }
            set
            {
                SetAttribute<Guid>("PreLevelID", value);
            }
        }

        
        /// <summary>
        /// 当前层ID
        /// </summary>
        public Guid CurrentLevelID
        {
            get
            {

                return GetAttribute<Guid>("CurrentLevelID");
            }
            set
            {
                SetAttribute<Guid>("CurrentLevelID", value);
            }
        }

        /// <summary>
        /// 上下文信息
        /// </summary>
        public string ContextInfo
        {
            get
            {
                return GetAttribute<string>("ContextInfo");
            }
            set
            {
                SetAttribute<string>("ContextInfo", value);
            }
        }

        /// <summary>
        /// 父上下文信息
        /// </summary>
        public string ParentContextInfo
        {
            get
            {
                return GetAttribute<string>("ParentContextInfo");
            }
            set
            {
                SetAttribute<string>("ParentContextInfo", value);
            }
        }

        /// <summary>
        /// 目录
        /// </summary>
        public string CategoryName
        {
            get
            {
                return GetAttribute<string>("CategoryName");
            }
            set
            {
                SetAttribute<string>("CategoryName", value);
            }
        }

        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName
        {
            get
            {
                return GetAttribute<string>("ActionName");
            }
            set
            {
                SetAttribute<string>("ActionName", value);
            }
        }

        /// <summary>
        /// 父动作名称
        /// </summary>
        public string ParentActionName
        {
            get
            {
                return GetAttribute<string>("ParentActionName");
            }
            set
            {
                SetAttribute<string>("ParentActionName", value);
            }
        }


        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequestBody
        {
            get
            {
                return GetAttribute<string>("RequestBody");
            }
            set
            {
                SetAttribute<string>("RequestBody", value);
            }
        }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseBody
        {
            get
            {
                return GetAttribute<string>("ResponseBody");
            }
            set
            {
                SetAttribute<string>("ResponseBody", value);
            }
        }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestUri
        {
            get
            {
                return GetAttribute<string>("RequestUri");
            }
            set
            {
                SetAttribute<string>("RequestUri", value);
            }
        }


        /// <summary>
        /// 内容
        /// </summary>
        public string Message
        {
            get
            {
                return GetAttribute<string>("Message");
            }
            set
            {
                SetAttribute<string>("Message", value);
            }
        }

        /// <summary>
        /// 是否是根日志
        /// </summary>
        public bool Root
        {
            get
            {
                return GetAttribute<bool>("Root");
            }
            set
            {
                SetAttribute<bool>("Root", value);
            }
        }
        /// <summary>
        /// 日志级别
        //Trace = 0,
        //Debug = 1,
        //Information = 2,
        //Warning = 3,
        //Error = 4,
        //Critical = 5,
        //None = 6
        /// </summary>
        public int Level
        {
            get
            {
               
                return GetAttribute<int>("Level");
            }
            set
            {
                SetAttribute<int>("Level", value);
            }
        }

        public long Duration
        {
            get
            {

                return GetAttribute<long>("Duration");
            }
            set
            {
                SetAttribute<long>("Duration", value);
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
        /// 新增到本地
        /// </summary>
        /// <returns></returns>
        public async Task AddLocal()
        {
            await _imp.AddLocal(this);
        }
    }

    public interface ICommonLogIMP
    {
        Task Add(CommonLog log);
        Task AddLocal(CommonLog log);
    }

    [Injection(InterfaceType = typeof(ICommonLogIMP), Scope = InjectionScope.Transient)]
    public class CommonLogIMP : ICommonLogIMP
    {
        private ICommonLogStore _commonLogStore;

        public CommonLogIMP(ICommonLogStore commonLogStore)
        {
            _commonLogStore = commonLogStore;
        }

        public async Task Add(CommonLog log)
        {
            await _commonLogStore.Add(log);
        }

        public async Task AddLocal(CommonLog log)
        {
            await _commonLogStore.AddLocal(log);
        }
    }
}
