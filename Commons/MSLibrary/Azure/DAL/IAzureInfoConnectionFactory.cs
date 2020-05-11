using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Azure.DAL
{
    public interface IAzureInfoConnectionFactory
    {
        string CreateAllForAzureInfo();
        string CreateReadForAzureInfo();
    }
}
