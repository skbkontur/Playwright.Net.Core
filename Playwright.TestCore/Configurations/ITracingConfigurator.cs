using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Интерфейс конфигуратора трассировки Playwright.
/// Определяет параметры для начала и окончания записи трассировки.
/// </summary>
public interface ITracingConfigurator
{
    /// <summary>
    /// Получить параметры для начала трассировки.
    /// </summary>
    /// <returns>Параметры начала трассировки</returns>
    TracingStartOptions GetTracingStartOptions();

    /// <summary>
    /// Получить параметры для окончания трассировки.
    /// </summary>
    /// <returns>Параметры окончания трассировки</returns>
    TracingStopOptions GetTracingStopOptions();
}