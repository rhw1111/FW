using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Storge
{
    public class MultipartStorgeInfoDetail : EntityBase<IMultipartStorgeInfoDetailIMP>
    {
        private static IFactory<IMultipartStorgeInfoDetailIMP> _multipartStorgeInfoDetailIMPFactory;
        public static IFactory<IMultipartStorgeInfoDetailIMP> MultipartStorgeInfoDetailIMPFactory
        {
            set
            {
                _multipartStorgeInfoDetailIMPFactory = value;
            }
        }
        public override IFactory<IMultipartStorgeInfoDetailIMP> GetIMPFactory()
        {
            return _multipartStorgeInfoDetailIMPFactory;
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
        /// 编号
        /// </summary>
        public int Number
        {
            get
            {

                return GetAttribute<int>("Number");
            }
            set
            {
                SetAttribute<int>("Number", value);
            }
        }


        /// <summary>
        /// 起始位置
        /// </summary>
        public long Start
        {
            get
            {

                return GetAttribute<long>("Start");
            }
            set
            {
                SetAttribute<long>("Start", value);
            }
        }

        /// <summary>
        /// 结束位置
        /// </summary>
        public long End
        {
            get
            {

                return GetAttribute<long>("End");
            }
            set
            {
                SetAttribute<long>("End", value);
            }
        }

        /// <summary>
        /// 完成时的附加信息
        /// </summary>
        public string CompleteExtensionInfo
        {
            get
            {

                return GetAttribute<string>("CompleteExtensionInfo");
            }
            set
            {
                SetAttribute<string>("CompleteExtensionInfo", value);
            }
        }

        /// <summary>
        /// 状态
        /// 0：未完成
        /// 1：已完成
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

    }

    public interface IMultipartStorgeInfoDetailIMP
    {

    }
}
