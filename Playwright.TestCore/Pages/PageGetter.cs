using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Browsers;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Pages;

public class PageGetter(IBrowserGetter browserGetter, ITracingConfigurator tracingConfigurator)
    : IPageGetter, IAsyncDisposable
{
    private readonly Lazy<Task<IPage>> _page = new(
        async () =>
        {
            var context = await browserGetter.GetAsync();
            await context.Tracing.StartAsync(tracingConfigurator.GetTracingStartOptions());
            return context.Pages.FirstOrDefault() ?? await context.NewPageAsync();
        }
    );

    public Task<IPage> GetAsync()
        => _page.Value;

    public async ValueTask DisposeAsync()
    {
        if (_page.IsValueCreated)
        {
            var context = (await _page.Value).Context;
            await context.Tracing.StopAsync(tracingConfigurator.GetTracingStopOptions());
            var page = await _page.Value;
            await page.CloseAsync();
        }
    }
}