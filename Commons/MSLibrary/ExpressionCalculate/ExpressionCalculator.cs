using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Thread;

namespace MSLibrary.ExpressionCalculate
{
    /// <summary>
    /// 表达式计算器
    /// </summary>
    public class ExpressionCalculator : EntityBase<IExpressionCalculatorIMP>
    {
        private static IFactory<IExpressionCalculatorIMP> _expressionCalculatorIMP;

        public static IFactory<IExpressionCalculatorIMP> ExpressionCalculatorIMPFactory
        {
            set
            {
                _expressionCalculatorIMP = value;
            }
        }
        public override IFactory<IExpressionCalculatorIMP> GetIMPFactory()
        {
            return _expressionCalculatorIMP;
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
        /// 公式执行并行度
        /// </summary>
        public int FormulaExecuteParallelism
        {
            get
            {
                return GetAttribute<int>("FormulaExecuteParallelism");
            }
            set
            {
                SetAttribute<int>("FormulaExecuteParallelism", value);
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
        /// 计算表达式
        /// 表达式格式为{$公式名称(参数1,参数2...)},可以嵌套公式，
        /// 特殊字符为} $ \ ,,需要在前面加\转义
        /// 公式名称为字符或数字开头的字母数字下划线组合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<object> Calcualte(string expression,object executeContext)
        {
            return await _imp.Calcualte(this, expression, executeContext);
        }
    }

    public interface IExpressionCalculatorIMP
    {
        Task<object> Calcualte(ExpressionCalculator calculator,string expression,object executeContext);
    }

    [Injection(InterfaceType = typeof(IExpressionCalculatorIMP), Scope = InjectionScope.Transient)]
    public class ExpressionCalculatorIMP : IExpressionCalculatorIMP
    {
        private static Dictionary<string, IDictionary<string, IFactory<IFormulaCalculateService>>> _formulaCalculateServiceFactories = new Dictionary<string, IDictionary<string, IFactory<IFormulaCalculateService>>>();

        public static IDictionary<string, IDictionary<string, IFactory<IFormulaCalculateService>>> FormulaCalculateServiceFactories
        {
            get
            {
                return _formulaCalculateServiceFactories;
            }
        }
        public async Task<object> Calcualte(ExpressionCalculator calculator, string expression,object executeContext)
        {
            if (string.IsNullOrEmpty(expression))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ExpressionEmptyInExpressionCalculatorByName,
                    DefaultFormatting = "在名称为{0}的表达式计算器中，要计算的表达式为空",
                    ReplaceParameters = new List<object>() { calculator.Name }
                };

                throw new UtilityException((int)Errors.ExpressionEmptyInExpressionCalculatorByName, fragment);
            }

            //公式的正则表达式
            Regex reg = new Regex(@"\{(?<!\\)\$[A-Za-z0-9]((?!(\{(?<!\\)\$[A-Za-z0-9][A-Za-z0-9_]+\(.*\)(?<!\\)\})).)+?(?<!\\)\}");
            //公式参数_ref的正则表达式
            Regex regRefExpression = new Regex(@"(?<!\\)\$_ref\((.*)\)");
            ///公式参数_ref的参数的正则表达式
            //Regex regRefParameterExpression = new Regex(@"(?<=\$_ref\().*(?=\))");

            ConcurrentDictionary<string, object> dictValues = new ConcurrentDictionary<string, object>();

            while (true)
            {
                //通过正则表达式获取公式参数
                var matchs = reg.Matches(expression);
                int length = matchs.Count;

                if (length == 0)
                {
                    break;
                }

                var matchList = matchs.Cast<Match>();

          
                ConcurrentDictionary<int, string> dictResult = new ConcurrentDictionary<int, string>();

                Dictionary<string, Semaphore> lockObjs = new Dictionary<string, Semaphore>();

                if (calculator.FormulaExecuteParallelism > 1)
                {
                    //生成锁定对象集
                    foreach (var item in matchList)
                    {
                        lockObjs[item.Value] = new Semaphore(1, 1);
                    }
                }

                //设定并行计算

                await ParallelHelper.ForEach<Match>(matchList, calculator.FormulaExecuteParallelism, async (match) =>
                {
                    //获取公式

                    var formula = getFormulaCalculator(calculator, match.Value);

                    var formulaService = getFormulaCalculateService(calculator.Name, formula.Name);

                    //检查公式参数，如果参数格式为$_ref(XXX),则值从dictValues中获取，
                    //如果为一般字符串，则需调用对应公式服务的ParameterConvert方法来获取实际值

                    List<object> formulaParameters = new List<object>();

                    for (var index=0;index<= formula.ParameterExpressions.Count-1;index++)
                    {
                        var peMatch= regRefExpression.Match(formula.ParameterExpressions[index]);
                        if(peMatch.Success)
                        {

                            if (!dictValues.TryGetValue(peMatch.Groups[1].Value,out object pValue))
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.NotFoundValueInExpressionStoreValues,
                                    DefaultFormatting = "在名称为{0}的表达式计算器中，找不到键为{1}的值，表达式为{2}",
                                    ReplaceParameters = new List<object>() { calculator.Name, peMatch.Groups[1].Value, peMatch.Value }
                                };

                                throw new UtilityException((int)Errors.NotFoundValueInExpressionStoreValues, fragment);
                
                            }

                            formulaParameters.Add(pValue);

                        }
                        else
                        {
                            formulaParameters.Add(await formulaService.ParameterConvert(index, formula.ParameterExpressions[index].Replace(@"\}", "}").Replace(@"\$", "$").Replace(@"\,", ",").Replace(@"\\", @"\")));
                        }
                        
                    }

                    //判断是否可重复使用
                    //如果可重复使用，则通过公式名称存储
                    //如果不可重复使用，则通过guid作为名称存储
                    if (await formulaService.IsIndividual())
                    {
                        if (!dictValues.TryGetValue(formula.Name, out object value))
                        {
                            try
                            {
                                if (calculator.FormulaExecuteParallelism > 1)
                                {
                                    lockObjs[match.Value].WaitOne();
                                }
                                if (!dictValues.TryGetValue(formula.Name, out value))
                                {
                                    //计算公式
                                    var formulaValue = await formulaService.Calculate(formulaParameters,executeContext);
                                    dictValues[formula.Name] = formulaValue;
                                }
                            }
                            finally
                            {
                                if (calculator.FormulaExecuteParallelism > 1)
                                {
                                    lockObjs[match.Value].Release();
                                }
                            }



                        }

                        dictResult[match.Index]=string.Format(@"$_ref({0})", formula.Name);
                    }
                    else
                    {
                        //计算公式
                        var formulaValue = await formulaService.Calculate(formulaParameters,executeContext);
                        string calculatedFormulaName = Guid.NewGuid().ToString();
                        dictValues[calculatedFormulaName] = formulaValue;
                        dictResult[match.Index]= string.Format(@"$_ref({0})", calculatedFormulaName);
                    }




                });

                //为每个匹配项做替换
                expression = reg.Replace(expression, (match) =>
                {

                    if (dictResult.TryGetValue(match.Index, out string matchResult))
                    {
                        return matchResult;
                    }
                    else
                    {
                        return match.Value;
                    }
                });
            }

            //content = content.Replace(@"\}", "}").Replace(@"\$", "$").Replace(@"\,", ",").Replace(@"\\", @"\");

            var lastMatch = regRefExpression.Match(expression);
            if (!lastMatch.Success)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ExpressionFormatError,
                    DefaultFormatting = "在名称为{0}的表达式计算器中，表达式格式不正确，表达式为{1}",
                    ReplaceParameters = new List<object>() { calculator.Name, expression }
                };

                throw new UtilityException((int)Errors.ExpressionFormatError, fragment);
            }

            if (!dictValues.TryGetValue(lastMatch.Groups[1].Value, out object value))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundValueInExpressionStoreValues,
                    DefaultFormatting = "在名称为{0}的表达式计算器中，找不到键为{1}的值，表达式为{2}",
                    ReplaceParameters = new List<object>() { calculator.Name, lastMatch.Groups[1].Value, expression }
                };

