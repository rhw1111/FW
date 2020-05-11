using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.EntityMetadata.DAL
{
    /// <summary>
    /// 选项集单项数据操作
    /// </summary>
    public interface IOptionSetValueItemStore
    {
        Task Add(OptionSetValueItem item);
        Task Update(OptionSetValueItem item);

        Task Delete(Guid optionSetValueId,Guid itemId);

        Task<OptionSetValueItem> QueryByValue(Guid optionSetValueId, int value);


        Task<OptionSetValueItem> QueryByValue(Guid optionSetValueId, string stringValue);

        Task<OptionSetValueItem> QueryById(Guid id);


        Task QueryAll(Guid optionSetValueId,Func<OptionSetValueItem,Task> callback);

        Task<QueryResult<OptionSetValueItem>> QueryAll(Guid optionSetValueId,int page,int pageSize);

        Task QueryChildAll(Guid metadataId,Guid childMetadataId,Guid id,Func<OptionSetValueItem,Task> callback);
    }
}
