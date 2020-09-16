using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Cache.DAL;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;

namespace MSLibrary.Cache
{
    public class CacheVersion : EntityBase<ICacheVersionIMP>
    {
        private static IFactory<ICacheVersionIMP>? _cacheVersionIMPFactory;

        public static IFactory<ICacheVersionIMP> CacheVersionIMPFactory
        {
            set
            {
                _cacheVersionIMPFactory = value;
            }
        }

        public override IFactory<ICacheVersionIMP>? GetIMPFactory()
        {
            return _cacheVersionIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {

                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get
            {

                return GetAttribute<string>(nameof(Version));
            }
            set
            {
                SetAttribute<string>(nameof(Version), value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(CreateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreateTime), value);
            }
        }



        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }

        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }

        public async Task UpdateVersion(CancellationToken cancellationToken = default)
        {
            await _imp.UpdateVersion(this, cancellationToken);
        }

        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }
    }

    public interface ICacheVersionIMP
    {
        Task Add(CacheVersion version, CancellationToken cancellationToken = default);
        Task UpdateVersion(CacheVersion version, CancellationToken cancellationToken = default);
        Task Delete(CacheVersion version, CancellationToken cancellationToken = default);
    }


    [Injection(InterfaceType = typeof(ICacheVersionIMP), Scope = InjectionScope.Transient)]
    public class CacheVersionIMP : ICacheVersionIMP
    {
        private readonly ICacheVersionStore _cacheVersionStore;

        public CacheVersionIMP(ICacheVersionStore cacheVersionStore)
        {
            _cacheVersionStore = cacheVersionStore;
        }
        public async Task Add(CacheVersion version, CancellationToken cancellationToken = default)
        {
            var existVersion=await _cacheVersionStore.QueryByName(version.Name, cancellationToken);
            if (existVersion != null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ExistCacheVersionByName,
                    DefaultFormatting = "已经存在名称为{0}的缓存版本号记录",
                    ReplaceParameters = new List<object>() { version.Name }
                };

                throw new UtilityException((int)Errors.ExistCacheVersionByName, fragment);
            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {

                await _cacheVersionStore.Add(version, cancellationToken);

                var newId = await _cacheVersionStore.QueryByNameNoLock(version.Name, cancellationToken);
                if (newId != version.ID)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.ExistCacheVersionByName,
                        DefaultFormatting = "已经存在名称为{0}的缓存版本号记录",
                        ReplaceParameters = new List<object>() { version.Name }
                    };

                    throw new UtilityException((int)Errors.ExistCacheVersionByName, fragment);
                }
                scope.Complete();
            }
        }

        public async Task Delete(CacheVersion version, CancellationToken cancellationToken = default)
        {
            await _cacheVersionStore.Delete(version.ID, cancellationToken);
        }

        public async Task UpdateVersion(CacheVersion version, CancellationToken cancellationToken = default)
        {
            var strVersion = Guid.NewGuid().ToString();
            await _cacheVersionStore.UpdateVersion(version.ID, strVersion, cancellationToken);
        }
    }
}
