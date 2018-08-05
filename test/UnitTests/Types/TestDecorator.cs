namespace Microsoft.Extensions.DependencyInjection.Decorator.UnitTests.Types
{
    internal class TestDecorator : ITestInterface
    {
        public TestDecorator(ITestInterface instance)
        {
            Instance = instance;
        }

        public ITestInterface Instance { get; }
        public int Id => Instance.Id;
    }
}