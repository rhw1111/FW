using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert
{
    public interface ICrmActionParameterConvertService
    {
        Task<JToken> Convert(object parameter);
    }
}
