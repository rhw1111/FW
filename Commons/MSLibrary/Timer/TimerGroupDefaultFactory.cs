using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Timer
{
    /// <summary>
    /// 定时处理组默认实现的工厂
    /// </summary>
    [Injection(InterfaceType = typeof(TimerGroupDefaultFactory), Scope = InjectionScope.Singleton)]
    public class TimerGroupDefaultFactory : IFactory<TimerGroup>
    {
        private TimerGroupDefault _timerGroupDefault;

        public TimerGroupDefaultFactory(TimerGroupDefault timerGroupDefault)
        {
            _timerGroupDefault = timerGroupDefault;
        }
        public TimerGroup Create()
        {
            return _timerGroupDefault;
        }
    }
}
