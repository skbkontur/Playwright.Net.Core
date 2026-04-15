using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

public class WebkitFactory(IPlaywrightGetter playwrightGetter, IBrowserConfigurator configurator)
    : IBrowserFactory
{
    public async Task<IBrowser> CreateAsync()
    {
        var pw = await playwrightGetter.GetPlaywrightAsync();
        return await pw.Webkit.LaunchAsync(configurator.GetLaunchOptions());
    }
}