using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Реализация управления трассировкой контекста браузера.
/// Использует конфигуратор трассировки для получения параметров.
/// </summary>
/// <param name="tracingConfigurator">Конфигуратор параметров трассировки</param>
public class FullTracing(ITracingConfigurator tracingConfigurator) : IContextTracing
{
    /// <summary>
    /// Начать трассировку для указанного контекста браузера.
    /// Использует параметры из конфигуратора трассировки.
    /// </summary>
    /// <param name="context">Контекст браузера для трассировки</param>
    /// <returns>Задача завершения начала трассировки</returns>
    public Task StartAsync(IBrowserContext context)
        => context.Tracing.StartAsync(tracingConfigurator.GetTracingStartOptions());

    /// <summary>
    /// Остановить трассировку для указанного контекста браузера.
    /// Сохраняет трассировку в соответствии с параметрами конфигуратора.
    /// </summary>
    /// <param name="context">Контекст браузера, трассировку которого нужно остановить</param>
    /// <returns>Задача завершения остановки трассировки</returns>
    public Task StopAsync(IBrowserContext context)
        => context.Tracing.StopAsync(tracingConfigurator.GetTracingStopOptions());
}

/// <summary>
/// Реализация управления трассировкой контекста браузера.
/// Использует конфигуратор трассировки для получения параметров.
/// </summary>
/// <param name="tracingConfigurator">Конфигуратор параметров трассировки</param>
public class FailureTestsTracing(
    ITracingConfigurator tracingConfigurator,
    IFailureTestResult failureTestResult
) : IContextTracing
{
    /// <summary>
    /// Начать трассировку для указанного контекста браузера.
    /// Использует параметры из конфигуратора трассировки.
    /// </summary>
    /// <param name="context">Контекст браузера для трассировки</param>
    /// <returns>Задача завершения начала трассировки</returns>
    public Task StartAsync(IBrowserContext context)
        => context.Tracing.StartAsync(tracingConfigurator.GetTracingStartOptions());

    /// <summary>
    /// Остановить трассировку для указанного контекста браузера.
    /// Сохраняет трассировку в соответствии с параметрами конфигуратора.
    /// </summary>
    /// <param name="context">Контекст браузера, трассировку которого нужно остановить</param>
    /// <returns>Задача завершения остановки трассировки</returns>
    public async Task StopAsync(IBrowserContext context)
    {
        var options = await failureTestResult.IsFailureAsync()
            ? tracingConfigurator.GetTracingStopOptions()
            : new TracingStopOptions() { Path = null };
        await context.Tracing.StopAsync(options);
    }
}