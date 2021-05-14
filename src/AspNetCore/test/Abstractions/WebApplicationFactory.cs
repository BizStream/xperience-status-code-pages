using System;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions
{

    public class WebApplicationFactory : WebApplicationFactory<WebApplicationFactory>
    {
        #region Fields
        private readonly Action<IApplicationBuilder> configureApp;
        private readonly Action<IFeaturesBuilder> configureKentico;
        private readonly Action<IServiceCollection> configureServices;
        #endregion

        #region Properties
        public IHost Host { get; protected set; }
        #endregion

        public WebApplicationFactory(
            Action<IApplicationBuilder> configureApp = null,
            Action<IFeaturesBuilder> configureKentico = null,
            Action<IServiceCollection> configureServices = null
        )
        {
            this.configureApp = configureApp;
            this.configureKentico = configureKentico;
            this.configureServices = configureServices;
        }

        protected override IHostBuilder CreateHostBuilder( )
            => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    builder => builder.ConfigureServices(
                            services =>
                            {
                                Startup.ConfigureServices( services, configureKentico );
                                configureServices?.Invoke( services );
                            }
                        ).Configure(
                           app =>
                           {
                               Startup.Configure( app );
                               configureApp?.Invoke( app );
                           }
                        )
                );

        protected override IHost CreateHost( IHostBuilder builder )
        {
            Host = builder.Build();
            return Host;
        }

        protected override void ConfigureWebHost( IWebHostBuilder builder )
            => builder.UseContentRoot( AppDomain.CurrentDomain.BaseDirectory )
                .UseEnvironment( Environments.Development );

        private static class Startup
        {

            public static void ConfigureServices( IServiceCollection services, Action<IFeaturesBuilder> configureKentico = null )
            {
                services.AddControllersWithViews();

                services.AddAuthentication();
                services.AddAuthorization();

                services.AddCors();
                services.AddRouting();

                services.AddKentico(
                    features =>
                    {
                        features.UsePageBuilder();
                        features.UsePageRouting(
                            new PageRoutingOptions
                            {
                                EnableAlternativeUrls = true
                            }
                        );

                        configureKentico?.Invoke( features );
                    }
                ).SetAdminCookiesSameSiteNone()
                    .DisableVirtualContextSecurityForLocalhost();
            }

            public static void Configure( IApplicationBuilder app )
            {
                app.UseCookiePolicy();
                app.UseCors();
                app.UseStaticFiles();

                app.UseKentico();

                app.UseRouting();
                app.UseAuthorization();
                app.UseAuthentication();
                app.UseEndpoints(
                   endpoints =>
                   {
                       endpoints.Kentico().MapRoutes();
                       endpoints.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}" );
                   }
               );
            }
        }

    }

}
