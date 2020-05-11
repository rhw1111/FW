using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Entity
{
    [Injection(InterfaceType = typeof(IEntityRepository), Scope = InjectionScope.Singleton)]
    public class EntityRepository : IEntityRepository
    {
        private static Dictionary<string, IFactory<IEntityRepositoryService>> _entityRepositoryServiceFactories = new Dictionary<string, IFactory<IEntityRepositoryService>>();

        /// <summary>
        /// 实体仓储服务工厂键值对
        /// 键为实体类型
        /// </summary>
        public static Dictionary<string, IFactory<IEntityRepositoryService>> EntityRepositoryServiceFactories
        {
            get
            {
                return _entityRepositoryServiceFactories;
            }
        }
        public async Task<ModelBase> QueryByTypeAndKey(string entityType, string entityKey)
        {
            if (!_entityRepositoryServiceFactories.TryGetValue(entityType,out IFactory<IEntityRepositoryService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityRepositoryServiceByEntityType,
                    DefaultFormatting = "实体类型为{0}的EntityRepositoryService找不到，位置为{1}",
                    ReplaceParameters = new List<object>() { entityType, "MSLibrary.Entity.EntityRepository" }
                };

                throw new UtilityException((int)Errors.NotFoundEntityRepositoryServiceByEntityType, fragment);
            }

            return await serviceFactory.Create().QueryByKey(entityKey);
        }
    }


    /// <summary>
    /// 实体仓储服务
    /// </summary>
    public interface IEntityRepositoryService
    {
        Task<ModelBase> QueryByKey(string entityKey);
    }
}
