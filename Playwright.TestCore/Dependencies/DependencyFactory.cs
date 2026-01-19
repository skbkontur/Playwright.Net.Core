using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SkbKontur.Playwright.TestCore.Dependencies;

/// <summary>
/// Фабрика зависимостей для интеграции с Microsoft.Extensions.DependencyInjection.
/// Создаёт зависимости для конструкторов page objects через DI контейнер.
/// </summary>
public class DependencyFactory(
    IServiceProvider serviceProvider,
    IDependenciesFilter filter)
    : IDependenciesFactory
{
    /// <summary>
    /// Создать массив зависимостей для указанного типа контрола.
    /// Использует DI контейнер для разрешения зависимостей.
    /// </summary>
    /// <param name="controlType">Тип контрола, для которого создаются зависимости</param>
    /// <returns>Массив разрешённых зависимостей для конструктора</returns>
    /// <exception cref="NotSupportedException">Выбрасывается, если класс имеет более одного конструктора</exception>
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