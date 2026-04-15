using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using NUnit.Framework;
using SkbKontur.Playwright.TestCore;
using SkbKontur.Playwright.TestCore.Browsers;
using SkbKontur.Playwright.TestCore.Pages;
using Tests.POM.Pages;

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
    private static readonly IServiceProvider ServiceProvider = new ServiceCollection()
        .AddPlaywrightTestCore()
        .UsePom()
        .BuildServiceProvider();

    /// <summary>
    /// Тест успешного выполнения с использованием обычного scope.
    /// Проверяет загрузку главной страницы kontur.ru и наличие заголовка.
    /// </summary>
    [Test]
    public async Task BeSuccess_FromScope()
    {
        using var scope = ServiceProvider.CreateScope();
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
        await using var scope = ServiceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var navigation = services.GetRequiredService<Navigation>();
        var page = await navigation.GoToUrlAsync(url);
        var logo = page.Locator(".kontur-logo_main");
        await Assertions.Expect(logo).ToContainClassAsync("kontur-logo");
    }

    /// <summary>
    /// Тест успешного выполнения с использованием обычного scope.
    /// Проверяет загрузку главной страницы kontur.ru и наличие заголовка.
    /// </summary>
    [Test]
    public async Task BeSuccess_FromScope_WithPom()
    {
        using var scope = ServiceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var navigation = services.GetRequiredService<Navigation>();
        var page = await navigation.GoToPageAsync<KonturPage>();
        await page.EcosystemHeader.Expect().ToContainTextAsync("для бизнеса");
    }

    /// <summary>
    /// Тест успешного выполнения с использованием async scope.
    /// Проверяет загрузку различных страниц kontur.ru и наличие логотипа.
    /// </summary>
    /// <param name="url">URL страницы для тестирования</param>
    [Test]
    public async Task BeSuccess_FromAsyncScope_WithPom([Values(
            "",
            "/products/docs",
            "/products/reporting")]
        string uri)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var navigation = services.GetRequiredService<Navigation>();
        var page = await navigation.GoToPageAsync<KonturPage>(uri);
        await page.Logo.Expect().ToContainClassAsync("kontur-logo");
    }

    [Test]
    public async Task LocalStorageShouldWork([Range(1, 5)] int value)
    {
        var key = value.ToString();
        var expectValue = (value * value).ToString();

        await using var scope = ServiceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var navigation = services.GetRequiredService<Navigation>();
        
        await navigation.GoToPageAsync<KonturPage>();
        var localStorage = services.GetRequiredService<ILocalStorage>();
        await localStorage.SetItemAsync(key, expectValue);

        var actual = await localStorage.GetItemAsync(key);
        Assert.That(actual, Is.EqualTo(expectValue));
    }
}