using BizStream.Kentico.Xperience.Administration.StatusCodePages;
using BizStream.Kentico.Xperience.Administration.StatusCodePages.Abstractions;
using CMS;
using CMS.Core;

[assembly: RegisterImplementation( typeof( IStatusCodePagesImportProvider ), typeof( StatusCodePagesImportProvider ), Priority = RegistrationPriority.SystemDefault )]
