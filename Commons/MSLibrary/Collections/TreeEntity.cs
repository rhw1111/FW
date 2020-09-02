using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Collections.DAL;
using MSLibrary.Transaction;
using MSLibrary.LanguageTranslate;
using System.Runtime.InteropServices;

namespace MSLibrary.Collections
{
    /// <summary>
    /// 树状结构实体
    /// </summary>
    public class TreeEntity : EntityBase<ITreeEntityIMP>
    {

        private static IFactory<ITreeEntityIMP>? _treeEntityIMPFactory;

        public static IFactory<ITreeEntityIMP> TreeEntityIMPFactory
        {
            set
            {
                _treeEntityIMPFactory = value;
            }
        }

        public override IFactory<ITreeEntityIMP>? GetIMPFactory()
        {
            return _treeEntityIMPFactory;
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
        /// 类型
        /// </summary>
        public int Type
        {
            get
            {
                return GetAttribute<int>(nameof(Type));
            }
            set
            {
                SetAttribute<int>(nameof(Type), value);
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public string Value
        {
            get
            {
                return GetAttribute<string>(nameof(Value));
            }
            set
            {
                SetAttribute<string>(nameof(Value), value);
            }
        }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public Guid? ParentID
        {
            get
            {
                return GetAttribute<Guid?>(nameof(ParentID));
            }
            set
            {
                SetAttribute<Guid?>(nameof(ParentID), value);
            }
        }
        /// <summary>
        /// 父节点
        /// </summary>
        public TreeEntity? Parent
        {
            get
            {
                return GetAttribute<TreeEntity?>(nameof(Parent));
            }
            set
            {
                SetAttribute<TreeEntity?>(nameof(Parent), value);
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

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateName(string name, CancellationToken cancellationToken = default)
        {
            await _imp.UpdateName(this, name, cancellationToken);
        }
        /// <summary>
        /// 修改父节点
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateParent(Guid? parentID, CancellationToken cancellationToken = default)
        {
            await _imp.UpdateParent(this, parentID, cancellationToken);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }

        /// <summary>
        /// 分页获取子节点
        /// </summary>
        /// <param name="matchName"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<QueryResult<TreeEntity>> GetChildren(string? matchName, int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _imp.GetChildren(this, matchName, type, page, pageSize, cancellationToken);
        }

        /// <summary>
        /// 获取唯一子节点
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <returns></returns>
        public async Task<TreeEntity?> GetChildren(string name, CancellationToken cancellationToken = default)
        {
            return await _imp.GetChildren(this, name, cancellationToken);
        }

        /// <summary>
        /// 检查是否存在子节点
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> HasChildren(CancellationToken cancellationToken = default)
        {
            return await _imp.HasChildren(this, cancellationToken);
        }

        /// <summary>
        /// 获取节点值转换成的JObject数据
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<JObject> GetFormatValue(CancellationToken cancellationToken = default)
        {
            return await _imp.GetFormatValue(this,cancellationToken);
        }
    }

    public interface ITreeEntityIMP
    {
        Task Add(TreeEntity entity, CancellationToken cancellationToken = default);
        Task Update(TreeEntity entity, CancellationToken cancellationToken = default);
        Task UpdateName(TreeEntity entity,string name, CancellationToken cancellationToken = default);
        Task UpdateParent(TreeEntity entity,Guid? parentID, CancellationToken cancellationToken = default);
        Task Delete(TreeEntity entity, CancellationToken cancellationToken = default);
        Task<QueryResult<TreeEntity>> GetChildren(TreeEntity entity,string? matchName,int? type,int page,int pageSize, CancellationToken cancellationToken = default);
        Task<TreeEntity?> GetChildren(TreeEntity entity, string name, CancellationToken cancellationToken = default);
        Task<bool> HasChildren(TreeEntity entity,CancellationToken cancellationToken = default);
        Task<JObject> GetFormatValue(TreeEntity entity, CancellationToken cancellationToken = default);
    }

    public interface ITreeEntityValueService
    {
        Task UpdateName(string name,string value, CancellationToken cancellationToken = default);
        Task Delete(string value, CancellationToken cancellationToken = default);
        Task<JObject> GetFormatValue(string value, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(ITreeEntityIMP), Scope = InjectionScope.Transient)]
    public class TreeEntityIMP : ITreeEntityIMP
    {
        public readonly static Dictionary<int, IFactory<ITreeEntityValueService>> ValueServices = new Dictionary<int, IFactory<ITreeEntityValueService>>();

        private readonly ITreeEntityStore _treeEntityStore;

        public TreeEntityIMP(ITreeEntityStore treeEntityStore)
        {
            _treeEntityStore = treeEntityStore;
        }

        public async Task Add(TreeEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity.ParentID!=null)
            {
                var parent= await _treeEntityStore.QueryByID(entity.ParentID.Value, cancellationToken);
                if (parent==null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundTreeEntityByID,
                        DefaultFormatting = "找不到Id为{0}的树状实体记录",
                        ReplaceParameters = new List<object>() { entity.ParentID.Value.ToString() }
                    };
                    throw new UtilityException((int)Errors.NotFoundTreeEntityByID, fragment, 1, 0);
                }

                if (parent.Type != 1)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.TreeEntityParentTypeError,
                        DefaultFormatting = "树状实体{0}的类型无法作为父记录，要求的类型为{1}",
                        ReplaceParameters = new List<object>() { entity.ParentID.ToString(), "0" }
                    };
                    throw new UtilityException((int)Errors.TreeEntityParentTypeError, fragment, 1, 0);
                }
            }

            await checkDuplicate(entity,
                async () =>
                {
                    await _treeEntityStore.Add(entity, cancellationToken);
                },
                cancellationToken);
        }

