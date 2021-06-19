using System.Linq;
using BizStream.Kentico.Xperience.AspNetCore.Mvc.Testing;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Models;
using CMS.DocumentEngine;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions
{

    public abstract class StatusCodePagesTests<TStartup> : AutomatedTestsWithIsolatedWebApplication<TStartup>
        where TStartup : StatusCodePagesTestsStartup
    {
        #region Properties
        //protected override TimeSpan ArtificialDelay => TimeSpan.FromSeconds( 1 );
        #endregion

        protected virtual void SeedData( )
        {
            var root = DocumentHelper.GetDocuments()
                .Type( SystemDocumentTypes.Root )
                .TopN( 1 )
                .First();

            DocumentHelper.InsertDocument(
                new StatusCodeNode
                {
                    Content = "Unauthorized",
                    DocumentName = "401",
                    HttpStatusCode = 401
                },
                root
            );

            DocumentHelper.InsertDocument(
                new StatusCodeNode
                {
                    Content = "Page Not Found",
                    DocumentName = "404",
                    HttpStatusCode = 404
                },
                root
            );

            DocumentHelper.InsertDocument(
                new StatusCodeNode
                {
                    Content = "Internal Server Error",
                    DocumentName = "500",
                    HttpStatusCode = 500
                },
                root
            );
        }

        [SetUp]
        public void StatusCodePagesTestsSetUp( )
        {
            SeedData();
        }

    }

}
