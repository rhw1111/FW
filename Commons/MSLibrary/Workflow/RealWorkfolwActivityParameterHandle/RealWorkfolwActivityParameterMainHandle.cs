using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwActivityParameterDataHandle;

namespace MSLibrary.Workflow.RealWorkfolwActivityParameterHandle
{
    [Injection(InterfaceType = typeof(IRealWorkfolwActivityParameterHandle), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityParameterMainHandle : IRealWorkfolwActivityParameterHandle
    {
        private static Dictionary<string, IFactory<IRealWorkfolwActivityParameterHandle>> _parameterHandleFactories = new Dictionary<string, IFactory<IRealWorkfolwActivityParameterHandle>>();

        public static Dictionary<string, IFactory<IRealWorkfolwActivityParameterHandle>> ParameterHandleFactories
        {
            get
            {
                return _parameterHandleFactories;
            }
        }
        private IRealWorkfolwActivityParameterDataHandleSelector _realWorkfolwActivityParameterDataHandleSelector;

        public RealWorkfolwActivityParameterMainHandle(IRealWorkfolwActivityParameterDataHandleSelector realWorkfolwActivityParameterDataHandleSelector)
        {
            _realWorkfolwActivityParameterDataHandleSelector = realWorkfolwActivityParameterDataHandleSelector;
        }
        public async Task<object> Execute(RealWorkfolwActivityParameter parameter, RealWorkfolwContext context)
        {
            if (!_parameterHandleFactories.TryGetValue(parameter.ExpressionType,out IFactory<IRealWorkfolwActivityParameterHandle> handleFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRealWorkfolwActivityParameterHandleByType,
                    DefaultFormatting = "找不到表达式类型为{0}的工作流活动参数处理",
                    ReplaceParameters = new List<object>() { parameter.ExpressionType }
                };

                throw new UtilityException((int)Errors.NotFoundRealWorkfolwActivityParameterHandleByType,fragment);
            }
            var value=await handleFactory.Create().Execute(parameter, context);
            var dataHandle=_realWorkfolwActivityParameterDataHandleSelector.Choose(parameter.DataType);
            var result=await dataHandle.Convert(value);
            if (!await dataHandle.ValidateType(result))
            {
                string strResult;
                if (result==null)
                {
                    strResult = "null";
                }
                else
                {
                    strResult = result.GetType().FullName;
                }

                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityParameterDataHandleValidateError,
                    DefaultFormatting = "工作流活动参数数据处理验证失败，验证器数据类型为{0}，要验证的实际数据类型为{1}，参数名称为{2}",
                    ReplaceParameters = new List<object>() { parameter.DataType, strResult, parameter.Name }
                };

                var exception= new UtilityException((int)Errors.RealWorkfolwActivityParameterDataHandleValidateError, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }
            return result;
        }
    }


}
