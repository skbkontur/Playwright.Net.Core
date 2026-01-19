using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Управляет жизненным циклом контекста браузера и автоматически запускает/останавливает трассировку.
/// </summary>
/// <param name="browserFactory">Фабрика для создания контекстов браузера</param>
/// <param name="contextTracing">Сервис для управления трассировкой контекста</param>
public class DefaultBrowserGetter(
    IBrowserFactory browserFactory,
    IContextTracing contextTracing
) : IBrowserGetter, IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Лениво инициализируемый контекст браузера.
    /// Создаётся при первом обращении.
    /// </summary>
    private readonly Lazy<Task<IBrowserContext>> _browserContext = new(browserFactory.CreateAsync);

    /// <summary>
    /// Получить контекст браузера с автоматическим запуском трассировки.
    /// При первом вызове создаёт контекст и начинает трассировку.
    /// </summary>
    /// <returns>Задача, возвращающая контекст браузера</returns>
    public async Task<IBrowserContext> GetAsync()
    {
        if (!_browserContext.IsValueCreated)
        {
            await contextTracing.StartAsync(await _browserContext.Value);
        }

        return await _browserContext.Value;
    }

    /// <summary>
    /// Останавливает трассировку и освобождает контекст.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_browserContext.IsValueCreated)
        {
            await contextTracing.StopAsync(await _browserContext.Value);
            var browserContext = await _browserContext.Value;
            await browserContext.DisposeAsync();
        }
    }

    /// <summary>
    /// Останавливает трассировку и освобождает контекст.
    /// </summary>
    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();
}