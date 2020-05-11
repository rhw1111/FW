using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle
{
    /// <summary>
    /// Crm唯一键类型处理
    /// </summary>
    public interface ICrmAlternateKeyTypeHandle
    {
        Task<string> Convert (object value);
    }
}
