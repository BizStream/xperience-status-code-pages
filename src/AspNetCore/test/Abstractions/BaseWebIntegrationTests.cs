using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Models;
using CMS.Base;
using CMS.Core;
using CMS.DocumentEngine;
using CMS.LicenseProvider;
using CMS.Tests;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions
{

    public abstract class BaseWebIntegrationTests : IsolatedIntegrationTests
    {
        private static readonly PropertyInfo IoCContainerProperty = typeof( TypeManager ).GetTypeInfo()
            .GetProperty( "IoCContainer", BindingFlags.Static | BindingFlags.NonPublic );

        private WebApplicationFactory factory;
        protected HttpClient client;

        protected virtual WebApplicationFactory CreateWebApplicationFactory( )
            => new WebApplicationFactory();

        private void EnsureLicenseKeyExists( )
        {
            /// I didn't feel like figuring out <see cref="TestsConfig"/>, so the logic duplicated the logic from <see cref="AutomatedTestsWithData.EnsureLicense"/>.
            /// @cryptoc1 - 05/13/2021 at 1:23am

            var key = Service.Resolve<IConfiguration>()[ "CMSTestLicenseKey" ];
            if( string.IsNullOrWhiteSpace( key ) )
            {
                throw new Exception( "Cannot execute tests without a test LicenseKey. Ensure the `CMSTestLicenseKey` is set in `appsettings.json` or `appsetings.Development.json`." );
            }

            var license = new LicenseKeyInfo();
            license.LoadLicense( key, string.Empty );

            if( LicenseKeyInfoProvider.GetLicenseKeyInfo( license.Domain ) == null )
            {
                LicenseKeyInfoProvider.SetLicenseKeyInfo( license );
            }
        }

        protected override void InitApplication( )
        {
            if( ApplicationInitialized )
            {
                return;
            }

            PreventCMSApplicationInitException();

            // Start the Mvc app's Host (results in CMSApplication.Init invocation, via the `ApplicationInitializerStartupFilter`)
            factory.Host.StartAsync()
                .GetAwaiter()
                .GetResult();

            EnsureLicenseKeyExists();
            SeedContentTree();

            ApplicationInitialized = true;
        }

        protected override void PreInitApplication( )
        {
            if( ApplicationInitialized )
            {
                return;
            }

            SystemContext.WebApplicationPhysicalPath = TemporaryAppPath;

            if( factory != null )
            {
                factory.Dispose();
                factory = null;
            }

            factory = CreateWebApplicationFactory();

            // Force the Mvc ServiceProvider to be built (results in CMSApplication.PreInit invocation, via `AddKentico(IServiceCollection)`)
            var services = factory.Services;

            // Ensure the IoCContainer is initialized (with the Mvc app's ServiceProvider)
            SetCMSServiceProvider( services );
        }

        private void PreventCMSApplicationInitException( )
        {
            /*
             * HACK:
             *  Use reflection to set/call `internal` properties/methods.
             *  This is required to prevent Kentico from throwing an exception when the `ApplicationInitializerStartupFilter` tries to set Kentico's `TypeManager` to use the Mvc app's configured ServiceProvider.
             */

            var container = IoCContainerProperty.GetValue( null );

            IoCContainerProperty.PropertyType
                .GetProperty( "IsContainerInitialized", BindingFlags.Instance | BindingFlags.NonPublic )
                .SetValue( container, false );

            IoCContainerProperty.PropertyType
                .GetField( "postInitAction", BindingFlags.Instance | BindingFlags.NonPublic )
                .SetValue( container, null );
        }

        protected virtual void SeedContentTree( )
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

        private void SetCMSServiceProvider( IServiceProvider services )
            /*
             * HACK:
             *  Use reflection to call an `internal` method; set's Kentico's TypeManager to use the Mvc app's configured ServiceProvider.
             *  This is required for the `IsolatedIntegrationTests` to be able to create the test database (requires an initialized IoCContainer for Service.Resolve calls)
             */
            => IoCContainerProperty.PropertyType
                .GetMethod( "SetServiceProvider", BindingFlags.Instance | BindingFlags.NonPublic )
                .Invoke( IoCContainerProperty.GetValue( null ), new[] { services } );

        [SetUp]
        public void SetUp( )
        {
            if( client != null )
            {
                client.Dispose();
                client = null;
            }

            client = factory.CreateClient();
        }

    }

}
