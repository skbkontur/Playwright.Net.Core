using System.Collections.Generic;
using System.Reflection;

namespace SkbKontur.Playwright.TestCore.Dependencies;

/// <summary>
/// Интерфейс фильтра зависимостей.
/// Определяет, какие параметры конструктора должны быть разрешены через DI.
/// </summary>
public interface IDependenciesFilter
{
    /// <summary>
    /// Применить фильтр к коллекции параметров конструктора.
    /// </summary>
    /// <param name="getParameters">Коллекция параметров для фильтрации</param>
    /// <returns>Отфильтрованная коллекция параметров, которые должны быть разрешены через DI</returns>
    IEnumerable<ParameterInfo> Apply(IEnumerable<ParameterInfo> getParameters);
}