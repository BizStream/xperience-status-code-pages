using System.Net;
using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests
{

    [TestFixture( Category = "Integration" )]
    [TestOf( typeof( XperienceStatusCodePagesExtensions ) )]
    public class StatusCodePagesWithReExecuteTests : BaseWebIntegrationTests
    {

        protected override WebApplicationFactory CreateWebApplicationFactory( )
            => new WebApplicationFactory(
                app => app.UseXperienceStatusCodePagesWithReExecute(),
                configureServices: services => services.AddXperienceStatusCodePages()
            );

        [Test]
        [TestCase( "/asfda" )]
        [TestCase( "/not-a-real-url" )]
        public async Task InvalidUrl_ShouldReturn404StatusCode( string url )
        {
            var client = WebApplicationFactory.CreateClient();

            var response = await client.GetAsync( url );

            Assert.AreEqual( HttpStatusCode.NotFound, response.StatusCode );
        }

    }

}
