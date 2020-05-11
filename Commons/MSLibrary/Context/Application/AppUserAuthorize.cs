using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Context.Application
{
    [Injection(InterfaceType = typeof(IAppUserAuthorize), Scope = InjectionScope.Singleton)]
    public class AppUserAuthorize : IAppUserAuthorize
    {
        private IClaimContextGeneratorRepositoryCacheProxy _claimContextGeneratorRepository;

        public AppUserAuthorize(IClaimContextGeneratorRepositoryCacheProxy claimContextGeneratorRepository)
        {
            _claimContextGeneratorRepository = claimContextGeneratorRepository;
        }
        public async Task<IAppUserAuthorizeResult> Do(IEnumerable<Claim> claims, string generatorName)
        {
            var generator=await _claimContextGeneratorRepository.QueryByName(generatorName);
            if (generator==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundClaimContextGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                    ReplaceParameters = new List<object>() { generatorName }
                };

                throw new UtilityException((int)Errors.NotFoundClaimContextGeneratorByName, fragment);
            }

            AppUserAuthorizeResult result = new AppUserAuthorizeResult(generator, claims);

            return result;
        }
    }


    
    public class AppUserAuthorizeResult : IAppUserAuthorizeResult
    {
        private ClaimContextGenerator _claimContextGenerator;
        private IEnumerable<Claim> _claims;

        public AppUserAuthorizeResult(ClaimContextGenerator claimContextGenerator, IEnumerable<Claim> claims)
        {
            _claimContextGenerator = claimContextGenerator;
            _claims = claims;
        }

        public void Execute()
        {
            _claimContextGenerator.ContextInit(_claims);
        }
    }
}
