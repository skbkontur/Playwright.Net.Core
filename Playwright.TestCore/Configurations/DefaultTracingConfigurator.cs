using System;
using System.IO;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Стандартный конфигуратор трассировки для отдельных тестов.
/// Сохраняет трассировку в файл с именем теста.
/// </summary>
/// <param name="infoGetter">Провайдер информации о текущем тесте</param>
public class DefaultTracingConfigurator : ITracingConfigurator
{
    public string TestName { get; } = $"Unknown_{nameof(TestName)}_{Guid.NewGuid()}";
    public string TestClassName { get; } = $"Unknown_{nameof(TestClassName)}_{Guid.NewGuid()}";
    public string WorkDirectory { get; } = AppContext.BaseDirectory;
    
    /// <summary>
    /// Получить параметры начала трассировки для текущего теста.
    /// Включает скриншоты, снимки DOM и исходный код.
    /// </summary>
    /// <returns>Параметры начала трассировки</returns>
    public TracingStartOptions GetTracingStartOptions()
    {
        return new TracingStartOptions
        {
            Title = $"{TestClassName}.{TestName}",
            Screenshots = true,
            Snapshots = true,
            Sources = true
        };
    }

    /// <summary>
    /// Получить параметры окончания трассировки.
    /// Сохраняет трассировку в ZIP файл в директории playwright-traces.
    /// </summary>
    /// <returns>Параметры окончания трассировки</returns>
    public TracingStopOptions GetTracingStopOptions()
    {
        var path = Path.Combine(
            WorkDirectory,
            "playwright-traces",
            $"{TestClassName}.{TestName}.zip"
        );
        return new TracingStopOptions {Path = path};
    }
}