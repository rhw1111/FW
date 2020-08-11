using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SerialNo
{
    public enum SerialNoErrorCodes
    {
        /// <summary>
        /// 已经存在指定前缀的流水号记录
        /// </summary>
        ExistSerialNoRecordByPrefix = 314900301,
        /// <summary>
        /// 已经存在指定名称的流水号配置
        /// </summary>
        ExistSerialNoConfigurationByName = 314900302,
    }
}
