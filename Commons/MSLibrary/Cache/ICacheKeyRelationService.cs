using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 缓存键值关键关系服务
    /// 用来存储管理有关联关系的键
    /// </summary>
    public interface ICacheKeyRelationService
    {
        Task AddOTN(string relationName, string oKey, string nKey);
        Task<IList<string>> GetNKeys(string relationName, string oKey);
        Task Delete(string relationName,string oKey);
    }
}
