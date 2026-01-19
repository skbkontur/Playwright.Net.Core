using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Browsers;
using SkbKontur.Playwright.TestCore.Configurations;
using SkbKontur.Playwright.TestCore.Dependencies;
using SkbKontur.Playwright.TestCore.Factories;
using SkbKontur.Playwright.TestCore.Pages;

namespace SkbKontur.Playwright.TestCore;

/// <summary>
/// Фабрика для создания объектов Navigation с предустановленными зависимостями.
/// Создаёт готовую к использованию инфраструктуру тестирования.
/// </summary>
public static class NavigationFactory
{
    /// <summary>
    /// Создать объект Navigation с предустановленными зависимостями.
    /// Использует Chrome браузер, стандартную конфигурацию и стратегию без аутентификации.
    /// </summary>
    /// <param name="dependenciesFactory">Фабрика зависимостей для создания page objects</param>
    /// <param name="testInfoGetter">Провайдер информации о текущем тесте</param>
    /// <returns>Настроенный объект Navigation</returns>
    public static Navigation Create(
        IDependenciesFactory dependenciesFactory,
        ITestInfoGetter testInfoGetter
    )
        => new Navigation(
            new PageGetter(new DefaultBrowserGetter(
                new ChromeFactory(
                    new PlaywrightFactory<DefaultPlaywrightConfiguration>(),
                    new HeadlessConfigurator(),
                    new WithoutAuthStrategy()
                    ),
                new ContextTracing(
                    new DefaultTracingConfigurator(testInfoGetter))
            )),
            new PageObjectsFactory(dependenciesFactory)
        );
}