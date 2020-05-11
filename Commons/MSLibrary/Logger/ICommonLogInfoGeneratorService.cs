using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 通用日志信息生成
    /// </summary>
    public interface ICommonLogInfoGeneratorService
    {
        /// <summary>
        /// 通用日志信息Http头信息键值对
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, string>> Generate();
    }
}
