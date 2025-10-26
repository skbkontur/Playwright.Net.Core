using System.Threading.Tasks;
using Kontur.Playwright.TestCore.Configurations;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Factories;

public class FirefoxFactory(
    IPlaywrightFactory playwrightFactory,
    IBrowserConfigurator browserConfigurator)
    : IBrowserFactory
{
    public async Task<IBrowserContext> CreateAsync()
    {
        var pw = await playwrightFactory.GetPlaywrightAsync();
        var browser = await pw.Firefox.LaunchAsync(browserConfigurator.GetLaunchOptions());
        return await browser.NewContextAsync();
    }
}