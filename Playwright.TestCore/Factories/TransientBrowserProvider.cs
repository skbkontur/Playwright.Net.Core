using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Factories;

public class TransientBrowserProvider(IBrowserFactory browserFactory) : IBrowserGetter, IDisposable, IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private IBrowser? _browser = null;

    public async Task<IBrowser> GetAsync()
    {
        if (_browser != null)
        {
            return _browser;
        }

        await _semaphore.WaitAsync();

        try
        {
            _browser ??= await browserFactory.CreateAsync();
        }
        finally
        {
            _semaphore.Release();
        }

        return _browser;
    }

    public void Dispose()
    {
        _semaphore.Dispose();
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
        if (_semaphore is IAsyncDisposable semaphoreAsyncDisposable)
        {
            await semaphoreAsyncDisposable.DisposeAsync();
        }
        else
        {
            _semaphore.Dispose();
        }
        if (_browser != null)
        {
            await _browser.DisposeAsync();
        }
    }
}