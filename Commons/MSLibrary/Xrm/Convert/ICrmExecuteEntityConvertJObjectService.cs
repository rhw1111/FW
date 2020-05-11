using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Convert
{
    public interface ICrmExecuteEntityConvertJObjectService
    {
        Task<JObject> Convert(CrmExecuteEntity entity);
    }
}
