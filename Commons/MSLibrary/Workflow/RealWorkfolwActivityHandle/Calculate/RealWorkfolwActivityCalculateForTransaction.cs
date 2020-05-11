using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Workflow.RealWorkfolwActivityHandle.Resolve;
using MSLibrary.Workflow.RealWorkfolwActivityParameterHandle;
using MSLibrary.Transaction;

namespace MSLibrary.Workflow.RealWorkfolwActivityHandle.Calculate
{
    [Injection(InterfaceType = typeof(RealWorkfolwActivityCalculateForTransaction), Scope = InjectionScope.Singleton)]
    public class RealWorkfolwActivityCalculateForTransaction : IRealWorkfolwActivityCalculate
    {
        private IRealWorkfolwActivityParameterHandle _realWorkfolwActivityParameterHandle;
        private IRealWorkfolwActivityCalculate _realWorkfolwActivityCalculate;

        public RealWorkfolwActivityCalculateForTransaction(IRealWorkfolwActivityParameterHandle realWorkfolwActivityParameterHandle, IRealWorkfolwActivityCalculate realWorkfolwActivityCalculate)
        {
            _realWorkfolwActivityParameterHandle = realWorkfolwActivityParameterHandle;
            _realWorkfolwActivityCalculate = realWorkfolwActivityCalculate;
        }
        public async Task<Dictionary<string, object>> Execute(RealWorkfolwActivityDescription activityDescription, RealWorkfolwContext context, Dictionary<string, object> runtimeParameters)
        {
            RealWorkfolwActivityDescriptionDataForTransaction data = activityDescription.Data as RealWorkfolwActivityDescriptionDataForTransaction;
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
                    ReplaceParameters = new List<object>() { typeof(RealWorkfolwActivityDescriptionDataForTransaction).FullName, realType, $"{this.GetType().FullName}.Execute" }
                };

                throw new UtilityException((int)Errors.RealWorkfolwActivityDescriptionDataTypeNotMatch, fragment);
            }

            TransactionScopeOption scope;
            IsolationLevel level;
            TransactionOptions transactionOptions = new TransactionOptions();

            switch (data.Scope)
            {
                case "1":
                    scope = TransactionScopeOption.Required;
                    break;
                case "2":
                    scope = TransactionScopeOption.RequiresNew;
                    break;
                case "3":
                    scope = TransactionScopeOption.Suppress;
                    break;
                default:
                    scope = TransactionScopeOption.Required;
                    break;
            }

            switch (data.Level)
            {
                case "1":
                    level = IsolationLevel.ReadCommitted;
                    break;
                case "2":
                    level = IsolationLevel.ReadUncommitted;
                    break;
                case "3":
                    level = IsolationLevel.RepeatableRead;
                    break;
                case "4":
                    level = IsolationLevel.Serializable;
                    break;
                case "5":
                    level = IsolationLevel.Snapshot;
                    break;
                case "6":
                    level = IsolationLevel.Unspecified;
                    break;
                case "7":
                    level = IsolationLevel.Chaos;
                    break;
                default:
                    level = IsolationLevel.ReadCommitted;
                    break;

            }

            transactionOptions.IsolationLevel = level;
            if (data.Timeout.HasValue)
            {
                transactionOptions.Timeout = data.Timeout.Value;
            }

            await using (DBTransactionScope transactionScope = new DBTransactionScope(scope, transactionOptions))
            {
                await ExecuteMatch(data.Activities, context);
                transactionScope.Complete();
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
                var matchItemResult = await _realWorkfolwActivityCalculate.Execute(matchItem, context, new Dictionary<string, object>());
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
