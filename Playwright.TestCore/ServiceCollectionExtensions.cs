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
    public static IServiceCollection AddPlaywrightTestCore(this IServiceCollection sc)
        => sc.AddPlaywrightTestCore<DefaultPlaywrightConfiguration, SingletonBrowserProvider>();
    
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
        sc.AddScoped<IContextTracing, ContextTracing>();
        sc.AddScoped<ITracingConfigurator, DefaultTracingConfigurator>();
        sc.AddScoped<IPageGetter, PageProvider>();
        sc.AddScoped<ILocalStorage>(x => new LocalStorage(x.GetRequiredService<IPageGetter>()));
        return sc;
    }

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

    public static IServiceCollection ReplaceTracing<TTracing, TConfig>(this IServiceCollection sc)
        where TTracing : class, IContextTracing
        where TConfig : class, ITracingConfigurator
    {
        sc.RemoveAll<IContextTracing>();
        sc.RemoveAll<ITracingConfigurator>();
        sc.AddScoped<IContextTracing, TTracing>();
        sc.AddScoped<ITracingConfigurator, TConfig>();
        return sc;
    }

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