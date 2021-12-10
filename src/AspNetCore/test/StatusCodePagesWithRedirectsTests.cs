using System.Net;
using BizStream.Kentico.Xperience.AspNetCore.Mvc.Testing;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests;

[TestFixture( Category = "IsolatedMvc" )]
[TestOf( typeof( XperienceStatusCodePagesExtensions ) )]
public class StatusCodePagesWithRedirectsTests : StatusCodePagesTests<StatusCodePagesWithRedirectsTests.Startup>
{
    public class Startup : StatusCodePagesTestsStartup
    {
        public override void ConfigureTests( IApplicationBuilder app )
        {
            app.UseXperienceStatusCodePagesWithRedirects();
        }
    }

    protected override XperienceWebApplicationFactory<Startup> CreateWebApplicationFactory( )
    {
        XperienceWebApplicationFactory<Startup>? factory = base.CreateWebApplicationFactory();

        // don't follow redirects, so that we can test the StatusCodePages redirect
        factory.ClientOptions.AllowAutoRedirect = false;

        return factory;
    }

    [Test]
    [TestCase( "/asfda" )]
    [TestCase( "/not-a-real-url" )]
    public async Task InvalidUrl_ShouldReturn302StatusCode( string url )
    {
        HttpResponseMessage? response = await Client.GetAsync( url );

        Assert.AreEqual( HttpStatusCode.Found, response.StatusCode );
    }

    [Test]
    [TestCase( "/asfda" )]
    [TestCase( "/not-a-real-url" )]
    public async Task InvalidUrl_ShouldReturnLocationHeader( string url )
    {
        var response = await Client.GetAsync( url );

        Assert.AreEqual( HttpStatusCode.Found, response?.StatusCode );
        Assert.AreEqual( "/404", response?.Headers.Location.ToString() );
    }
}
