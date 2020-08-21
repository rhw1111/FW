using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Collections.DAL
{
    public interface ICollectionConnectionFactory
    {
        string CreateAllForCollection();
        string CreateReadForCollection();
    }
}
