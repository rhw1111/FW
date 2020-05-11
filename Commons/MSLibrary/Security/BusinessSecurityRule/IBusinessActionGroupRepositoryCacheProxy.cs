using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.BusinessSecurityRule
{
    public interface IBusinessActionGroupRepositoryCacheProxy
    {
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BusinessActionGroup> QueryByName(string name);
    }
}