                throw new UtilityException((int)Errors.NotFoundValueInExpressionStoreValues, fragment);
            }

            return value;

        }

        private FormulaCalculator getFormulaCalculator(ExpressionCalculator calculator,string expression)
        {
            //分割出标表达式和参数
            Regex reg = new Regex(@"\{\$([A-Za-z0-9][A-Za-z0-9_]+)\((.*)\)\}", RegexOptions.None);
            var match = reg.Match(expression);

            if (match == null || !match.Success || match.Groups.Count != 3)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ExpressionFormatError,
                    DefaultFormatting = "在名称为{0}的表达式计算器中，表达式格式不正确，表达式为{1}",
                    ReplaceParameters = new List<object>() { calculator.Name, expression }
                };

                throw new UtilityException((int)Errors.ExpressionFormatError, fragment);
            }



            //获取表达式名
            var strName = match.Groups[1].Value;
            //获取参数集
            var strParameters = match.Groups[2].Value;

            //分割多个参数
            Regex regex = new Regex(@"(?<!\\),");
            var arrayParamaters = regex.Split(strParameters);

            FormulaCalculator formula = new FormulaCalculator()
            {
                 Name= strName
            };

            foreach(var item in arrayParamaters)
            {
                formula.ParameterExpressions.Add(item);
            }


            return formula;
        }

        private IFormulaCalculateService getFormulaCalculateService(string calculatorName,string formulaName)
        {
            if (!_formulaCalculateServiceFactories.TryGetValue(calculatorName,out IDictionary<string,IFactory<IFormulaCalculateService>> serviceFactories))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundFormulaServiceListFromExpression,
                    DefaultFormatting = "找不到名称为{0}的表达式所对应的公式服务列表",
                    ReplaceParameters = new List<object>() { calculatorName }
                };

                throw new UtilityException((int)Errors.NotFoundFormulaServiceListFromExpression, fragment);
            }

            if (!serviceFactories.TryGetValue(formulaName,out IFactory<IFormulaCalculateService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundFormulaServiceFormServiceList,
                    DefaultFormatting = "名称为{0}的表达式所对应的公式服务列表中，找不到公式名称为{1}的公式服务",
                    ReplaceParameters = new List<object>() { calculatorName,formulaName }
                };

                throw new UtilityException((int)Errors.NotFoundFormulaServiceFormServiceList, fragment);
            }

            return serviceFactory.Create();
        }

        private class FormulaCalculator
        {
            public string Name { get; set; }

            public List<string> ParameterExpressions { get; set; } = new List<string>();
        }
    }
}
