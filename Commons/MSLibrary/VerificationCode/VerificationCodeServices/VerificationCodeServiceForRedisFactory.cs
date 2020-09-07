using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.VerificationCode.VerificationCodeServices
{
    [Injection(InterfaceType = typeof(VerificationCodeServiceForRedisFactory), Scope = InjectionScope.Singleton)]
    public class VerificationCodeServiceForRedisFactory : IFactory<IVerificationCodeService>
    {
        private readonly VerificationCodeServiceForRedis _verificationCodeServiceForRedis;
        public VerificationCodeServiceForRedisFactory(VerificationCodeServiceForRedis verificationCodeServiceForRedis)
        {
            _verificationCodeServiceForRedis = verificationCodeServiceForRedis;
        }
        public IVerificationCodeService Create()
        {
            return _verificationCodeServiceForRedis;
        }
    }
}
