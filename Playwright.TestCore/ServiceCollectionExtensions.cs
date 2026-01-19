using Microsoft.Extensions.DependencyInjection;
using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Browsers;
using SkbKontur.Playwright.TestCore.Configurations;
using SkbKontur.Playwright.TestCore.Dependencies;
using SkbKontur.Playwright.TestCore.Factories;
using SkbKontur.Playwright.TestCore.Pages;

namespace SkbKontur.Playwright.TestCore;

/// <summary>
/// Расширения для IServiceCollection для интеграции Playwright TestCore.
/// Предоставляет метод для регистрации всех необходимых компонентов в DI контейнере.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Зарегистрировать все компоненты Playwright TestCore в DI контейнере.
    /// Включает полный набор сервисов с указанным провайдером информации о тестах.
    /// </summary>
    /// <typeparam name="TTestInfoGetter">Тип провайдера информации о тестах, реализующий ITestInfoGetter</typeparam>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    public static IServiceCollection AddPlaywrightTestCore<TTestInfoGetter>(this IServiceCollection sc)
        where TTestInfoGetter : class, ITestInfoGetter
    {
        sc.AddSingleton<IPlaywrightFactory, PlaywrightFactory<DefaultPlaywrightConfiguration>>();
        sc.AddScoped<IAuthStrategy, AuthWithCacheStrategy>();
        sc.AddScoped<IAutentificator, WithoutAuthAutentificator>();
        sc.AddScoped<IBrowserGetter, DefaultBrowserGetter>();
        sc.AddScoped<IContextTracing, ContextTracing>();
        sc.AddScoped<ITracingConfigurator, DefaultTracingConfigurator>();
        sc.AddScoped<Navigation>();
        sc.AddScoped<IPageGetter, PageGetter>();
        sc.AddScoped<ILocalStorage, LocalStorage>();
        sc.AddScoped<IDependenciesFilter, DefaultDependenciesFilter>();
        sc.AddScoped<IBrowserConfigurator, HeadlessConfigurator>();
        sc.AddScoped<IPageObjectsFactory, PageObjectsFactory>();
        sc.AddScoped<IControlFactory, PageObjectsFactory>();
        sc.AddScoped<IPageFactory, PageObjectsFactory>();
        sc.AddScoped<IBrowserFactory, FirefoxFactory>();
        sc.AddScoped<IDependenciesFactory, DependencyFactory>();
        sc.AddScoped<ITestInfoGetter, TTestInfoGetter>();
        return sc;
    }
}