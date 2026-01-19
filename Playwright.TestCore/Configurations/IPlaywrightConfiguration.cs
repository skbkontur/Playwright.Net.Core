using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Интерфейс конфигурации Playwright.
/// Определяет метод для применения настроек к экземпляру Playwright.
/// </summary>
public interface IPlaywrightConfiguration
{
    /// <summary>
    /// Применить конфигурацию к экземпляру Playwright.
    /// Вызывается после создания экземпляра Playwright.
    /// </summary>
    /// <param name="pw">Экземпляр Playwright для настройки</param>
    void ApplyConfiguration(IPlaywright pw);
}