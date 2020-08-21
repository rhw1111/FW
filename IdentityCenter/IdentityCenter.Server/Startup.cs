using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using IdentityServer4.Hosting;
using IdentityServer4.Stores;
using IdentityServer4.Extensions;
using IdentityServer4.Infrastructure;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Endpoints;
using IdentityServer4.Test;

using MSLibrary;
using MSLibrary.Logger;
using MSLibrary.Context;
using MSLibrary.Context.Middleware;
using MSLibrary.Context.Filter;
using MSLibrary.Logger.Middleware;
using MSLibrary.AspNet.Filter;
using MSLibrary.AspNet.Middleware;
using MSLibrary.DI;
using MSLibrary.AspNet.AuthenticationHandlers;
using MSLibrary.Configuration;

using IdentityCenter.Main;
using IdentityCenter.Main.Application;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var coreConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            var appHostContextInitGenerate= DIContainerContainer.Get<IAppHostContextInitGenerate>();
            var appInitIdentityOption = DIContainerContainer.Get<IAppInitIdentityOption>();
            var appIdentityHostHandle = DIContainerContainer.Get<IAppIdentityHostHandle>();

            services.AddTransient<IAuthorizationCodeStore, MainAuthorizationCodeStore>();
            services.AddTransient<IRefreshTokenStore, MainRefreshTokenStore>();
            services.AddTransient<IUserConsentStore, MainUserConsentStore>();
            //services.AddTransient<IPersistedGrantService, MainPersistedGrantService>();
            services.AddTransient<IProfileService, MainProfileService>();
            services.AddTransient<IResourceOwnerPasswordValidator, MainResourceOwnerPasswordValidator>();


            var task = Task.Run(async () =>
              {
                  var initControll = await appHostContextInitGenerate.Do();
                  initControll.Init();

                  var (hostHandleResult, serverOptionsInit) = await appIdentityHostHandle.Do();

                  services.AddCors(options =>
                  {
                      options.AddPolicy("default", policy =>
                      {
                          policy.WithOrigins(hostHandleResult.AllowedCorsOrigins.ToArray())
                              .AllowCredentials()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                      });
                  });



                  var identityServerBuilder = services.AddIdentityServer((options) =>
                   {                 
                      
                       serverOptionsInit.Init(options);
                   });

                  if (hostHandleResult.SignCredentialCertificate != null)
                  {
                      identityServerBuilder.AddSigningCredential(hostHandleResult.SignCredentialCertificate);
                  }
                  else
                  {
                      identityServerBuilder.AddSigningCredential(hostHandleResult.SigningCredentials);
                  }

                  identityServerBuilder.AddClientStore<MainClientStore>()
                  .AddCorsPolicyService<MainCorsPolicyService>()
                  .AddResourceStore<MainResourceStore>();

                 /* identityServerBuilder.AddInMemoryApiResources(new List<ApiResource>
                  {
                      new ApiResource("api1", "My API")
                  })
                  .AddInMemoryIdentityResources(new List<IdentityResource>
                  {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                  })
                  .AddInMemoryClients(new List<Client>
                      {
                    new Client
                    {
                        ClientId = "client",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "api1" }

                    },
                    new Client
                    {
                        ClientId = "openid",
                        ClientName = "Open ID",

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                        AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                        RequireConsent=false,



                        RedirectUris = { "http://localhost:5002/xxx" },
                        PostLogoutRedirectUris = { "http://localhost:5002" },


                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile
                        }
                    }
                      })
                      ;*/



                  services.AddMvc((options) => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
              });


            task.Wait();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.UseCors("default");

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}
