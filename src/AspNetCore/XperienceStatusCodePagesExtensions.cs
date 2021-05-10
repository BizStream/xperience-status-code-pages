using System;
using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages
{

    /// <summary> Extensions to <see cref="IServiceCollection"/> and <see cref="IApplicationBuilder"/> that allow enabling and configuring of Xperience Status Pages. </summary>
    public static class XperienceStatusCodePagesExtensions
    {

        /// <summary> Registers services required for usage of Xperience Status Code Pages. </summary>
        public static IServiceCollection AddXperienceStatusCodePages( this IServiceCollection services )
        {
            if( services == null )
            {
                throw new ArgumentNullException( nameof( services ) );
            }

            services.TryAddTransient<IStatusCodePageRetriever, StatusCodePageRetriever>();
            services.TryAddTransient<IStatusCodePageUrlRetriever, StatusCodePageUrlRetriever>();

            return services;
        }

        private static Task<string> GetStatusCodePathAsync( HttpContext context )
            => context.RequestServices.GetRequiredService<IStatusCodePageUrlRetriever>()
                .RetrieveAsync( context.Response.StatusCode )
                .ContinueWith( task => task.Result?.RelativePath?.TrimStart( '~' ) );

        /// <summary> Short-hand for <see cref="UseXperienceStatusCodePagesWithReExecute(IApplicationBuilder)"/>. </summary>
        public static IApplicationBuilder UseXperienceStatusCodePages( this IApplicationBuilder app )
            => UseXperienceStatusCodePagesWithReExecute( app );

        /// <summary> Adds a StatusCodePages middleware to the pipeline. Specifies that responses should be handled by redirecting to a document in the Xperience Content Tree. </summary>
        public static IApplicationBuilder UseXperienceStatusCodePagesWithRedirect( this IApplicationBuilder app )
        {
            if( app == null )
            {
                throw new ArgumentNullException( nameof( app ) );
            }

            return app.UseStatusCodePages(
                async context =>
                {
                    var path = await GetStatusCodePathAsync( context.HttpContext );
                    if( !string.IsNullOrEmpty( path ) )
                    {
                        context.HttpContext.Response.Redirect( context.HttpContext.Request.PathBase + path );
                    }
                }
            );
        }

        /// <summary> Adds a StatusCodePages middleware to the pipeline. Specifies that the response body should be generated by re-executing the request pipeline using a document in the Xperience Content Tree. </summary>
        /// <remarks> The following implementation was adapted from the <see href="https://github.com/dotnet/aspnetcore/blob/e79d740ba95a1b1486e81aebace8d38ba06d35b9/src/Middleware/Diagnostics/src/StatusCodePage/StatusCodePagesExtensions.cs#L167">the source implementation</see> of the <see cref="StatusCodePagesExtensions.UseStatusCodePagesWithReExecute(IApplicationBuilder, string, string)"/> extension provided by the AspNetCore Mvc Framework. </remarks>
        public static IApplicationBuilder UseXperienceStatusCodePagesWithReExecute( this IApplicationBuilder app )
        {
            if( app == null )
            {
                throw new ArgumentNullException( nameof( app ) );
            }

            return app.UseStatusCodePages(
                async context =>
                {
                    var path = await GetStatusCodePathAsync( context.HttpContext );
                    if( string.IsNullOrEmpty( path ) )
                    {
                        return;
                    }

                    var originalPath = context.HttpContext.Request.Path;
                    var originalQueryString = context.HttpContext.Request.QueryString;

                    // Store the original paths so the app can check it.
                    context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(
                        new StatusCodeReExecuteFeature
                        {
                            OriginalPathBase = context.HttpContext.Request.PathBase.Value!,
                            OriginalPath = originalPath.Value!,
                            OriginalQueryString = originalQueryString.HasValue ? originalQueryString.Value : null,
                        }
                    );

                    // An endpoint may have already been set. Since we're going to re-invoke the middleware pipeline we need to reset
                    // the endpoint and route values to ensure things are re-calculated.
                    context.HttpContext.SetEndpoint( endpoint: null );

                    var routeValuesFeature = context.HttpContext.Features.Get<IRouteValuesFeature>();
                    routeValuesFeature?.RouteValues?.Clear();

                    context.HttpContext.Request.Path = path;
                    context.HttpContext.Request.QueryString = QueryString.Empty;

                    try
                    {
                        await context.Next( context.HttpContext );
                    }
                    finally
                    {
                        context.HttpContext.Request.QueryString = originalQueryString;
                        context.HttpContext.Request.Path = originalPath;
                        context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>( null );
                    }
                }
            );
        }

    }

}
