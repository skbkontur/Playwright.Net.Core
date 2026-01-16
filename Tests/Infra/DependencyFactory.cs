using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SkbKontur.Playwright.TestCore.Dependencies;

namespace Tests.Infra;

public class DependencyFactory(
    IServiceProvider serviceProvider,
    IDependenciesFilter filter)
    : IDependenciesFactory
{
    public object[] CreateDependency(Type controlType)
    {
        var constructors = controlType.GetConstructors();
        if (constructors.Length != 1)
        {
            throw new NotSupportedException($"{controlType} должен иметь только один конструктор");
        }

        var constructor = constructors.Single();

        var parameters =
            filter
                .Apply(constructor.GetParameters())
                .Select<ParameterInfo, object>(x => serviceProvider.GetRequiredService(x.ParameterType));
        return parameters.ToArray();
    }
}