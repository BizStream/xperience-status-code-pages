using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.Mvc.Testing;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Infrastructure;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;
using CMS.Core;
using Kentico.Content.Web.Mvc;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests
{

    [TestFixture( Category = "IsolatedMvc" )]
    [TestOf( typeof( StatusCodePageRetriever ) )]
    public class StatusCodePageRetrieverTests : StatusCodePagesTests<StatusCodePagesTestsStartup>
    {

        protected override XperienceWebApplicationFactory<StatusCodePagesTestsStartup> CreateWebApplicationFactory( )
        {
            var factory = base.CreateWebApplicationFactory();

            // NOTE: ensures current site context is preserved
            factory.Server.PreserveExecutionContext = true;

            return factory;
        }

        [Test]
        [TestCase( 401 )]
        [TestCase( 404 )]
        [TestCase( 500 )]
        public async Task RetrieveAsync_ShouldReturnNodeByStatusCode( int statusCode )
        {
            var pageRetriever = Service.Resolve<IPageRetriever>();
            var statusCodePageRetriever = new StatusCodePageRetriever( pageRetriever );

            var node = await statusCodePageRetriever.RetrieveAsync( statusCode );

            Assert.IsNotNull( node );
            Assert.AreEqual( statusCode, node.HttpStatusCode );
        }

        [SetUp]
        public async Task StatusCodePageRetrieverTestsSetUp( )
            => await Client.GetAsync( "/" );

    }

}
