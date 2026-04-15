using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Factories;

public class SingletonBrowserProvider(IBrowserFactory browserFactory) : IBrowserGetter, IDisposable, IAsyncDisposable
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    private static IBrowser? _browser = null;

    public async Task<IBrowser> GetAsync()
    {
        if (_browser != null)
        {
            return _browser;
        }

        await Semaphore.WaitAsync();

        try
        {
            _browser ??= await browserFactory.CreateAsync();
        }
        finally
        {
            Semaphore.Release();
        }

        return _browser;
    }

    public void Dispose()
    {
        Semaphore.Dispose();
        if (_browser is IDisposable browserDisposable)
        {
            browserDisposable.Dispose();
        }
        else if (_browser != null)
        {
            _ = _browser.DisposeAsync().AsTask();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Semaphore is IAsyncDisposable semaphoreAsyncDisposable)
        {
            await semaphoreAsyncDisposable.DisposeAsync();
        }
        else
        {
            Semaphore.Dispose();
        }
        if (_browser != null)
        {
            await _browser.DisposeAsync();
        }
    }
}