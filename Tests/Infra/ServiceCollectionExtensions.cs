using Microsoft.Extensions.DependencyInjection;
using SkbKontur.Playwright.TestCore;

namespace Tests.Infra;

/// <summary>
/// Расширения для IServiceCollection для интеграции Playwright TestCore с NUnit.
/// Предоставляет метод для регистрации всех необходимых компонентов в DI контейнере.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Зарегистрировать все компоненты Playwright TestCore в DI контейнере для использования с NUnit.
    /// Удобный метод, который использует TestInfoGetter для получения информации о тестах из NUnit TestContext.
    /// </summary>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    public static IServiceCollection UsePlaywright(this IServiceCollection sc)
    {
        sc.AddPlaywrightTestCore<TestInfoGetter>();
        sc.UsePom();
        return sc;
    }
}