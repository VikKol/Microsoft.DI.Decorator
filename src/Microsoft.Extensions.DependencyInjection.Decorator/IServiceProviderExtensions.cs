using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.Decorator
{
    internal static class IServiceProviderExtensions
    {
        public static object GetService(this IServiceProvider serviceProvider, ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor == null)
                throw new ArgumentNullException(nameof(serviceDescriptor));

            if (serviceDescriptor.ImplementationInstance != null)
            {
                return serviceDescriptor.ImplementationInstance;
            }

            if (serviceDescriptor.ImplementationFactory != null)
            {
                return serviceDescriptor.ImplementationFactory.Invoke(serviceProvider);
            }

            var ctor = GetConstructor(serviceDescriptor.ImplementationType);
            var ctorParameters = ctor.GetParameters();
            var ctorDepencencies = new object[ctorParameters.Length];

            for (int i = 0; i < ctorParameters.Length; i++)
            {
                ctorDepencencies[i] = serviceProvider.GetService(ctorParameters[i].ParameterType);
            }

            return ctor.Invoke(ctorDepencencies);
        }

        private static ConstructorInfo GetConstructor(Type type)
        {
            return type.GetConstructors().SingleOrThrow(
                c => c.IsPublic,
                notFoundException: () => new InvalidOperationException($"No public constructor found for the type: '{type.Name}'."),
                multipleFoundException: () => new InvalidOperationException($"Type: '{type.Name}' must have single constructor."));
        }
    }
}