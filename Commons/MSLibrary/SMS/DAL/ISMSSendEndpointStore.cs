using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SMS.DAL
{
    /// <summary>
    /// 短信发送终结点
    /// 负责短信的真正发送
    /// </summary>
    public interface ISMSSendEndpointStore
    {
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Add(SMSSendEndpoint endpoint);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Delete(SMSSendEndpoint endpoint);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Update(SMSSendEndpoint endpoint);

    }
}
