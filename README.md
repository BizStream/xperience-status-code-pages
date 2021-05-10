# xperience-status-code-pages ![License](https://img.shields.io/github/license/BizStream/xperience-status-code-pages)

ASP.NET Core StatusCodePages driven by the Xperience Content Tree.

## Usage

- Install the `Administration` package into your CMSApp project

```bash
Install-Package BizStream.Kentico.Xperience.Adminstration.StatusCodePages
```

- Install the `AspNetCore` package into your Xperience Mvc project:

```bash
dotnet add package BizStream.Kentico.Xperience.AspNetCore.StatusCodePages
```

OR

```csproj
<PackageReference Include="BizStream.Kentico.Xperience.AspNetCore.StatusCodePages" Version="x.x.x" />
```

- Configure services in `Startup.cs` of your Xperience Mvc project:

```csharp
using BizStream.Kentico.Xperience.AspNetCore.StatusCodePages;
using Microsoft.Extensions.DependencyInjection;

// ...

public void ConfigureServices( IServiceCollection services )
{
    services.AddControllersWithViews();

    services.AddXperienceStatusCodePages();

    // ...
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
