using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.FileManagement
{
    /// <summary>
    /// 上传文件处理配置
    /// </summary>
    public class UploadFileHandleConfiguration : EntityBase<IUploadFileHandleConfigurationIMP>
    {
        private static IFactory<IUploadFileHandleConfigurationIMP> _uploadFileHandleConfigurationIMPFactory;
        public static IFactory<IUploadFileHandleConfigurationIMP> UploadFileHandleConfigurationIMPFactory
        {
            set
            {
                _uploadFileHandleConfigurationIMPFactory = value;
            }
        }

        public override IFactory<IUploadFileHandleConfigurationIMP> GetIMPFactory()
        {
            return _uploadFileHandleConfigurationIMPFactory;
        }

        /// <summary>
        /// 配置Id
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
        /// 允许的文件后缀名
        /// </summary>
        public string[] AllowSuffixs
        {
            get
            {

                return GetAttribute<string[]>("AllowSuffixs");
            }
            set
            {
                SetAttribute<string[]>("AllowSuffixs", value);
            }
        }


        /// <summary>
        /// 配置信息
        /// 根据不同的处理，有不同的实现
        /// </summary>
        public string Configuration
        {
            get
            {

                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
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
        /// 状态
        /// 0，可用
        /// 1，禁用
        /// </summary>
        public int Status
        {
            get
            {

                return GetAttribute<int>("Status");
            }
            set
            {
                SetAttribute<int>("Status", value);
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

        public async Task<T> ConvertConfiguration<T>()
        {
            return await _imp.ConvertConfiguration<T>(this);
        }
    }

    public interface IUploadFileHandleConfigurationIMP
    {
        /// <summary>
        /// 转换Configuration属性为指定的类型
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<T> ConvertConfiguration<T>(UploadFileHandleConfiguration configuration);
    }

    [Injection(InterfaceType = typeof(IUploadFileHandleConfigurationIMP), Scope = InjectionScope.Transient)]
    public class UploadFileHandleConfigurationIMP : IUploadFileHandleConfigurationIMP
    {
        public async Task<T> ConvertConfiguration<T>(UploadFileHandleConfiguration configuration)
        {
            return await Task.FromResult(JsonSerializerHelper.Deserialize<T>(configuration.Configuration));
        }
    }
}
