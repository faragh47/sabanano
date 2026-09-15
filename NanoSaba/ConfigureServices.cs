using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Orders;
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
using Services.Services.V2;
using Data.Contracts;
using Data.Repositories;
using Services;
using Services.IServices.V2;
using Services.IServices;
using Services.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using WebApi.Services;

namespace WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        //  services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddScoped<IJwtService, JwtService>();
        //services.AddSingleton<IFileServerProvider, FileServerProvider>();
        services.AddScoped(typeof(ICrudService<,,,,>), typeof(CrudService<,,,,>));
        services.AddScoped(typeof(IReportService<,,,>), typeof(ReportService<,,,>));
        services.AddHttpContextAccessor();

        //services.AddHealthChecks()
        //    .AddDbContextCheck<ApplicationDbContext>();

        //services.AddControllersWithViews(options =>
        //    options.Filters.Add<ApiResultFilterAttribute>())
        //        .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        //services.AddOpenApiDocument(configure =>
        //{
        //    configure.Title = "CleanArchitecture API";
        //    configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
        //    {
        //        Type = OpenApiSecuritySchemeType.ApiKey,
        //        Name = "Authorization",
        //        In = OpenApiSecurityApiKeyLocation.Header,
        //        Description = "Type into the textbox: Bearer {your JWT token}."
        //    });

        //    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        //});

        addDependecy(services);


        return services;
    }

    private static void addDependecy(IServiceCollection Services)
    {
        var InfrastructureAssembly = typeof(UsersService).Assembly;

        Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        var types = InfrastructureAssembly.GetExportedTypes();

        //var types =
        //  from assembly in AppDomain.CurrentDomain.GetAssemblies()
        //  from type in assembly.GetTypes()
        //  select type;

        var registrationScoppeds =
             from type in types
             where !type.IsAbstract
             where type.GetInterfaces().Contains(typeof(IScopedDependency))
             let services =
                 from iface in type.GetInterfaces()
                 where iface.IsInterface
                 where !iface.IsAssignableFrom(typeof(IScopedDependency))
                 select iface
             from service in services
             select new { service, type };

        foreach (var reg in registrationScoppeds)
        {
            Services.AddScoped(reg.service, reg.type);
        }

        var registrationTransients =
           from type in types
           where !type.IsAbstract
           where type.GetInterfaces().Contains(typeof(ITransientDependency))
           let services =
               from iface in type.GetInterfaces()
               where iface.IsInterface
               where !iface.IsAssignableFrom(typeof(ITransientDependency))
               select iface
           from service in services
           select new { service, type };

        foreach (var reg in registrationTransients)
        {
            Services.AddTransient(reg.service, reg.type);
        }

    }
}
