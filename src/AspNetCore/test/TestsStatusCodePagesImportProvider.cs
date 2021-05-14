using BizStream.Kentico.Xperience.Administration.StatusCodePages;
using CMS.Core;
using CMS.Membership;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests
{

    public class TestsStatusCodePagesImportProvider : StatusCodePagesImportProvider
    {
        // Use the Test's Export (which includes the 'Integration Tests' Site, along with contents of the standard `BizStream_StatusCodePages.zip` archive)
        protected override string ImportPackagePath => @"CMSSiteUtils\Import\BizStream_StatusCodePages_Tests.zip";

        public TestsStatusCodePagesImportProvider( IEventLogService eventLog, IUserInfoProvider userProvider )
            : base( eventLog, userProvider )
        {
        }

    }

}
