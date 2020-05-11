using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.DAL
{
    /// <summary>
    /// 需要分库分表的实体框架基类
    /// </summary>
    public class DBContextPartitionBase:DbContext
    {
        private Dictionary<string, string> _entityTableMapping;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="entityTableMapping">实体表名映射键值对</param>
        /// <param name="options"></param>
        public DBContextPartitionBase(Dictionary<string, string> entityTableMapping, DbContextOptions options) : base(options)
        {
            _entityTableMapping = entityTableMapping;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.ReplaceService<IModelCacheKeyFactory, ModelCacheKeyFactory>();
            }
        }

        /// <summary>
        /// 获取指定实体名称对应的表名
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        protected string GetTableName(string entityName)
        {
            if (!_entityTableMapping.TryGetValue(entityName,out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityTableMappintForDBContext,
                    DefaultFormatting = "在实体上下文类型{0}的对象中，找不到实体名称为{1}的实体表名映射键值对",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, entityName }
                };

                throw new UtilityException((int)Errors.NotFoundEntityTableMappintForDBContext, fragment);
            }
            return tableName;
        }

        private class ModelCacheKeyFactory : IModelCacheKeyFactory
        {
            public object Create(DbContext context)
            {
                return Guid.NewGuid();
            }
        }
    }


}
