using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
    /// Зарегистрировать все компоненты Playwright TestCore в DI контейнере с конфигурацией по умолчанию.
    /// Использует <see cref="DefaultPlaywrightConfiguration"/> и <see cref="SingletonBrowserProvider"/>.
    /// </summary>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    public static IServiceCollection AddPlaywrightTestCore(this IServiceCollection sc)
        => sc.AddPlaywrightTestCore<DefaultPlaywrightConfiguration, SingletonBrowserProvider>();
    
    /// <summary>
    /// Зарегистрировать все компоненты Playwright TestCore в DI контейнере с указанной конфигурацией и провайдером браузера.
    /// </summary>
    /// <typeparam name="TConfig">Тип конфигурации Playwright, реализующий <see cref="IPlaywrightConfiguration"/></typeparam>
    /// <typeparam name="TBrowserProvider">Тип провайдера браузера, реализующий <see cref="IBrowserGetter"/></typeparam>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    public static IServiceCollection AddPlaywrightTestCore<TConfig, TBrowserProvider>(this IServiceCollection sc)
        where TConfig : class, IPlaywrightConfiguration, new()
        where TBrowserProvider : class, IBrowserGetter
    {
        sc.AddSingleton<IPlaywrightGetter, PlaywrightProvider<TConfig>>();
        sc.AddSingleton<IBrowserGetter, TBrowserProvider>();
        sc.UseBrowser<ChromeFactory, HeadlessConfigurator, ViewportSizeUpdater>();
        sc.AddScoped<IBrowserContextGetter, DefaultBrowserContextProvider>();
        sc.AddScoped<IBrowserContextFactory, DefaultBrowserContextFactory>();
        sc.UseAuthenticator<WithoutAuthAuthenticator, AuthWithCacheStrategy>();
        sc.AddScoped<IContextTracing, FullTracing>();
        sc.AddScoped<ITracingConfigurator, DefaultTracingConfigurator>();
        sc.AddScoped<IPageGetter, PageProvider>();
        sc.AddScoped<IBeforeDisposePageActions, NoActions>();
        sc.AddScoped<ILocalStorage>(x => new LocalStorage(x.GetRequiredService<IPageGetter>()));
        return sc;
    }

    /// <summary>
    /// Заменить регистрацию фабрики браузера и его конфигуратора в DI контейнере.
    /// </summary>
    /// <typeparam name="TFactory">Тип фабрики браузера, реализующий <see cref="IBrowserFactory"/></typeparam>
    /// <typeparam name="TConfig">Тип конфигуратора браузера, реализующий <see cref="IBrowserConfigurator"/></typeparam>
    /// <typeparam name="TUpdater">Тип обновителя параметров контекста, реализующий <see cref="IContextOptionsUpdater"/></typeparam>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    public static IServiceCollection UseBrowser<TFactory, TConfig, TUpdater>(this IServiceCollection sc)
        where TFactory : class, IBrowserFactory
        where TConfig : class, IBrowserConfigurator
        where TUpdater : class, IContextOptionsUpdater
    {
        sc.RemoveAll<IBrowserFactory>();
        sc.RemoveAll<IBrowserConfigurator>();
        sc.RemoveAll<IContextOptionsUpdater>();
        sc.AddScoped<IBrowserFactory, TFactory>();
        sc.AddScoped<IBrowserConfigurator, TConfig>();
        sc.AddScoped<IContextOptionsUpdater, TUpdater>();
        return sc;
    }

    /// <summary>
    /// Зарегистрировать компоненты Page Object Model (POM) в DI контейнере.
    /// </summary>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    public static IServiceCollection UsePom(this IServiceCollection sc)
    {
        sc.AddScoped<Navigation>();
        sc.AddScoped<IPageObjectsFactory, PageObjectsFactory>();
        sc.AddScoped<IControlFactory, PageObjectsFactory>();
        sc.AddScoped<IPageFactory, PageObjectsFactory>();
        sc.AddScoped<IDependenciesFactory, DependencyFactory>();
        sc.AddScoped<IDependenciesFilter, DefaultDependenciesFilter>();
        return sc;
    }

    /// <summary>
    /// Заменить реализации механизмов трассировки Playwright в DI контейнере.
    /// </summary>
    /// <typeparam name="TTracing">Тип реализации трассировки контекста, реализующий <see cref="IContextTracing"/></typeparam>
    /// <typeparam name="TConfig">Тип конфигуратора трассировки, реализующий <see cref="ITracingConfigurator"/></typeparam>
    /// <typeparam name="TFailureProvider">Тип провайдера результата теста, реализующий <see cref="IFailureTestResult"/></typeparam>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    public static IServiceCollection ReplaceTracing<TTracing, TConfig, TFailureProvider>(this IServiceCollection sc)
        where TTracing : class, IContextTracing
        where TConfig : class, ITracingConfigurator
        where TFailureProvider : class, IFailureTestResult
    {
        sc.RemoveAll<IContextTracing>();
        sc.RemoveAll<ITracingConfigurator>();
        sc.RemoveAll<IFailureTestResult>();
        sc.AddScoped<IContextTracing, TTracing>();
        sc.AddScoped<ITracingConfigurator, TConfig>();
        sc.AddScoped<IFailureTestResult, TFailureProvider>();
        return sc;
    }

    /// <summary>
    /// Заменить реализации аутентификатора и стратегии аутентификации в DI контейнере.
    /// </summary>
    /// <typeparam name="TAuthenticator">Тип аутентификатора, реализующий <see cref="IAuthenticator"/></typeparam>
    /// <typeparam name="TStrategy">Тип стратегии аутентификации, реализующий <see cref="IAuthStrategy"/></typeparam>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    public static IServiceCollection UseAuthenticator<TAuthenticator, TStrategy>(this IServiceCollection sc)
        where TAuthenticator : class, IAuthenticator
        where TStrategy : class, IAuthStrategy
    {
        sc.RemoveAll<IAuthenticator>();
        sc.RemoveAll<IAuthStrategy>();
        sc.AddScoped<IAuthenticator, TAuthenticator>();
        sc.AddScoped<IAuthStrategy, TStrategy>();
        return sc;
    }
}