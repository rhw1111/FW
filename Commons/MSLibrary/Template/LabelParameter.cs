using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Template
{
    public class LabelParameter : EntityBase<ILabelParameterIMP>
    {
        private static IFactory<ILabelParameterIMP> _labelParameterIMPFactory;

        public static IFactory<ILabelParameterIMP> LabelParameterIMPFactory
        {
            set
            {
                _labelParameterIMPFactory = value;
            }
        }
        public override IFactory<ILabelParameterIMP> GetIMPFactory()
        {
            return _labelParameterIMPFactory;
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
        /// 标签名称
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
        /// 改标签是否是独立标签
        /// 如果是独立标签，表示即使标签的参数完全一样，每次计算后的参数值也是不一样的
        /// 如果不是独立标签，表示只要标签的参数完全一样，则计算后的参数值是一样的
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsIndividual()
        {
            return await _imp.IsIndividual(this);
        }

        /// <summary>
        /// 计算标签值
        /// </summary>
        /// <param name="context">用到该标签参数的模板上下文</param>
        /// <param name="parameters">标签参数包含的参数值列表</param>
        /// <returns></returns>
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            return await _imp.Execute(this, context, parameters);
        }
    }


    public interface ILabelParameterIMP
    {
        /// <summary>
        /// 改标签是否是独立标签
        /// 如果是独立标签，表示即使标签的参数完全一样，每次计算后的参数值也是不一样的
        /// 如果不是独立标签，表示只要标签的参数完全一样，则计算后的参数值是一样的
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        Task<bool> IsIndividual(LabelParameter label);

        /// <summary>
        /// 计算标签值
        /// </summary>
        /// <param name="label"></param>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<string> Execute(LabelParameter label,TemplateContext context, string[] parameters);
    }


    [Injection(InterfaceType = typeof(ILabelParameterIMP), Scope = InjectionScope.Transient)]
    public class LabelParameterIMP : ILabelParameterIMP
    {
        private static Dictionary<string, IFactory<ILabelParameterHandler>> _handlerFactories=new Dictionary<string, IFactory<ILabelParameterHandler>>();

        /// <summary>
        /// 标签参数处理器工厂键值对
        /// 在系统初始化时需要添加每个处理器
        /// </summary>
        public static Dictionary<string, IFactory<ILabelParameterHandler>> HandlerFactories
        {
            get
            {
                return _handlerFactories;
            }
        }

        public async Task<string> Execute(LabelParameter label, TemplateContext context, string[] parameters)
        {
            var handler= GetHandler(label.Name);
            return await handler.Execute(context, parameters);
        }

        public async Task<bool> IsIndividual(LabelParameter label)
        {
            var handler = GetHandler(label.Name);
            return await handler.IsIndividual();
        }

        private ILabelParameterHandler GetHandler(string name)
        {
            if (!_handlerFactories.TryGetValue(name.ToLower(),out IFactory<ILabelParameterHandler> handlerFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundLabelParameterHandlerByName,
                    DefaultFormatting = "找不到名称为{0}的标签参数处理器",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundLabelParameterHandlerByName, fragment);
            }

            return handlerFactory.Create();
        }
    }

    /// <summary>
    /// 标签参数处理接口
    /// </summary>
    public interface ILabelParameterHandler
    {
        /// <summary>
        /// 计算标签参数值
        /// </summary>
        /// <param name="context">模板上下文</param>
        /// <param name="parameters">标签参数包含的参数列表</param>
        /// <returns>标签参数值</returns>
        Task<string> Execute(TemplateContext context, string[] parameters);
        /// <summary>
        /// 是否是独立的标签
        /// 如果是独立标签，表示即使标签的参数完全一样，每次计算后的参数值也是不一样的
        /// 如果不是独立标签，表示只要标签的参数完全一样，则计算后的参数值是一样的
        /// </summary>
        /// <returns></returns>
        Task<bool> IsIndividual();

    }

}
