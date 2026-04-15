using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Стандартная конфигурация Playwright без параметров
/// </summary>
public class DefaultPlaywrightConfiguration : IPlaywrightConfiguration
{
    /// <summary>
    /// Применить стандартную конфигурацию к экземпляру Playwright.
    /// </summary>
    /// <param name="pw">Экземпляр Playwright для настройки</param>
    public void ApplyConfiguration(IPlaywright pw)
    {
    }
}