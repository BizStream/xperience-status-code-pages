using System.IO;
using BizStream.Kentico.Xperience.Administration.StatusCodePages.Abstractions;
using CMS.Base;
using CMS.CMSImportExport;
using CMS.Core;
using CMS.DataEngine;
using CMS.Membership;

namespace BizStream.Kentico.Xperience.Administration.StatusCodePages
{

    /// <summary> Ensures the import of data and settings required by StatusCodePages. </summary>
    public class StatusCodePagesImportProvider : IStatusCodePagesImportProvider
    {
        #region Fields
        // TODO: these consts could be options?
        private const string ImportPackagePath = @"CMSSiteUtils\Import\BizStream_StatusCodePages.zip";
        private const string ImportAsUserName = "administrator";

        private readonly IEventLogService eventLog;
        private readonly IUserInfoProvider userProvider;
        #endregion

        public StatusCodePagesImportProvider(
            IEventLogService eventLog,
            IUserInfoProvider userProvider
        )
        {
            this.eventLog = eventLog;
            this.userProvider = userProvider;
        }

        /// <inheritdoc />
        public virtual void ImportObjectsData( )
        {
            eventLog.LogInformation( nameof( StatusCodePagesImportProvider ), nameof( ImportObjectsData ), "Auto-import starting." );

            var user = userProvider.Get( ImportAsUserName );
            if( user == null )
            {
                eventLog.LogWarning(
                    nameof( StatusCodePagesImportProvider ),
                    nameof( ImportObjectsData ),
                    $"Unable to retrieve user '{ImportAsUserName}'. StatusCodePages auto-import cannot complete."
                );

                return;
            }

            // Creates an object containing the import settings
            var settings = new SiteImportSettings( user )
            {
                SourceFilePath = Path.Combine( SystemContext.WebApplicationPhysicalPath, ImportPackagePath ),
                ImportType = ImportTypeEnum.AllNonConflicting,
                WebsitePath = SystemContext.WebApplicationPhysicalPath
            };

            settings.LoadDefaultSelection();

            ImportProvider.ImportObjectsData( settings );
            ImportProvider.DeleteTemporaryFiles( settings, false );

            SettingsKeyInfoProvider.SetGlobalValue( SettingKeys.General.AreObjectsImported, true );
            eventLog.LogInformation( nameof( StatusCodePagesImportProvider ), nameof( ImportObjectsData ), "Auto-import has completed." );
        }

        /// <inheritdoc />
        /// <remarks> This implementation determines whether objects have been imported using a Global [hidden] <see cref="SettingsKeyInfo"/>. </remarks>
        public virtual bool IsObjectsDataImported( )
            => SettingsKeyInfoProvider.GetBoolValue( SettingKeys.General.AreObjectsImported );

    }

}
