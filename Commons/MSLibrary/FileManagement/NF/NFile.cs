using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSLibrary.DI;

namespace MSLibrary.FileManagement.NF
{
    /// <summary>
    /// 文件
    /// </summary>
    public class NFile : EntityBase<INFileIMP>
    {
        private static IFactory<INFileIMP> _nFileIMPFactory;
        public static IFactory<INFileIMP> NFileIMPFactory
        {
            set
            {
                _nFileIMPFactory = value;
            }
        }

        public override IFactory<INFileIMP> GetIMPFactory()
        {
            return _nFileIMPFactory;
        }


        /// <summary>
        /// 文件Id
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
        /// 文件显示名称
        /// </summary>
        public string DisplayName
        {
            get
            {

                return GetAttribute<string>("DisplayName");
            }
            set
            {
                SetAttribute<string>("DisplayName", value);
            }
        }

        /// <summary>
        /// 唯一文件名
        /// </summary>
        public string UniqueName
        {
            get
            {

                return GetAttribute<string>("UniqueName");
            }
            set
            {
                SetAttribute<string>("UniqueName", value);
            }
        }


        /// <summary>
        /// 文件类型
        /// </summary>
        public FileType? FileType
        {
            get
            {

                return GetAttribute<FileType?>("FileType");
            }
            set
            {
                SetAttribute<FileType?>("FileType", value);
            }
        }


        /// <summary>
        /// 文件的后缀名
        /// </summary>
        public string Suffix
        {
            get
            {

                return GetAttribute<string>("Suffix");
            }
            set
            {
                SetAttribute<string>("Suffix", value);
            }
        }

        /// <summary>
        /// 文件长度
        /// </summary>
        public long Size
        {
            get
            {

                return GetAttribute<long>("Size");
            }
            set
            {
                SetAttribute<long>("Size", value);
            }
        }

        /// <summary>
        /// 状态
        /// 0，草稿
        /// 1，已提交
        /// 2，已删除
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
        /// 文件关联的类型
        /// </summary>
        public string RegardingType
        {
            get
            {

                return GetAttribute<string>("RegardingType");
            }
            set
            {
                SetAttribute<string>("RegardingType", value);
            }
        }


        /// <summary>
        /// 文件关联的关键信息
        /// </summary>
        public string RegardingKey
        {
            get
            {

                return GetAttribute<string>("RegardingKey");
            }
            set
            {
                SetAttribute<string>("RegardingKey", value);
            }
        }


        /// <summary>
        /// 文件来源类型
        /// </summary>
        public string SourceType
        {
            get
            {

                return GetAttribute<string>("SourceType");
            }
            set
            {
                SetAttribute<string>("SourceType", value);
            }
        }

        /// <summary>
        /// 文件来源关键信息
        /// </summary>
        public string SourceKey
        {
            get
            {

                return GetAttribute<string>("SourceKey");
            }
            set
            {
                SetAttribute<string>("SourceKey", value);
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


    public interface INFileIMP
    {

    }
}
