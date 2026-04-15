using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Конфигурация Playwright, которая устанавливает атрибут "data-tid" как селектор для поиска элементов по тестовому идентификатору.
/// </summary>
public class DataTidConfiguration : IPlaywrightConfiguration
{
    /// <summary>
    /// Устанавливает "data-tid" как атрибут для поиска элементов.
    /// </summary>
    /// <param name="pw">Экземпляр Playwright для настройки</param>
    public void ApplyConfiguration(IPlaywright pw)
    {
        pw.Selectors.SetTestIdAttribute("data-tid");
    }
}