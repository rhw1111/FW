using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// Grpc通道池
    /// </summary>
    public interface IGrpcChannelPool
    {
        /// <summary>
        /// 新增通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="configuration">通道配置</param>
        /// <returns></returns>
        Task Add(string channelName, GrpcChannelConfiguration configuration);

        /// <summary>
        /// 新增新版本的通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="configuration">通道配置</param>
        /// <returns></returns>
        Task AddNewVersion(string channelName, GrpcChannelConfiguration configuration);

        /// <summary>
        /// 移除指定通道的旧版本
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        Task RemoveOldVersion(string channelName);
        /// <summary>
        /// 获取指定通道的最新版本
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        Task<Channel> Get(string channelName);
        /// <summary>
        /// 移除指定通道
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        Task Remove(string channelName);
        /// <summary>
        /// 全局配置信息设置
        /// </summary>
        /// <returns></returns>
        Task GlobalConfig(Func<GrpcChannelGlobalConfiguration,Task> action);

        /// <summary>
        /// 清理
        /// </summary>
        /// <returns></returns>
        Task Clear();
    }



}
