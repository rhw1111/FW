using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MSLibrary.StreamingDB.InfluxDB
{
    [DataContract]
    public class InfluxDBQueryData
    {
        [DataMember(Name = "results")]
        public List<InfluxDBQueryDataItem> Results { get; set; } = null!;
    }

    [DataContract]
    public class InfluxDBQueryDataItem
    {
        [DataMember(Name = "statement_id")]
        public string StatementID { get; set; } = null!;
        [DataMember(Name = "error")]
        public string? Error { get; set; }
        [DataMember(Name = "series")]
        public List<InfluxDBQueryDataSerieItem>? Series { get; set; }
    }

    [DataContract]
    public class InfluxDBQueryDataSerieItem
    {
        [DataMember(Name = "name")]
        public string Name { get; set; } = null!;
        [DataMember(Name= "columns")]
        public List<string> Columns { get; set; } = null!;
        [DataMember(Name= "values")]
        public List<List<JValue>> Values = null!;
    }
}
