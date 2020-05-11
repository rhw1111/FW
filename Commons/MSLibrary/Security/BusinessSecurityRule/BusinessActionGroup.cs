using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.BusinessSecurityRule.DAL;

namespace MSLibrary.Security.BusinessSecurityRule
{
    /// <summary>
    /// 业务动作组
    /// 与业务动作为N:N关系
    /// </summary>
    public class BusinessActionGroup : EntityBase<IBusinessActionGroupIMP>
    {
        private static IFactory<IBusinessActionGroupIMP> _businessActionGroupIMPFactory;

        public static IFactory<IBusinessActionGroupIMP> BusinessActionGroupIMPFactory
        {
            set
            {
                _businessActionGroupIMPFactory=value;
            }
        }
        public override IFactory<IBusinessActionGroupIMP> GetIMPFactory()
        {
            return _businessActionGroupIMPFactory;
        }


        /// <summary>
        /// ID
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
        /// 组名称
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
        /// 新增与业务动作关联关系
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task AddActionRelation(Guid actionId)
        {
            await _imp.AddActionRelation(this,actionId);
        }
        /// <summary>
        /// 移除与业务动作关联关系
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task RemoveActionRelation(Guid actionId)
        {
            await _imp.RemoveActionRelation(this, actionId);
        }
        /// <summary>
        /// 获取所有关联的业务动作
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetAllAction(Func<BusinessAction,Task> callback)
        {
            await _imp.GetAllAction(this,callback);
        }
        /// <summary>
        /// 分页获取所有关联的业务动作
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<BusinessAction>> GetAction(int page, int pageSize)
        {
            return await _imp.GetAction(this,page,pageSize);
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
        /// 分页获取没有与自身关联的业务动作
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<BusinessAction>> GetNullRelationAction(int page, int pageSize)
        {
            return await _imp.GetNullRelationAction(this, page, pageSize);
        }
        /// <summary>
        /// 验证指定业务动作组包含的所有动作
        /// </summary>
        /// <param name="group"></param>
        /// <param name="originalInfo"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task ValidateAllAction(Dictionary<string, object> originalParameters, Func<BusinessActionGroupValidateResult, Task> callback)
        {
            await _imp.ValidateAllAction(this,originalParameters,callback);
        }
    }

    /// <summary>
    /// 业务动作组具体实现接口
    /// </summary>
    public interface IBusinessActionGroupIMP
    {
        Task Add(BusinessActionGroup group);
        Task Update(BusinessActionGroup group);
        Task Delete(BusinessActionGroup group);
        Task AddActionRelation(BusinessActionGroup group,Guid actionId);
        Task RemoveActionRelation(BusinessActionGroup group, Guid actionId);
        Task GetAllAction(BusinessActionGroup group,Func<BusinessAction, Task> callback);
        Task<QueryResult<BusinessAction>> GetAction(BusinessActionGroup group,int page, int pageSize);
        Task ValidateAllAction(BusinessActionGroup group,Dictionary<string, object> originalParameters, Func<BusinessActionGroupValidateResult, Task> callback);
        Task<QueryResult<BusinessAction>> GetNullRelationAction(BusinessActionGroup group, int page, int pageSize);
    }

    [Injection(InterfaceType = typeof(IBusinessActionGroupIMP), Scope = InjectionScope.Transient)]
    public class BusinessActionGroupIMP : IBusinessActionGroupIMP
    {
        private IBusinessActionStore _businessActionStore;
        private IBusinessActionGroupStore _businessActionGroupStore;

        public BusinessActionGroupIMP(IBusinessActionStore businessActionStore, IBusinessActionGroupStore businessActionGroupStore)
        {
            _businessActionStore = businessActionStore;
            _businessActionGroupStore = businessActionGroupStore;
        }
        public async Task Add(BusinessActionGroup group)
        {
            await _businessActionGroupStore.Add(group);
        }

        public async Task AddActionRelation(BusinessActionGroup group, Guid actionId)
        {
            await _businessActionStore.AddGroupRelation(actionId, group.ID);
        }

        public async Task Delete(BusinessActionGroup group)
        {
            await _businessActionGroupStore.Delete(group.ID);
        }

        public async Task<QueryResult<BusinessAction>> GetAction(BusinessActionGroup group, int page, int pageSize)
        {
            return await _businessActionStore.QueryByGroup(group.ID, page, pageSize);
        }

        public async Task GetAllAction(BusinessActionGroup group, Func<BusinessAction, Task> callback)
        {
            await _businessActionStore.QueryByGroup(group.ID, callback);
        }

        public async Task<QueryResult<BusinessAction>> GetNullRelationAction(BusinessActionGroup group, int page, int pageSize)
        {
             return await _businessActionStore.QueryByNullRelationGroup(group.ID, page, pageSize);
        }

        public async Task RemoveActionRelation(BusinessActionGroup group, Guid actionId)
        {
            await _businessActionStore.DeleteGroupRelation(actionId, group.ID);
        }

        public async Task Update(BusinessActionGroup group)
        {
            await _businessActionGroupStore.Update(group);
        }

        public async Task ValidateAllAction(BusinessActionGroup group,Dictionary<string, object> originalParameters, Func<BusinessActionGroupValidateResult, Task> callback)
        {
            await GetAllAction(group, async (action) =>
            {
                var actionResult = await action.Validate(originalParameters);

                var result = new BusinessActionGroupValidateResult()
                {
                    Action = action,
                    Result = actionResult
                };

                await callback(result);
            });
        }
    }


    /// <summary>
    /// 业务动作组验证结果
    /// </summary>
    public class BusinessActionGroupValidateResult
    {
        /// <summary>
        /// 业务动作
        /// </summary>
        public BusinessAction Action { get; set; }
        /// <summary>
        /// 验证结果
        /// </summary>
        public ValidateResult Result { get; set; }

    }

}
