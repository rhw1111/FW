using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.StreamingDB
{
    public static class StreamingDBTextCodes
    {
        /// <summary>
        /// InfluxDB终结点执行操作时出错
        /// 格式为“名称为{0}的InfluxDB执行操作时出错,错误信息为{1}，发生位置为{2}”
        /// {0}：终结点名称
        /// {1}：错误信息
        /// {2}：发生位置
        /// </summary>
        public static string InfluxDBEndpointOperateError = "InfluxDBEndpointOperateError";
    }
}
