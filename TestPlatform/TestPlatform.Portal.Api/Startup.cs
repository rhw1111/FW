using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSLibrary;
using MSLibrary.Logger;
using MSLibrary.Context;
using MSLibrary.Context.Middleware;
using MSLibrary.Context.Filter;
using Microsoft.AspNetCore.Mvc;
using MSLibrary.Logger.Middleware;
using Exceptionless;
using MSLibrary.AspNet.Filter;
using MSLibrary.AspNet.Middleware;
using MSLibrary.DI;
using MSLibrary.AspNet.AuthenticationHandlers;
using MSLibrary.Configuration;
using FW.TestPlatform.Main;
using FW.TestPlatform.Main.Configuration;
using FW.TestPlatform.Main.Application;
using FW.TestPlatform.Main.AspNet.AuthenticationHandlers;

namespace FW.TestPlatform.Portal.Api
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var coreConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);

            var appGetApplicationCrosOrgin = DIContainerContainer.Get<IAppGetApplicationCrosOrgin>();
            var crosOrgins=appGetApplicationCrosOrgin.Do();

            services.AddControllers((opts) =>
            {
                opts.Filters.AddService<UserAuthorizeActionGolbalFilter>();
                opts.Filters.AddService<HttpExtensionContextActionGolbalFilter>();
                opts.Filters.Add(DIContainerContainer.Get<UserAuthorizeFilter>(new object?[] { true, null, ClaimContextGeneratorNames.Default, ClaimContextGeneratorNames.Default, null }, new Type[] { typeof(bool), typeof(string), typeof(string), typeof(string), typeof(string) }));
                opts.Filters.Add(DIContainerContainer.Get<ExceptionFilter>(LoggerCategoryNames.TestPlatform_Portal_Api, coreConfiguration.Debug));

                //opts.MaxIAsyncEnumerableBufferLimit
            });

            services.AddCors(options =>
            {
                options.AddPolicy("any",
                builder =>
                {
                    builder.WithOrigins(crosOrgins).AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddAuthentication(AuthenticationSchemes.Default)

                .AddDefaultScheme(AuthenticationSchemes.Default, (options) =>
                {
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var applicationConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseDIWrapper(ContextTypes.DI, LoggerCategoryNames.DIWrapper);
            app.UseExceptionWrapper(LoggerCategoryNames.HttpRequest, applicationConfiguration.Debug);
            app.UserHttpExtensionContext(string.Empty, HttpExtensionContextHandleServiceNames.Internationalization, LoggerCategoryNames.ContextExtension);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}
