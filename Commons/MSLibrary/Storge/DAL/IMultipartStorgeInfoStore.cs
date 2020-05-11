using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Storge.DAL
{
    public interface IMultipartStorgeInfoStore
    {
        Task Add(MultipartStorgeInfo info);
        Task Delete(Guid infoID);
        Task DeleteBySourceID(string sourceInfo,Guid infoID);
        Task Updtae(MultipartStorgeInfo info);

        Task<MultipartStorgeInfo> QueryByID(Guid id);
        Task<MultipartStorgeInfo> QueryRunByName(string name);
        Task<MultipartStorgeInfo> QueryBySourceID(string sourceInfo, Guid id);
        Task<MultipartStorgeInfo> QueryRunBySourceName(string sourceInfo, string name);


        Task<QueryResult<MultipartStorgeInfo>> QueryByPage(string name, string displayName, string sourceInfo, string credentialInfo, int? status, int page, int pageSize);


        Task<QueryResult<MultipartStorgeInfo>> QueryBySourcePage(string sourceInfo, int? status, int page, int pageSize);

    }
}
