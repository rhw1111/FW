using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using MSLibrary.DI;
using MSLibrary.Entity.DAL;
using MSLibrary.Entity.FillEntityServices;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Entity
{
    /// <summary>
    /// 实体属性映射配置
    /// </summary>
    public class EntityAttributeMapConfiguration : EntityBase<IEntityAttributeMapConfigurationIMP>
    {
        private static IFactory<IEntityAttributeMapConfigurationIMP> _entityAttributeMapConfigurationIMPFactory;

        public static IFactory<IEntityAttributeMapConfigurationIMP> EntityAttributeMapConfigurationIMPFactory
        {
            set
            {
                _entityAttributeMapConfigurationIMPFactory = value;
            }
        }

        public override IFactory<IEntityAttributeMapConfigurationIMP> GetIMPFactory()
        {
            return _entityAttributeMapConfigurationIMPFactory;
        }



        /// <summary>
        /// id
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
        /// 实体名称
        /// </summary>
        public string EntityName
        {
            get
            {
                return GetAttribute<string>("EntityName");
            }
            set
            {
                SetAttribute<string>("EntityName", value);
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
        /// 通过DbDataReader和前缀为指定实体填充数据
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task FillEntity(DbDataReader reader, string prefix, ModelBase entity)
        {
            await _imp.FillEntity(this, reader, prefix, entity);
        }

        /// <summary>
        /// 获取所有明细
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetDetails(Func<EntityAttributeMapConfigurationDetail, Task> callback)
        {
            await _imp.GetDetails(this, callback);
        }

        /// <summary>
        /// 分页获取明细
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<EntityAttributeMapConfigurationDetail>> GetDetails(int page, int pageSize)
        {
            return await _imp.GetDetails(this,page,pageSize);
        }

        /// <summary>
        /// 根据前缀生成查询列字符串
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public async Task<string> GenerateQueryColumn(string prefix)
        {
            return await _imp.GenerateQueryColumn(this, prefix);
        }

        /// <summary>
        /// 通过DbDataReader和前缀为指定实体填充数据（同步）
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void FillEntitySync(DbDataReader reader, string prefix, ModelBase entity)
        {
             _imp.FillEntitySync(this, reader, prefix, entity);
        }

        /// <summary>
        /// 获取所有明细（同步）
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public  void GetDetailsSync( Action<EntityAttributeMapConfigurationDetail> callback)
        {
             _imp.GetDetailsSync(this,callback);
        }

        /// <summary>
        /// 分页获取明细（同步）
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public QueryResult<EntityAttributeMapConfigurationDetail> GetDetailsSync(int page, int pageSize)
        {
            return _imp.GetDetailsSync(this, page, pageSize);
        }

        /// <summary>
        /// 根据前缀生成查询列字符串（同步）
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string GenerateQueryColumnSync(string prefix)
        {
            return _imp.GenerateQueryColumnSync(this, prefix);
        }

    }


    public interface IEntityAttributeMapConfigurationIMP
    {
        /// <summary>
        /// 为实体赋值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task FillEntity(EntityAttributeMapConfiguration configuration,DbDataReader reader, string prefix, ModelBase entity);

        /// <summary>
        /// 获取所有关联的明细项
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetDetails(EntityAttributeMapConfiguration configuration,Func<EntityAttributeMapConfigurationDetail,Task> callback);

        /// <summary>
        /// 获取所有关联的明细项
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task<QueryResult<EntityAttributeMapConfigurationDetail>> GetDetails(EntityAttributeMapConfiguration configuration, int page,int pageSize);

        /// <summary>
        /// 生成查询列字符串
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string> GenerateQueryColumn(EntityAttributeMapConfiguration configuration, string prefix);


        /// <summary>
        /// 为实体赋值（同步）
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        void FillEntitySync(EntityAttributeMapConfiguration configuration, DbDataReader reader, string prefix, ModelBase entity);

        /// <summary>
        /// 获取所有关联的明细项（同步）
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        void GetDetailsSync(EntityAttributeMapConfiguration configuration, Action<EntityAttributeMapConfigurationDetail> callback);

        /// <summary>
        /// 分页获取明细（同步）
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        QueryResult<EntityAttributeMapConfigurationDetail> GetDetailsSync(EntityAttributeMapConfiguration configuration, int page, int pageSize);


        /// <summary>
        /// 生成查询列字符串（同步）
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        string GenerateQueryColumnSync(EntityAttributeMapConfiguration configuration, string prefix);

    }


    [Injection(InterfaceType = typeof(IEntityAttributeMapConfigurationIMP), Scope = InjectionScope.Transient)]
    public class EntityAttributeMapConfigurationIMP : IEntityAttributeMapConfigurationIMP
    {
        private static Dictionary<int, IFactory<IFillEntityService>> _fillEntityServiceFactiories = new Dictionary<int, IFactory<IFillEntityService>>();
        /// <summary>
        /// 需要在系统初始化时为填充服务工厂赋值
        /// </summary>
        public static Dictionary<int, IFactory<IFillEntityService>> FillEntityServiceFactiories
        {
            get
            {
                return _fillEntityServiceFactiories;
            }
        }

        private IEntityAttributeMapConfigurationDetailStore _entityAttributeMapConfigurationDetailStore;

        public EntityAttributeMapConfigurationIMP(IEntityAttributeMapConfigurationDetailStore entityAttributeMapConfigurationDetailStore)
        {
            _entityAttributeMapConfigurationDetailStore = entityAttributeMapConfigurationDetailStore;
        }

        public async Task FillEntity(EntityAttributeMapConfiguration configuration, DbDataReader reader, string prefix, ModelBase entity)
        {
            await _entityAttributeMapConfigurationDetailStore.QueryAllByConfigurationId(configuration.ID, async (detail) =>
             {
                 //为每个属性赋值
                 if (!_fillEntityServiceFactiories.TryGetValue(detail.AttributeType,out IFactory<IFillEntityService> serviceFactory))
                 {
                     var fragment = new TextFragment()
                     {
                         Code = TextCodes.NotFoundFillEntityService,
                         DefaultFormatting = "类型为{0}的FillEntityService未找到",
                         ReplaceParameters = new List<object>() { detail.AttributeType }
                     };

                     throw new UtilityException((int)Errors.NotFoundFillEntityService, fragment);
                 }

                 await serviceFactory.Create().FillEntity(detail, reader, prefix, entity);
             });
        }



        public async Task<string> GenerateQueryColumn(EntityAttributeMapConfiguration configuration, string prefix)
        {
            StringBuilder strQuery = new StringBuilder();
            await _entityAttributeMapConfigurationDetailStore.QueryAllByConfigurationId(configuration.ID, async (detail) =>
            {
                //为每个属性生成查询列，然后附加到strQuery
                strQuery.Append(string.Format("{0}.{1} as {0}{1},",prefix,detail.DBColumnName));
                await Task.FromResult(0);
            });

            if (strQuery.Length>0)
            {
                strQuery.Remove(strQuery.Length - 1, 1);
            }

            return await Task.FromResult(strQuery.ToString());
        }

   

        public async Task GetDetails(EntityAttributeMapConfiguration configuration, Func<EntityAttributeMapConfigurationDetail, Task> callback)
        {
            await _entityAttributeMapConfigurationDetailStore.QueryAllByConfigurationId(configuration.ID,callback);
        }

        public void FillEntitySync(EntityAttributeMapConfiguration configuration, DbDataReader reader, string prefix, ModelBase entity)
        {
            _entityAttributeMapConfigurationDetailStore.QueryAllByConfigurationIdSync(configuration.ID, (detail) =>
            {
                //为每个属性赋值
                if (!_fillEntityServiceFactiories.TryGetValue(detail.AttributeType, out IFactory<IFillEntityService> serviceFactory))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundFillEntityService,
                        DefaultFormatting = "类型为{0}的FillEntityService未找到",
                        ReplaceParameters = new List<object>() { detail.AttributeType }
                    };

                    throw new UtilityException((int)Errors.NotFoundFillEntityService, fragment);
                }

                 serviceFactory.Create().FillEntitySync(detail, reader, prefix, entity);
            });
        }

        public void GetDetailsSync(EntityAttributeMapConfiguration configuration, Action<EntityAttributeMapConfigurationDetail> callback)
        {
             _entityAttributeMapConfigurationDetailStore.QueryAllByConfigurationIdSync(configuration.ID, callback);
        }

        public string GenerateQueryColumnSync(EntityAttributeMapConfiguration configuration, string prefix)
        {
            StringBuilder strQuery = new StringBuilder();
            _entityAttributeMapConfigurationDetailStore.QueryAllByConfigurationIdSync(configuration.ID, (detail) =>
            {
                //为每个属性生成查询列，然后附加到strQuery
                strQuery.Append(string.Format("{0}.{1},", prefix, detail.DBColumnName));
            });

            if (strQuery.Length > 0)
            {
                strQuery.Remove(strQuery.Length - 1, 1);
            }

            return strQuery.ToString();
        }

        public async Task<QueryResult<EntityAttributeMapConfigurationDetail>> GetDetails(EntityAttributeMapConfiguration configuration, int page, int pageSize)
        {
            return await _entityAttributeMapConfigurationDetailStore.QueryByPage(configuration.ID, page, pageSize);
        }

        public QueryResult<EntityAttributeMapConfigurationDetail> GetDetailsSync(EntityAttributeMapConfiguration configuration, int page, int pageSize)
        {
            return _entityAttributeMapConfigurationDetailStore.QueryByPageSync(configuration.ID,page,pageSize);
        }
    }



    public class EntityAttributeMapConfigurationDetail : EntityBase<IEntityAttributeMapConfigurationDetailIMP>
    {
        private static IFactory<IEntityAttributeMapConfigurationDetailIMP> _entityAttributeMapConfigurationDetailIMPFactory;

        public static IFactory<IEntityAttributeMapConfigurationDetailIMP> EntityAttributeMapConfigurationDetailIMPFactory
        {
            set
            {
                _entityAttributeMapConfigurationDetailIMPFactory = value;
            }
        }

        public override IFactory<IEntityAttributeMapConfigurationDetailIMP> GetIMPFactory()
        {
            return _entityAttributeMapConfigurationDetailIMPFactory;
        }


        /// <summary>
        /// id
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
        /// 属性名称
        /// </summary>
        public string EntityAttributeName
        {
            get
            {
                return GetAttribute<string>("EntityAttributeName");
            }
            set
            {
                SetAttribute<string>("EntityAttributeName", value);
            }
        }

        /// <summary>
        /// 对应数据库字段名
        /// </summary>
        public string DBColumnName
        {
            get
            {
                return GetAttribute<string>("DBColumnName");
            }
            set
            {
                SetAttribute<string>("DBColumnName", value);
            }
        }

        

        /// <summary>
        /// 属性类型
        /// 0:Guid;1:int;2:intnull;3:long;4:longnull,5:decimal,6:decimalnull
        /// 7:double;8:doublenull;9:datetime;10:datetimenull;11:bool;12:bool?;13:string
        /// </summary>
        public int AttributeType
        {    
            get
            {
                
                return GetAttribute<int>("AttributeType");
            }
            set
            {
                SetAttribute<int>("AttributeType", value);
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


    public interface IEntityAttributeMapConfigurationDetailIMP
    {

    }

    [Injection(InterfaceType = typeof(IEntityAttributeMapConfigurationDetailIMP), Scope = InjectionScope.Transient)]
    public class EntityAttributeMapConfigurationDetailIMP : IEntityAttributeMapConfigurationDetailIMP
    {
        public EntityAttributeMapConfigurationDetailIMP()
        {
        }
    }
}
