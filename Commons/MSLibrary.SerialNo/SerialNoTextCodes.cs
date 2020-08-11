using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SerialNo
{
    public static class SerialNoTextCodes
    {
        /// <summary>
        /// 已经存在指定前缀的流水号记录
        /// 格式为“已经存在前缀为{0}的流水号记录”
        /// {0}：前缀
        /// </summary>
        public const string ExistSerialNoRecordByPrefix = "ExistSerialNoRecordByPrefix";
        /// <summary>
        /// 已经存在指定名称的流水号配置
        /// 格式为“已经存在名称为{0}的流水号配置”
        /// {0}：前缀
        /// </summary>
        public const string ExistSerialNoConfigurationByName = "ExistSerialNoConfigurationByName";
    }
}
