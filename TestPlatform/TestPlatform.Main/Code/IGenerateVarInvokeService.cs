using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code
{
    public interface IGenerateVarInvokeService
    {
        /// <summary>
        /// 生成代码块
        /// </summary>
        /// <param name="name">函数名称</param>
        /// <param name="data">参数</param>
        /// <returns>代码块</returns>
        Task<string> Generate(string name, string[] parameters);
    }
}
