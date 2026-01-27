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
    /// Зарегистрировать все компоненты Playwright TestCore в DI контейнере.
    /// Включает полный набор сервисов с указанным провайдером информации о тестах.
    /// </summary>
    /// <typeparam name="TTestInfoGetter">Тип провайдера информации о тестах, реализующий ITestInfoGetter</typeparam>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    /// <remarks>
    /// <para>
    /// Метод регистрирует базовую конфигурацию Playwright TestCore, включая:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Фабрику Playwright</description></item>
    /// <item><description>Стратегию аутентификации с кэшированием</description></item>
    /// <item><description>Браузер по умолчанию (Chromium)</description></item>
    /// <item><description>Конфигурацию трассировки</description></item>
    /// <item><description>Менеджер страниц и localStorage</description></item>
    /// <item><description>Конфигурацию headless-режима</description></item>
    /// <item><description>Указанный провайдер информации о тестах</description></item>
    /// </list>
    /// <para>
    /// Для кастомизации браузера или конфигурации используйте метод <see cref="UseBrowser{TFactory, TConfig}"/>.
    /// Для добавления Page Object Model используйте метод <see cref="UsePom"/>.
    /// </para>
    /// <example>
    /// <code>
    /// services.AddPlaywrightTestCore&lt;XunitTestInfoGetter&gt;()
    ///        .UseBrowser&lt;ChromiumFactory, HeadfulConfigurator&gt;()
    ///        .UsePom();
    /// </code>
    /// </example>
    /// </remarks>
    public static IServiceCollection AddPlaywrightTestCore<TTestInfoGetter>(this IServiceCollection sc)
        where TTestInfoGetter : class, ITestInfoGetter
    {
        sc.AddSingleton<IPlaywrightFactory, PlaywrightFactory<DefaultPlaywrightConfiguration>>();
        sc.AddScoped<IAuthStrategy, AuthWithCacheStrategy>();
        sc.AddScoped<IAuthenticator, WithoutAuthAuthenticator>();
        sc.AddScoped<IBrowserGetter, DefaultBrowserGetter>();
        sc.AddScoped<IContextTracing, ContextTracing>();
        sc.AddScoped<ITracingConfigurator, DefaultTracingConfigurator>();
        sc.AddScoped<IPageGetter, PageGetter>();
        sc.AddScoped<ILocalStorage, LocalStorage>();
        sc.AddScoped<IBrowserConfigurator, HeadlessConfigurator>();
        sc.AddScoped<IBrowserFactory, ChromeFactory>();
        sc.AddScoped<ITestInfoGetter, TTestInfoGetter>();
        return sc;
    }

    /// <summary>
    /// Заменить регистрацию браузера и его конфигуратора в DI контейнере
    /// </summary>
    /// <typeparam name="TFactory">Тип фабрики браузера, реализующий <see cref="IBrowserFactory"/></typeparam>
    /// <typeparam name="TConfig">Тип конфигуратора браузера, реализующий <see cref="IBrowserConfigurator"/></typeparam>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    /// <remarks>
    /// <para>
    /// Метод удаляет существующие регистрации <see cref="IBrowserFactory"/> и <see cref="IBrowserConfigurator"/>,
    /// затем регистрирует указанные реализации.
    /// </para>
    /// <para>
    /// Используйте этот метод для переопределения браузера по умолчанию (Chromium) и его конфигурации.
    /// </para>
    /// </remarks>
    public static IServiceCollection UseBrowser<TFactory, TConfig>(this IServiceCollection sc)
        where TFactory : class, IBrowserFactory
        where TConfig : class, IBrowserConfigurator
    {
        sc.RemoveAll<IBrowserFactory>();
        sc.RemoveAll<IBrowserConfigurator>();
        sc.AddScoped<IBrowserFactory, TFactory>();
        sc.AddScoped<IBrowserConfigurator, TConfig>();
        return sc;
    }

    /// <summary>
    /// Зарегистрировать компоненты Page Object Model (POM) в DI контейнере
    /// </summary>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    /// <remarks>
    /// <para>
    /// Метод добавляет все необходимые сервисы для работы с Page Object Model:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="Navigation"/> - сервис навигации по страницам</description></item>
    /// <item><description><see cref="IPageObjectsFactory"/> - фабрика для создания page objects</description></item>
    /// <item><description><see cref="IControlFactory"/> - фабрика для создания контролов</description></item>
    /// <item><description><see cref="IPageFactory"/> - фабрика для создания страниц</description></item>
    /// <item><description><see cref="IDependenciesFactory"/> - фабрика зависимостей для page objects</description></item>
    /// <item><description><see cref="IDependenciesFilter"/> - фильтр зависимостей</description></item>
    /// </list>
    /// <para>
    /// Все фабрики регистрируются как реализация <see cref="PageObjectsFactory"/>.
    /// </para>
    /// <para>
    /// Метод должен вызываться после <see cref="AddPlaywrightTestCore{TTestInfoGetter}"/>.
    /// </para>
    /// <example>
    /// <code>
    /// services.AddPlaywrightTestCore&lt;NUnitTestInfoGetter&gt;()
    ///        .UsePom();
    /// </code>
    /// </example>
    /// </remarks>
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
}