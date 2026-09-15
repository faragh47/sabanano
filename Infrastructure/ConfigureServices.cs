using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

using CleanArchitecture.Infrastructure.Files;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MediatR;
using System.Reflection;

namespace WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitectureDb"));
        }
        else
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
               builder => builder.MigrationsAssembly(typeof(ConfigureServices).Assembly.FullName)));
            // builder => builder.MigrationsAssembly("WebApi")));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped<ApplicationDbContextInitialiser>();


        var _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        services.AddCustomIdentity(_siteSetting.IdentitySettings);
        //services.AddIdentityServer();//.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICsvFileBuilder, CsvFileBuilder>();

        services.AddAuthentication();
        // .AddIdentityServerJwt();

        //services.AddAuthorization(options =>
        //        options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
    public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
    {
        IdentityBuilderExtensions.AddDefaultTokenProviders(services.AddIdentity<ApplicationUser, AccRole>(identityOptions =>
        {
            //Password Settings
            identityOptions.Password.RequireDigit = settings.PasswordRequireDigit;
            identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
            identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumeric; //#@!
            identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;
            identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;

            //UserName Settings
            identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;

            //Singin Settings
            //identityOptions.SignIn.RequireConfirmedEmail = false;
            //identityOptions.SignIn.RequireConfirmedPhoneNumber = false;

            //Lockout Settings
            //identityOptions.Lockout.MaxFailedAccessAttempts = 5;
            //identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //identityOptions.Lockout.AllowedForNewUsers = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>());
    }
}
