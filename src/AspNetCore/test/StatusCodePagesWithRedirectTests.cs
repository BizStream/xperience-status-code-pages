using System.Net;
using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests
{

    [TestFixture( Category = "Integration" )]
    [TestOf( typeof( XperienceStatusCodePagesExtensions ) )]
    public class StatusCodePagesWithRedirectTests : BaseWebIntegrationTests
    {

        protected override WebApplicationFactory CreateWebApplicationFactory( )
        {
            var factory = new WebApplicationFactory(
                app => app.UseXperienceStatusCodePagesWithRedirect(),
                configureServices: services => services.AddXperienceStatusCodePages()
            );

            // prevent redirect, so that we can test the redirect
            factory.ClientOptions.AllowAutoRedirect = false;
            return factory;
        }

        [Test]
        [TestCase( "/asfda" )]
        [TestCase( "/not-a-real-url" )]
        public async Task InvalidUrl_ShouldReturn302StatusCode( string url )
        {
            var response = await client.GetAsync( url );

            Assert.AreEqual( HttpStatusCode.Found, response.StatusCode );
        }

        [Test]
        [TestCase( "/asfda" )]
        [TestCase( "/not-a-real-url" )]
        public async Task InvalidUrl_ShouldReturnLocationHeader( string url )
        {
            var response = await client.GetAsync( url );

            Assert.AreEqual( HttpStatusCode.Found, response.StatusCode );
            Assert.AreEqual( "/404", response.Headers.Location.ToString() );
        }

    }

}
