using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Context.DAL
{
    public interface IContextConnectionFactory
    {
        string CreateAllForContext();
        string CreateReadForContext();
    }
}
