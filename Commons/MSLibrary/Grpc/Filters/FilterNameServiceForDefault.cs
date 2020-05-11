using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Grpc.Filters
{
    public class FilterNameServiceForDefault:IFilterNameService
    {
        private Dictionary<string, FilterBase> _filterList = new Dictionary<string, FilterBase>();

        /// <summary>
        /// 新增过滤器
        /// </summary>
        /// <param name="name"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task Add(string name, FilterBase filter)
        {
            lock (_filterList)
            {
                _filterList.Add(name, filter);
            }
            await Task.FromResult(0);
        }
        /// <summary>
        /// 获取过滤器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<FilterBase> Get(string name)
        {
            if (!_filterList.TryGetValue(name,out FilterBase filter))
            {
                filter = null;
            }

            return await Task.FromResult(filter);
        }
    }

    public class FilterNameServiceForDefaultFactory: SingletonFactory<IFilterNameService>
    {
        protected override IFilterNameService RealCreate()
        {
            return new FilterNameServiceForDefault();
        }
    }
}
