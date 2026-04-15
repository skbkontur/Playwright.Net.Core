using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Управляет жизненным циклом контекста браузера и автоматически запускает/останавливает трассировку.
/// </summary>
/// <param name="browserContextFactory">Фабрика для создания контекстов браузера</param>
/// <param name="contextTracing">Сервис для управления трассировкой контекста</param>
public class DefaultBrowserContextProvider(
    IBrowserContextFactory browserContextFactory,
    IContextTracing tracing
) : IBrowserContextGetter, IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Лениво инициализируемый контекст браузера.
    /// Создаётся при первом обращении.
    /// </summary>
    private readonly Lazy<Task<IBrowserContext>> _browserContext = new(async () =>
    {
        var context = await browserContextFactory.CreateAsync();
        await tracing.StartAsync(context);
        return context;
    });

    /// <summary>
    /// Получить контекст браузера с автоматическим запуском трассировки.
    /// При первом вызове создаёт контекст и начинает трассировку.
    /// </summary>
    /// <returns>Задача, возвращающая контекст браузера</returns>
    public Task<IBrowserContext> GetAsync()
        => _browserContext.Value;

    /// <summary>
    /// Останавливает трассировку и освобождает контекст.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (!_browserContext.IsValueCreated)
            return;

        var task = _browserContext.Value;
        if (task.IsFaulted || task.IsCanceled)
            return;

        try
        {
            var browserContext = await task;
            await tracing.StopAsync(browserContext);
            await browserContext.DisposeAsync();
        }
        catch (PlaywrightException)
        {
        }
    }

    /// <summary>
    /// Останавливает трассировку и освобождает контекст.
    /// </summary>
    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();
}