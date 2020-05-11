using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert
{
    public interface ICrmFunctionParameterConvertService
    {
        Task<string> Convert(object parameter);
    }
}
