using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code
{
    /// <summary>
    /// 附加函数生成服务
    /// </summary>
    public interface IGenerateAdditionFuncService
    {
        Task<string> Generate();
    }
}
