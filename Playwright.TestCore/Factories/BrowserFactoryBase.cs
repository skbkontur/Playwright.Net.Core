using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Auth;

namespace SkbKontur.Playwright.TestCore.Factories;

public abstract class BrowserFactoryBase(
    IPlaywrightFactory playwrightFactory,
    IAuthStrategy authStrategy
) : IBrowserFactory
{
    public virtual async Task<IBrowserContext> CreateAsync()
    {
        var pw = await playwrightFactory.GetPlaywrightAsync();
        var browser = await LaunchAsync(pw);
        var contextOptions = authStrategy.GetOrCreateContextOptionsAsync();
        return await browser.NewContextAsync(contextOptions);
    }

    protected abstract Task<IBrowser> LaunchAsync(IPlaywright pw);
}