using System.Threading.Tasks;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;
using CMS.Core;
using CMS.SiteProvider;
using Kentico.Content.Web.Mvc;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests
{

    [TestFixture( Category = "Integration" )]
    [TestOf( typeof( StatusCodePageRetriever ) )]
    public class StatusCodePageRetrieverTests : BaseWebIntegrationTests
    {

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
        public void StatusCodePageRetrieverSetUp( )
        {
            SiteContext.CurrentSiteName = "NewSite";
        }

    }

}
