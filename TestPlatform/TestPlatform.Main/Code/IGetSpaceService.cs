using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code
{
    public interface IGetSpaceService
    {
        Task<string> GetSpace(int n);
    }
}
