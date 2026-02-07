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
    /// <item><description>Дефолтный провайдер информации о тестах</description></item>
    /// </list>
    /// <para>
    /// Для кастомизации браузера или конфигурации используйте метод <see cref="UseBrowser{TFactory, TConfig, TUpdater}"/>.
    /// Для добавления Page Object Model используйте метод <see cref="UsePom"/>.
    /// Для настройки трассировок используйте метод <see cref="ReplaceTracing{TTracing, TConfig}"/>.
    /// Для настройки автоматической аутентификации используйте метод <see cref="UseAuthenticator{TAuthenticator, TStrategy}"/>.
    /// </para>
    /// <example>
    /// <code>
    /// services.AddPlaywrightTestCore&lt;XunitTestInfoGetter&gt;()
    ///        .UseBrowser&lt;ChromiumFactory, HeadfulConfigurator&gt;()
    ///        .UsePom();
    /// </code>
    /// </example>
    /// </remarks>
    public static IServiceCollection AddPlaywrightTestCore(this IServiceCollection sc)
        => sc.AddPlaywrightTestCore<DefaultPlaywrightConfiguration>();
    
    /// <typeparam name="TTestInfoGetter">Тип конфигуратора реализующий IPlaywrightConfiguration</typeparam>
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
    /// <item><description>Дефолтный провайдер информации о тестах</description></item>
    /// </list>
    /// <para>
    /// Для кастомизации браузера или конфигурации используйте метод <see cref="UseBrowser{TFactory, TConfig, TUpdater}"/>.
    /// Для добавления Page Object Model используйте метод <see cref="UsePom"/>.
    /// Для настройки трассировок используйте метод <see cref="ReplaceTracing{TTracing, TConfig}"/>.
    /// Для настройки автоматической аутентификации используйте метод <see cref="UseAuthenticator{TAuthenticator, TStrategy}"/>.
    /// </para>
    /// <example>
    /// <code>
    /// services.AddPlaywrightTestCore&lt;XunitTestInfoGetter&gt;()
    ///        .UseBrowser&lt;ChromiumFactory, HeadfulConfigurator&gt;()
    ///        .UsePom();
    /// </code>
    /// </example>
    /// </remarks>
    public static IServiceCollection AddPlaywrightTestCore<TConfig>(this IServiceCollection sc)
        where TConfig : class, IPlaywrightConfiguration, new()
    {
        sc.AddSingleton<IPlaywrightFactory, PlaywrightFactory<TConfig>>();
        sc.UseBrowser<ChromeFactory, HeadlessConfigurator, ViewportSizeUpdater>();
        sc.UseAuthenticator<WithoutAuthAuthenticator, AuthWithCacheStrategy>();
        sc.UseTestInfoProvider<EmptyTestInfoProvider>();

        sc.AddScoped<IBrowserGetter, DefaultBrowserProvider>();
        sc.AddScoped<IContextTracing, ContextTracing>();
        sc.AddScoped<ITracingConfigurator, DefaultTracingConfigurator>();
        sc.AddScoped<IPageGetter, PageProvider>();
        sc.AddScoped<ILocalStorage, LocalStorage>();
        return sc;
    }

    /// <summary>
    /// Заменить регистрацию браузера и его конфигуратора в DI контейнере
    /// </summary>
    /// <typeparam name="TFactory">Тип фабрики браузера, реализующий <see cref="IBrowserFactory"/></typeparam>
    /// <typeparam name="TConfig">Тип конфигуратора браузера, реализующий <see cref="IBrowserConfigurator"/></typeparam>
    /// <typeparam name="TUpdater">Тип конфигуратора контекста, реализующий <see cref="IContextOptionsUpdater"/></typeparam>
    /// <param name="sc">Коллекция сервисов для расширения</param>
    /// <returns>Расширенная коллекция сервисов</returns>
    /// <remarks>
    /// <para>
    /// Используйте этот метод для переопределения браузера по умолчанию (Chromium) и его конфигурации.
    /// </para>
    /// </remarks>
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
    /// services.AddPlaywrightTestCore().UsePom();
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

    /// <summary>
    /// Заменяет текущие реализации механизмов трассировки Playwright в контейнере зависимостей.
    /// </summary>
    /// <typeparam name="TTracing">Тип, реализующий <see cref="IContextTracing"/>, отвечающий за логику записи трассировок.</typeparam>
    /// <typeparam name="TConfig">Тип, реализующий <see cref="ITracingConfigurator"/>, отвечающий за параметры конфигурации трассировки.</typeparam>
    /// <param name="sc">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
    /// <returns>Измененная коллекция сервисов для возможности построения цепочки вызовов (Fluent API).</returns>
    /// <remarks>
    /// Метод сначала удаляет все ранее зарегистрированные реализации интерфейсов 
    /// <see cref="IContextTracing"/> и <see cref="ITracingConfigurator"/>, 
    /// а затем регистрирует новые типы с жизненным циклом Scoped. 
    /// Это гарантирует, что в системе будет использоваться только одна конкретная стратегия трассировки на один Scope.
    /// </remarks>
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

    /// <summary>
    /// Регистрирует или заменяет текущий механизм аутентификации в контейнере зависимостей.
    /// </summary>
    /// <typeparam name="TAuthenticator">Тип реализации аутентификатора, который необходимо использовать.</typeparam>
    /// <typeparam name="TStrategy">Тип стратегии аутентификации.</typeparam>
    /// <param name="sc">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
    /// <returns>Коллекция сервисов для продолжения настройки (Fluent API).</returns>
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

    /// <summary>
    /// Регистрирует или переопределяет реализацию для получения метаданных текущего теста.
    /// </summary>
    /// <typeparam name="TTestInfoGetter">Тип, реализующий доступ к информации о тесте.</typeparam>
    /// <param name="sc">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
    /// <returns>Коллекция сервисов для дальнейшей настройки.</returns>
    /// <remarks>
    /// Метод удаляет все ранее зарегистрированные реализации <see cref="ITestInfoGetter"/> 
    /// и устанавливает новую в жизненном цикле <c>Scoped</c>. 
    /// Это позволяет инфраструктуре трассировки получать актуальные данные о тесте (ID, имя, директория) 
    /// независимо от используемого тестового фреймворка.
    /// </remarks>
    public static IServiceCollection UseTestInfoProvider<TTestInfoGetter>(this IServiceCollection sc)
        where TTestInfoGetter : class, ITestInfoGetter
    {
        sc.RemoveAll<ITestInfoGetter>();
        sc.AddScoped<ITestInfoGetter, TTestInfoGetter>();
        return sc;
    }
}