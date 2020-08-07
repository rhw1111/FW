using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SerialNo.DAL
{
    public interface SerialNoConnectionFactory
    {
        string CreateAllForSerialNoConfiguration();
        string CreateReadForSerialNoConfiguration();
        string CreateAllForSerialNo();
        string CreateReadForSerialNo();
    }
}
