using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息请求响应容器
    /// </summary>
    public interface ISRRHostContainer
    {
        ISRRHostContainer Register(string name, Action<ISRRHostConfiguration> configure);
        Task<ISRRHostService> Get(string name);
    }
}
