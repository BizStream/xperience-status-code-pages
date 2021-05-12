using BizStream.Kentico.Xperience.Administration.StatusCodePages.Abstractions;
using CMS.Core;
using CMS.DataEngine;

namespace BizStream.Kentico.Xperience.Administration.StatusCodePages
{

    /// <summary> Ensures import of data required by StatusCodePages upon app initialization, via <see cref="IStatusCodePagesImportProvider" />. </summary>
    public sealed class StatusCodePagesModule : Module
    {

        public StatusCodePagesModule( )
            : base( nameof( StatusCodePagesModule ), false )
        {
        }

        protected override void OnInit( )
        {
            Service.Resolve<IEventLogService>()
                .LogInformation( nameof( StatusCodePagesModule ), nameof( OnInit ) );

            var importProvider = Service.Resolve<IStatusCodePagesImportProvider>();
            if( !importProvider.IsObjectsDataImported() )
            {
                importProvider.ImportObjectsData();
            }
        }

    }

}
