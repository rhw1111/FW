using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Transaction.DAL;

namespace MSLibrary.Transaction.DTOperationRecordServices
{
    /// <summary>
    /// 基于本地记录的分布式操作记录服务
    /// 在该实现中，操作数据来源自本地数据库
    /// </summary>
    [Injection(InterfaceType = typeof(DTOperationRecordServiceForLocal), Scope = InjectionScope.Singleton)]
    public class DTOperationRecordServiceForLocal : IDTOperationRecordService
    {
        private IDTOperationDataStore _dtOperationDataStore;

        public DTOperationRecordServiceForLocal(IDTOperationDataStore dtOperationDataStore)
        {
            _dtOperationDataStore = dtOperationDataStore;
        }

        public async Task Cancel(string typeInfo,string uniqueName)
        {
            //获取所有操作数据
            List<DTOperationData> datas = new List<DTOperationData>();
                  
            await _dtOperationDataStore.QueryByRecordUniqueName(null,null,uniqueName,(int)DTOperationDataStatus.UnCancel,
                async(data)=>
                {
                    datas.Add(data);
                    await data.Cancel();
                }
                );
            foreach(var item in datas)
            {
                await item.Delete();
            }
        }
    }
}
