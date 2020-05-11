using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;

namespace MSLibrary.Security.RequestController.TrackerStrategyServices
{
    /// <summary>
    /// 实现令牌桶算法的跟踪策略服务
    /// 这里的令牌桶令牌的恢复依赖于exit
    /// </summary>
    [Injection(InterfaceType = typeof(ITrackerStrategyService), Scope = InjectionScope.Singleton)]
    public class TrackerStrategyServiceForTokenBucket : ITrackerStrategyService
    {
        private const string _name = "TokenBucket";

        private AsyncLocal<Dictionary<Guid, Guid>> _execute = new AsyncLocal<Dictionary<Guid, Guid>>();
        public async Task<ValidateResult> Access(RequestTracker tracker)
        {
            ValidateResult result = new ValidateResult()
            {
                Result = true
            };
            if (!tracker.Extensions.TryGetValue(_name, out object objNumber))
            {
                lock (tracker.Extensions)
                {
                    if (!tracker.Extensions.TryGetValue(_name, out objNumber))
                    {
                        objNumber = new Number();
                        tracker.Extensions.Add(_name, objNumber);
                    }
                }
            }

            Number number = (Number)objNumber;

            if (number.Value + 1 > tracker.MaxNumber)
            {
                result.Result = false;
                result.Description = StringLanguageTranslate.Translate(TextCodes.TokenBucketOverflow.ToString(), string.Format("令牌桶超出最大阈值{0}", tracker.MaxNumber));
            }
            else
            {
                lock (number)
                {
                    if (number.Value + 1 > tracker.MaxNumber)
                    {
                        result.Result = false;
                        result.Description = StringLanguageTranslate.Translate(TextCodes.TokenBucketOverflow.ToString(), string.Format("令牌桶超出最大阈值{0}", tracker.MaxNumber));
                    }
                    else
                    {
                        SetExecute(tracker.ID);
                        number.Value++;
                    }
                }
            }


            return await Task.FromResult(result);
        }

        public async Task Exit(RequestTracker tracker)
        {
            if (GetExecute(tracker.ID))
            {
                if (tracker.Extensions.TryGetValue(_name, out object objNumber))
                {
                    Number number = (Number)objNumber;
                    lock (number)
                    {
                        number.Value--;
                    }
                }
            }

            await Task.FromResult(0);
        }

        private void SetExecute(Guid id)
        {
            if (_execute.Value == null)
            {
                _execute.Value = new Dictionary<Guid, Guid>();
            }
            _execute.Value.Add(id, id);
        }

        private bool GetExecute(Guid id)
        {
            if (_execute.Value == null)
            {
                _execute.Value = new Dictionary<Guid, Guid>();
            }
            if (_execute.Value.ContainsKey(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        class Number
        {
            public Number()
            {
                Value = 0;
            }
            public int Value { get; set; }
        }
    }
}
