using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using NUnit.Framework;
using SkbKontur.Playwright.TestCore;
using SkbKontur.Playwright.TestCore.Browsers;
using Tests.Infra;

namespace Tests;

/// <summary>
/// Фикстура для подготовки Playwright перед запуском всех тестов.
/// Устанавливает необходимые браузеры.
/// </summary>
[SetUpFixture]
public class PreparePlaywright
{
    /// <summary>
    /// Выполняется один раз перед запуском всех тестов.
    /// Устанавливает браузеры chromium и firefox.
    /// </summary>
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        new BrowsersInstaller().Install(["chromium", "firefox"]);
    }
}

/// <summary>
/// Демонстрирует использование инфраструктуры Playwright TestCore.
/// </summary>
[Parallelizable(ParallelScope.All)]
public class RunPwShould
{
    /// <summary>
    /// Статический провайдер сервисов с зарегистрированными компонентами Playwright.
    /// </summary>
    private static IServiceProvider serviceProvider = new ServiceCollection()
        .UsePlaywright()
        .BuildServiceProvider();

    /// <summary>
    /// Тест успешного выполнения с использованием обычного scope.
    /// Проверяет загрузку главной страницы kontur.ru и наличие заголовка.
    /// </summary>
    [Test]
    public async Task BeSuccess_FromScope()
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var navigation = services.GetRequiredService<Navigation>();
        var page = await navigation.GoToUrlAsync("https://kontur.ru");
        var header = page.Locator("h1", new() { HasText = "Экосистема" });
        await Assertions.Expect(header).ToContainTextAsync("для бизнеса");
    }

    /// <summary>
    /// Тест успешного выполнения с использованием async scope.
    /// Проверяет загрузку различных страниц kontur.ru и наличие логотипа.
    /// </summary>
    /// <param name="url">URL страницы для тестирования</param>
    [Test]
    public async Task BeSuccess_FromAsyncScope([Values(
            "https://kontur.ru",
            "https://kontur.ru/products/docs",
            "https://kontur.ru/products/reporting")]
        string url)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var navigation = services.GetRequiredService<Navigation>();
        var page = await navigation.GoToUrlAsync(url);
        var logo = page.Locator(".kontur-logo_main");
        await Assertions.Expect(logo).ToContainClassAsync("kontur-logo");
    }
}