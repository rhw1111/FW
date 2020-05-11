using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Xrm.MessageHandle.CrmAttributeMetadataHandle
{
    public interface ICrmAttributeMetadataHandle
    {
        Task<object> Execute(string body);
    }
}
