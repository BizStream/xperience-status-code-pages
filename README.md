# xperience-status-code-pages ![License](https://img.shields.io/github/license/BizStream/xperience-status-code-pages)

ASP.NET Core StatusCodePages driven by the Xperience Content Tree.

## Usage

- Install the `Administration` package into the CMSApp project

```bash
Install-Package BizStream.Kentico.Xperience.Adminstration.StatusCodePages
```

- Install the `AspNetCore` package into the Xperience ASP.NET Core Mvc project:

```bash
dotnet add package BizStream.Kentico.Xperience.AspNetCore.StatusCodePages
```

OR

```csproj
<PackageReference Include="BizStream.Kentico.Xperience.AspNetCore.StatusCodePages" Version="x.x.x" />
```

- Configure services in the `Startup.cs` of the Xperience Mvc project:

```csharp
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages;
using Microsoft.Extensions.DependencyInjection;

// ...

public void ConfigureServices( IServiceCollection services )
{
    services.AddControllersWithViews();

    // ...

    services.AddXperienceStatusCodePages();
}

public void Configure( IApplicationBuilder app, IWebHostEnvironment environment )
{
    if( environment.IsDevelopment() )
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseXperienceStatusCodePages();
        app.UseHsts();
    }

    // ...
}
```

- Register the _Page Route_ in the Xperience Mvc App:
  
> _We recommend creating a `RegisterPageRoutes.cs` file in the root of the Xperience Mvc project for usage of the `RegisterPageRouteAttribute`s._

```csharp
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages;
using Kentico.Content.Web.Mvc.Routing;

// For "out-of-the-box" functionality
[assembly: RegisterStatusCodePageRoute]

// OR

// To use a custom Controller
[assembly: RegisterPageRoute( StatusCodeNode.CLASS_NAME, typeof( MyStatusCodeController ) )]
```
