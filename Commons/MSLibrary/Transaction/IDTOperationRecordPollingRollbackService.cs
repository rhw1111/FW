using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Transaction
{
    /// <summary>
    /// 分布式操作记录轮询回滚服务
    /// </summary>
    public interface IDTOperationRecordPollingRollbackService
    {
        Task<IDTOperationRecordPollingRollbackController> Execute(string storeGroupName,string memberName);
    }

    public interface IDTOperationRecordPollingRollbackController
    {
        Task Stop();
    }
}
