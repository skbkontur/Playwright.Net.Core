using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

public class ChromeFactory(
    IPlaywrightFactory playwrightFactory,
    IBrowserConfigurator browserConfigurator)
    : IBrowserFactory
{
    public async Task<IBrowserContext> CreateAsync()
    {
        var pw = await playwrightFactory.GetPlaywrightAsync();
        var browser = await pw.Chromium.LaunchAsync(browserConfigurator.GetLaunchOptions());
        return await browser.NewContextAsync();
    }
}