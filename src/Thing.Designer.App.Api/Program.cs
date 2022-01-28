global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Thing.Domain;
global using Thing.Designer.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Thing.Designer.Infrastructure.Local;
using Thing.Designer.Services;
using ProtoBuf.Grpc.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Thing.Designer.Infrastructure.Local.Models;
using System.IdentityModel.Tokens.Jwt;
using Duende.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");

builder.Services.AddThingDesignerInfrastructure(
    sqliteConnectionString: connectionString,
    configureIdentity: identity => identity.AddDefaultUI(),
    configureIdentityServer: server => server
        .AddApiAuthorization<ApplicationUser, ApplicationDbContext>());

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt()
    .AddLocalApi();

builder.Services.AddCodeFirstGrpc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Allow requests from the web client
app.UseCors(cors => cors.WithOrigins(
    builder.Configuration["Apps:BlazorClient:Origin"]
).AllowAnyMethod().AllowAnyHeader());

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseGrpcWeb();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<CustomerDesignService>()
        .EnableGrpcWeb()
        .RequireAuthorization(
            new AuthorizeAttribute()
            {
                AuthenticationSchemes = IdentityServerConstants.LocalApi.AuthenticationScheme
            })
        .RequireCors(cors => cors.WithOrigins(
            builder.Configuration["Apps:BlazorClient:Origin"])
            .WithExposedHeaders("Grpc-Status", "Grpc-Message"));
});

app.MapRazorPages();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        //context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();
