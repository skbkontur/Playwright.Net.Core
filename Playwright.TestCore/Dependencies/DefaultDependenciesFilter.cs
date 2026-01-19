using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Dependencies;

/// <summary>
/// Стандартный фильтр зависимостей.
/// Исключает базовые типы Playwright (IPage, ILocator) из списка параметров для DI.
/// </summary>
public class DefaultDependenciesFilter : IDependenciesFilter
{
    /// <summary>
    /// Отфильтровать параметры конструктора, исключив базовые типы Playwright.
    /// </summary>
    /// <param name="getParameters">Коллекция параметров конструктора</param>
    /// <returns>Отфильтрованная коллекция параметров без базовых типов Playwright</returns>
    public IEnumerable<ParameterInfo> Apply(IEnumerable<ParameterInfo> getParameters)
    {
        return getParameters
            .Where(x => x.ParameterType != typeof(IPage))
            .Where(x => x.ParameterType != typeof(ILocator))
            .Where(x => x.ParameterType != typeof(Task<IPage>))
            .Where(x => x.ParameterType != typeof(Task<ILocator>))
            .Where(x => x.ParameterType != typeof(Func<ILocator>))
            .Where(x => x.ParameterType != typeof(Func<IPage>))
            .Where(x => x.ParameterType != typeof(Func<Task<ILocator>>))
            .Where(x => x.ParameterType != typeof(Func<Task<IPage>>));
    }
}