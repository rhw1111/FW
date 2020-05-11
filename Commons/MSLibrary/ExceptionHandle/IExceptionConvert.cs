using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.ExceptionHandle
{
    public interface IExceptionConvert
    {
        Task<(Type, object)> Convert(Exception exception);
        (Type, object) ConvertSync(Exception exception);

    }
}
