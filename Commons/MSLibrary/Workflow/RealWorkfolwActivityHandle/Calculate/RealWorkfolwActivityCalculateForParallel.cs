using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve;
using MSLibrary.Workflow.RealWorkfolwActivityParameterHandle;
using MSLibrary.Thread;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Calculate
{
    [Injection(InterfaceType = typeof(RealWorkfolwActivityCalculateForParallel), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityCalculateForParallel : IRealWorkfolwActivityCalculate
    {
        private IRealWorkfolwActivityParameterHandle _realWorkfolwActivityParameterHandle;
        private IRealWorkfolwActivityCalculate _realWorkfolwActivityCalculate;

        public RealWorkfolwActivityCalculateForParallel(IRealWorkfolwActivityParameterHandle realWorkfolwActivityParameterHandle, IRealWorkfolwActivityCalculate realWorkfolwActivityCalculate)
        {
            _realWorkfolwActivityParameterHandle = realWorkfolwActivityParameterHandle;
            _realWorkfolwActivityCalculate = realWorkfolwActivityCalculate;
        }

        public async Task<Dictionary<string, object>> Execute(RealWorkfolwActivityDescription activityDescription, RealWorkfolwContext context, Dictionary<string, object> runtimeParameters)
        {
            RealWorkfolwActivityDescriptionDataForParallel data = activityDescription.Data as RealWorkfolwActivityDescriptionDataForParallel;
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
                    ReplaceParameters = new List<object>() { typeof(RealWorkfolwActivityDescriptionDataForParallel).FullName, realType, $"{this.GetType().FullName}.Execute" }
                };

                throw new UtilityException((int)Errors.RealWorkfolwActivityDescriptionDataTypeNotMatch,fragment);
            }

            ParallelHelper parallel = new ParallelHelper(data.Max);

            List<RunAsyncAction> actionList = new List<RunAsyncAction>();
            

            foreach(var item in data.Items)
            {
                RunAsyncAction action = new RunAsyncAction()
                {
                    Action = async () =>
                      {
                          await ExecuteMatch(item.Activities, context);
                      },
                    ErrorHandler = async (ex) =>
                      {
                          Dictionary<string, object> newRuntimeParameters = new Dictionary<string, object>();
                          newRuntimeParameters.Add("exception", ex);

                          var errorHandleResult = await _realWorkfolwActivityCalculate.Execute(item.ErrorHandle, context, newRuntimeParameters);
                          foreach (var resultItem in errorHandleResult)
                          {
                              if (!context.ActivityInnerOutputParameters.TryGetValue($"{item.ErrorHandle.Id.ToString()}_{resultItem.Key}", out List<object> outputResutList))
                              {
                                  lock (context.ActivityInnerOutputParameters)
                                  {
                                      if (!context.ActivityInnerOutputParameters.TryGetValue($"{item.ErrorHandle.Id.ToString()}_{resultItem.Key}", out outputResutList))
                                      {
                                          outputResutList = new List<object>();
                                          context.ActivityInnerOutputParameters[$"{item.ErrorHandle.Id.ToString()}_{resultItem.Key}"] = outputResutList;
                                      }
                                  }
                              }

                              lock (outputResutList)
                              {
                                  outputResutList.Add(resultItem.Value);
                              }
                          }
                      }
                };
            }

            await parallel.RunAsync(actionList);

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
