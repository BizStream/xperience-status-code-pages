using BizStream.Kentico.Xperience.AspNetCore.Mvc.Testing;

namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages.Tests.Abstractions;

public class StatusCodePagesTestsStartup : XperienceTestStartup
{
    public override void ConfigureTests( IApplicationBuilder app )
    {
    }

    public override void ConfigureTestServices( IServiceCollection services )
    {
        services.AddXperienceStatusCodePages();
    }
}
