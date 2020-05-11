using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Grpc.Filters
{
    /// <summary>
    /// 过滤器命名服务
    /// 为各个过滤器命名不同的名称
    /// </summary>
    public interface IFilterNameService
    {
        /// <summary>
        /// 新增过滤器
        /// </summary>
        /// <param name="name"></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task Add(string name, FilterBase filter);
        /// <summary>
        /// 获取过滤器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<FilterBase> Get(string name);
    }
}
