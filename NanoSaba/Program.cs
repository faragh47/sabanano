using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure.Persistence;
using System.Reflection;
using Common;
using WebApi;
using CleanArchitecture.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using WebFramework.Middlewares;
using WebApiClient.Middlewares;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));

configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(Assembly.GetAssembly(typeof(ApplicationDbContext)));
builder.Services.AddWebApiServices();
builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(590);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
AssemblyScanner
       .FindValidatorsInAssembly(Assembly.GetAssembly(typeof(ValidationBehaviour<,>)))
           .ForEach(result => builder.Services.AddScoped(result.InterfaceType, result.ValidatorType));
var _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

var app = builder.Build();

app.UseAuthenticationMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.MapRazorPages();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Home/Error");
//app.UseCustomExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

