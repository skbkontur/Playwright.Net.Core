using System.Threading.Tasks;
using Kontur.Playwright.TestCore.Configurations;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Factories;

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