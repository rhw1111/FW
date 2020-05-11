using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// Grpc通道池的静态容器
    /// 所有针对通道的操作都使用该容器
    /// </summary>
    public static class GrpcChannelPoolContainetr
    {
        private static object _lockObj = new object();
        private static IGrpcChannelPool _pool;
        private static IFactory<IGrpcChannelPool> _grpcVhannelPoolFactroy = new GrpcChannelPoolForDefaultFactory();

        public static IFactory<IGrpcChannelPool> GrpcVhannelPoolFactroy
        {
            set
            {
                _grpcVhannelPoolFactroy = value;
            }
        }

        private static void initPool()
        {
            if (_pool==null)
            {
                lock(_lockObj)
                {
                    if (_pool==null)
                    {
                        _pool = _grpcVhannelPoolFactroy.Create();
                    }
                }
            }
        }

        /// <summary>
        /// 新增通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="configuration">通道配置</param>
        /// <returns></returns>
        public static async Task Add(string channelName, GrpcChannelConfiguration configuration)
        {
            initPool();
            await _pool.Add(channelName, configuration);
        }

        /// <summary>
        /// 新增新版本的通道
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="configuration">通道配置</param>
        /// <returns></returns>
        public static async Task AddNewVersion(string channelName, GrpcChannelConfiguration configuration)
        {
            initPool();
            await _pool.AddNewVersion(channelName, configuration);
        }

        /// <summary>
        /// 移除指定通道的旧版本
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public static async Task RemoveOldVersion(string channelName)
        {
            initPool();
            await _pool.RemoveOldVersion(channelName);
        }
        /// <summary>
        /// 获取指定通道的最新版本
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public static async Task<Channel> Get(string channelName)
        {
            initPool();
            return await _pool.Get(channelName);
        }
        /// <summary>
        /// 移除指定通道
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public static async Task Remove(string channelName)
        {
            initPool();
            await _pool.Remove(channelName);
        }
        /// <summary>
        /// 全局配置信息设置
        /// </summary>
        /// <returns></returns>
        public static async Task GlobalConfig(Func<GrpcChannelGlobalConfiguration, Task> action)
        {
            initPool();
            await _pool.GlobalConfig(action);
        }

        public static async Task Clear()
        {
            initPool();
            await _pool.Clear();
        }
    }
}
