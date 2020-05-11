using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using MSLibrary.Logger;
using MSLibrary.DI;
using MSLibrary.Template;
using MSLibrary.Security.BusinessSecurityRule.DAL;
using MSLibrary.Security.ConditionController;

namespace MSLibrary.Security.BusinessSecurityRule
{
    /// <summary>
    /// 业务动作，表示一个操作场景
    /// 可以对这个动作进行安全验证
    /// 用来控制业务是否可以处理
    /// </summary>
    public class BusinessAction : EntityBase<IBusinessActionIMP>
    {
        private static IFactory<IBusinessActionIMP> _businessActionIMPFactory;

        public static IFactory<IBusinessActionIMP> BusinessActionIMPFactory
        {
            set
            {
                _businessActionIMPFactory = value;
            }
        }

        public override IFactory<IBusinessActionIMP> GetIMPFactory()
        {
            return _businessActionIMPFactory;
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
        /// 操作的名称
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
        /// 规则内容
        /// </summary>
        public string Rule
        {
            get
            {
                return GetAttribute<string>("Rule");
            }
            set
            {
                SetAttribute<string>("Rule", value);
            }
        }

        /// <summary>
        /// 原始参数过滤处理类型
        /// </summary>
        public string OriginalParametersFilterType
        {
            get
            {
                return GetAttribute<string>("OriginalParametersFilterType");
            }
            set
            {
                SetAttribute<string>("OriginalParametersFilterType", value);
            }
        }

        /// <summary>
        /// 验证失败时替代的提示信息
        /// 采用模板格式
        /// </summary>
        public string ErrorReplaceText
        {
            get
            {
                return GetAttribute<string>("ErrorReplaceText");
            }
            set
            {
                SetAttribute<string>("ErrorReplaceText", value);
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


        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Update()
        {
            await _imp.Update(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        public async Task<QueryResult<BusinessActionGroup>> GetGroup(int page, int pageSize)
        {
            return await _imp.GetGroup(this, page, pageSize);
        }

        /// <summary>
        /// 验证当前动作是否通过验证
        /// </summary>
        /// <param name="originalParameters">原始参数列表</param>
        /// <returns></returns>
        public async Task<ValidateResult> Validate(Dictionary<string, object> originalParameters)
        {
            return await _imp.Validate(this,originalParameters);
        }
        /// <summary>
        /// 验证当前动作是否通过验证(直接传入参数，不通过过滤函数)
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public async Task<ValidateResult> ValidateDirect(Dictionary<string, object> parameters)
        {
            return await _imp.ValidateDirect(this, parameters);
        }
    }


    /// <summary>
    /// 业务动作原始参数信息过滤处理服务接口
    /// </summary>
    public interface IOriginalParametersFilterService
    {
        Task<Dictionary<string, object>> Execute(Dictionary<string, object> originalParameters);
    }


    /// <summary>
    /// 业务动作具体实现接口
    /// </summary>
    public interface IBusinessActionIMP
    {
        Task Add(BusinessAction action);
        Task Update(BusinessAction action);
        Task Delete(BusinessAction action);
        Task<QueryResult<BusinessActionGroup>> GetGroup(BusinessAction action,int page, int pageSize);
        /// <summary>
        ///  验证当前动作是否通过验证
        /// </summary>
        /// <param name="action"></param>
        /// <param name="originalParameters"></param>
        /// <returns></returns>
        Task<ValidateResult> Validate(BusinessAction action, Dictionary<string, object> originalParameters);
        /// <summary>
        /// 验证当前动作是否通过验证(直接传入参数，不通过过滤函数)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="originalParameters"></param>
        /// <returns></returns>
        Task<ValidateResult> ValidateDirect(BusinessAction action, Dictionary<string, object> parameters);
    }

    [Injection(InterfaceType = typeof(IBusinessActionIMP), Scope = InjectionScope.Transient)]
    public class BusinessActionIMP : IBusinessActionIMP
    {
        private static string _informationCategory;
        public static string InformationCategory
        {
            set
            {
                _informationCategory = value;
            }
        }

        private ISelector<IOriginalParametersFilterService> _originalParametersFilterServiceSelector;
        private IBusinessActionStore _businessActionStore;
        private IBusinessActionGroupStore _businessActionGroupStore;
        private IConditionService _conditionService;
        private ITemplateService _templateService;

        public BusinessActionIMP(ISelector<IOriginalParametersFilterService> originalParametersFilterServiceSelector, IBusinessActionStore businessActionStore, IBusinessActionGroupStore businessActionGroupStore, IConditionService conditionService, ITemplateService templateService)
        {
            _originalParametersFilterServiceSelector = originalParametersFilterServiceSelector;
            _businessActionStore = businessActionStore;
            _businessActionGroupStore = businessActionGroupStore;
            _conditionService = conditionService;
            _templateService = templateService;
        }

        public async Task Add(BusinessAction action)
        {
            await _businessActionStore.Add(action);
        }

        public async Task Delete(BusinessAction action)
        {
            await _businessActionStore.Delete(action.ID);
        }

        public async Task<QueryResult<BusinessActionGroup>> GetGroup(BusinessAction action, int page, int pageSize)
        {
            return await _businessActionGroupStore.QueryByAction(action.ID, page, pageSize);
        }

        public async Task Update(BusinessAction action)
        {
            await _businessActionStore.Update(action);
        }

        public async Task<ValidateResult> Validate(BusinessAction action,Dictionary<string, object> originalParameters)
        {

            //获取参数信息过滤器
            var filter = _originalParametersFilterServiceSelector.Choose(action.OriginalParametersFilterType);

            Dictionary<string, object> parameters = null;
            if (filter != null)
            {
                //获取过滤后的参数信息
                parameters = await filter.Execute(originalParameters);
            }


    

            //获取规则信息,转成xml
            var doc = XDocument.Parse(action.Rule);


            var result= await _conditionService.Validate(doc, parameters);
            //如果需要替换
            if (!result.Result && !string.IsNullOrEmpty(action.ErrorReplaceText))
            {
                LoggerHelper.LogInformation(_informationCategory, $"BusinessAction {action.Name} validate fail,detail:{result.Description}");

                var lcId = ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);
                TemplateContext templateContext = new TemplateContext(lcId, parameters);
                result.Description=await _templateService.Convert(action.ErrorReplaceText, templateContext);
            }

            return result;
        }

        public async Task<ValidateResult> ValidateDirect(BusinessAction action, Dictionary<string, object> parameters)
        {
            //获取规则信息,转成xml
            var doc = XDocument.Parse(action.Rule);


            var result = await _conditionService.Validate(doc, parameters);
            //如果需要替换
            if (!result.Result && !string.IsNullOrEmpty(action.ErrorReplaceText))
            {
                LoggerHelper.LogInformation(_informationCategory, $"BusinessAction {action.Name} vlidate fail,detail:{result.Description}");

                var lcId = ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);
                TemplateContext templateContext = new TemplateContext(lcId, parameters);
                result.Description = await _templateService.Convert(action.ErrorReplaceText, templateContext);
            }

            return result;
        }
    }
}
