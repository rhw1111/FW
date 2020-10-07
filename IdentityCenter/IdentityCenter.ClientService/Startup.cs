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
using MSLibrary.AspNet.Filter;
using MSLibrary.AspNet.Middleware;
using MSLibrary.DI;
using MSLibrary.AspNet.AuthenticationHandlers;
using MSLibrary.Configuration;

using IdentityCenter.Main;
using IdentityCenter.Main.Application;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.ClientService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var coreConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            var appClientHostContextInitGenerate = DIContainerContainer.Get<IAppClientHostContextInitGenerate>();
            var appGetAllIdentityClientBindings = DIContainerContainer.Get<IAppGetAllIdentityClientBindings>();


            var task = Task.Run(async () =>
            {
                var initControll = await appClientHostContextInitGenerate.Do();
                initControll.Init();

                var (hostInfo, bindingInfos) = await appGetAllIdentityClientBindings.Do();

                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddCors(options =>
                {
                    options.AddPolicy("default", policy =>
                    {
                        foreach(var item in hostInfo.AllowedCorsOrigins)
                        {
                            policy.WithOrigins(item);
                        }

                        policy.AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });

                var authenticationBuilder = services.AddAuthentication();
                
                foreach(var item in bindingInfos)
                {
                    
                    switch(item.Item1.BindingType)
                    {
                        case IdentityClientBindingTypes.OpenID:
                            authenticationBuilder
                                .AddOpenIdConnect(item.Item1.BindingName, (options) =>
                             {
                                 
                                 item.Item2.Init(options);
                             });
                            break;
                    }

                    services.AddAuthorization((options) =>
                    {
                        options.AddPolicy(item.Item1.BindingName, policy =>
                        {
                            policy.RequireAuthenticatedUser();
                            policy.AddAuthenticationSchemes(item.Item1.BindingName);
                        });
                    });
                }



                services.AddMvc((options) => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            });


            task.Wait();



            /*services.AddAuthentication("default")
                .AddIdentityServerAuthentication("default", options =>
                 {
                     options.RequireHttpsMetadata = false;
                     options.Authority = "http://localhost:5000";
                     options.ApiName = "api1";

                     options.JwtBearerEvents.OnMessageReceived = async (context) =>
                       {
                           var aa = context;
                       };
                 })
                .AddOpenIdConnect("openid", (options) =>
                 {

                     options.Authority = "http://localhost:5000";
                     options.RequireHttpsMetadata = false;
                 
                     options.ClientId = "openid";
                     options.Events.OnRedirectToIdentityProvider = async (context) =>
                       {
                           
                       };
                     options.Events.OnAccessDenied = async (context) =>
                     {

                     };
                     options.Events.OnTokenValidated = async (context) =>
                     {

                     };

                   
                 })
                ;*/
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCookiePolicy();
            app.UseAuthentication();
          

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("default");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }

}
