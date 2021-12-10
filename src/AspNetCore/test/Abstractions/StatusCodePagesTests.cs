using BizStream.Kentico.Xperience.AspNetCore.Mvc.Testing;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Models;
using CMS.Base;
using CMS.CMSImportExport;
using CMS.DocumentEngine;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using NUnit.Framework;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;

public abstract class StatusCodePagesTests<TStartup> : AutomatedTestsWithIsolatedWebApplication<TStartup>
    where TStartup : StatusCodePagesTestsStartup
{
    #region Fields
    private const string ImportPackagePath = @"CMSSiteUtils\Import\BizStream_StatusCodePages_Tests.zip";
    #endregion

    private void ImportObjectsData( )
    {
        var settings = new SiteImportSettings( UserInfo.Provider.Get( "administrator" ) )
        {
            EnableSearchTasks = false,
            ImportType = ImportTypeEnum.AllNonConflicting,
            SourceFilePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, ImportPackagePath ),
            WebsitePath = SystemContext.WebApplicationPhysicalPath
        };

        settings.LoadDefaultSelection();

        ImportProvider.ImportObjectsData( settings );
        ImportProvider.DeleteTemporaryFiles( settings, false );
    }

    protected virtual void SeedData( )
    {
        ImportObjectsData();

        var site = SeedSite();
        var root = SeedRootNode( site );

        SeedStatusCodeNodes( root );
    }

    private TreeNode SeedRootNode( SiteInfo site )
    {
        var data = new DataContainer();
        data.SetValue( nameof( TreeNode.NodeSiteID ), site.SiteID );

        var root = TreeNode.New<TreeNode>( SystemDocumentTypes.Root, data );
        root.Insert( null, false );

        return root;
    }

    private SiteInfo SeedSite( )
    {
        var site = SiteInfo.Provider.Get( "NewSite" );
        CultureSiteInfo.Provider.Add(
            CultureInfo.Provider.Get( "en-US" ).CultureID,
            site.SiteID
        );

        SiteContext.CurrentSite = site;
        return site;
    }

    private void SeedStatusCodeNodes( TreeNode parent )
    {
        DocumentHelper.InsertDocument(
            new StatusCodeNode
            {
                Content = "Unauthorized",
                DocumentName = "401",
                HttpStatusCode = 401
            },
            parent
        );

        DocumentHelper.InsertDocument(
            new StatusCodeNode
            {
                Content = "Page Not Found",
                DocumentName = "404",
                HttpStatusCode = 404
            },
            parent
        );

        DocumentHelper.InsertDocument(
            new StatusCodeNode
            {
                Content = "Internal Server Error",
                DocumentName = "500",
                HttpStatusCode = 500
            },
            parent
        );
    }

    [SetUp]
    public void StatusCodePagesTestsSetUp( )
    {
        SeedData();
    }
}
