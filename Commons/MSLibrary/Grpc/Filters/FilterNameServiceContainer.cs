using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Grpc.Filters
{
    /// <summary>
    /// Grpc过滤器命名服务容器
    /// </summary>
    public static class FilterNameServiceContainer
    {
        private static IFactory<IFilterNameService> _filterNameServiceFactory=new FilterNameServiceForDefaultFactory();

        public static IFactory<IFilterNameService> FilterNameServiceFactory
        {
            set
            {
                _filterNameServiceFactory = value;
            }
        }
        /// <summary>
        /// 新增过滤器
        /// </summary>
        /// <param name="name"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task Add(string name, FilterBase filter)
        {
            await _filterNameServiceFactory.Create().Add(name, filter);
        }
        /// <summary>
        /// 获取过滤器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async Task<FilterBase> Get(string name)
        {
            return await _filterNameServiceFactory.Create().Get(name);
        }

    }
}
