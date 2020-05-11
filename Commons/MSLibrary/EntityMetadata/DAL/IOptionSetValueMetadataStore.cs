using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.EntityMetadata.DAL
{
    /// <summary>
    /// 选项集元数据数据操作
    /// </summary>
    public interface IOptionSetValueMetadataStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task Add(OptionSetValueMetadata metadata);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task Update(OptionSetValueMetadata metadata);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<OptionSetValueMetadata>> QueryByName(string name, int page, int pageSize);
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<OptionSetValueMetadata> QueryByName(string name);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OptionSetValueMetadata> QueryById(Guid id);
    }
}
