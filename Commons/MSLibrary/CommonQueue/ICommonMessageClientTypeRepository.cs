using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.CommonQueue
{
    public interface ICommonMessageClientTypeRepository
    {
        Task<CommonMessageClientType> QueryByName(string name);
    }
}
