using Microsoft.Extensions.DependencyInjection;
using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Browsers;
using SkbKontur.Playwright.TestCore.Configurations;
using SkbKontur.Playwright.TestCore.Dependencies;
using SkbKontur.Playwright.TestCore.Factories;
using SkbKontur.Playwright.TestCore.Pages;

namespace Tests.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UsePlaywright(this IServiceCollection sc)
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
        sc.AddScoped<ITestInfoGetter, TestInfoGetter>();
        sc.AddScoped<IPageObjectsFactory, PageObjectsFactory>();
        sc.AddScoped<IControlFactory, PageObjectsFactory>();
        sc.AddScoped<IPageFactory, PageObjectsFactory>();
        sc.AddScoped<IDependenciesFactory, DependencyFactory>();
        sc.AddScoped<IBrowserFactory, FirefoxFactory>();
        return sc;
    }
}