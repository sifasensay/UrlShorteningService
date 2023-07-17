using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Reflection.Metadata;
using UrlShorteningService.Data;
using UrlShorteningService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//create connextion string with options
var connectionStr = builder.Configuration.GetConnectionString(name: "DefaultConnection");
builder.Services.AddDbContext<ShortUrlDbContext>(options => options.UseSqlite(connectionStr));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/generateshorturl", async (UrlRequestModel url, ShortUrlDbContext db) =>
{
    //check if the url is in valid format
    if(!Uri.TryCreate(url.Url, UriKind.Absolute, out var uriResult))
    {
        return Results.BadRequest("Invalid input URL format."); //bad request with a message
    }

    var hashCode = url.UserHashCode;
    if (string.IsNullOrEmpty(url.UserHashCode))
    {
        //generate random alphanumeric string - max 6 chars
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        hashCode = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
    }

  
    //map the hash and the long url
    var shortUrl = new ShortUrlModel()
    {
        Url = url.Url,
        ShortUrl = hashCode,
    };

    db.ShortUrls.Add(shortUrl);
    await db.SaveChangesAsync();

    //generate short url by using the long one's host and scheme details
    Uri requestedUri = new Uri(url.Url);
    string host = requestedUri.Host;
    string scheme = requestedUri.Scheme;

    var generatedUrl = $"{scheme}://{host}/{hashCode}";
    var responseModel = new UrlResponseModel()
    {
        ShortUrl = generatedUrl
    };

    return Results.Ok(responseModel); //return the short url with success response code

});


app.MapPost("/redirectshorturl", async (UrlRedirectRequestModel url, ShortUrlDbContext db) =>
{
    
   if (string.IsNullOrEmpty(url.ShortUrl))
    {
        return Results.BadRequest("Short url is empty");
    }
    Uri uri = new Uri(url.ShortUrl);
    var hashCode = uri?.Segments.LastOrDefault(); //get the hash code form the uri
    var urlAvailable = await db.ShortUrls.FirstOrDefaultAsync(x => x.ShortUrl.Trim() == hashCode.Trim()); //check if the short url exist in db

    if (urlAvailable == null)
    {
        return Results.BadRequest("Url not found");
    }

    //return Results.Ok(urlAvailable.Url);
    return Results.Redirect(urlAvailable.Url);

});


app.Run();


