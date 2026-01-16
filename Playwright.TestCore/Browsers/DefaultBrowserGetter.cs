using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Browsers;

public class DefaultBrowserGetter(
    IBrowserFactory browserFactory,
    IContextTracing contextTracing
) : IBrowserGetter, IAsyncDisposable, IDisposable
{
    private readonly Lazy<Task<IBrowserContext>> _browserContext = new(browserFactory.CreateAsync);

    public async Task<IBrowserContext> GetAsync()
    {
        if (!_browserContext.IsValueCreated)
        {
            await contextTracing.StartAsync(await _browserContext.Value);
        }

        return await _browserContext.Value;
    }

    public async ValueTask DisposeAsync()
    {
        if (_browserContext.IsValueCreated)
        {
            await contextTracing.StopAsync(await _browserContext.Value);
            var browserContext = await _browserContext.Value;
            await browserContext.DisposeAsync();
        }
    }

    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();
}