        public async Task Delete(TreeEntity entity, CancellationToken cancellationToken = default)
        {
            var hasChildren=await HasChildren(entity, cancellationToken);
            if (hasChildren)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.TreeEntityHasChildren,
                    DefaultFormatting = "id为{0}的树状实体记录下存在子记录",
                    ReplaceParameters = new List<object>() { entity.ID.ToString() }
                };
                throw new UtilityException((int)Errors.TreeEntityHasChildren, fragment, 1, 0);
            }

            var valueService = getValueService(entity.Type);
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {                
                await _treeEntityStore.Delete(entity.ID, cancellationToken);
                await valueService.Delete(entity.Value, cancellationToken);
                scope.Complete();
            }
        }

        public async Task<QueryResult<TreeEntity>> GetChildren(TreeEntity entity, string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _treeEntityStore.QueryChildren(entity.ID, matchName,type, page, pageSize, cancellationToken);
        }

        public async Task<TreeEntity?> GetChildren(TreeEntity entity, string name, CancellationToken cancellationToken = default)
        {
            return await _treeEntityStore.QueryByName(entity.ID, name, cancellationToken);
        }

        public async Task<JObject> GetFormatValue(TreeEntity entity, CancellationToken cancellationToken = default)
        {
            var valueService = getValueService(entity.Type);
            return await valueService.GetFormatValue(entity.Value, cancellationToken);
        }

        public async Task<bool> HasChildren(TreeEntity entity, CancellationToken cancellationToken = default)
        {
            var firstId = await _treeEntityStore.QueryFirstChildren(entity.ID, cancellationToken);
            if (firstId!=null)
            {
                return true;
            }
            return false;

        }

        public async Task Update(TreeEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity.ParentID != null)
            {
                var parent = await _treeEntityStore.QueryByID(entity.ParentID.Value, cancellationToken);
                if (parent == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundTreeEntityByID,
                        DefaultFormatting = "找不到Id为{0}的树状实体记录",
                        ReplaceParameters = new List<object>() { entity.ParentID.Value.ToString() }
                    };
                    throw new UtilityException((int)Errors.NotFoundTreeEntityByID, fragment, 1, 0);
                }

                if (parent.Type != 1)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.TreeEntityParentTypeError,
                        DefaultFormatting = "树状实体{0}的类型无法作为父记录，要求的类型为{1}",
                        ReplaceParameters = new List<object>() { entity.ParentID.ToString(), "1" }
                    };
                    throw new UtilityException((int)Errors.TreeEntityParentTypeError, fragment, 1, 0);
                }
            }

            await checkDuplicate(entity,
                async () =>
                {
                    await _treeEntityStore.Update(entity, cancellationToken);
                },
                cancellationToken);

        }

        public async Task UpdateParent(TreeEntity entity, Guid? parentID, CancellationToken cancellationToken = default)
        {
            if (parentID != null)
            {
                var parent = await _treeEntityStore.QueryByID(parentID.Value, cancellationToken);
                if (parent == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundTreeEntityByID,
                        DefaultFormatting = "找不到Id为{0}的树状实体记录",
                        ReplaceParameters = new List<object>() { parentID.Value.ToString() }
                    };
                    throw new UtilityException((int)Errors.NotFoundTreeEntityByID, fragment, 1, 0);
                }
                if (parent.Type != 1)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.TreeEntityParentTypeError,
                        DefaultFormatting = "树状实体{0}的类型无法作为父记录，要求的类型为{1}",
                        ReplaceParameters = new List<object>() { parentID.ToString(), "0" }
                    };
                    throw new UtilityException((int)Errors.TreeEntityParentTypeError, fragment, 1, 0);
                }
            }

            await _treeEntityStore.UpdateParent(entity.ID, parentID, cancellationToken);
        }

        private ITreeEntityValueService getValueService(int type)
        {
            if (!ValueServices.TryGetValue(type,out IFactory<ITreeEntityValueService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundTreeEntityValueServiceByType,
                    DefaultFormatting = "找不到类型为{0}的树状实体数据服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { type.ToString(),$"{this.GetType().FullName}.ValueServices" }
                };
                throw new UtilityException((int)Errors.NotFoundTreeEntityValueServiceByType, fragment, 1, 0);
            }

            return serviceFactory.Create();
        }
        private async Task checkDuplicate(TreeEntity treeEntity,Func<Task> action, CancellationToken cancellationToken = default)
        {
            var tree = await _treeEntityStore.QueryByName(treeEntity.ParentID, treeEntity.Name, cancellationToken);
            if (tree != null && tree.ID!=treeEntity.ID)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ExistSameNameTreeEntity,
                    DefaultFormatting = "已经存在名称为{0}的树状实体记录",
                    ReplaceParameters = new List<object>() { treeEntity.Name }
                };
                throw new UtilityException((int)Errors.ExistSameNameTreeEntity, fragment, 1, 0);
            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {

                await action();

                var newId = await _treeEntityStore.QueryByNameNoLock(treeEntity.ParentID, treeEntity.Name, cancellationToken);
                if (newId != treeEntity.ID)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.ExistSameNameTreeEntity,
                        DefaultFormatting = "已经存在名称为{0}的树状实体记录",
                        ReplaceParameters = new List<object>() { treeEntity.Name }
                    };
                    throw new UtilityException((int)Errors.ExistSameNameTreeEntity, fragment, 1, 0);
                }
                scope.Complete();
            }
        }

        public async Task UpdateName(TreeEntity entity, string name, CancellationToken cancellationToken = default)
        {
            var valueService = getValueService(entity.Type);
            await checkDuplicate(entity,
                async () =>
                {
                    await _treeEntityStore.UpdateName(entity.ID,name, cancellationToken);
                    await valueService.UpdateName(name, entity.Value, cancellationToken);
                },
                cancellationToken);
        }
    }
}
