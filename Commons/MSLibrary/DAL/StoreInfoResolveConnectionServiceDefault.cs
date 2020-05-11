using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.DAL
{
    [Injection(InterfaceType = typeof(IStoreInfoResolveConnectionService), Scope = InjectionScope.Singleton)]
    public class StoreInfoResolveConnectionServiceDefault : IStoreInfoResolveConnectionService
    {
        public async Task<string> GetRead(StoreInfo info)
        {
            return await Task.FromResult(GetReadSync(info));
        }

        public async Task<string> GetReadAndWrite(StoreInfo info)
        {
            return await Task.FromResult(GetReadAndWriteSync(info));
        }

        public string GetReadAndWriteSync(StoreInfo info)
        {
            return info.DBConnectionNames.ReadAndWrite;
        }

        public string GetReadSync(StoreInfo info)
        {
            if (DBAllScope.IsAll())
            {
                return info.DBConnectionNames.ReadAndWrite;
            }
            else
            {
                return info.DBConnectionNames.Read;
            }
        }
    }
}
