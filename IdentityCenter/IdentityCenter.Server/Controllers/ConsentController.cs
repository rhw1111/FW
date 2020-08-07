using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MSLibrary;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main;
using IdentityCenter.Main.Application;
using IdentityCenter.Main.DTOModel;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Server.Controllers
{
    [Route("api/consent")]
    [ApiController]
    public class ConsentController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IEventService _events;

        public ConsentController(IIdentityServerInteractionService interaction, IClientStore clientStore, IResourceStore resourceStore, IEventService events)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _events = events;
        }

        [HttpGet("getconsent")]
        public async Task<ConsentViewModel> GetConsent(string returnUrl)
        {
            var vm = await BuildViewModelAsync(returnUrl);
            return vm;
        }

        [HttpPost("postconsent")]
        public async Task<IActionResult> PostConsent(ConsentInputModel model)
        {
            var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (request == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.ConsentRequestResolveError,
                    DefaultFormatting = "授权准许请求解析失败",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.ConsentRequestResolveError, fragment, 1, 0);
            }

            ConsentResponse grantedConsent = null;
          
            // user clicked 'no' - send back the standard 'access_denied' response
            if (!model.Accept)
            {
                grantedConsent = new ConsentResponse()
                {
                    Error = AuthorizationError.AccessDenied
                }; 
                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.Client.AllowedScopes));
            }
            // user clicked 'yes' - validate the data
            else
            {
                grantedConsent = new ConsentResponse
                {
                    RememberConsent = model.RememberConsent,
                     ScopesValuesConsented= request.Client.AllowedScopes
                };
                // emit event
                await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.Client.AllowedScopes, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
            }
            // communicate outcome of consent back to identityserver
            await _interaction.GrantConsentAsync(request, grantedConsent);

            return Redirect(model.ReturnUrl);
        }

        private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl)
        {
            ConsentViewModel vm;
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(request.Client.ClientId);
                if (client != null)
                {
                    vm = new ConsentViewModel
                    {

                        ClientName = client.ClientName ?? client.ClientId,
                        ClientUrl = client.ClientUri,
                        ClientLogoUrl = client.LogoUri,
                        AllowRememberConsent = client.AllowRememberConsent
                    };
                    var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.Client.AllowedScopes);
                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        vm.IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x)).ToList();
                        vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).ToList();
                    }
                    else
                    {
                        vm.IdentityScopes = new List<ScopeViewModel>();
                        vm.ResourceScopes = new List<string>();
                    }

                }
                else
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.NotFoundIdentityServerClientByID,
                        DefaultFormatting = "找不到id为{0}的认证服务客户端",
                        ReplaceParameters = new List<object>() { request.Client.ClientId }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityServerClientByID, fragment, 1, 0);
                }
            }
            else
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.ConsentRequestResolveError,
                    DefaultFormatting = "授权准许请求解析失败",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.ConsentRequestResolveError, fragment, 1, 0);
            }
            return vm;
        }


        private ScopeViewModel CreateScopeViewModel(IdentityResource identity)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required
            };
        }


    }
}