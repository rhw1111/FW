using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Collections.Hash
{
    /// <summary>
    /// 用来描述参与哈希散列的数据源
    /// </summary>
    public class HashDataSource : EntityBase<IHashDataSourceIMP>
    {
        private static IFactory<IHashDataSourceIMP> _hashDataSourceIMPFactory;
        public static IFactory<IHashDataSourceIMP> HashDataSourceIMPFactory
        {
            set
            {
                _hashDataSourceIMPFactory = value;
            }
        }
        public override IFactory<IHashDataSourceIMP> GetIMPFactory()
        {
            return _hashDataSourceIMPFactory;
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
        /// 所属哈希组
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
        /// 数据存储类型
        /// 表示使用该哈希组的数据使用的存储方式，如SqlServer、MySql等等
        /// </summary>
        public string StoreType
        {
            get
            {
                return GetAttribute<string>("StoreType");
            }
            set
            {
                SetAttribute<string>("StoreType", value);
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
    }



    public interface IHashDataSourceIMP
    {
        /// <summary>
        /// 数据迁移（部分）
        /// </summary>
        /// <returns></returns>
        Task Migrate(HashDataSource resource, Func<HashDataMigrateContext, Task> callback);
        /// <summary>
        /// 整体迁移
        /// </summary>
        /// <returns></returns>
        Task TotalMigrate(HashDataSource resource, Func<HashDataMigrateContext, Task> callback);
    }


    [Injection(InterfaceType = typeof(IHashDataSourceIMP), Scope = InjectionScope.Transient)]
    public class HashDataResourceIMP : IHashDataSourceIMP
    {
        public Task Migrate(HashDataSource resource, Func<HashDataMigrateContext, Task> callback)
        {
            throw new NotImplementedException();
        }

        public Task TotalMigrate(HashDataSource resource, Func<HashDataMigrateContext, Task> callback)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 哈希散列数据迁移上下文
    /// </summary>
    public class HashDataMigrateContext
    {
        /// <summary>
        /// 正在执行的哈希节点
        /// </summary>
        public HashNode ExecuteHashNode { get; internal set; }
        /// <summary>
        /// 正在执行的真实哈希节点
        /// </summary>
        public HashRealNode ExecuteHashRealNode { get; internal set; }
        /// <summary>
        /// 节点运行状态
        /// 1：开始执行，2：执行完成，3：不需要执行
        /// </summary>
        public int ExecuteStatus { get; internal set; }
    }

    /// <summary>
    /// 哈希数据迁移服务接口
    /// </summary>
    public interface IHashDataMigrateService
    {
    }


}
