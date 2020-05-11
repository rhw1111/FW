﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve;
using MSLibrary.Workflow.RealWorkfolwActivityParameterHandle;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Calculate
{
    [Injection(InterfaceType = typeof(RealWorkfolwActivityCalculateForWhile), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityCalculateForWhile : IRealWorkfolwActivityCalculate
    {
        private IRealWorkfolwActivityParameterHandle _realWorkfolwActivityParameterHandle;
        private IRealWorkfolwActivityCalculate _realWorkfolwActivityCalculate;

        public RealWorkfolwActivityCalculateForWhile(IRealWorkfolwActivityParameterHandle realWorkfolwActivityParameterHandle, IRealWorkfolwActivityCalculate realWorkfolwActivityCalculate)
        {
            _realWorkfolwActivityParameterHandle = realWorkfolwActivityParameterHandle;
            _realWorkfolwActivityCalculate = realWorkfolwActivityCalculate;
        }

        public async Task<Dictionary<string, object>> Execute(RealWorkfolwActivityDescription activityDescription, RealWorkfolwContext context, Dictionary<string, object> runtimeParameters)
        {
            Exception exception;
            RealWorkfolwActivityDescriptionDataForWhile data = activityDescription.Data as RealWorkfolwActivityDescriptionDataForWhile;
            if (data == null)
            {
                string realType;
                if (activityDescription.Data == null)
                {
                    realType = "null";
                }
                else
                {
                    realType = activityDescription.Data.GetType().FullName;
                }

                var fragment = new TextFragment()
                {
                    Code = TextCodes.RealWorkfolwActivityDescriptionDataTypeNotMatch,
                    DefaultFormatting = "工作流活动描述的Data属性的类型不匹配，期待的类型为{0}，实际类型为{1}，发生位置：{2}",
                    ReplaceParameters = new List<object>() { typeof(RealWorkfolwActivityDescriptionDataForWhile).FullName, realType, $"{this.GetType().FullName}.Execute" }
                };

                throw new UtilityException((int)Errors.RealWorkfolwActivityDescriptionDataTypeNotMatch, fragment);
            }

            if (!activityDescription.InputParameters.TryGetValue("condition", out RealWorkfolwActivityParameter conditionParameter))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRealWorkfolwActivityDescriptionInputByName,
                    DefaultFormatting = "名称为{0}的工作流描述中找不到名称为{1}的输入参数",
                    ReplaceParameters = new List<object>() { activityDescription.Name, "condition" }
                };

                exception = new UtilityException((int)Errors.NotFoundRealWorkfolwActivityDescriptionInputByName, fragment);
                exception.Data[UtilityExceptionDataKeys.Catch] = true;
                throw exception;
            }

            while(true)
            {
                string newName = string.Empty; ;
                string[] arrayName = conditionParameter.Name.Split('_');
                if (arrayName.Length>1)
                {
                    arrayName[arrayName.Length - 1] = Guid.NewGuid().ToString();
                    foreach(var itemName in arrayName)
                    {
                        if (newName == string.Empty)
                        {
                            newName = itemName;
                        }
                        else
                        {
                            newName = $"{newName}_{itemName}";
                        }              
                    }
                }
                conditionParameter.Name = newName;

                var conditionParameterResultObj = await _realWorkfolwActivityParameterHandle.Execute(conditionParameter, context);

                var conditionParameterResult = conditionParameterResultObj as bool?;
                if (conditionParameterResult == null)
                {
                    string realType;
                    if (conditionParameterResultObj == null)
                    {
                        realType = null;
                    }
                    else
                    {
                        realType = conditionParameterResultObj.GetType().FullName;
                    }

                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.RealWorkfolwActivityDescriptionInputResultTypeNotMatch,
                        DefaultFormatting = "名称为{0}的工作流描述中名称为{1}的输入参数的结果值类型不匹配，期待的类型为{2}，实际类型为{3},参数配置为{4}",
                        ReplaceParameters = new List<object>() { activityDescription.Name, "condition", typeof(bool).FullName, realType, conditionParameter.Configuration }
                    };

                    exception = new UtilityException((int)Errors.RealWorkfolwActivityDescriptionInputResultTypeNotMatch, fragment);
                    exception.Data[UtilityExceptionDataKeys.Catch] = true;
                    throw exception;
                }

                if (conditionParameterResult.Value)
                {
                    await ExecuteMatch(data.Activities, context);
                }
                else
                {
                    break;
                }

            }

            Dictionary<string, object> outputDict = new Dictionary<string, object>();
            foreach (var outputItem in activityDescription.OutputParameters)
            {
                var outputResult = await _realWorkfolwActivityParameterHandle.Execute(outputItem.Value, context);
                outputDict[outputItem.Key] = outputResult;
            }

            return outputDict;

        }


        public async Task ExecuteMatch(List<RealWorkfolwActivityDescription> descriptions, RealWorkfolwContext context)
        {
            foreach (var matchItem in descriptions)
            {
                var matchItemResult = await _realWorkfolwActivityCalculate.Execute(matchItem, context,new Dictionary<string, object>());
                foreach (var resultItem in matchItemResult)
                {
                    if (!context.ActivityInnerOutputParameters.TryGetValue($"{matchItem.Id.ToString()}_{resultItem.Key}", out List<object> outputResutList))
                    {
                        lock (context.ActivityInnerOutputParameters)
                        {
                            if (!context.ActivityInnerOutputParameters.TryGetValue($"{matchItem.Id.ToString()}_{resultItem.Key}", out outputResutList))
                            {
                                outputResutList = new List<object>();
                                context.ActivityInnerOutputParameters[$"{matchItem.Id.ToString()}_{resultItem.Key}"] = outputResutList;
                            }
                        }
                    }

                    lock (outputResutList)
                    {
                        outputResutList.Add(resultItem.Value);
                    }
                }

            }
        }
    }
}
