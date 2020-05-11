using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.EntityMetadata.DAL;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.EntityMetadata
{
    /// <summary>
    /// 实体元数据
    /// EntityType为唯一值
    /// </summary>
    public class EntityInfo : EntityBase<IEntityInfoIMP>
    {
        private static IFactory<IEntityInfoIMP> _entityInfoIMPFactory;

        public static IFactory<IEntityInfoIMP> EntityInfoIMPFactory
        {
            set
            {
                _entityInfoIMPFactory = value;
            }
        }
        public override IFactory<IEntityInfoIMP> GetIMPFactory()
        {
            return _entityInfoIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }
        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType
        {
            get
            {
                return GetAttribute<string>("EntityType");
            }
            set
            {
                SetAttribute<string>("EntityType", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }


        /// <summary>
        /// 从实体对象中生成实体关键字
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<string> GenerateEntityKey(ModelBase entity)
        {
            return await _imp.GenerateEntityKey(this,entity);
        }

        /// <summary>
        /// 从实体对象中生成指定名称的备选实体关键字
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="alternatKeyName"></param>
        /// <returns></returns>
        public async Task<string> GenerateAlternateEntityKey(ModelBase entity, string alternatKeyName)
        {
            return await _imp.GenerateAlternateEntityKey(this, entity, alternatKeyName);
        }

        /// <summary>
        /// 解析实体关键字
        /// </summary>
        /// <param name="entityKey"></param>
        /// <returns></returns>
        public async Task<object[]> ResolveEntityKey(string entityKey)
        {
            return await _imp.ResolveEntityKey(this, entityKey);
        }
        /// <summary>
        /// 解析指定名称的备选实体关键字
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="alternatKeyName"></param>
        /// <returns></returns>
        public async Task<object[]> ResolveAlternateEntityKey(string entityKey, string alternatKeyName)
        {
            return await _imp.ResolveAlternateEntityKey(this, entityKey, alternatKeyName);
        }
    }

    public interface IEntityInfoIMP
    {
        /// <summary>
        /// 从实体对象中生成实体关键字
        /// </summary>
        /// <param name="info"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<string> GenerateEntityKey(EntityInfo info, ModelBase entity);
        /// <summary>
        /// 从实体对象中生成指定名称的备选实体关键字
        /// </summary>
        /// <param name="info"></param>
        /// <param name="entity"></param>
        /// <param name="alternatKeyName"></param>
        /// <returns></returns>
        Task<string> GenerateAlternateEntityKey(EntityInfo info, ModelBase entity, string alternatKeyName);
        /// <summary>
        /// 解析实体关键字
        /// </summary>
        /// <param name="info"></param>
        /// <param name="entityKey"></param>
        /// <returns></returns>
        Task<object[]> ResolveEntityKey(EntityInfo info, string entityKey);
        /// <summary>
        /// 解析指定名称的备选实体关键字
        /// </summary>
        /// <param name="info"></param>
        /// <param name="entityKey"></param>
        /// <param name="alternatKeyName"></param>
        /// <returns></returns>
        Task<object[]> ResolveAlternateEntityKey(EntityInfo info, string entityKey, string alternatKeyName);
    }

    [Injection(InterfaceType = typeof(IEntityInfoIMP), Scope = InjectionScope.Transient)]
    public class EntityInfoIMP : IEntityInfoIMP
    {


        private IEntityInfoKeyRelationStore _entityInfoKeyRelationStore;
        private IEntityInfoAlternateKeyStore _entityInfoAlternateKeyStore;
        private IEntityAttributeValueKeyConvertService _entityAttributeValueKeyConvertService;

        public EntityInfoIMP(IEntityInfoKeyRelationStore entityInfoKeyRelationStore, IEntityInfoAlternateKeyStore entityInfoAlternateKeyStore, IEntityAttributeValueKeyConvertService entityAttributeValueKeyConvertService)
        {
            _entityInfoKeyRelationStore = entityInfoKeyRelationStore;
            _entityInfoAlternateKeyStore = entityInfoAlternateKeyStore;
            _entityAttributeValueKeyConvertService = entityAttributeValueKeyConvertService;
        }
        public async Task<string> GenerateAlternateEntityKey(EntityInfo info, ModelBase entity, string alternatKeyName)
        {
            //获取指定名称的备用主关键字
            var alternateKey = await _entityInfoAlternateKeyStore.QueryByEntityInfoIdAndName(info.ID, alternatKeyName);

            if (alternateKey == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityInfoAlternateKeyByName,
                    DefaultFormatting = "实体类型为{0}的实体元数据下，找不到备用关键字名称为{1}的备用关键字",
                    ReplaceParameters = new List<object>() { info.EntityType, alternatKeyName }
                };

                throw new UtilityException((int)Errors.NotFoundEntityInfoAlternateKeyByName, fragment);
            }
            //获取备用关键字下面的所有关联关系
            List<EntityInfoAlternateKeyRelation> alternateKeyRelations = new List<EntityInfoAlternateKeyRelation>();
            await alternateKey.GetAllAlternateKeyRelation(async (relation) =>
            {
                alternateKeyRelations.Add(relation);
                await Task.FromResult(0);
            });

            if (alternateKeyRelations.Count == 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityInfoAlternateKeyRelation,
                    DefaultFormatting = "实体类型为{0}的实体元数据下，备用关键字名称为{1}的备用关键字下，找不到任何备用关键字关联关系",
                    ReplaceParameters = new List<object>() { info.EntityType, alternatKeyName }
                };

                throw new UtilityException((int)Errors.NotFoundEntityInfoAlternateKeyRelation, fragment);
            }

            StringBuilder strKey = new StringBuilder();
            int index = 0, length = alternateKeyRelations.Count;
            //根据不同的属性类型获取对应值的字符串形式
            for (index = 0; index <= length - 1; index++)
            {

                object attributeValue = null;
                entity.Attributes.TryGetValue(alternateKeyRelations[index].EntityAttributeInfo.Name, out attributeValue);
                strKey.Append(await _entityAttributeValueKeyConvertService.ConvertTo(alternateKeyRelations[index].EntityAttributeInfo, attributeValue));

                if (index != length - 1)
                {
                    strKey.Append("|");
                }
            }
            return strKey.ToString();
        }

        public async Task<string> GenerateEntityKey(EntityInfo info, ModelBase entity)
        {

            //获取主关键字关联关系
            List<EntityInfoKeyRelation> keyRelations = new List<EntityInfoKeyRelation>();
            await _entityInfoKeyRelationStore.QueryAllByEntityInfoId(info.ID, async (relation) =>
             {
                 keyRelations.Add(relation);
                 await Task.FromResult(0);
             });

            if (keyRelations.Count == 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityInfoKeyRelation,
                    DefaultFormatting = "实体类型为{0}的实体元数据下，找不到任何主关键字关联关系",
                    ReplaceParameters = new List<object>() { info.EntityType }
                };

                throw new UtilityException((int)Errors.NotFoundEntityInfoKeyRelation, fragment);
            }

            StringBuilder strKey = new StringBuilder();
            int index = 0, length = keyRelations.Count;
            //根据不同的属性类型获取对应值的字符串形式
            for (index = 0; index <= length - 1; index++)
            {

                object attributeValue = null;
                entity.Attributes.TryGetValue(keyRelations[index].EntityAttributeInfo.Name, out attributeValue);
                strKey.Append(await _entityAttributeValueKeyConvertService.ConvertTo(keyRelations[index].EntityAttributeInfo, attributeValue));

                if (index != length - 1)
                {
                    strKey.Append("|");
                }
            }
            return strKey.ToString();
        }

        public async Task<object[]> ResolveAlternateEntityKey(EntityInfo info, string entityKey, string alternatKeyName)
        {
            //分解实体关键字
            var arrayEntityKey = entityKey.Split('|');

            //获取指定名称的备用关键字
            var alternateKey = await _entityInfoAlternateKeyStore.QueryByEntityInfoIdAndName(info.ID, alternatKeyName);

            if (alternateKey == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityInfoAlternateKeyByName,
                    DefaultFormatting = "实体类型为{0}的实体元数据下，找不到备用关键字名称为{1}的备用关键字",
                    ReplaceParameters = new List<object>() { info.EntityType, alternatKeyName }
                };

                throw new UtilityException((int)Errors.NotFoundEntityInfoAlternateKeyByName, fragment);
            }
            //获取备用关键字下面的所有关联关系
            List<EntityInfoAlternateKeyRelation> alternateKeyRelations = new List<EntityInfoAlternateKeyRelation>();
            await alternateKey.GetAllAlternateKeyRelation(async (relation) =>
            {
                alternateKeyRelations.Add(relation);
                await Task.FromResult(0);
            });

            if (alternateKeyRelations.Count != arrayEntityKey.Length)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.EntityKeyValueCountNotEqualAlternateKeyAtributeCount,
                    DefaultFormatting = "实体类型为{0}的实体元数据的备用关键字{1}所包含的属性数量为{2}，但实体关键字{3}数量为{4}，两者不一致,发生位置为{5}",
                    ReplaceParameters = new List<object>() { info.EntityType, alternatKeyName, alternateKeyRelations.Count, entityKey, arrayEntityKey.Length, $"{this.GetType().FullName},ResolveAlternateEntityKey" }
                };

                throw new UtilityException((int)Errors.EntityKeyValueCountNotEqualAlternateKeyAtributeCount, fragment);
            }

            object[] result = new object[alternateKeyRelations.Count];
            int index = 0, length = alternateKeyRelations.Count;

            for (index = 0; index <= length - 1; index++)
            {
                result[index] = await _entityAttributeValueKeyConvertService.ConvertFrom(alternateKeyRelations[index].EntityAttributeInfo, arrayEntityKey[index]);
            }

            return result;
        }

        public async Task<object[]> ResolveEntityKey(EntityInfo info, string entityKey)
        {
            //分解实体关键字
            var arrayEntityKey = entityKey.Split('|');


            //获取主关键字下面的所有关联关系
            List<EntityInfoKeyRelation> keyRelations = new List<EntityInfoKeyRelation>();
            await _entityInfoKeyRelationStore.QueryAllByEntityInfoId(info.ID, async (relation) =>
             {
                 keyRelations.Add(relation);
                 await Task.FromResult(0);
             });

            if (keyRelations.Count != arrayEntityKey.Length)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.EntityKeyValueCountNotEqualKeyAtributeCount,
                    DefaultFormatting = "实体类型为{0}的实体元数据的主关键字所包含的属性数量为{1}，但实体关键字{2}数量为{3}，两者不一致,发生位置为{4}",
                    ReplaceParameters = new List<object>() { info.EntityType, keyRelations.Count, entityKey, arrayEntityKey.Length, $"{this.GetType().FullName},ResolveEntityKey" }
                };

                throw new UtilityException((int)Errors.EntityKeyValueCountNotEqualKeyAtributeCount, fragment);
            }

            object[] result = new object[keyRelations.Count];
            int index = 0, length = keyRelations.Count;

            for (index = 0; index <= length - 1; index++)
            {
                result[index] = await _entityAttributeValueKeyConvertService.ConvertFrom(keyRelations[index].EntityAttributeInfo, arrayEntityKey[index]);
            }

            return result;
        }



    }

    /// <summary>
    /// 实体属性值转换成关键字字符串服务接口
    /// </summary>
    public interface IEntityAttributeValueKeyConvertService
    {
        /// <summary>
        /// 将属性值转换为字符串
        /// </summary>
        /// <param name="attributeInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<string> ConvertTo(EntityAttributeInfo attributeInfo, object value);
        /// <summary>
        /// 从字符串转回属性值
        /// </summary>
        /// <param name="attributeInfo"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        Task<object> ConvertFrom(EntityAttributeInfo attributeInfo, string strKey);
    }



    /// <summary>
    /// 实体属性值转换成关键字字符串服务的主服务
    /// </summary>
    [Injection(InterfaceType = typeof(IEntityAttributeValueKeyConvertService), Scope = InjectionScope.Transient)]
    public class EntityAttributeValueKeyConvertMainService : IEntityAttributeValueKeyConvertService
    {
        private static Dictionary<string, IFactory<IEntityAttributeValueKeyConvertService>> _entityAttributeValueKeyConvertServiceFactories = new Dictionary<string, IFactory<IEntityAttributeValueKeyConvertService>>();

        /// <summary>
        /// 实体属性值转换成关键字字符串服务键值对
        /// 键为实体属性的类型，来源自EntityAttributeTypes
        /// </summary>
        public static Dictionary<string, IFactory<IEntityAttributeValueKeyConvertService>> EntityAttributeValueKeyConvertServiceFactories
        {
            get
            {
                return _entityAttributeValueKeyConvertServiceFactories;
            }
        }

        public async Task<object> ConvertFrom(EntityAttributeInfo attributeInfo, string strKey)
        {
            var service = GetEntityAttributeValueKeyConvertService(attributeInfo.Type);
            return await service.ConvertFrom(attributeInfo, strKey);
        }

        public async Task<string> ConvertTo(EntityAttributeInfo attributeInfo, object value)
        {
            var service = GetEntityAttributeValueKeyConvertService(attributeInfo.Type);
            return await service.ConvertTo(attributeInfo, value);
        }

        private IEntityAttributeValueKeyConvertService GetEntityAttributeValueKeyConvertService(string attributeType)
        {
            if (!_entityAttributeValueKeyConvertServiceFactories.TryGetValue(attributeType, out IFactory<IEntityAttributeValueKeyConvertService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityAttributeValueKeyConvertServiceByAttributeType,
                    DefaultFormatting = "找不到针对实体属性类型为{0}的实体属性值转换成关键字字符串服务",
                    ReplaceParameters = new List<object>() { attributeType }
                };

                throw new UtilityException((int)Errors.NotFoundEntityAttributeValueKeyConvertServiceByAttributeType, fragment);
            }

            return serviceFactory.Create();
        }
    }

    /// <summary>
    /// 实体属性元数据
    /// </summary>
    public class EntityAttributeInfo : EntityBase<IEntityAttributeInfoIMP>
    {
        private static IFactory<IEntityAttributeInfoIMP> _entityAttributeInfoIMPFactory;

        public static IFactory<IEntityAttributeInfoIMP> EntityAttributeInfoIMPFactory
        {
            set
            {
                _entityAttributeInfoIMPFactory = value;
            }
        }
        public override IFactory<IEntityAttributeInfoIMP> GetIMPFactory()
        {
            return _entityAttributeInfoIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// 所属实体元数据的Id
        /// </summary>
        public Guid EntityInfoId
        {
            get
            {
                return GetAttribute<Guid>("EntityInfoId");
            }
            set
            {
                SetAttribute<Guid>("EntityInfoId", value);
            }
        }

        /// <summary>
        /// 所属实体元数据
        /// </summary>
        public EntityInfo EntityInfo
        {
            get
            {
                return GetAttribute<EntityInfo>("EntityInfo");
            }
            set
            {
                SetAttribute<EntityInfo>("EntityInfo", value);
            }
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }

        /// <summary>
        /// 属性类型
        /// 来源于MSLibrary.EntityAttributeTypes
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }
    }


    public interface IEntityAttributeInfoIMP
    {

    }

    [Injection(InterfaceType = typeof(IEntityAttributeInfoIMP), Scope = InjectionScope.Transient)]
    public class EntityAttributeInfoIMP : IEntityAttributeInfoIMP
    {
        public EntityAttributeInfoIMP()
        {
        }
    }




    /// <summary>
    /// 实体元数据主关键字的关联关系
    /// EntityInfoId+EntityAttributeInfoId唯一
    /// EntityInfoId+Order唯一
    /// </summary>
    public class EntityInfoKeyRelation : EntityBase<IEntityInfoKeyRelationIMP>
    {
        private static IFactory<IEntityInfoKeyRelationIMP> _entityInfoKeyRelationIMPFactory;

        public static IFactory<IEntityInfoKeyRelationIMP> EntityInfoKeyRelationIMPFactory
        {
            set
            {
                _entityInfoKeyRelationIMPFactory = value;
            }
        }

        public override IFactory<IEntityInfoKeyRelationIMP> GetIMPFactory()
        {
            return _entityInfoKeyRelationIMPFactory;
        }


        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }
        /// <summary>
        /// 关联的实体元数据Id
        /// </summary>
        public Guid EntityInfoId
        {
            get
            {
                return GetAttribute<Guid>("EntityInfoId");
            }
            set
            {
                SetAttribute<Guid>("EntityInfoId", value);
            }
        }
        /// <summary>
        /// 关联的实体元数据
        /// </summary>
        public EntityInfo EntityInfo
        {
            get
            {
                return GetAttribute<EntityInfo>("EntityInfo");
            }
            set
            {
                SetAttribute<EntityInfo>("EntityInfo", value);
            }
        }

        /// <summary>
        /// 关联的实体属性元数据Id
        /// </summary>
        public Guid EntityAttributeInfoId
        {
            get
            {
                return GetAttribute<Guid>("EntityAttributeInfoId");
            }
            set
            {
                SetAttribute<Guid>("EntityAttributeInfoId", value);
            }
        }
        /// <summary>
        /// 关联的实体属性元数据
        /// </summary>
        public EntityAttributeInfo EntityAttributeInfo
        {
            get
            {
                return GetAttribute<EntityAttributeInfo>("EntityAttributeInfo");
            }
            set
            {
                SetAttribute<EntityAttributeInfo>("EntityAttributeInfo", value);
            }
        }

        /// <summary>
        /// 关联的实体属性元数据所属的次序
        /// </summary>
        public int Order
        {
            get
            {
                return GetAttribute<int>("Order");
            }
            set
            {
                SetAttribute<int>("Order", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }



    }

    public interface IEntityInfoKeyRelationIMP
    {

    }

    [Injection(InterfaceType = typeof(IEntityInfoKeyRelationIMP), Scope = InjectionScope.Transient)]
    public class EntityInfoKeyRelationIMP : IEntityInfoKeyRelationIMP
    {
        public EntityInfoKeyRelationIMP()
        {
        }
    }

    /// <summary>
    /// 实体元数据备用关键字
    /// EntityInfoId+Name唯一
    /// </summary>
    public class EntityInfoAlternateKey : EntityBase<IEntityInfoAlternateKeyIMP>
    {
        private static IFactory<IEntityInfoAlternateKeyIMP> _entityInfoAlternateKeyIMPFactory;

        public static IFactory<IEntityInfoAlternateKeyIMP> EntityInfoAlternateKeyIMPFactory
        {
            set
            {
                _entityInfoAlternateKeyIMPFactory = value;
            }
        }
        public override IFactory<IEntityInfoAlternateKeyIMP> GetIMPFactory()
        {
            return _entityInfoAlternateKeyIMPFactory;
        }


        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }
        /// <summary>
        /// 备用关键字名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }
        /// <summary>
        /// 关联的实体元数据Id
        /// </summary>
        public Guid EntityInfoId
        {
            get
            {
                return GetAttribute<Guid>("EntityInfoId");
            }
            set
            {
                SetAttribute<Guid>("EntityInfoId", value);
            }
        }
        /// <summary>
        /// 关联的实体元数据
        /// </summary>
        public EntityInfo EntityInfo
        {
            get
            {
                return GetAttribute<EntityInfo>("EntityInfo");
            }
            set
            {
                SetAttribute<EntityInfo>("EntityInfo", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        /// <summary>
        /// 获取该备用关键字下面的所有被用关键字关联关系
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetAllAlternateKeyRelation(Func<EntityInfoAlternateKeyRelation, Task> callback)
        {
            await _imp.GetAllAlternateKeyRelation(this, callback);
        }
    }


    public interface IEntityInfoAlternateKeyIMP
    {
        /// <summary>
        /// 获取备用关键字下面的所有被用关键字关联关系
        /// </summary>
        /// <param name="alternateKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetAllAlternateKeyRelation(EntityInfoAlternateKey alternateKey, Func<EntityInfoAlternateKeyRelation, Task> callback);
    }


    [Injection(InterfaceType = typeof(IEntityInfoAlternateKeyIMP), Scope = InjectionScope.Transient)]
    public class EntityInfoAlternateKeyIMP : IEntityInfoAlternateKeyIMP
    {
        private IEntityInfoAlternateKeyRelationStore _entityInfoAlternateKeyRelationStore;

        public EntityInfoAlternateKeyIMP(IEntityInfoAlternateKeyRelationStore entityInfoAlternateKeyRelationStore)
        {
            _entityInfoAlternateKeyRelationStore = entityInfoAlternateKeyRelationStore;
        }
        public async Task GetAllAlternateKeyRelation(EntityInfoAlternateKey alternateKey, Func<EntityInfoAlternateKeyRelation, Task> callback)
        {
            await _entityInfoAlternateKeyRelationStore.QueryAllByAlternateKeyId(alternateKey.ID, callback);
        }
    }


    /// <summary>
    /// 实体元数据备用关键字的关联关系
    /// EntityInfoAlternateKeyId+EntityAttributeInfoId唯一
    /// EntityInfoAlternateKeyId+Order唯一
    /// </summary>
    public class EntityInfoAlternateKeyRelation : EntityBase<IEntityInfoAlternateKeyRelationIMP>
    {
        private static IFactory<IEntityInfoAlternateKeyRelationIMP> _entityInfoAlternateKeyRelationIMPFactory;

        public static IFactory<IEntityInfoAlternateKeyRelationIMP> EntityInfoAlternateKeyRelationIMPFactory
        {
            set
            {
                _entityInfoAlternateKeyRelationIMPFactory = value;
            }
        }
        public override IFactory<IEntityInfoAlternateKeyRelationIMP> GetIMPFactory()
        {
            return _entityInfoAlternateKeyRelationIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// 关联的实体元数据备用关键字Id
        /// </summary>
        public Guid EntityInfoAlternateKeyId
        {
            get
            {
                return GetAttribute<Guid>("EntityInfoAlternateKeyId");
            }
            set
            {
                SetAttribute<Guid>("EntityInfoAlternateKeyId", value);
            }
        }
        /// <summary>
        /// 关联的实体元数据备用关键字
        /// </summary>
        public EntityInfoAlternateKey EntityInfoAlternateKey
        {
            get
            {
                return GetAttribute<EntityInfoAlternateKey>("EntityInfoAlternateKey");
            }
            set
            {
                SetAttribute<EntityInfoAlternateKey>("EntityInfoAlternateKey", value);
            }
        }

        /// <summary>
        /// 关联的实体属性元数据Id
        /// </summary>
        public Guid EntityAttributeInfoId
        {
            get
            {
                return GetAttribute<Guid>("EntityAttributeInfoId");
            }
            set
            {
                SetAttribute<Guid>("EntityAttributeInfoId", value);
            }
        }
        /// <summary>
        /// 关联的实体属性元数据
        /// </summary>
        public EntityAttributeInfo EntityAttributeInfo
        {
            get
            {
                return GetAttribute<EntityAttributeInfo>("EntityAttributeInfo");
            }
            set
            {
                SetAttribute<EntityAttributeInfo>("EntityAttributeInfo", value);
            }
        }

        /// <summary>
        /// 关联的实体属性元数据所属的次序
        /// </summary>
        public int Order
        {
            get
            {
                return GetAttribute<int>("Order");
            }
            set
            {
                SetAttribute<int>("Order", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }


    }

    public interface IEntityInfoAlternateKeyRelationIMP
    {

    }

    [Injection(InterfaceType = typeof(IEntityInfoAlternateKeyRelationIMP), Scope = InjectionScope.Transient)]
    public class EntityInfoAlternateKeyRelationIMP : IEntityInfoAlternateKeyRelationIMP
    {
        public EntityInfoAlternateKeyRelationIMP()
        {
        }
    }
}
