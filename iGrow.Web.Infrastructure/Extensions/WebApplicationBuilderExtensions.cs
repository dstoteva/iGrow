namespace iGrow.Web.Infrastructure.Extensions
{
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public static class WebApplicationBuilderExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services, Type repositoryType)
        {
            Assembly repositoriesAssembly = repositoryType.Assembly;

            IEnumerable<Type> repositoryInterfaces = repositoriesAssembly.GetTypes()
                .Where(t => t.IsInterface && t.Name.StartsWith("I") && t.Name.EndsWith("Repository"))
                .ToArray();

            foreach (Type serviceType in repositoryInterfaces)
            {
                Type implementationType = repositoriesAssembly
                    .GetTypes()
                    .Single(t => t.IsClass && !t.IsAbstract && serviceType.IsAssignableFrom(t));

                services.AddScoped(serviceType, implementationType);
            }

            return services;
        }

        public static IServiceCollection RegisterUserServices(this IServiceCollection serviceCollection, Type serviceType)
        {
            Assembly servicesAssembly = serviceType.Assembly;

            IEnumerable<Type> serviceInterfaces = servicesAssembly
                .GetTypes()
                .Where(t => t.IsInterface && t.Name.StartsWith("I") && t.Name.EndsWith("Service"))
                .ToArray();

            foreach (Type currentServiceType in serviceInterfaces)
            {
                Type implementationType = servicesAssembly
                    .GetTypes()
                    .Single(t => t is { IsClass: true, IsAbstract: false } && currentServiceType.IsAssignableFrom(t));

                serviceCollection.AddScoped(currentServiceType, implementationType);
            }

            return serviceCollection;
        }
    }
}
