using System;
using System.Threading.Tasks;
using Kontur.Playwright.TestCore.Factories;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Browsers;

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