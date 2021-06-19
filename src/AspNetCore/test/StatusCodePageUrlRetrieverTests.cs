using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.Mvc.Testing;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;
using CMS.Core;
using Kentico.Content.Web.Mvc;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests
{

    [TestFixture( Category = "IsolatedMvc" )]
    [TestOf( typeof( StatusCodePageUrlRetriever ) )]
    public class StatusCodePageUrlRetrieverTests : StatusCodePagesTests<StatusCodePagesTestsStartup>
    {
        //protected override TimeSpan ArtificialDelay => TimeSpan.FromSeconds( 2.5 );

        protected override XperienceWebApplicationFactory<StatusCodePagesTestsStartup> CreateWebApplicationFactory( )
        {
            var factory = base.CreateWebApplicationFactory();

            // NOTE: ensures current site context is preserved
            factory.Server.PreserveExecutionContext = true;

            return factory;
        }

        private StatusCodePageUrlRetriever CreateStatusCodePageUrlRetriever( )
        {
            var pageRetriever = Service.Resolve<IPageRetriever>();
            var pageUrlRetriever = Service.Resolve<IPageUrlRetriever>();

            var statusCodePageRetriever = new StatusCodePageRetriever( pageRetriever );
            return new StatusCodePageUrlRetriever( pageUrlRetriever, statusCodePageRetriever );
        }

        [Test]
        [TestCase( 401 )]
        [TestCase( 404 )]
        [TestCase( 500 )]
        public async Task RetrieveAsync_ShouldReturnPageUrlByStatusCode( int statusCode )
        {
            var statusCodePageUrlRetriever = CreateStatusCodePageUrlRetriever();
            var url = await statusCodePageUrlRetriever.RetrieveAsync( statusCode );

            Assert.IsNotNull( url );
        }

        [Test]
        [TestCase( 401 )]
        [TestCase( 404 )]
        [TestCase( 500 )]
        public async Task RetrieveAsync_ShouldReturnPageUrlByStatusCode_WithRelativePath( int statusCode )
        {
            var statusCodePageUrlRetriever = CreateStatusCodePageUrlRetriever();
            var url = await statusCodePageUrlRetriever.RetrieveAsync( statusCode );

            Assert.IsNotNull( url );
            Assert.AreEqual( $"~/{statusCode}", url.RelativePath );
        }

        [SetUp]
        public async Task StatusCodePageRetrieverTestsSetUp( )
            => await Client.GetAsync( "/" );

    }

}
