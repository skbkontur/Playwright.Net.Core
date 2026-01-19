using System.IO;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Конфигуратор трассировки для фикстур (наборов тестов).
/// Сохраняет трассировку в файл с именем класса тестов.
/// </summary>
/// <param name="infoGetter">Провайдер информации о текущем тесте</param>
public class FixtureTracingConfigurator(ITestInfoGetter infoGetter) : ITracingConfigurator
{
    /// <summary>
    /// Получить параметры начала трассировки для класса тестов.
    /// Включает скриншоты, снимки DOM и исходный код.
    /// </summary>
    /// <returns>Параметры начала трассировки</returns>
    public TracingStartOptions GetTracingStartOptions()
    {
        return new TracingStartOptions
        {
            Title = $"{infoGetter.TestClassName}",
            Screenshots = true,
            Snapshots = true,
            Sources = true
        };
    }

    /// <summary>
    /// Получить параметры окончания трассировки.
    /// Сохраняет трассировку в ZIP файл с именем класса в директории playwright-traces.
    /// </summary>
    /// <returns>Параметры окончания трассировки</returns>
    public TracingStopOptions GetTracingStopOptions()
    {
        var path = Path.Combine(
            infoGetter.WorkDirectory,
            "playwright-traces",
            $"{infoGetter.TestClassName}.zip"
        );
        return new TracingStopOptions {Path = path};
    }
}