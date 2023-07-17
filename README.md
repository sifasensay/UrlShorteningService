# UrlShorteningService

Main Steps:

1.Visual Studio -> Create new project -> ASP.NET Core Web API -> .NET 7 & minimal selected(so Controller folder is not used)

2.Created Models folder and ShortUrlModel.cs

3.Created UrlRequestModel.cs and UrlResponseModel.cs for user request and response.

4.Microsoft.EntityFrameworkCore.Sqlite installed from the NuGet as an embedded db.

5.Connection string added in appsettings.json, Program.cs and new folder called Data with ShortUrlDbContext.cs created.

6.Entity Framework migrations used to create initial database (reference from: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli documentation).

7.VS PowerShell -> dotnet tool install --global dotnet-ef.

8.Microsoft.EntityFrameworkCore.Design installed from the NuGet.

9.VS PowerShell -> dotnet ef migrations add InitialCreate

10.VS PowerShell -> dotnet ef database update

11. /generateshorturl Post method created in Program.js so short url can be generated with/without user input.

12. if userHashCode is null or empty then random hash is generated, if not user requested hash is used.

13. /redirectshorturl takes the short url as an input and redirects the url.


Run the code:

Run the code with https from Visual Studio and use Swagger to test the post methods.
