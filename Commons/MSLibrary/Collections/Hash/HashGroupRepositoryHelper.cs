using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Cache;

namespace MSLibrary.Collections.Hash
{
    /// <summary>
    /// 哈希组仓储静态帮助器
    /// 提供缓存服务
    /// 简化需要缓存服务的调用方使用
    /// </summary>
    public class HashGroupRepositoryHelper
    {
        private IHashGroupRepository _hashGroupRepository;

        public HashGroupRepositoryHelper(IHashGroupRepository hashGroupRepository)
        {
            _hashGroupRepository = hashGroupRepository;
        }

        private static int _cacheSize = 2000;

        private static int CacheSize
        {
            get
            {
                return _cacheSize;
            }
            set
            {
                _cacheSize = value;
                _groupsByID.Length = value;
                _groupsByName.Length = value;
            }
        }
        private static int CacheTimeout { get; set; } = 600;

        private static HashLinkedCache<string, CacheTimeContainer<HashGroup>> _groupsByName = new HashLinkedCache<string, CacheTimeContainer<HashGroup>>() { Length = CacheSize };

        private static HashLinkedCache<Guid, CacheTimeContainer<HashGroup>> _groupsByID = new HashLinkedCache<Guid, CacheTimeContainer<HashGroup>>() { Length = CacheSize };


        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HashGroup> QueryById(Guid id)
        {
            CacheTimeContainer<HashGroup> groupItem = _groupsByID.GetValue(id);
            if (groupItem == null || groupItem.Expire())
            {
                var group = await _hashGroupRepository.QueryById(id);
                groupItem = new CacheTimeContainer<HashGroup>(group, CacheTimeout);
                _groupsByID.SetValue(id, groupItem);
            }

            return groupItem.Value;
        }
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<HashGroup> QueryByName(string name)
        {
            CacheTimeContainer<HashGroup> groupItem = _groupsByName.GetValue(name);
            if (groupItem == null || groupItem.Expire())
            {
                var group = await _hashGroupRepository.QueryByName(name);
                groupItem = new CacheTimeContainer<HashGroup>(group, CacheTimeout);
                _groupsByName.SetValue(name, groupItem);
            }

            return groupItem.Value;
        }
        public  HashGroup QueryByNameSync(string name)
        {
            CacheTimeContainer<HashGroup> groupItem = _groupsByName.GetValue(name);
            if (groupItem == null || groupItem.Expire())
            {
                var group = _hashGroupRepository.QueryByNameSync(name);
                groupItem = new CacheTimeContainer<HashGroup>(group, CacheTimeout);
                _groupsByName.SetValue(name, groupItem);
            }

            return groupItem.Value;
        }


    }


    /// <summary>
    /// 哈希组仓储静态帮助器工厂
    /// </summary>
    public static class HashGroupRepositoryHelperFactory
    {
        public static HashGroupRepositoryHelper Create(IHashGroupRepository repository)
        {
            return new HashGroupRepositoryHelper(repository);
        }
    }
}
