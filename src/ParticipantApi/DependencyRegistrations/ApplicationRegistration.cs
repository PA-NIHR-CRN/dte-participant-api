using System.Reflection;
using Application.Enrichers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ParticipantApi.DependencyRegistrations
{
    public static class ApplicationRegistration
    {
        private const string ApplicationAssemblyName = "Application";

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.Load(ApplicationAssemblyName));
            services.Scan(s => s
                .FromAssemblies(Assembly.Load(ApplicationAssemblyName))
                .AddClasses(c => c.AssignableTo(typeof(IEnricher<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            
            return services;
        }
    }
}