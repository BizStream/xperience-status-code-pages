using System.Net;
using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;
using Microsoft.AspNetCore.Builder;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests
{

    [TestFixture( Category = "IsolatedMvc" )]
    [TestOf( typeof( XperienceStatusCodePagesExtensions ) )]
    public class StatusCodePagesWithReExecuteTests : StatusCodePagesTests<StatusCodePagesWithReExecuteTests.Startup>
    {
        public class Startup : StatusCodePagesTestsStartup
        {
            public override void ConfigureTests( IApplicationBuilder app )
                => app.UseXperienceStatusCodePagesWithReExecute();
        }

        [Test]
        [TestCase( "/asfda" )]
        [TestCase( "/not-a-real-url" )]
        public async Task InvalidUrl_ShouldReturn404StatusCode( string url )
        {
            var response = await Client.GetAsync( url );

            Assert.AreEqual( HttpStatusCode.NotFound, response.StatusCode );
        }

    }

}
