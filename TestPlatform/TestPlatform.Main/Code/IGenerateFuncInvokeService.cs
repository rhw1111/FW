using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code
{
    public interface IGenerateFuncInvokeService
    {
        /// <summary>
        /// 生成代码块
        /// </summary>
        /// <param name="name">变量名称</param>
        /// <param name="data">数据</param>
        /// <returns>代码块</returns>
        Task<string> Generate(string name, string data);
    }
}
