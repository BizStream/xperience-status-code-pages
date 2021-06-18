using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions
{

    public class StatusCodePagesTestsStartup
    {

        public void Configure( IApplicationBuilder app )
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

            ConfigureTests( app );
        }

        public void ConfigureServices( IServiceCollection services )
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
                    features.UsePageRouting( new PageRoutingOptions { EnableAlternativeUrls = true } );
                }
            ).SetAdminCookiesSameSiteNone()
                .DisableVirtualContextSecurityForLocalhost();

            services.AddXperienceStatusCodePages();
            ConfigureTestServices( services );
        }

        public virtual void ConfigureTests( IApplicationBuilder app )
        {
        }

        public virtual void ConfigureTestServices( IServiceCollection services )
        {
        }

    }

}
