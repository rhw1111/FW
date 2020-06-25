using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.StreamingDB.InfluxDB
{
    /// <summary>
    /// InfluxDB记录
    /// </summary>
    public class InfluxDBRecord
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string MeasurementName
        {
            get; set;
        } = null!;

        /// <summary>
        /// 标签集合
        /// </summary>
        public Dictionary<string, string> Tags
        {
            get;
        } = new Dictionary<string, string>();

        /// <summary>
        /// 栏位集合
        /// </summary>
        public Dictionary<string, string> Fields
        {
            get;
        } = new Dictionary<string, string>();

        /// <summary>
        /// 时间戳
        /// </summary>
        public long? Timestamp
        {
            get;set;
        }

        public string ToDataString()
        {
            StringBuilder builder = new StringBuilder(MeasurementName);

            var strTags=Tags.ToDisplayString((kv) =>
            {
                return $"{kv.Key}={kv.Value}";
            },
            ()=>",");

            if (strTags!=string.Empty)
            {
                builder.Append($",{strTags}");
            }
            var strFields= Fields.ToDisplayString((kv) =>
            {
                return $"{kv.Key}={kv.Value}";
            }
            ,
            () => ",");

            if (strFields!=string.Empty)
            {
                builder.Append($" {strFields}");
            }

            if (Timestamp!=null)
            {
                builder.Append($" {Timestamp}");
            }

            return builder.ToString();
        }
    }
}
