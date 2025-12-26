using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Browsers;

public class DefaultBrowserGetter(IBrowserFactory browserFactory) : IBrowserGetter, IAsyncDisposable
{
    private readonly Lazy<Task<IBrowserContext>> _browserContext = new(browserFactory.CreateAsync);

    public Task<IBrowserContext> GetAsync()
        => _browserContext.Value;

    public async ValueTask DisposeAsync()
    {
        if (_browserContext.IsValueCreated)
        {
            var browserContext = await _browserContext.Value;
            await browserContext.DisposeAsync();
        }
    }
}