using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Провайдер браузера с временем жизни Transient.
/// Каждый экземпляр провайдера хранит свой экземпляр браузера (не статический).
/// Потокобезопасная инициализация через SemaphoreSlim.
/// </summary>
/// <param name="browserFactory">Фабрика для создания экземпляра браузера</param>
public class TransientBrowserProvider(IBrowserFactory browserFactory) : IBrowserGetter, IDisposable, IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private IBrowser? _browser = null;

    /// <summary>
    /// Получить экземпляр браузера для данного провайдера.
    /// При первом вызове создаёт новый экземпляр, последующие вызовы возвращают его же.
    /// </summary>
    /// <returns>Задача, возвращающая экземпляр браузера</returns>
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