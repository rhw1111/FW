using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Transaction
{
    public interface IDTOperationService
    {
        Task Execute(string name,string storeGroupName,string hashInfo,string type,string typeInfo,int timeout, Func<Task> action);
    }
}
