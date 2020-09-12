using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSLibrary.Grpc.Context
{
    /// <summary>
    /// 基于Grpc的ServerCallContext生成扩展的上下文初始化动作
    /// </summary>
    public interface IGrpcExtensionContextHandleService
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetInfo(ServerCallContext callContext);
        /// <summary>
        /// 生成上下文
        /// </summary>
        /// <param name="info"></param>
        void GenerateContext(object info);
    }
}
