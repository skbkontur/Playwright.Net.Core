using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

public class ChromeFactory(IPlaywrightGetter playwrightGetter, IBrowserConfigurator configurator)
    : IBrowserFactory
{
    public async Task<IBrowser> CreateAsync()
    {
        var pw = await playwrightGetter.GetPlaywrightAsync();
        return await pw.Chromium.LaunchAsync(configurator.GetLaunchOptions());
    }
}