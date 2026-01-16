using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Browsers;

namespace SkbKontur.Playwright.TestCore.Pages;

public class PageGetter(IBrowserGetter browserGetter)
    : IPageGetter, IAsyncDisposable, IDisposable
{
    private readonly Lazy<Task<IPage>> _page = new(
        async () =>
        {
            var context = await browserGetter.GetAsync();
            return context.Pages.FirstOrDefault() ?? await context.NewPageAsync();
        }
    );

    public Task<IPage> GetAsync()
        => _page.Value;

    public async ValueTask DisposeAsync()
    {
        if (_page.IsValueCreated)
        {
            var page = await _page.Value;
            await page.CloseAsync();
        }
    }

    public void Dispose() 
        => DisposeAsync().GetAwaiter().GetResult();
}