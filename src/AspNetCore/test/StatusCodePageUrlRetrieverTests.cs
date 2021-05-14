using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;
using CMS.Core;
using CMS.SiteProvider;
using Kentico.Content.Web.Mvc;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests
{

    [TestFixture( Category = "Integration" )]
    [TestOf( typeof( StatusCodePageUrlRetriever ) )]
    public class StatusCodePageUrlRetrieverTests : BaseWebIntegrationTests
    {

        private StatusCodePageUrlRetriever CreateStatusCodePageUrlRetriever( )
        {
            var pageRetriever = Service.Resolve<IPageRetriever>();
            var pageUrlRetriever = Service.Resolve<IPageUrlRetriever>();

            var statusCodePageRetriever = new StatusCodePageRetriever( pageRetriever );
            return new StatusCodePageUrlRetriever( statusCodePageRetriever, pageUrlRetriever );
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
        public void StatusCodePageUrlRetrieverSetUp( )
        {
            SiteContext.CurrentSiteName = "NewSite";
        }

    }

}
