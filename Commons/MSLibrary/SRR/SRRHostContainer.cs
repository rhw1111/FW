using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息请求响应主机静态容器
    /// </summary>
    public static class SRRHostContainer
    {
        public static ISRRHostContainer Container { get; set; } = DIContainerContainer.Get<ISRRHostContainer>();

        public static ISRRHostContainer Register(string name, Action<ISRRHostConfiguration> configure)
        {
            return  Container.Register(name, configure);
        }
        public static async Task<ISRRHostService> Get(string name)
        {
            return await Container.Get(name);
        }
    }
}
