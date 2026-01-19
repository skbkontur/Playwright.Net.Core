using System;

namespace SkbKontur.Playwright.TestCore.Dependencies;

/// <summary>
/// Интерфейс фабрики зависимостей для создания объектов с dependency injection.
/// Определяет контракт для разрешения зависимостей конструкторов page objects.
/// </summary>
public interface IDependenciesFactory
{
    /// <summary>
    /// Создать массив зависимостей для указанного типа контрола.
    /// </summary>
    /// <param name="controlType">Тип контрола</param>
    /// <returns>Массив разрешённых зависимостей для конструктора</returns>
    object[] CreateDependency(Type controlType);
}