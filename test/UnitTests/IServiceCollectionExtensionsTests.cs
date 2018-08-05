using Microsoft.Extensions.DependencyInjection.Decorator.UnitTests.Types;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection.Decorator.UnitTests
{
    public class IServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddDecoratorSingletonTest()
        {
            var services = new ServiceCollection();
            var instance = new TestImplementation() { Id = 1 };

            services.AddSingleton<ITestInterface>(instance);
            services.AddDecorator<ITestInterface>(originalInstance => new TestDecorator(originalInstance));

            var serviceProvider = services.BuildServiceProvider();
            var decorator = serviceProvider.GetService<ITestInterface>();

            Assert.True(decorator is TestDecorator);
            Assert.Equal(instance, ((TestDecorator)decorator).Instance);
            Assert.Equal(decorator.Id, instance.Id);

            var decoratorRequestedSecondTime = serviceProvider.GetService<ITestInterface>();
            Assert.Equal(decorator, decoratorRequestedSecondTime);
        }

        [Fact]
        public void AddDecoratorTransientTest()
        {
            const int instanceValue = 1;
            var services = new ServiceCollection();

            services.AddTransient<ITestInterface>(sp => new TestImplementation() { Id = instanceValue });
            services.AddDecorator<ITestInterface>(originalInstance => new TestDecorator(originalInstance));

            var serviceProvider = services.BuildServiceProvider();

            var decorator = serviceProvider.GetService<ITestInterface>();
            var decoratorRequestedSecondTime = serviceProvider.GetService<ITestInterface>();

            Assert.True(decorator is TestDecorator);
            Assert.True(decoratorRequestedSecondTime is TestDecorator);
            Assert.NotEqual(decorator, decoratorRequestedSecondTime);
            Assert.Equal(instanceValue, decorator.Id);
            Assert.Equal(instanceValue, decoratorRequestedSecondTime.Id);
        }

        [Fact]
        public void AddDecoratorScopedTest()
        {
            const int instanceValue = 1;
            var services = new ServiceCollection();

            services.AddScoped<ITestInterface>(sp => new TestImplementation() { Id = instanceValue });
            services.AddDecorator<ITestInterface>(originalInstance => new TestDecorator(originalInstance));

            var serviceProvider = services.BuildServiceProvider();

            var scope1 = serviceProvider.CreateScope();
            var decorator1 = scope1.ServiceProvider.GetService<ITestInterface>();
            var decorator1RequestedSecondTime = scope1.ServiceProvider.GetService<ITestInterface>();
            Assert.True(decorator1 is TestDecorator);
            Assert.Equal(decorator1, decorator1RequestedSecondTime);
            Assert.Equal(instanceValue, decorator1.Id);
            Assert.Equal(instanceValue, decorator1RequestedSecondTime.Id);

            var scope2 = serviceProvider.CreateScope();
            var decorator2 = scope2.ServiceProvider.GetService<ITestInterface>();
            var decorator2RequestedSecondTime = scope2.ServiceProvider.GetService<ITestInterface>();
            Assert.True(decorator2 is TestDecorator);
            Assert.Equal(decorator2, decorator2RequestedSecondTime);
            Assert.Equal(instanceValue, decorator2.Id);
            Assert.Equal(instanceValue, decorator2RequestedSecondTime.Id);

            Assert.NotEqual(decorator1, decorator2);
        }
    }
}