using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Xrm.DAL
{
    /// <summary>
    /// Crm服务工厂数据操作
    /// </summary>
    public interface ICrmServiceFactoryStore
    {
        Task<CrmServiceFactory> QueryByName(string name);
    }
}
