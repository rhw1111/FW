using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.EntityMetadata.DAL;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.EntityMetadata
{
    /// <summary>
    /// 选项集元数据
    /// </summary>

    public class OptionSetValueMetadata : EntityBase<IOptionSetValueMetadataIMP>
    {
        private static IFactory<IOptionSetValueMetadataIMP> _optionSetValueMetadataIMPFactory;

        public static IFactory<IOptionSetValueMetadataIMP> OptionSetValueMetadataIMPFactory
        {
            set
            {
                _optionSetValueMetadataIMPFactory = value;
            }
        }
        public override IFactory<IOptionSetValueMetadataIMP> GetIMPFactory()
        {
            return _optionSetValueMetadataIMPFactory;
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
        /// 名称
        /// 格式为实体的FullName+"."+实体字段名称
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
        /// 新增
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public async Task Update()
        {
            await _imp.Update(this);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        /// <summary>
        /// 为选项集增加项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddItem(OptionSetValueItem item)
        {
            await _imp.AddItem(this,item);
        }

        /// <summary>
        /// 为选项集修改项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateItem(OptionSetValueItem item)
        {
            await _imp.UpdateItem(this,item);
        }

        /// <summary>
        /// 为选项集删除项
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task DeleteItem(Guid itemId)
        {
            await _imp.DeleteItem(this,itemId);
        }

        /// <summary>
        /// 获取该选项集下的所有项
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetAllItem(Func<OptionSetValueItem, Task> callback)
        {
            await _imp.GetAllItem(this, callback);
        }

        /// <summary>
        /// 获取该选项集下指定值的项
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<OptionSetValueItem> GetItem(int value)
        {
            return await _imp.GetItem(this,value);
        }

        /// <summary>
        /// 获取该选项集下指定值的项
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public async Task<OptionSetValueItem> GetItem(string stringValue)
        {
            return await _imp.GetItem(this, stringValue);
        }

        /// <summary>
        /// 分页获取该选项集下的所有项
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<OptionSetValueItem>> GetItem(int page, int pageSize)
        {
            return await _imp.GetItem(this,page,pageSize);
        }

        /// <summary>
        /// 获取指定项中关联指定子选项集的所有子选项集项
        /// </summary>
        /// <param name="childId">当前选项集关联的子选项集Id</param>
        /// <param name="itemId">当前选项集中的项Id</param>
        /// <param name="callback">所有关联的子选项集中的项</param>
        /// <returns></returns>
        public async Task GetAllChildItem(Guid childId, Guid itemId, Func<OptionSetValueItem, Task> callback)
        {
            await _imp.GetAllChildItem(this, childId, itemId, callback);
        }

    }

    /// <summary>
    /// 选项集元数据具体实现接口
    /// </summary>
    public interface IOptionSetValueMetadataIMP
    {
        Task Add(OptionSetValueMetadata metadata);
        Task Update(OptionSetValueMetadata metadata);

        Task Delete(OptionSetValueMetadata metadata);

        Task AddItem(OptionSetValueMetadata metadata,OptionSetValueItem item);

        Task UpdateItem(OptionSetValueMetadata metadata, OptionSetValueItem item);

        Task DeleteItem(OptionSetValueMetadata metadata,Guid itemId);

        Task GetAllItem(OptionSetValueMetadata metadata, Func<OptionSetValueItem, Task> callback);

        Task<OptionSetValueItem> GetItem(OptionSetValueMetadata metadata,int value);

        Task<OptionSetValueItem> GetItem(OptionSetValueMetadata metadata, string stringValue);


        Task<QueryResult<OptionSetValueItem>> GetItem(OptionSetValueMetadata metadata,int page, int pageSize);

        Task GetAllChildItem(OptionSetValueMetadata metadata,Guid childId,Guid itemId,Func<OptionSetValueItem,Task> callback);
    }


    [Injection(InterfaceType = typeof(IOptionSetValueMetadataIMP), Scope = InjectionScope.Transient)]
    public class OptionSetValueMetadataIMP : IOptionSetValueMetadataIMP
    {
        private IOptionSetValueMetadataStore _optionSetValueMetadataStore;
        private IOptionSetValueItemStore _optionSetValueItemStore;


        public OptionSetValueMetadataIMP(IOptionSetValueMetadataStore optionSetValueMetadataStore, IOptionSetValueItemStore optionSetValueItemStore)
        {
            _optionSetValueMetadataStore = optionSetValueMetadataStore;
            _optionSetValueItemStore = optionSetValueItemStore;
        }
        public async Task Add(OptionSetValueMetadata metadata)
        {
            await _optionSetValueMetadataStore.Add(metadata);
        }

        public async Task AddItem(OptionSetValueMetadata metadata, OptionSetValueItem item)
        {
            item.OptionSetValue = metadata;
            await _optionSetValueItemStore.Add(item);
        }

        public async Task Delete(OptionSetValueMetadata metadata)
        {
            await _optionSetValueMetadataStore.Delete(metadata.ID);
        }

        public async Task DeleteItem(OptionSetValueMetadata metadata, Guid itemId)
        {
            await _optionSetValueItemStore.Delete(metadata.ID, itemId);
        }

        public async Task GetAllChildItem(OptionSetValueMetadata metadata, Guid childId,Guid itemId, Func<OptionSetValueItem, Task> callback)
        {
            await _optionSetValueItemStore.QueryChildAll(metadata.ID, childId, itemId, callback);
        }

        public async Task GetAllItem(OptionSetValueMetadata metadata, Func<OptionSetValueItem, Task> callback)
        {
            await _optionSetValueItemStore.QueryAll(metadata.ID, callback);
        }


        public async Task<OptionSetValueItem> GetItem(OptionSetValueMetadata metadata, int value)
        {
            return await _optionSetValueItemStore.QueryByValue(metadata.ID, value);
        }

        public async Task<QueryResult<OptionSetValueItem>> GetItem(OptionSetValueMetadata metadata, int page, int pageSize)
        {
            return await _optionSetValueItemStore.QueryAll(metadata.ID, page, pageSize);
        }

        public async Task<OptionSetValueItem> GetItem(OptionSetValueMetadata metadata, string stringValue)
        {
            return await _optionSetValueItemStore.QueryByValue(metadata.ID, stringValue);
        }

        public async Task Update(OptionSetValueMetadata metadata)
        {
            await _optionSetValueMetadataStore.Update(metadata);
        }

        public async Task UpdateItem(OptionSetValueMetadata metadata, OptionSetValueItem item)
        {
            item.OptionSetValue = metadata;
            await _optionSetValueItemStore.Update(item);
        }
    }

    /// <summary>
    /// 选项集单项
    /// </summary>
    public class OptionSetValueItem : EntityBase<IOptionSetValueItemIMP>
    {
        private static IFactory<IOptionSetValueItemIMP> _optionSetValueItemIMPFactory;

        public static IFactory<IOptionSetValueItemIMP> OptionSetValueItemIMPFactory
        {
            set
            {
                _optionSetValueItemIMPFactory = value;
            }
        }
        public override IFactory<IOptionSetValueItemIMP> GetIMPFactory()
        {
            return _optionSetValueItemIMPFactory;
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


        public OptionSetValueMetadata OptionSetValue
        {
            get
            {
                return GetAttribute<OptionSetValueMetadata>("OptionSetValue");
            }
            set
            {
                SetAttribute<OptionSetValueMetadata>("OptionSetValue", value);
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public int Value
        {
            get
            {
                return GetAttribute<int>("Value");
            }
            set
            {
                SetAttribute<int>("Value", value);
            }
        }

        /// <summary>
        /// 字符串值
        /// </summary>
        public string StringValue
        {
            get
            {
                return GetAttribute<string>("StringValue");
            }
            set
            {
                SetAttribute<string>("StringValue", value);
            }
        }


        /// <summary>
        /// 默认标签
        /// </summary>
        public string DefaultLabel
        {
            get
            {
                return GetAttribute<string>("DefaultLabel");
            }
            set
            {
                SetAttribute<string>("DefaultLabel", value);
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
        /// 获取在当前上下文中的标签值
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCurrentLabel()
        {
            return await _imp.GetCurrentLabel(this);
        }

    }

    /// <summary>
    /// 选项集单项具体实现接口
    /// </summary>
    public interface IOptionSetValueItemIMP
    {
        Task<string> GetCurrentLabel(OptionSetValueItem item);

    }

    [Injection(InterfaceType = typeof(IOptionSetValueItemIMP), Scope = InjectionScope.Transient)]
    public class OptionSetValueItemIMP : IOptionSetValueItemIMP
    {
        public OptionSetValueItemIMP()
        {
        }


        public async Task<string> GetCurrentLabel(OptionSetValueItem item)
        {
            //通过使用StringLanguageTranslate来获取基于当前上下文语言的标签
            //多语言键为选项集名称+"."+选项集项的值
            var result= StringLanguageTranslate.Translate($"{ item.OptionSetValue.Name}.{item.Value.ToString()}", item.DefaultLabel);
            return await Task.FromResult(result);
        }
    }
}
