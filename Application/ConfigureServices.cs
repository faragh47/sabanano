using System.Reflection;
using AutoMapper;
using CleanArchitecture.Application.Common.Behaviours;
using DataTransferObjects.CustomMapping;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, params Assembly[] assemblies)
    {

        services.AddAutoMapper(config =>
        {
            AddCustomMappingProfile((IMapperConfigurationExpression)config,assemblies);
            //config.Advanc(configProvicer =>
            //{
            //    configProvicer.CompileMappings();
            //});
        }, assemblies);

        // services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        return services;
    }

    //public static void AddCustomMappingProfile(this IMapperConfigurationExpression config, params Assembly[] assemblies)
    //{
    //    //AddCustomMappingProfile(config, new[] {Assembly.GetAssembly()});
    //    AddCustomMappingProfile(config, assemblies);
    //}

    public static void AddCustomMappingProfile(this IMapperConfigurationExpression config, params Assembly[] assemblies)
    {

       var assembliesList= assemblies.ToList();

        assembliesList.Add(Assembly.GetExecutingAssembly());

        var allTypes = assembliesList.SelectMany(a => a.ExportedTypes);

        var list = allTypes.Where(type => type.IsClass && !type.IsAbstract &&
            type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
            .Select(type => (IHaveCustomMapping)Activator.CreateInstance(type));

        var profile = new CustomMappingProfile(list);

        config.AddProfile(profile);

        //config.AddProfile<BranchTestProfile>();
    }
}
