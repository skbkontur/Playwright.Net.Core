using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SkbKontur.Playwright.TestCore.Dependencies;

namespace Tests.Infra;

/// <summary>
/// Фабрика зависимостей для интеграции с Microsoft.Extensions.DependencyInjection.
/// Создаёт зависимости для конструкторов page objects через DI контейнер.
/// </summary>
/// <param name="serviceProvider">Провайдер сервисов для разрешения зависимостей</param>
/// <param name="filter">Фильтр для определения, какие параметры разрешать через DI</param>
public class DependencyFactory(
    IServiceProvider serviceProvider,
    IDependenciesFilter filter)
    : IDependenciesFactory
{
    /// <summary>
    /// Создать массив зависимостей для указанного типа элемента управления.
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