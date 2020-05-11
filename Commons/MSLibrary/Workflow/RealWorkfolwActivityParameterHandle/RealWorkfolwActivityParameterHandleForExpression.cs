using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwExpressionHandle;

namespace MSLibrary.Workflow.RealWorkfolwActivityParameterHandle
{
    /// <summary>
    /// 针对表达式的参数处理
    /// </summary>
    [Injection(InterfaceType = typeof(RealWorkfolwActivityParameterHandleForExpression), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityParameterHandleForExpression : IRealWorkfolwActivityParameterHandle
    {
        private static Regex _regRoot = new Regex(@"^\$([A-Aa-z0-9_-]+)\((.*)\)$");
        private static Regex _regExpression = new Regex(@"\$([A-Aa-z0-9_-]+)\([^((?<!\\)\()((?<!\\)\))]*(((?'Open'\$([A-Aa-z0-9_-]+)\()[^((?<!\\)\()((?<!\\)\))]*)+((?'-Open'\))[^((?<!\\)\()((?<!\\)\))]*)+)*(?(Open)(?!))\)");
        private static Regex _regParSep = new Regex(@"(?<!\\),");
        private IRealWorkfolwExpressionHandleSelector _realWorkfolwExpressionHandleSelector;

        public RealWorkfolwActivityParameterHandleForExpression(IRealWorkfolwExpressionHandleSelector realWorkfolwExpressionHandleSelector)
        {
            _realWorkfolwExpressionHandleSelector = realWorkfolwExpressionHandleSelector;
        }
        public async Task<object> Execute(RealWorkfolwActivityParameter parameter, RealWorkfolwContext context)
        {
            RealWorkfolwActivityParameterExpressionDescription parameterDescription = null;
            if (!parameter.Extensions.TryGetValue("description",out object objDescription))
            {
                parameterDescription = await Resolve(parameter, parameter.Configuration);
                parameter.Extensions["description"] = parameterDescription;
            }
            else
            {
                parameterDescription = (RealWorkfolwActivityParameterExpressionDescription)objDescription;
            }
            return await Calculate(parameterDescription, context);
        }

        private async Task<object> Calculate(RealWorkfolwActivityParameterExpressionDescription description, RealWorkfolwContext context)
        {
            if (!context.ParameterExpressionResults.TryGetValue(description.Expression, out object result))
            {
                List<object> inputs = new List<object>();
                foreach (var paraitem in description.Parameters)
                {
                    if (paraitem is RealWorkfolwActivityParameterExpressionDescription)
                    {
                        inputs.Add(await Calculate((RealWorkfolwActivityParameterExpressionDescription)paraitem, context));
                    }
                    else
                    {
                        inputs.Add(paraitem.ToString());
                    }
                }

                var expressionHandle = _realWorkfolwExpressionHandleSelector.Choose(description.Name);
                result=await expressionHandle.Execute(inputs, context);

                context.ParameterExpressionResults[description.Expression] = result;

            }
            return result;
        }

        private async Task<RealWorkfolwActivityParameterExpressionDescription> Resolve(RealWorkfolwActivityParameter parameter, string expression)
        {
            RealWorkfolwActivityParameterExpressionDescription result = new RealWorkfolwActivityParameterExpressionDescription();
            result.Expression = expression;

            var rootMatch = _regRoot.Match(expression);

            if (rootMatch.Success == false)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityParameterHandleExecuteError,
                    DefaultFormatting = "工作流活动参数处理出错，错误原因：{0}，参数名称：{1}，参数数据类型：{2}，参数表达式类型：{3}，参数配置：{4}，发生位置：{5}",
                    ReplaceParameters = new List<object>() { parameter.Name, parameter.DataType, parameter.ExpressionType, parameter.Configuration, $"{this.GetType().FullName}.Execute" }
                };

                throw new UtilityException((int)Errors.RealWorkfolwActivityParameterHandleExecuteError, fragment);
            }
            string rootName;
            string strRootName = rootMatch.Groups[1].Value;
            var arrayStrRootName = strRootName.Split('_');
            rootName = arrayStrRootName[0];
            result.Name = rootName;
            var expressions = _regExpression.Matches(rootMatch.Groups[2].Value);




            Dictionary<int, RealWorkfolwActivityParameterExpressionDescription> expressionParameters = new Dictionary<int, RealWorkfolwActivityParameterExpressionDescription>();

            int parIndex = 0;
            for (var index = 0; index <= expressions.Count - 1; index++)
            {
                expressionParameters[parIndex] = await Resolve(parameter, expressions[index].Value);
                parIndex++;

            }

            var newStrPars = _regExpression.Replace(rootMatch.Groups[2].Value, @"\n");

            var arrayParas = _regParSep.Split(newStrPars);

            parIndex = 0;
            List<object> inputs = new List<object>();
            foreach (var itemPara in arrayParas)
            {
                if (itemPara == @"\n")
                {
                    inputs.Add(expressionParameters[parIndex]);
                }
                else
                {
                    inputs.Add(itemPara.Replace(@"\,", @",").Replace(@"\)", @")").Replace(@"\(", @"(").Replace(@"\\", @"\"));
                }
            }

            result.Parameters = inputs;

            return result;
        }

    }


    public class RealWorkfolwActivityParameterExpressionDescription
    {
        public string Name { get; set; }
        public string Expression { get; set; }

        public List<object> Parameters { get; set; }
    }
}
