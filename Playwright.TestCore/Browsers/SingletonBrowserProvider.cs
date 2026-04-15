using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Провайдер браузера с временем жизни Singleton.
/// Использует статическое поле для хранения единственного экземпляра браузера на весь процесс.
/// Потокобезопасная инициализация через SemaphoreSlim.
/// </summary>
/// <param name="browserFactory">Фабрика для создания экземпляра браузера</param>
public class SingletonBrowserProvider(IBrowserFactory browserFactory) : IBrowserGetter, IDisposable, IAsyncDisposable
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    private static IBrowser? _browser = null;

    /// <summary>
    /// Получить общий экземпляр браузера.
    /// При первом вызове создаёт новый экземпляр, последующие вызовы возвращают его же.
    /// </summary>
    /// <returns>Задача, возвращающая экземпляр браузера</returns>
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