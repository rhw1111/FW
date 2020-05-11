using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Configuration.DAL;

namespace MSLibrary.Configuration
{
    /// <summary>
    /// 系统配置实体
    /// 负责维护系统的相关配置信息
    /// </summary>
    public class SystemConfiguration : EntityBase<ISystemConfigurationIMP>
    {
        private static IFactory<ISystemConfigurationIMP> _systemConfigurationIMPFactory;

        public static IFactory<ISystemConfigurationIMP> SystemConfigurationIMPFactory
        {
            set
            {
                _systemConfigurationIMPFactory = value;
            }
        }

        public override IFactory<ISystemConfigurationIMP> GetIMPFactory()
        {
            return _systemConfigurationIMPFactory;
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
        /// 配置名称
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
        /// 配置内容
        /// </summary>
        public string Content
        {
            get
            {
                return GetAttribute<string>("Content");
            }
            set
            {
                SetAttribute<string>("Content", value);
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
        /// 获取指定类型的配置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public T GetConfigurationValue<T>()
        {
            return _imp.GetConfigurationValue<T>(this);
        }

        /// <summary>
        /// 修改配置内容
        /// </summary>
        /// <returns></returns>
        public async Task UpdateContent()
        {
            await _imp.UpdateContent(this);
        }
    }


    public interface ISystemConfigurationIMP
    {
        /// <summary>
        /// 获取指定类型的配置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        T GetConfigurationValue<T>(SystemConfiguration configuration);
        /// <summary>
        /// 修改配置内容
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task UpdateContent(SystemConfiguration configuration);
    }


    [Injection(InterfaceType = typeof(ISystemConfigurationIMP), Scope = InjectionScope.Transient)]
    public class SystemConfigurationIMP : ISystemConfigurationIMP
    {
        private ISystemConfigurationStore _systemConfigurationStore;

        public SystemConfigurationIMP(ISystemConfigurationStore systemConfigurationStore)
        {
            _systemConfigurationStore = systemConfigurationStore;
        }

        public T GetConfigurationValue<T>(SystemConfiguration configuration)
        {
            //使用Json反序列化将配置内容转换成配置对象
            return JsonSerializerHelper.Deserialize<T>(configuration.Content);
        }

        public async Task UpdateContent(SystemConfiguration configuration)
        {
            await _systemConfigurationStore.UpdateContent(configuration);
        }
    }
}
