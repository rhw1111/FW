using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary
{
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class QueryResult<T>
    {
        public QueryResult()
        {
            Results = new List<T>();
        }
        [DataMember]
        public int CurrentPage { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public List<T> Results { get; set; }

    }
}
