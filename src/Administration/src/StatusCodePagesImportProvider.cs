using System;
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
        private readonly IEventLogService eventLog;
        private readonly IUserInfoProvider userProvider;
        #endregion

        #region Properties

        /// <summary> The path at which the Export Archive containing StatusCodePages ObjectsTypes is located. </summary>
        protected virtual string ImportPackagePath => @"CMSSiteUtils\Import\BizStream_StatusCodePages.zip";

        /// <summary> The <see cref="UserInfo.UserName"/> of the User to execute the import as. </summary>
        protected virtual string ImportAsUserName => "administrator";
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

            var settings = new SiteImportSettings( user )
            {
                SourceFilePath = Path.Combine( SystemContext.WebApplicationPhysicalPath, ImportPackagePath ),
                ImportType = ImportTypeEnum.AllNonConflicting,
                WebsitePath = SystemContext.WebApplicationPhysicalPath
            };

            try
            {
                settings.LoadDefaultSelection();

                ImportProvider.ImportObjectsData( settings );
                ImportProvider.DeleteTemporaryFiles( settings, false );

                SettingsKeyInfoProvider.SetGlobalValue( SettingKeys.General.AreObjectsImported, true );
            }
            catch( Exception exception )
            {
                eventLog.LogException( nameof( StatusCodePagesImportProvider ), nameof( ImportObjectsData ), exception );
            }

            eventLog.LogInformation( nameof( StatusCodePagesImportProvider ), nameof( ImportObjectsData ), "Auto-import has completed." );
        }

        /// <inheritdoc />
        /// <remarks> This implementation determines whether objects have been imported using a Global [hidden] <see cref="SettingsKeyInfo"/>. </remarks>
        public virtual bool IsObjectsDataImported( )
            => SettingsKeyInfoProvider.GetBoolValue( SettingKeys.General.AreObjectsImported );

    }

}
