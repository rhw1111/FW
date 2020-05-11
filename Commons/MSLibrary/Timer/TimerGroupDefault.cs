using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Timer
{
    /// <summary>
    /// 定时处理组默认实现
    /// </summary>
    [Injection(InterfaceType = typeof(TimerGroupDefault), Scope = InjectionScope.Singleton)]
    public class TimerGroupDefault : TimerGroup
    {
        private Dictionary<string, IFactory<ITimerHandler>> _handlerFactorys = new Dictionary<string, IFactory<ITimerHandler>>();


        public override async Task Execute()
        {
            foreach (var item in _handlerFactorys)
            {
                var handler = item.Value.Create();
                await handler.Do();
            }
        }

        public override void Register(IFactory<ITimerHandler> handlerFactory)
        {
            _handlerFactorys[handlerFactory.GetType().FullName] = handlerFactory;
        }
    }
}
