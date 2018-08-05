using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection.Decorator
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDecorator<TService>(this IServiceCollection services, Func<TService, TService> decoratorFactory)
        {
            if (decoratorFactory == null)
                throw new ArgumentNullException(nameof(decoratorFactory));

            var serviceType = typeof(TService);

            var originalDescriptor = services.SingleOrThrow(
                d => d.ServiceType == serviceType,
                notFoundException: () => new InvalidOperationException($"Original type of '{serviceType.Name}' is not registered."),
                multipleFoundException: () => new InvalidOperationException($"Multiple registrations found for the type: '{serviceType.Name}'."));

            var decoratorDescriptor = new ServiceDescriptor(
                serviceType,
                sp => decoratorFactory.Invoke((TService)sp.GetService(originalDescriptor)),
                originalDescriptor.Lifetime);

            return services.Replace(decoratorDescriptor);
        }
    }
}