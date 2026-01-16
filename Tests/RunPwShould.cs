using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using NUnit.Framework;
using SkbKontur.Playwright.TestCore;
using SkbKontur.Playwright.TestCore.Browsers;
using Tests.Infra;

namespace Tests;

[SetUpFixture]
public class PreparePlaywright
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        new BrowsersInstaller().Install(["chromium", "firefox"]);
    }
}

[Parallelizable(ParallelScope.All)]
public class RunPwShould
{
    private static IServiceProvider serviceProvider = new ServiceCollection()
        .UsePlaywright()
        .BuildServiceProvider();

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