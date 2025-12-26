using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Browsers;

public class SingletonBrowserGetter(IBrowserFactory browserFactory) : IBrowserGetter, IAsyncDisposable
{
    private static readonly object Lock = new();
    private static Lazy<Task<IBrowserContext>>? _browserStaticContext;

    public Task<IBrowserContext> GetAsync()
    {
        if (_browserStaticContext != null) 
            return _browserStaticContext.Value;
        
        lock (Lock)
        {
            _browserStaticContext ??= new(browserFactory.CreateAsync);
        }

        return _browserStaticContext.Value;
    }

    public ValueTask DisposeAsync()
        => new ValueTask();
}