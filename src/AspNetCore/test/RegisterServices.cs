using BizStream.Kentico.Xperience.Administration.StatusCodePages.Abstractions;
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests;
using CMS;

[assembly: RegisterImplementation( typeof( IStatusCodePagesImportProvider ), typeof( TestsStatusCodePagesImportProvider ) )]
