# VikKol.Extensions.Microsoft.DependencyInjection.Decorator
Simple implementation of Decorators in Microsoft.Extensions.DependencyInjection

### Example
```csharp
   services.AddTransient<ITestInterface, TestImplementation>();
   services.AddDecorator<ITestInterface>(original => new TestDecorator(original));
```

[Available in Nuget](https://www.nuget.org/packages/VikKol.Extensions.Microsoft.DependencyInjection.Decorator)